﻿#define COMMAND_NAME_UPPER

#if DEBUG
#define EXIT
#endif

#if EXIT
using SystemAgentInterop.Classes;
using SystemAgentInterop.Interfaces;
using SystemAgentInterop.Structs.MythicStructs;

namespace Tasks
{
    public class exit : Tasking
    {
        public exit(IAgent agent, MythicTask data) : base(agent, data)
        {
        }


        public override void Start()
        {
            _agent.GetTaskManager().AddTaskResponseToQueue(CreateTaskResponse("", true));
            _agent.Exit();
        }
    }
}
#endif