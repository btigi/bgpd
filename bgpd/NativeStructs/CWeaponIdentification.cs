﻿using System;
using WinApiBindings;

namespace bgpd.NativeStructs
{
    public class CWeaponIdentification
    {
        public CWeaponIdentification(IntPtr addr)
        {
            addr = WinAPIBindings.FindDMAAddy(addr);
            this.Type = WinAPIBindings.ReadUInt32(addr);
            this.Flags = WinAPIBindings.ReadUInt32(addr + 0x04);
            this.FlagMask = WinAPIBindings.ReadUInt32(addr + 0x08);
            this.Attributes = WinAPIBindings.ReadUInt32(addr + 0x0C);
        }

        public UInt32 Type { get; }
        public UInt32 Flags { get; }
        public UInt32 FlagMask { get; }
        public UInt32 Attributes { get; }
    }
}