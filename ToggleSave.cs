using MelonLoader;

namespace FC_AP
{
    public class ToggleSave
    {
        public static bool FC_APEnabled
        {
            get
            {
                return fc_apEnabled.Value;
            }
            set
            {
                fc_apEnabled.Value = value;
            }
        }

        public static bool RestartEnabled
        {
            get
            {
                return restartEnabled.Value;
            }
            set
            {
                restartEnabled.Value = value;
            }
        }

        public static void Load()
        {
            ToggleCategory = MelonPreferences.CreateCategory("FC AP");
            ToggleCategory.SetFilePath("UserData/FC AP.cfg", true);
            fc_apEnabled = MelonPreferences.CreateEntry<bool>("FC AP", "fc_apEnabled", false, null, "Whether the FC AP indicator is enabled.", false, false, null);
            restartEnabled = MelonPreferences.CreateEntry<bool>("Restart", "restartEnabled", false, null, "Whether the auto restart is enabled.", false, false, null);
        }

        public static MelonPreferences_Category ToggleCategory;

        public static MelonPreferences_Entry<bool> fc_apEnabled;

        public static MelonPreferences_Entry<bool> restartEnabled;
    }
}