﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices;
using SystemAgentInterop.Classes.Api;
using SystemAgentInterop.Interfaces;
using SystemAgentInterop.Utils;

namespace SimpleResolver
{
    public class GetProcResolver : IWin32ApiResolver
    {
        private Dictionary<Library, IntPtr> _modulePointers = new Dictionary<Library, IntPtr>();
        
        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr LoadLibraryA([MarshalAs(UnmanagedType.LPStr)] string lpFileName);
        
        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr GetProcAddress(IntPtr hModule, [MarshalAs(UnmanagedType.LPStr)] string lpProcName);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr GetModuleHandleA([MarshalAs(UnmanagedType.LPStr)] string lpModuleName);
        
        public T GetLibraryFunction<T>(Library library, string functionName, bool canLoadFromDisk = true, bool resolveForwards = true) where T : Delegate
        {
            IntPtr functionHandle = IntPtr.Zero;
            if (!_modulePointers.ContainsKey(library))
            {
                IntPtr libraryHandle = GetModuleHandleA(library.ToString());
                if (libraryHandle == IntPtr.Zero)
                {
                    libraryHandle = LoadLibraryA(library.ToString());
                }

                if (libraryHandle == IntPtr.Zero)
                {
                    DebugHelp.DebugWriteLine($"Failed to load library {library}");
                    throw new Win32Exception($"Failed to load library {functionName}",
                        new Win32Exception(Marshal.GetLastWin32Error()));
                }
                //DebugHelp.DebugWriteLine($"Loaded library {library}");
                _modulePointers[library] = libraryHandle;
            }

            functionHandle = GetProcAddress(_modulePointers[library], functionName);
            
            if (functionHandle != IntPtr.Zero)
            {
                //DebugHelp.DebugWriteLine($"Found function {functionName} in library {library}");
                //todo: check if this is giving valid values back ?
                return Marshal.GetDelegateForFunctionPointer(functionHandle, typeof(T)) as T;
            }
            DebugHelp.DebugWriteLine($"Could not find function {functionName} in library {library}");
            throw new Exception("Could not find function " + functionName + " in library " + library.ToString(),
                new Win32Exception(Marshal.GetLastWin32Error()));
        }

        public T GetLibraryFunction<T>(Library library, short ordinal, bool canLoadFromDisk = true, bool resolveForwards = true) where T : Delegate
        {
            throw new NotImplementedException();
        }

        public T GetLibraryFunction<T>(Library library, string functionHash, long key, bool canLoadFromDisk = true,
            bool resolveForwards = true) where T : Delegate
        {
            throw new NotImplementedException();
        }
    }
}