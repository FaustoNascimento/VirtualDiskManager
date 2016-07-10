using System;
using System.IO;
using System.ComponentModel;
using System.Security.AccessControl;
using VirtualDiskInterop;

namespace VirtualDiskManager.Core
{
    public static class VirtualDiskCore
    {
        public static VirtualDiskSafeHandle OpenVirtualDisk(VirtualStorageType storageType, string filename, VirtualDiskAccessMasks mask, OpenVirtualDiskFlags flag, OpenVirtualDiskParameters parameters)
        {
            if (!File.Exists(filename))
            {
                throw new FileNotFoundException("File not found.");
            }

            var handle = new VirtualDiskSafeHandle();
            var result = VirtualDiskApi.OpenVirtualDisk(storageType, filename, mask, flag, parameters, handle);

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

        public static VirtualDiskSafeHandle CreateVirtualDisk(VirtualStorageType storageType, string filename,
            VirtualDiskAccessMasks mask, RawSecurityDescriptor securityDescriptor, CreateVirtualDiskFlags flags,
            uint providerSpecificFlags, CreateVirtualDiskParameters parameters, Overlapped overlapped)
        {
            if (!Directory.GetParent(Path.GetFullPath(filename)).Exists ||
                string.IsNullOrEmpty(Path.GetFileName(filename)))
            {
                throw new FileNotFoundException("Specified path does not exist or is invalid.");
            }

            var handle = new VirtualDiskSafeHandle();
            var result = VirtualDiskApi.CreateVirtualDisk(storageType, filename, mask, securityDescriptor, flags,
                providerSpecificFlags, parameters, overlapped, handle);

            if (result != 0)
            {
                throw new Win32Exception((int)result);
            }

            return handle;
        }
    }
}
