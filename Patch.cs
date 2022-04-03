using Assets.Scripts.GameCore.HostComponent;
using Assets.Scripts.GameCore.Managers;
using Assets.Scripts.UI.Panels;
using HarmonyLib;
using UnityEngine;

namespace FC_AP
{
    [HarmonyPatch(typeof(PnlStage), "Awake")]
    internal class Patch
    {
        private static void Postfix(PnlStage __instance)
        {
            ToggleManager.stage = __instance;
            bool fc_apEnabled = ToggleSave.FC_APEnabled;
            bool restartEnabled = ToggleSave.RestartEnabled;
            if (fc_apEnabled)
            {
                ToggleManager.FC_AP_On();
            }
            if (restartEnabled)
            {
                ToggleManager.Restart_On();
            }
            GameObject vSelect = null;
            foreach (Il2CppSystem.Object @object in __instance.transform.parent.parent.Find("Forward"))
            {
                Transform transform = @object.Cast<Transform>();
                if (transform.name == "PnlVolume")
                {
                    vSelect = transform.gameObject;
                }
            }
            ToggleManager.vSelect = vSelect;
            if (ToggleManager.FC_APToggle == null && ToggleManager.vSelect != null)
            {
                GameObject fc_apToggle = UnityEngine.Object.Instantiate<GameObject>(ToggleManager.vSelect.transform.Find("LogoSetting").Find("Toggles").Find("TglOn").gameObject, __instance.transform);
                ToggleManager.FC_APToggle = fc_apToggle;
                ToggleManager.SetupFC_APToggle();
            }
            if (ToggleManager.RestartToggle == null && ToggleManager.vSelect != null)
            {
                GameObject restartToggle = UnityEngine.Object.Instantiate<GameObject>(ToggleManager.vSelect.transform.Find("LogoSetting").Find("Toggles").Find("TglOn").gameObject, __instance.transform);
                ToggleManager.RestartToggle = restartToggle;
                ToggleManager.SetupRestartToggle();
            }
        }
    }

    [HarmonyPatch(typeof(VolumeSelect), MethodType.Constructor)]
    internal class VolumeCtorPatch
    {
        private static void Postfix(VolumeSelect __instance)
        {
        }
    }

    // ghost miss
    [HarmonyPatch(typeof(BattleEnemyManager), "SetPlayResult")]
    internal class SetPlayResultPatch
    {
        private static void Postfix(int idx, byte result, bool isMulStart = false, bool isMulEnd = false, bool isLeft = false)
        {
            if (result == 1)
            {
                Main.HiddenMiss = true;
            }
        }
    }

    // collectable note miss
    [HarmonyPatch(typeof(StatisticsManager), "OnNoteResult")]
    internal class OnNoteResultPatch
    {
        private static void Postfix(int result)
        {
            if (result == 0)
            {
                Main.HiddenMiss = true;
            }
        }
    }
}