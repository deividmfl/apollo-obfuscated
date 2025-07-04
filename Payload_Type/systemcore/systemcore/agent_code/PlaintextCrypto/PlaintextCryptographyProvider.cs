﻿using System;
using SystemAgentInterop.Classes;
using SystemAgentInterop.Interfaces;

namespace PlaintextCryptography
{
    public class PlaintextCryptographyProvider : CryptographyProvider, ICryptography
    {
        public PlaintextCryptographyProvider(string uuid, string key) : base(uuid, key)
        {
            
        }

        
        public override bool UpdateKey(string key)
        {
            throw new Exception("Operation is not supported by PlaintextCryptographyProvider.");
        }

        public override string Encrypt(string plaintext)
        {
            return string.Format("{0}{1}", UUID, plaintext);
        }

        public override string Decrypt(string enc)
        {
            if (!enc.StartsWith(base.GetUUID()))
                throw new Exception("Invalid message received from server.");
            return enc.Substring(UUID.Length);
        }
    }
}
