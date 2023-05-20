using System.IO;
using Tomlet;
using Tomlet.Attributes;

namespace FC_AP;

internal static class Save
{
    internal static Data Settings { get; private set; }

    public static void Load()
    {
        if (!File.Exists(Path.Combine("UserData", "FC AP.cfg")))
        {
            var defaultConfig = TomletMain.TomlStringFrom(new Data(true, true, true));
            File.WriteAllText(Path.Combine("UserData", "FC AP.cfg"), defaultConfig);
        }

        var data = File.ReadAllText(Path.Combine("UserData", "FC AP.cfg"));
        try
        {
            Settings = TomletMain.To<Data>(data);
        }
        catch
        {
            File.Delete(Path.Combine("UserData", "FC AP.cfg"));
            Load();
        }
    }
}

public class Data
{
    [TomlPrecedingComment("Whether delete FC indicator when missing a collectable notes")]
    internal readonly bool CollectableMissEnabled;

    [TomlPrecedingComment("Whether delete FC indicator when missing a ghost")]
    internal readonly bool GhostMissEnabled;

    [TomlPrecedingComment("Whether the FC AP indicator is enabled")]
    internal bool IndicatorEnabled;

    public Data()
    {
    }

    internal Data(bool ghostMissEnabled, bool collectableMissEnabled, bool indicatorEnabled)
    {
        CollectableMissEnabled = collectableMissEnabled;
        GhostMissEnabled = ghostMissEnabled;
        IndicatorEnabled = indicatorEnabled;
    }
}