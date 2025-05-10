using System.Diagnostics;
using WinApiBindings;

namespace bgpd
{
    public class ProcessHacker
    {
        public Process Proc;
        private IntPtr hProc;
        private IntPtr moduleBase;
        public List<BGEntity> entityList;
        private List<BGEntity> entityListTemp = [];

        public ResourceManager ResourceManager { get; private set; }

        public void MainLoop()
        {
            entityListTemp.Clear();
            if (Proc.HasExited)
            {
                this.Init();
            }

            var staticEntityList = moduleBase + 0x68D438 + 0x18;
            var test = WinAPIBindings.FindDMAAddy(staticEntityList, []);
            var length = WinAPIBindings.ReadInt32(moduleBase + 0x68D434);
            var marginOfError = 500;

            // First i = 32016
            for (int i = 2000 * 16; i < length * 16 + marginOfError; i += 16)
            {
                try
                {
                    var index = WinAPIBindings.ReadInt32(test + i);
                    if (index == 65535)
                        continue;

                    var entityPtr = WinAPIBindings.FindDMAAddy(test + i + 0x8);

                    var newEntity = new BGEntity(ResourceManager, entityPtr);
                    if (!newEntity.Loaded)
                        continue;

                    if (newEntity.Name2 == "<ERROR>" || newEntity.CurrentHP == 0 || newEntity.EnemyAlly != 2)
                        continue;

                    newEntity.tag = index;
                    newEntity.LoadCREResource();
                    newEntity.LoadTimedEffects();
                    newEntity.LoadDerivedStats();
                    entityListTemp.Add(newEntity);
                }
                catch (Exception ex)
                {
                    Logger.Error("Error during actor list scan!", ex);
                }
            }

            entityList = entityListTemp;
            Thread.Sleep(Configuration.RefreshTimeMS);
        }

        public void Init()
        {
            Logger.Init();
            Logger.Info("Waiting for game process ...");

            while (Process.GetProcessesByName("Baldur").Length == 0)
            {
                Thread.Sleep(3000);
            }

            this.Proc = Process.GetProcessesByName("Baldur")[0];
            Logger.Info("Game process found!");

            Configuration.Init(Process.GetProcessesByName("Baldur")[0]);
            this.ResourceManager = new ResourceManager();
            ResourceManager.Init();
            this.hProc = WinAPIBindings.OpenProcess(WinAPIBindings.ProcessAccessFlags.All, false, Proc.Id);
            this.moduleBase = WinAPIBindings.GetModuleBaseAddress(Proc, "Baldur.exe");
            this.entityList = [];
            Configuration.hProc = hProc;
        }
    }
}