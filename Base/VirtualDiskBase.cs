using System;
using System.Security.Permissions;
using VirtualDiskInterop;
using VirtualDiskManager.Core;

namespace VirtualDiskManager.Base
{
    public abstract class VirtualDiskBase : IDisposable
    {
        protected readonly VirtualDiskSafeHandle Handle;

        internal VirtualDiskBase(string filename, VirtualDiskSafeHandle safeHandle)
        {
            Filename = filename;
            Handle = safeHandle;
        }

        public string Filename { get; }

        public virtual void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        [SecurityPermission(SecurityAction.Demand, UnmanagedCode = true)]
        protected virtual void Dispose(bool disposing)
        {
            if (Handle != null && !Handle.IsInvalid)
            {
                // Free the handle
                Handle.Dispose();
            }
        }

        public virtual void Close()
        {
            if (!Handle.IsClosed)
            {
                Handle.Close();
            }
        }

        #region Detach method
        public virtual void Detach()
        {
            Detach(0);
        }

        public virtual void Detach(uint providerSpecificFlags)
        {
            VirtualDiskCore.DetachVirtualDisk(Handle, DetachVirtualDiskFlags.None, providerSpecificFlags);
        }
        #endregion

        #region Properties
        private GetVirtualDiskInfo GetVirtualDiskInfo(GetVirtualDiskInfoVersions infoVersion)
        {
            if (Handle == null || Handle.IsClosed)
            {
                return new GetVirtualDiskInfo();
            }

            var diskInfo = new GetVirtualDiskInfo { Version = infoVersion };
            return VirtualDiskCore.GetVirtualDiskInformation(Handle, diskInfo);
        }

        public virtual GetVirtualDiskInfoSize Size
        {
            get
            {
                var info = GetVirtualDiskInfo(GetVirtualDiskInfoVersions.Size);
                return info.Size;
            }
        }

        public virtual VirtualStorageType VirtualStorageType
        {
            get
            {
                var info = GetVirtualDiskInfo(GetVirtualDiskInfoVersions.VirtualStorageType);
                return info.VirtualStorageType;
            }
        }
        #endregion
    }
}