using System;
using VirtualDiskInterop;
using VirtualDiskManager.Base;
using VirtualDiskManager.Core;

namespace VirtualDiskManager
{
    public class VirtualHardDisk2 : VirtualHardDiskBase
    {
        private VirtualHardDisk2(string filename, VirtualDiskSafeHandle handle) : base(filename, handle) { }

        public static VirtualHardDisk2 Open(string filename, OpenVirtualDiskFlags flag = OpenVirtualDiskFlags.None,
            bool getInfoOnly = false, bool readOnly = false, Guid resiliencyGuid = default(Guid))
        {
            var storageType = new VirtualStorageType();

            var parameters = new OpenVirtualDiskParameters
            {
                Version = OpenVirtualDiskVersions.Version2,
                Version2 =
                    new OpenVirtualDiskParametersVersion2
                    {
                        GetInfoOnly = getInfoOnly,
                        ReadOnly = readOnly,
                        ResiliencyGuid = resiliencyGuid
                    }
            };

            // When using OpenVirtulaDiskVersions.Version2, VirtualDiskAccessMasks *must* be set to None
            var handle = VirtualDiskCore.OpenVirtualDisk(storageType, filename, VirtualDiskAccessMasks.None, flag, parameters);

            var vhd = new VirtualHardDisk2(filename, handle);

            // ReSharper disable once InvertIf
            if (vhd.VirtualStorageType.DeviceId == VirtualStorageDeviceTypes.Iso)
            {
                vhd.Dispose();
                throw new NotSupportedException("This class does not support ISO files.");
            }

            return vhd;
        }

        #region Properties
        public virtual bool Is4KAligned
        {
            get
            {
                var info = GetVirtualDiskInfo(GetVirtualDiskInfoVersions.Is4kAligned);
                return info.Is4kAligned;
            }
        }

        public virtual bool IsLoaded
        {
            get
            {
                var info = GetVirtualDiskInfo(GetVirtualDiskInfoVersions.IsLoaded);
                return info.IsLoaded;
            }
        }

        public virtual GetVirtualDiskInfoPhysicalDisk PhysicalDisk
        {
            get
            {
                var info = GetVirtualDiskInfo(GetVirtualDiskInfoVersions.PhysicalDisk);
                return info.PhysicalDisk;
            }
        }

        public virtual uint PhysicalSectorSize
        {
            get
            {
                var info = GetVirtualDiskInfo(GetVirtualDiskInfoVersions.VhdPhysicalSectorSize);
                return info.VhdPhysicalSectorSize;
            }
        }

        public virtual ulong SmallestSafeVirtualSize
        {
            get
            {
                var info = GetVirtualDiskInfo(GetVirtualDiskInfoVersions.SmallestSafeVirtualSize);
                return info.SmallestSafeVirtualSize;
            }
        }

        public virtual uint FragmentationPercentage
        {
            get
            {
                var info = GetVirtualDiskInfo(GetVirtualDiskInfoVersions.Fragmentation);
                return info.FragmentationPercentage;
            }
        }

        public virtual Guid VirtualDiskId
        {
            get
            {
                var info = GetVirtualDiskInfo(GetVirtualDiskInfoVersions.VirtualDiskID);
                return info.VirtualDiskId;
            }
        }
        #endregion
    }
}