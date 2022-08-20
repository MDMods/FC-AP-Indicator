using Assets.Scripts.GameCore.HostComponent;
using Assets.Scripts.PeroTools.Commons;
using Assets.Scripts.UI.Panels;
using GameLogic;
using HarmonyLib;
using UnityEngine;

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
                    ToggleManager.SetupToggle(ToggleManager.FC_APToggle, "FC AP Indicator Toggle", new Vector3(3.45f, -5.05f, 100f), fc_apEnabled, "FC/AP On/Off");
                }
            }
            fixed (bool* chartReviewEnabled = &Save.Settings.ChartReviewEnabled)
            {
                if (ToggleManager.ChartReviewToggle == null && vSelect != null)
                {
                    GameObject chartReviewToggle = UnityEngine.Object.Instantiate<GameObject>(vSelect.transform.Find("LogoSetting").Find("Toggles").Find("TglOn").gameObject, __instance.transform);
                    ToggleManager.ChartReviewToggle = chartReviewToggle;
                    ToggleManager.SetupToggle(ToggleManager.ChartReviewToggle, "Chart Review Toggle", new Vector3(6.4f, -5.05f, 100f), chartReviewEnabled, "Chart Review On/Off");
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