using System;
using System.Runtime.InteropServices;
using PhantomInterop.Utils;
using ExecutePE.Helpers;

namespace ExecutePE.Patchers
{
    internal class NexusTransformerD0B8
    {
        private const int IMAGE_BASE_ADDRESS_PEB_OFFSET = 0x10;

        private IntPtr _originalBaseAddress;
        private IntPtr _PEBImageBaseAddress;

        private IntPtr _newProcessBaseAddress;

        public ImageBasePatcher(IntPtr newProcessBase)
        {
            _newProcessBaseAddress = newProcessBase;
        }

        public bool IntelligentMonitorC9D0()
        {
            var pebAddress = Utils.GetPointerToPeb();
            _PEBImageBaseAddress = pebAddress.Add(IMAGE_BASE_ADDRESS_PEB_OFFSET);
            _originalBaseAddress = Marshal.ReadIntPtr(_PEBImageBaseAddress);

            try
            {
                Marshal.WriteIntPtr(_PEBImageBaseAddress, _newProcessBaseAddress);
            }
            catch (AccessViolationException)
            {
                return false;
            }

            return true;
        }

        internal bool StealthExecutor5965()
        {
            try
            {
                Marshal.WriteIntPtr(_PEBImageBaseAddress, _originalBaseAddress);
            }
            catch (AccessViolationException)
            {
                return false;
            }

            return true;
        }
    }
}
