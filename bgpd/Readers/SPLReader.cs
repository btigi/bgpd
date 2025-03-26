﻿using static bgpd.Resources.BIFResourceEntry;

namespace bgpd.Readers
{
    public class SPLReader
    {
        public SPLReader(ResourceManager resourceManager, string splFilename)
        {
            splFilename = splFilename.ToUpper();
            var overrideDir = $"{Configuration.GameFolder}\\override";
            //var overrideSPLs = Directory.Exists(overrideDir) ? Directory.GetFiles(overrideDir, "*.SPL").Select(x => x.ToUpper()).ToList() : new List<string>();
            var filename = $"{Configuration.GameFolder}\\override\\{splFilename}".ToUpper();
            var originalOffset = 0;
            if (!new FileInfo(filename).Exists)
            {
                var bifResourceEntry = resourceManager.SPLResourceEntries.FirstOrDefault(x => x.FullName == splFilename);
                if (bifResourceEntry == null)
                {
                    return;
                }
                var resourceLocator = bifResourceEntry.ResourceLocator;
                var bifFilePath = bifResourceEntry.BiffEntry.FileName;
                var allEntries = resourceManager.GetBIFFReader(bifFilePath.Substring(bifFilePath.LastIndexOf('/') + 1)).BIFFV1FileEntries;
                var biffFileEntry = allEntries[resourceLocator & 0xfffff];
                if (biffFileEntry.Ext != Extension.SPL)
                {
                    throw new Exception();
                }
                filename = $"{Configuration.GameFolder}\\{bifFilePath}";
                originalOffset = biffFileEntry.Offset;
            }

            using BinaryReader reader = new BinaryReader(File.OpenRead(filename));
            reader.BaseStream.Seek(originalOffset + 0x0008, SeekOrigin.Begin);
            this.Name1 = resourceManager.GetStrRefText(reader.ReadInt32());
            this.Name2 = resourceManager.GetStrRefText(reader.ReadInt32());

            reader.BaseStream.Seek(originalOffset + 0x003A, SeekOrigin.Begin);
            this.IconBAM = new string(reader.ReadChars(8));

            reader.BaseStream.Seek(originalOffset + 0x0050, SeekOrigin.Begin);
            this.SpellDescription = resourceManager.GetStrRefText(reader.ReadInt32());
        }
        public string Name1 { get; }
        public string Name2 { get; }
        public string IconBAM { get; }
        public string SpellDescription { get; }
    }
}