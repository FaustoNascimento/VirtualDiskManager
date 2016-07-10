using System;
using System.Security.AccessControl;
using VirtualDiskInterop;
using VirtualDiskManager.Core;

namespace VirtualDiskManager.Base
{
    public abstract class VirtualHardDiskBase : VirtualDiskBase
    {
        protected VirtualHardDiskBase(string filename, VirtualDiskSafeHandle handle) : base(filename, handle) { }

        public virtual void Attach(AttachVirtualDiskFlags flags = AttachVirtualDiskFlags.None,
            RawSecurityDescriptor securityDescriptor = null,
            uint providerSpecificFlags = 0, Overlapped overlapped = null)
        {
            var parameters = new AttachVirtualDiskParameters { Version = AttachVirtualDiskVersions.Version1 };

            VirtualDiskCore.AttachVirtualDisk(Handle, securityDescriptor, flags, providerSpecificFlags, parameters,
                overlapped);
        }

        #region Properties
        public virtual Guid Identifier
        {
            get
            {
                var info = GetVirtualDiskInfo(GetVirtualDiskInfoVersions.Size);
                return info.Identifier;
            }
        }

        public virtual string ParentLocation
        {
            get
            {
                if (!HasParent)
                {
                    return null;
                }

                var info = GetVirtualDiskInfo(GetVirtualDiskInfoVersions.ParentLocation);
                return !info.ParentLocation.ParentResolved ? string.Empty : info.ParentLocation.ParentLocations[0];
            }
        }

        public virtual Guid? ParentIdentifier
        {
            get
            {
                if (!HasParent)
                {
                    return null;
                }

                var info = GetVirtualDiskInfo(GetVirtualDiskInfoVersions.ParentIdentifier);
                return info.ParentIdentifier;
            }
        }

        public virtual uint? ParentTimestamp
        {
            get
            {
                if (!HasParent)
                {
                    return null;
                }

                var info = GetVirtualDiskInfo(GetVirtualDiskInfoVersions.ParentTimestamp);
                return info.ParentTimestamp;
            }
        }

        public virtual uint ProviderSubtype
        {
            get
            {
                var info = GetVirtualDiskInfo(GetVirtualDiskInfoVersions.ProviderSubtype);
                return info.ProviderSubtype;
            }
        }

        public virtual bool HasParent => ProviderSubtype == 4;
        #endregion
    }
}