using Assets.Scripts.GameCore.HostComponent;
using Assets.Scripts.GameCore.Managers;
using Assets.Scripts.UI.Panels;
using HarmonyLib;
using UnityEngine;
using GameLogic;
using MelonLoader;
using Assets.Scripts.PeroTools.Commons;

namespace FC_AP
{
    [HarmonyPatch(typeof(PnlStage), "Awake")]
    internal class Patch
    {
        private static unsafe void Postfix(PnlStage __instance)
        {
            GameObject vSelect = null;
            foreach (Il2CppSystem.Object @object in __instance.transform.parent.parent.Find("Forward"))
            {
                Transform transform = @object.Cast<Transform>();
                if (transform.name == "PnlVolume")
                {
                    vSelect = transform.gameObject;
                }
            }
            fixed (bool* fc_apEnabled = &Save.Settings.FC_APEnabled)
            {
                if (ToggleManager.FC_APToggle == null && vSelect != null)
                {
                    GameObject fc_apToggle = UnityEngine.Object.Instantiate<GameObject>(vSelect.transform.Find("LogoSetting").Find("Toggles").Find("TglOn").gameObject, __instance.transform);
                    ToggleManager.FC_APToggle = fc_apToggle;
                    ToggleManager.SetupToggle(ToggleManager.FC_APToggle, "FC AP Indicator Toggle", new Vector3(3.5f, -5f, 100f), fc_apEnabled, "FC/AP On/Off");
                }
            }
            fixed (bool* restartEnabled = &Save.Settings.RestartEnabled)
            {
                if (ToggleManager.RestartToggle == null && vSelect != null)
                {
                    GameObject restartToggle = UnityEngine.Object.Instantiate<GameObject>(vSelect.transform.Find("LogoSetting").Find("Toggles").Find("TglOn").gameObject, __instance.transform);
                    ToggleManager.RestartToggle = restartToggle;
                    ToggleManager.SetupToggle(ToggleManager.RestartToggle, "Restart Toggle", new Vector3(6.5f, -5f, 100f), restartEnabled, "Auto Restart On/Off");
                }
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
    /*[HarmonyPatch(typeof(BattleEnemyManager), "SetPlayResult")]
    internal class SetPlayResultPatch
    {
        private static void Postfix(int idx, byte result, bool isMulStart = false, bool isMulEnd = false, bool isLeft = false)
        {
            if (result == 0)
            {
                Indicator.CollectableNoteMiss++;
            }
            if (result == 1)
            {
                Indicator.GhostMiss++;
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
                Indicator.CollectableNoteMiss++;
            }
        }
    }*/

    [HarmonyPatch(typeof(GameMissPlay), "MissCube")]
    internal class MissCubePatch
    {
        private static void Postfix(int idx, decimal currentTick)
        {
            int result = Singleton<BattleEnemyManager>.instance.GetPlayResult(idx);
            if (result == 0)
            {
                Indicator.CollectableNoteMiss++;
            }
            if (result == 1)
            {
                Indicator.GhostMiss++;
            }
        }
    }
}