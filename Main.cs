using MelonLoader;
using UnhollowerRuntimeLib;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace FC_AP
{
    public class Main : MelonMod
    {
        //public static Main instance;

        public override void OnApplicationStart()
        {
            //instance = this;
            ToggleSave.Load();
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
                Indicator.Set = false;
                Indicator.GhostMiss = false;
                Indicator.CollectableNoteMiss = false;
                Indicator.IsAP = true;
                Indicator.IsTrueFC = true;
                Indicator.IsFC = true;
                Indicator.IsRestarted = false;
                if (Indicator.font != null)
                {
                    Addressables.Release(Indicator.font);
                }
            }
        }

        /*public void Log(object log)
        {
            LoggerInstance.Msg(log);
        }*/
    }
}