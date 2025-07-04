﻿using SystemAgentInterop.Interfaces;
using HttpTransport;
using System;
using System.Collections.Generic;

namespace SystemAgent.Management.C2
{
    public class C2ProfileManager : SystemInterop.Classes.C2ProfileManager
    {
        public C2ProfileManager(IAgent agent) : base(agent)
        {

        }

        public override IC2Profile NewC2Profile(Type c2, ISerializer serializer, Dictionary<string, string> parameters)
        {
            if (c2 == typeof(HttpProfile))
            {
                return new HttpProfile(parameters, serializer, Agent);
            } else
            {
                throw new ArgumentException($"Unsupported C2 Profile type: {c2.Name}");
            }
        }
    }
}
