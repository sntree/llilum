//
// Copyright (c) Microsoft Corporation.    All rights reserved.
//


namespace Microsoft.DeviceModels.Chipset.CortexM.Runtime
{
    using System;

    using Microsoft.Zelig.Runtime.TargetPlatform.ARMv7;
    using RT = Microsoft.Zelig.Runtime;

    public abstract class ThreadManager : RT.ARMv7ThreadManager
    {

        //
        // State
        //

        protected Drivers.ContextSwitchTimer m_contextSwitchTimer;        

        //
        // Helper Methods
        //

        public override void InitializeBeforeStaticConstructors()
        {
            base.InitializeBeforeStaticConstructors();
        }

        public override void InitializeAfterStaticConstructors( uint[] systemStack )
        {
            base.InitializeAfterStaticConstructors( systemStack );
        }
        
        //--//
        
        //
        // Extensibility 
        //
        
        public override void Activate()
        {
            //
            // Activate the quantum timer, when the Idle Thread will run will enable exceptions, 
            // thus letting the context switching to start 
            //
            RT.BugCheck.AssertInterruptsOff( );

            m_contextSwitchTimer = Drivers.ContextSwitchTimer.Instance;
            m_contextSwitchTimer.Reset();
        }

        public override void CancelQuantumTimer()
        {
            m_contextSwitchTimer.Cancel();
        }

        public override void SetNextQuantumTimer()
        {
            m_contextSwitchTimer.Reset( );
        }

        public override void SetNextQuantumTimer( RT.SchedulerTime nextTimeout )
        {
            DateTime   dt                  = ( DateTime )nextTimeout;
            const long TicksPerMillisecond = 10000; // Number of 100ns ticks per time unit
            long       ms                  = dt.Ticks / TicksPerMillisecond;

            if(ms > Drivers.ContextSwitchTimer.c_MaxCounterValue)
            {
                RT.BugCheck.Assert( false, RT.BugCheck.StopCode.IllegalSchedule );
            }

            m_contextSwitchTimer.Schedule( (uint)ms );
        }
        
        public override void TimeQuantumExpired()
        {
            //
            // this will cause the reschedule
            //
            base.TimeQuantumExpired( );

            //
            // stage a PendSV request to complete the ContextSwitch
            //
            ProcessorARMv7M.CompleteContextSwitch( );
        }
    } 
}
