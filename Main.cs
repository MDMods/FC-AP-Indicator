using MelonLoader;
using UnhollowerRuntimeLib;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace FC_AP
{
    public class Main : MelonMod
    {
        public override void OnApplicationStart()
        {
            Save.Load();
            LoggerInstance.Msg("FC/AP indicator is loaded!");
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
                Indicator.SetAP = false;
                Indicator.SetFC = false;
                Indicator.SetMiss = false;
                Indicator.Restarted = false;
                Indicator.GreatNum = 0;
                Indicator.GhostMiss = 0;
                Indicator.CollectableNoteMiss = 0;
                if (Indicator.font != null)
                {
                    Addressables.Release(Indicator.font);
                }
            }
        }
    }
}