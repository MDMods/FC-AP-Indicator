using Assets.Scripts.UI.Panels;
using Il2CppSystem.IO;
using MelonLoader;
using MuseDashMirror;
using MuseDashMirror.CommonPatches;
using MuseDashMirror.UICreate;
using System;
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
            PatchEvents.PnlMenuEvent += new Action<PnlMenu>(Patch.PnlMenuPostfix);
            PatchEvents.SwitchLanguagesEvent += new Action(Patch.SwitchLanguagesPostfix);
            PatchEvents.MenuSelectEvent += new Action<int, int, bool>(DisableToggle);
            SceneInfo.EnterGameScene += new Action(RegisterGameObject);
            SceneInfo.ExitGameScene += new Action(Reset);
            LoggerInstance.Msg("FC/AP indicator is loaded!");
        }

        public override void OnDeinitializeMelon()
        {
            File.WriteAllText(Path.Combine("UserData", "FC AP.cfg"), TomletMain.TomlStringFrom(Save.Settings));
        }

        private void RegisterGameObject()
        {
            ClassInjector.RegisterTypeInIl2Cpp<Indicator>();
            GameObject gameObject = new GameObject("Indicator");
            UnityEngine.Object.DontDestroyOnLoad(gameObject);
            gameObject.AddComponent<Indicator>();
        }

        private void Reset()
        {
            Indicator.GreatNum = 0;
            Indicator.CurrentMissNum = 0;
            Indicator.MissNum = 0;
            Indicator.GhostMiss = 0;
            Indicator.CollectableNoteMiss = 0;
            Fonts.UnloadFonts();
        }

        private void DisableToggle(int listIndex, int index, bool isOn)
        {
            if (listIndex == 0 && index == 0 && isOn)
            {
                Patch.FC_APToggle.SetActive(true);
            }
            else
            {
                Patch.FC_APToggle.SetActive(false);
            }
        }
    }
}