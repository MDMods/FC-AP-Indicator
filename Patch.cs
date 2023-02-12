using Assets.Scripts.GameCore.HostComponent;
using Assets.Scripts.PeroTools.Commons;
using Assets.Scripts.UI.Panels;
using FormulaBase;
using GameLogic;
using HarmonyLib;
using MuseDashMirror.UICreate;
using UnityEngine;
using UnityEngine.UI;

namespace FC_AP
{
    internal static class Patch
    {
        internal static GameObject FC_APToggle { get; set; }

        internal static unsafe void SwitchLanguagesPostfix()
        {
            fixed (bool* fc_apEnabled = &Save.Settings.FC_APEnabled)
            {
                FC_APToggle.transform.Find("Txt").GetComponent<Text>().text = "FC/AP On/Off";
            }
        }

        internal static unsafe void PnlMenuPostfix(PnlMenu __instance)
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
                if (FC_APToggle == null && vSelect != null)
                {
                    FC_APToggle = ToggleCreate.CreatePnlMenuToggle("FC AP Indicator Toggle", fc_apEnabled, "FC/AP On/Off");
                }
            }
        }
    }

    // Hold miss
    [HarmonyPatch(typeof(BattleEnemyManager), "SetPlayResult")]
    internal static class SetPlayResultPatch
    {
        private static void Postfix(int idx, byte result, bool isMulStart = false, bool isMulEnd = false, bool isLeft = false)
        {
            MusicData musicDataByIdx = Singleton<StageBattleComponent>.instance.GetMusicDataByIdx(idx);
            if (result == 1 && musicDataByIdx.noteData.type == 3)
            {
                Indicator.CurrentMissNum++;
            }
        }
    }

    [HarmonyPatch(typeof(GameMissPlay), "MissCube")]
    internal static class MissCubePatch
    {
        private static void Postfix(int idx, decimal currentTick)
        {
            int result = Singleton<BattleEnemyManager>.instance.GetPlayResult(idx);
            MusicData musicDataByIdx = Singleton<StageBattleComponent>.instance.GetMusicDataByIdx(idx);
            if (result == 0)
            {
                // air ghost miss
                if (musicDataByIdx.noteData.type == 4)
                {
                    Indicator.GhostMiss++;
                    if (Save.Settings.GhostMissEnabled)
                    {
                        Indicator.CurrentMissNum++;
                    }
                }
                // air collectable note miss
                else if (musicDataByIdx.noteData.type == 7)
                {
                    Indicator.CollectableNoteMiss++;
                    if (Save.Settings.CollectableMissEnabled)
                    {
                        Indicator.CurrentMissNum++;
                    }
                }
                // normal miss
                else if (musicDataByIdx.noteData.type != 2 && !musicDataByIdx.isDouble)
                {
                    Indicator.CurrentMissNum++;
                }
            }
            if (result == 1)
            {
                // ground ghost miss
                if (musicDataByIdx.noteData.type == 4)
                {
                    Indicator.GhostMiss++;
                    if (Save.Settings.GhostMissEnabled)
                    {
                        Indicator.CurrentMissNum++;
                    }
                }
                // ground collectable note miss
                else if (musicDataByIdx.noteData.type == 7)
                {
                    Indicator.CollectableNoteMiss++;
                    if (Save.Settings.CollectableMissEnabled)
                    {
                        Indicator.CurrentMissNum++;
                    }
                }
                // normal miss
                else
                {
                    Indicator.CurrentMissNum++;
                }
            }
        }
    }
}