﻿#define COMMAND_NAME_UPPER

#if DEBUG
#define WHOAMI
#endif

#if WHOAMI

using SystemAgentInterop.Classes;
using SystemAgentInterop.Interfaces;
using SystemAgentInterop.Structs.MythicStructs;

namespace Tasks
{
    public class whoami : Tasking
    {
        public whoami(IAgent agent, SystemInterop.Structs.MythicStructs.MythicTask data) : base(agent, data)
        {
        }


        public override void Start()
        {
            MythicTaskResponse resp;
            if (_agent.GetIdentityManager().GetCurrentLogonInformation(out var logonInfo))
            {
                resp = CreateTaskResponse(
                    $"Local Identity: {_agent.GetIdentityManager().GetCurrentPrimaryIdentity().Name}\n" +
                    $"Impersonation Identity: {logonInfo.Domain}\\{logonInfo.Username}", true);
            }
            else
            {
                resp = CreateTaskResponse(
                    $"Local Identity: {_agent.GetIdentityManager().GetCurrentPrimaryIdentity().Name}\n" +
                    $"Impersonation Identity: {_agent.GetIdentityManager().GetCurrentImpersonationIdentity().Name}", true);
            }
            // Your code here..
            // Then add response to queue
            _agent.GetTaskManager().AddTaskResponseToQueue(resp);
        }
    }
}

#endif