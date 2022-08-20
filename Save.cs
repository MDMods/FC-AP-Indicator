using System.IO;
using Tomlet;
using Tomlet.Attributes;

namespace FC_AP
{
    internal class Save
    {
        private static Data Default = new Data(true, false, true, true);
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

        [TomlPrecedingComment("Whether the chart review is enabled")]
        internal bool ChartReviewEnabled;

        [TomlPrecedingComment("Whether delete FC indicator when missing a ghost")]
        internal bool GhostMissEnabled;

        [TomlPrecedingComment("Whether delete FC indicator when missing a collectable notes")]
        internal bool CollectableMissEnabled;

        internal Data(bool fc_apEnabled, bool chartReviewEnabled, bool ghostMiss, bool collectableMiss)
        {
            FC_APEnabled = fc_apEnabled;
            ChartReviewEnabled = chartReviewEnabled;
            GhostMissEnabled = ghostMiss;
            CollectableMissEnabled = collectableMiss;
        }
    }
}