using System.IO;
using Tomlet;
using Tomlet.Attributes;

namespace FC_AP
{
    internal class Save
    {
        private static Data Default = new Data(true, false, true, true, true, true);
        internal static Data Settings;

        public static void Load()
        {
            if (!File.Exists(Path.Combine("UserData", "FC AP.cfg")))
            {
                string defaultConfig = TomletMain.TomlStringFrom(Default);
                File.WriteAllText(Path.Combine("UserData", "FC AP.cfg"), defaultConfig);
            }
            string Datas = File.ReadAllText(Path.Combine("UserData", "FC AP.cfg"));
            Settings = TomletMain.To<Data>(Datas);
        }
    }

    internal struct Data
    {
        [TomlPrecedingComment("Whether the FC AP indicator is enabled")]
        internal bool FC_APEnabled;

        [TomlPrecedingComment("Whether the auto restart is enabled")]
        internal bool RestartEnabled;

        [TomlPrecedingComment("Whether auto restart when you hit a great")]
        internal bool GreatRestartEnabled;

        [TomlPrecedingComment("Whether auto restart when you get a miss")]
        internal bool MissRestartEnabled;

        [TomlPrecedingComment("Whether delete FC indicator when missing a ghost")]
        internal bool GhostMissEnabled;

        [TomlPrecedingComment("Whether delete FC indicator when missing a collectable notes")]
        internal bool CollectableMissEnabled;

        internal Data(bool fc_apEnabled, bool restartEnabled, bool greatRestart, bool missRestart, bool ghostMiss, bool collectableMiss)
        {
            FC_APEnabled = fc_apEnabled;
            RestartEnabled = restartEnabled;
            GreatRestartEnabled = greatRestart;
            MissRestartEnabled = missRestart;
            GhostMissEnabled = ghostMiss;
            CollectableMissEnabled = collectableMiss;
        }
    }
}