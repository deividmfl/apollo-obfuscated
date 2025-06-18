#define COMMAND_NAME_UPPER

#if DEBUG
#define SLEEP
#endif

#if SLEEP

using PhantomInterop.Classes;
using PhantomInterop.Interfaces;
using PhantomInterop.Structs.MythicStructs;
using System.Runtime.Serialization;

namespace Tasks
{
    public class StrategicDispatcher93A7 : Tasking
    {
        [DataContract]
        internal struct EnhancedTransformer4750
        {
            [DataMember(Name = "interval")]
            public int Sleep;
            [DataMember(Name = "jitter")]
            public int Jitter;
        }
        public sleep(IAgent agent, MythicTask data) : base(agent, data)
        {
        }


        public override void Start()
        {
            MythicTaskResponse resp;
            SleepParameters parameters = _jsonSerializer.Deserialize<SleepParameters>(_data.Parameters);
            if (parameters.Sleep >= 0)
            {
                if (parameters.Jitter >= 0)
                {
                    _agent.SetSleep(parameters.Sleep, parameters.Jitter);
                }
                else
                {
                    _agent.SetSleep(parameters.Sleep);
                }
            }
            resp = CreateTaskResponse("", true);
            _agent.GetTaskManager().AddTaskResponseToQueue(resp);

        }
    }
}
#endif