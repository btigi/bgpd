﻿﻿using System.IO;
using static bgpd.Resources.BIFResourceEntry;

namespace bgpd.Resources
{
    public class BIFFV1FileEntry
    {
        public BIFFV1FileEntry(BinaryReader reader)
        {
            this.ResourceLocator = reader.ReadInt32() & 0xfffff;
            this.Offset = reader.ReadInt32();
            this.Size = reader.ReadInt32();
            this.Type = reader.ReadInt16();
            this.Ext = (Extension)Type;
            reader.ReadInt16();
        }

        public override string ToString()
        {
            return $"{ResourceLocator}, {Ext}, {Offset}, {Size}";
        }

        public int ResourceLocator { get; }
        public int Offset { get; }
        public int Size { get; }
        public short Type { get; }
        public Extension Ext { get; }
    }
}