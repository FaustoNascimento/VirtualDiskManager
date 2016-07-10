using System;
using System.Security.AccessControl;
using VirtualDiskInterop;
using VirtualDiskManager.Base;
using VirtualDiskManager.Core;

namespace VirtualDiskManager
{
    public class VirtualHardDisk : VirtualHardDiskBase
    {
        private VirtualHardDisk(string filename, VirtualDiskSafeHandle handle) : base(filename, handle) { }

        public static VirtualHardDisk Open(string filename,
            VirtualDiskAccessMasks accessMask = VirtualDiskAccessMasks.All,
            OpenVirtualDiskFlags flag = OpenVirtualDiskFlags.None, uint rwDepth = 0)
        {
            var storageType = new VirtualStorageType();

            var parameters = new OpenVirtualDiskParameters
            {
                Version = OpenVirtualDiskVersions.Version1,
                Version1 = new OpenVirtualDiskParametersVersion1 { RWDepth = rwDepth}
            };

            var handle = VirtualDiskCore.OpenVirtualDisk(storageType, filename, accessMask, flag, parameters);

            var vhd = new VirtualHardDisk(filename, handle);

            // ReSharper disable once InvertIf
            if (vhd.VirtualStorageType.DeviceId == VirtualStorageDeviceTypes.Iso)
            {
                vhd.Dispose();
                throw new NotSupportedException("This class does not support ISO files.");
            }

            return vhd;
        }

        public static VirtualHardDisk Create(string filename, ulong maximumSize,
            VirtualStorageDeviceTypes deviceType = VirtualStorageDeviceTypes.Vhd,
            VirtualDiskAccessMasks mask = VirtualDiskAccessMasks.All, RawSecurityDescriptor securityDescriptor = null,
            CreateVirtualDiskFlags flags = CreateVirtualDiskFlags.None, uint providerSpecificFlags = 0,
            Guid uniqueId = default(Guid), uint blockSizeInBytes = 0, uint sectorSizeInBytes = 0,
            string parentPath = null, string sourcePath = null, Overlapped overlapped = null)
        {
            if ()

            var storageType = new VirtualStorageType {DeviceId = deviceType};

            var parameters = new CreateVirtualDiskParameters
            {
                Version = CreateVirtualDiskVersions.Version1,
                Version1 = new CreateVirtualDiskParametersVersion1
                {
                    UniqueId = uniqueId,
                    MaximumSize = maximumSize,
                    BlockSizeInBytes = blockSizeInBytes,
                    SectorSizeInBytes = sectorSizeInBytes,
                    ParentPath = parentPath,
                    SourcePath = sourcePath
                }
            };

            var handle = VirtualDiskCore.CreateVirtualDisk(storageType, filename, mask, securityDescriptor, flags,
                providerSpecificFlags, parameters, overlapped);

            var vhd = new VirtualHardDisk(filename, handle);

            // ReSharper disable once InvertIf
            if (vhd.VirtualStorageType.DeviceId == VirtualStorageDeviceTypes.Iso)
            {
                vhd.Dispose();
                throw new NotSupportedException("This class does not support ISO files.");
            }

            return vhd;
        }


    }
}