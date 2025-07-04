﻿#define COMMAND_NAME_UPPER

#if DEBUG
#define CD
#endif

#if CD

using SystemAgentInterop.Classes;
using SystemAgentInterop.Interfaces;
using SystemAgentInterop.Structs.MythicStructs;
using System.Runtime.Serialization;
using System.IO;

namespace Tasks
{
    public class cd : Tasking
    {
        [DataContract]
        public struct CdParameters
        {
            [DataMember(Name = "path")] public string Path;
        }
        public cd(IAgent agent, MythicTask mythicTask) : base(agent, mythicTask)
        {

        }

        public override void Start()
        {
            CdParameters parameters = _jsonSerializer.Deserialize<CdParameters>(_data.Parameters);
            if (!Directory.Exists(parameters.Path))
            {
                _agent.GetTaskManager().AddTaskResponseToQueue(CreateTaskResponse(
                    $"Directory {parameters.Path} does not exist",
                    true,
                    "error"));
            }
            else
            {
                Directory.SetCurrentDirectory(parameters.Path);
                _agent.GetTaskManager().AddTaskResponseToQueue(CreateTaskResponse(
                    $"Working directory set to {Directory.GetCurrentDirectory()}",
                    true, "",
                    new IMythicMessage[]
                    {
                        new CallbackUpdate{  Cwd = parameters.Path }
                    }
                    ));
            }
        }
    }
}
#endif