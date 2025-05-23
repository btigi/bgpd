﻿using bgpd.Resources;

namespace bgpd.Readers
{
    public class TLKReader
    {
        public TLKReader(ResourceManager resourceManager)
        {
            string locale = Configuration.Locale;
            string tlkFilePath = $"{Configuration.GameFolder}/lang/{locale}/dialog.tlk";

            using BinaryReader reader = new BinaryReader(File.OpenRead(tlkFilePath));
            Signature = new string(reader.ReadChars(4));
            Version = new string(reader.ReadChars(4));
            LanguageId = reader.ReadInt16();
            StrRefCount = reader.ReadInt32();
            OffsetToStringData = reader.ReadInt32();

            reader.BaseStream.Seek(18, SeekOrigin.Begin);
            for (int i = 0; i < StrRefCount; ++i)
            {
                //var entry = new TLKEntry(reader, OffsetToStringData);
                //entry.LoadText(reader);
                //resourceManager.StringRefs[i] = entry;
            }
            resourceManager.StringRefs.Values.ToList().ForEach(x => x.LoadText(reader));
        }

        public string Signature { get; }
        public string Version { get; }
        public short LanguageId { get; }
        public int StrRefCount { get; }
        public int OffsetToStringData { get; private set; }
    }
}