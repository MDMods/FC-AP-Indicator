using Il2CppSystem.IO;
using MelonLoader;
using MuseDashMirror;
using Tomlet;
using UnhollowerRuntimeLib;
using UnityEngine;

namespace FC_AP
{
    public class Main : MelonMod
    {
        public override void OnInitializeMelon()
        {
            Save.Load();
            LoggerInstance.Msg("FC/AP indicator is loaded!");
        }

        public override void OnApplicationQuit()
        {
            File.WriteAllText(Path.Combine("UserData", "FC AP.cfg"), TomletMain.TomlStringFrom(Save.Settings));
        }

        public override void OnSceneWasLoaded(int buildIndex, string sceneName)
        {
            if (sceneName == "GameMain")
            {
                ClassInjector.RegisterTypeInIl2Cpp<Indicator>();
                GameObject gameObject = new GameObject("Indicator");
                Object.DontDestroyOnLoad(gameObject);
                gameObject.AddComponent<Indicator>();
            }
            else
            {
                Indicator.ObjectDisabled = false;
                Indicator.GreatNum = 0;
                Indicator.CurrentMissNum = 0;
                Indicator.MissNum = 0;
                Indicator.GhostMiss = 0;
                Indicator.CollectableNoteMiss = 0;
                UICreate.UnloadFonts();
            }
        }

        public override void OnUpdate()
        {
            if (!GameObject.Find("PnlOption") && Patch.FC_APToggle != null)
            {
                Patch.FC_APToggle.SetActive(false);
                Patch.ChartReviewToggle.SetActive(false);
            }
            else if (GameObject.Find("PnlOption") && Patch.FC_APToggle != null)
            {
                Patch.FC_APToggle.SetActive(true);
                Patch.ChartReviewToggle.SetActive(true);
            }
        }
    }
}