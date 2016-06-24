using System;
using VirtualDiskInterop;

namespace VirtualDiskManager.Enums.Iso9660
{
    [Flags]
    public enum Iso9660AttachVirtualDiskFlags
    {
        ReadOnly = AttachVirtualDiskFlags.ReadOnly,

        NoDriveLetter = AttachVirtualDiskFlags.NoDriveLetter,

        PermanentLifetime = AttachVirtualDiskFlags.PermanentLifetime
    }
}