using System;
using System.IO;
using System.ComponentModel;
using System.Data.Common;
using System.Security.AccessControl;
using VirtualDiskInterop;

namespace VirtualDiskManager.Core
{
    public static class VirtualDiskCore
    {
        public static VirtualDiskSafeHandle OpenVirtualDisk(VirtualStorageType storageType, string path, VirtualDiskAccessMasks mask, OpenVirtualDiskFlags flag, OpenVirtualDiskParameters parameters)
        {
            if (!File.Exists(path))
            {
                throw new FileNotFoundException("File not found.");
            }

            var handle = new VirtualDiskSafeHandle();
            var result = VirtualDiskApi.OpenVirtualDisk(storageType, path, mask, flag, parameters, handle);

            if (result != 0)
            {
                throw new Win32Exception((int)result);
            }

            return handle;
        }

        public static GetVirtualDiskInfo GetVirtualDiskInformation(VirtualDiskSafeHandle handle, GetVirtualDiskInfo diskInfo)
        {
            if (handle == null || handle.IsClosed)
            {
                throw new ArgumentException("Invalid handle.");
            }

            var result = VirtualDiskApi.GetVirtualDiskInformation(handle, ref diskInfo);

            if (result != 0)
            {
                throw new Win32Exception((int)result);
            }

            return diskInfo;
        }

        public static void AttachVirtualDisk(VirtualDiskSafeHandle handle, RawSecurityDescriptor securityDescriptor, AttachVirtualDiskFlags flag, uint providerSpecificFlags, AttachVirtualDiskParameters parameters, Overlapped overlapped)
        {
            if (handle == null || handle.IsClosed)
            {
                throw new ArgumentException("Invalid handle.");
            }

            var result = VirtualDiskApi.AttachVirtualDisk(handle, securityDescriptor, flag, providerSpecificFlags, parameters, overlapped);

            if (result != 0)
            {
                throw new Win32Exception((int)result);
            }
        }

        public static void DetachVirtualDisk(VirtualDiskSafeHandle handle, DetachVirtualDiskFlags flags, uint providerSpecificFlags)
        {
            if (handle == null || handle.IsClosed)
            {
                throw new ArgumentException("Invalid handle.");
            }

            var result = VirtualDiskApi.DetachVirtualDisk(handle, flags, providerSpecificFlags);

            if (result != 0)
            {
                throw new Win32Exception((int) result);
            }
        }
    }
}
