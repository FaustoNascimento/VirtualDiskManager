using System;
using System.Linq;
using System.Security.AccessControl;
using VirtualDiskInterop;
using VirtualDiskManager.Base;
using VirtualDiskManager.Core;
using VirtualDiskManager.Enums.Iso9660;

namespace VirtualDiskManager
{
    public class Iso9660 : VirtualDiskBase
    {
        private Iso9660(string filename, VirtualDiskSafeHandle handle) : base(filename, handle) { }

        #region Open method
        public static Iso9660 Open(string filename)
        {
            // Since no mask is specified, we open the disk with *all* valid ISO masks.
            // As valid values may change and we don't want to hardcode it, 
            // we loop through all values on the enum
            var accessMask =
                Enum.GetValues(typeof(Iso9660VirtualDiskAccessMask))
                    .Cast<Iso9660VirtualDiskAccessMask>()
                    .Aggregate((Iso9660VirtualDiskAccessMask) 0, (current, mask) => current | mask);

            return Open(filename, accessMask);
        }

        public static Iso9660 Open(string filename, Iso9660VirtualDiskAccessMask accessMask)
        {
            var storageType = new VirtualStorageType();

            var parameters = new OpenVirtualDiskParameters
            {
                Version = OpenVirtualDiskVersions.Version1
            };

            var handle = VirtualDiskCore.OpenVirtualDisk(storageType, filename, (VirtualDiskAccessMasks) accessMask,
                OpenVirtualDiskFlags.None, parameters);
            
            var iso = new Iso9660(filename, handle);

            // ReSharper disable once InvertIf
            if (iso.VirtualStorageType.DeviceId != VirtualStorageDeviceTypes.Iso)
            {
                iso.Dispose();
                throw new NotSupportedException("This class only supports ISO files.");
            }

            return iso;
        }
        #endregion

        #region Attach method
        public void Attach()
        {
            var parameters = new AttachVirtualDiskParameters {Version = AttachVirtualDiskVersions.Version1};

            Attach(null, Iso9660AttachVirtualDiskFlags.ReadOnly, 0, null);
        }

        public void Attach(Iso9660AttachVirtualDiskFlags flags)
        {
            Attach(null, flags, 0, null);
        }

        public void Attach(RawSecurityDescriptor securityDescriptor, Iso9660AttachVirtualDiskFlags flags)
        {
            Attach(securityDescriptor, flags, 0, null);
        }

        public void Attach(RawSecurityDescriptor securityDescriptor, Iso9660AttachVirtualDiskFlags flags,
            uint providerSpecificFlags)
        {
            Attach(securityDescriptor, flags, providerSpecificFlags, null);
        }

        public void Attach(RawSecurityDescriptor securityDescriptor, Iso9660AttachVirtualDiskFlags flags,
            uint providerSpecificFlags, Overlapped overlapped)
        {
            var parameters = new AttachVirtualDiskParameters { Version = AttachVirtualDiskVersions.Version1 };

            VirtualDiskCore.AttachVirtualDisk(Handle, securityDescriptor, (AttachVirtualDiskFlags) flags,
                providerSpecificFlags, parameters, overlapped);
        }
        #endregion
    }
}