using SystemAgentInterop.Structs.SystemCoreStructs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace SystemAgentInterop.Serializers
{
    public class SystemSerializationBinderHandler : SerializationBinder
    {
        public override Type BindToType(string assemblyName, string typeName)
        {
            if (typeName == "SystemInterop.Structs.SystemCoreStructs.PeerMessage")
            {
                return typeof(PeerMessage);
            }
            else
            {
                return typeof(Nullable);
            }
        }
    }
}
