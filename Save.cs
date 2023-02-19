using System.IO;
using Tomlet;
using Tomlet.Attributes;

namespace FC_AP;

internal static class Save
{
    private static readonly Data Default = new(true, true, true);
    internal static Data Settings;

    public static void Load()
    {
        if (!File.Exists(Path.Combine("UserData", "FC AP.cfg")))
        {
            var defaultConfig = TomletMain.TomlStringFrom(Default);
            File.WriteAllText(Path.Combine("UserData", "FC AP.cfg"), defaultConfig);
        }

        var data = File.ReadAllText(Path.Combine("UserData", "FC AP.cfg"));
        Settings = TomletMain.To<Data>(data);
    }
}

internal struct Data
{
    [TomlPrecedingComment("Whether the FC AP indicator is enabled")]
    internal bool FC_APEnabled;

    [TomlPrecedingComment("Whether delete FC indicator when missing a ghost")]
    internal bool GhostMissEnabled;

    [TomlPrecedingComment("Whether delete FC indicator when missing a collectable notes")]
    internal bool CollectableMissEnabled;

    internal Data(bool fc_apEnabled, bool ghostMissEnabled, bool collectableMissEnabled)
    {
        FC_APEnabled = fc_apEnabled;
        GhostMissEnabled = ghostMissEnabled;
        CollectableMissEnabled = collectableMissEnabled;
    }
}