using System;
using System.Linq;
using System.Security.AccessControl;
using VirtualDiskInterop;
using VirtualDiskManager.Base;
using VirtualDiskManager.Core;
using VirtualDiskManager.Enums.Iso9660;

namespace VirtualDiskManager
{
    public class Iso : VirtualDiskBase
    {
        private Iso(string filename, VirtualDiskSafeHandle handle) : base(filename, handle) { }

        public static Iso Open(string filename)
        {
            // Since no mask is specified, we open the disk with *all* valid ISO masks.
            // As valid values may change and we don't want to hardcode it, 
            // we loop through all values on the enum
            var accessMask =
                Enum.GetValues(typeof(IsoVirtualDiskAccessMasks))
                    .Cast<IsoVirtualDiskAccessMasks>()
                    .Aggregate((IsoVirtualDiskAccessMasks) 0, (current, mask) => current | mask);

            return Open(filename, accessMask);
        }

        public static Iso Open(string filename, IsoVirtualDiskAccessMasks accessMask)
        {
            var storageType = new VirtualStorageType();

            var parameters = new OpenVirtualDiskParameters
            {
                Version = OpenVirtualDiskVersions.Version1
            };

            var handle = VirtualDiskCore.OpenVirtualDisk(storageType, filename, (VirtualDiskAccessMasks) accessMask,
                OpenVirtualDiskFlags.None, parameters);
            
            var iso = new Iso(filename, handle);

            // ReSharper disable once InvertIf
            if (iso.VirtualStorageType.DeviceId != VirtualStorageDeviceTypes.Iso)
            {
                iso.Dispose();
                throw new NotSupportedException("This class only supports ISO files.");
            }

            return iso;
        }

        public void Attach(IsoAttachVirtualDiskFlags flags = IsoAttachVirtualDiskFlags.ReadOnly, RawSecurityDescriptor securityDescriptor = null,
            uint providerSpecificFlags = 0, Overlapped overlapped = null)
        {
            var parameters = new AttachVirtualDiskParameters { Version = AttachVirtualDiskVersions.Version1 };

            VirtualDiskCore.AttachVirtualDisk(Handle, securityDescriptor, (AttachVirtualDiskFlags) flags,
                providerSpecificFlags, parameters, overlapped);
        }
    }
}