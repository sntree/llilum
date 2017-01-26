﻿//
// Copyright (c) Microsoft Corporation.    All rights reserved.
//

namespace Microsoft.Llilum.BoardConfigurations
{
    using Microsoft.Zelig.Runtime;
    using Microsoft.Zelig.Configuration.Environment;

    [DisplayName("STM32F429")]
    public sealed class STM32F429SoC : ProcessorCategory
    {
        [DependsOn(typeof(STM32F429SoC))]
        [DisplayName("Internal 128KB Static RAM")]
        [Defaults("BaseAddress", 0x20000000U)]
        [Defaults("SizeInBytes", 128 * 1024)]
        [Defaults("WordSize", 32)]
        [Defaults("WaitStates", 0)]
        [EnumDefaults("Characteristics", MemoryAttributes.RAM |
                                          MemoryAttributes.RandomAccessMemory |
                                          MemoryAttributes.InternalMemory |
                                          MemoryAttributes.ConfiguredAtEntryPoint)]
        public sealed class InternalRAM128KB : RamMemoryCategory
        {
        }


        [DependsOn(typeof(STM32F429SoC))]
        [DisplayName("Internal 512KB FLASH")]
        [Defaults("BaseAddress", 0x08000000U)]
        [Defaults("SizeInBytes", 512 * 1024)]
        [Defaults("WordSize", 32)]
        [Defaults("WaitStates", 0)]
        [EnumDefaults("Characteristics", MemoryAttributes.FLASH |
                                          MemoryAttributes.RandomAccessMemory |
                                          MemoryAttributes.InternalMemory |
                                          MemoryAttributes.ConfiguredAtEntryPoint)]
        public sealed class InternalFlash512KB : FlashMemoryCategory
        {
        }
    }

    //--//
    //--//
    //--//

    [DisplayName("Memory Map for STM32F429")]
    public sealed class STM32F429MemoryMap : MemoryMapCategory
    {

        [MergeEnumDefaults("Characteristics", MemoryAttributes.InternalMemory |
                                               MemoryAttributes.RandomAccessMemory |
                                               MemoryAttributes.ConfiguredAtEntryPoint)]
        [MemorySection(MemoryUsage.Stack |
                        MemoryUsage.Heap |
                        MemoryUsage.DataRW |
                        MemoryUsage.Code)]
        [AllowedOptions(typeof(STM32F429SoC.InternalRAM128KB))]
        [Defaults("BaseAddress", 0x20000000U)]
        public RamMemoryCategory InternalRamChip;

        [MergeEnumDefaults("Characteristics", MemoryAttributes.InternalMemory |
                                               MemoryAttributes.RandomAccessMemory |
                                               MemoryAttributes.ConfiguredAtEntryPoint |
                                               MemoryAttributes.LoadedAtEntrypoint)]
        [MemorySection(MemoryUsage.Bootstrap |
                        MemoryUsage.Code |
                        MemoryUsage.DataRO |
                        MemoryUsage.VectorsTable |
                        MemoryUsage.Relocation)]
        [AllowedOptions(typeof(STM32F429SoC.InternalFlash512KB))]
        [Defaults("BaseAddress", 0x08000000U)]
        public FlashMemoryCategory InternalFlashChip;
    }

    //--//
    //--//
    //--//

    [DisplayName("STM32F429 MBED")]
    public sealed class STM32F429 : ProductCategory
    {
        [AllowedOptions(typeof(STM32F429SoC))]
        [Defaults("CoreClockFrequency"     , 100000000UL)]
        [Defaults("RealTimeClockFrequency" , 1000000UL)]
        [Defaults("DefaultThreadPoolThreads", 2)]
        [Defaults("DefaultTimerPoolThreads" , 2)]
        public STM32F429SoC Processor;

        //--//

        [AllowedOptions(typeof(STM32F429SoC.InternalRAM128KB))]
        [Defaults("BaseAddress", 0x20000000U)]
        public RamMemoryCategory InternalRam;

        [AllowedOptions(typeof(STM32F429SoC.InternalFlash512KB))]
        [Defaults("BaseAddress", 0x08000000U)]
        public FlashMemoryCategory InternalFlashChip;
    }

    //--//
    //--//
    //--//

    [DisplayName("LLVM Compilation for STM32F429")]
    [Defaults("Platform", typeof(Microsoft.Zelig.Configuration.Environment.Abstractions.Architectures.LlvmForArmV7M_VFP))]
    [Defaults("CallingConvention", typeof(Microsoft.Zelig.Configuration.Environment.Abstractions.Architectures.LlvmForArmV7MCallingConvention))]
    [Defaults("Product", typeof(STM32F429))]
    [Defaults("MemoryMap", typeof(STM32F429MemoryMap))]
    public sealed class STM32F429MBEDCompilationSetup : CompilationSetupCategory
    {
    }

}
