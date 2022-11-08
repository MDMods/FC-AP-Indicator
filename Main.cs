using Il2CppSystem.IO;
using MelonLoader;
using Tomlet;
using UnhollowerRuntimeLib;
using UnityEngine;
using UnityEngine.AddressableAssets;

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
                if (Indicator.font != null)
                {
                    Addressables.Release(Indicator.font);
                }
            }
        }

        public override void OnUpdate()
        {
            if (!GameObject.Find("PnlOption") && ToggleManager.FC_APToggle != null)
            {
                ToggleManager.FC_APToggle.SetActive(false);
                ToggleManager.ChartReviewToggle.SetActive(false);
            }
            else if (GameObject.Find("PnlOption") && ToggleManager.FC_APToggle != null)
            {
                ToggleManager.FC_APToggle.SetActive(true);
                ToggleManager.ChartReviewToggle.SetActive(true);
            }
        }
    }
}