using System;
using VirtualDiskInterop;

namespace VirtualDiskManager.Enums.Iso9660
{
    [Flags]
    public enum Iso9660VirtualDiskAccessMask
    {
        AttachReadOnly = VirtualDiskAccessMasks.AttachReadOnly,

        Detach = VirtualDiskAccessMasks.Detach,

        GetInfo = VirtualDiskAccessMasks.GetInfo
    }
}