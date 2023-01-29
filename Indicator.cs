using Assets.Scripts.GameCore.HostComponent;
using Assets.Scripts.PeroTools.Commons;
using System;
using UnityEngine;
using UnityEngine.UI;
using static MuseDashMirror.BattleComponent;
using static MuseDashMirror.PlayerData;
using static MuseDashMirror.UICreate;

namespace FC_AP

{
    internal class Indicator : MonoBehaviour
    {
        // indicator

        internal static int GreatNum { get; set; }
        internal static int MissNum { get; set; }
        internal static int CurrentMissNum { get; set; }
        internal static int GhostMiss { get; set; }
        internal static int CollectableNoteMiss { get; set; }
        private static GameObject AP { get; set; }
        private static GameObject FC { get; set; }
        private static GameObject Miss { get; set; }

        //chart review

        internal static bool ObjectDisabled { get; set; }
        private static bool Set { get; set; }
        private static bool Reset { get; set; }
        private static int LastOffset { get; set; }

        private static int LastCharacter { get; set; } = -1;

        private static int LastElfin { get; set; } = -1;

        public Indicator(IntPtr intPtr) : base(intPtr)
        {
        }

        private void Start()
        {
            LoadFonts();
        }

        private void Update()
        {
            // if indicator is enabled and not set, also is in game
            if (Save.Settings.FC_APEnabled && AP == null && FC == null && Miss == null && IsInGame)
            {
                CreateCanvas("Indicator Canvas", "Camera_2D");
                AP = SetGameObject("AP", Color.yellow, "AP");
            }

            //if AP is set, FC is not set and get a great
            if (AP != null && FC == null && Miss == null && Great != 0)
            {
                Destroy(AP);
                //if not AP then it must be 1 Great
                GreatNum = 1;
                FC = SetGameObject("FC", Blue, "FC " + GreatNum + "G");
            }

            //if AP is set, FC is not set and ghost not count as miss
            if (AP != null && FC == null && GhostMiss != 0)
            {
                Destroy(AP);
                FC = SetGameObject("FC", Blue, "FC");
            }

            //if AP is set, FC is not set and collectable note not count as miss
            if (AP != null && FC == null && CollectableNoteMiss != 0)
            {
                Destroy(AP);
                FC = SetGameObject("FC", Blue, "FC");
            }

            //if AP or FC is set, Miss doesn't set and get normal miss or ghost miss when enabled or collectable note miss when enabled
            if (Miss == null && (AP != null || FC != null) && CurrentMissNum != 0)
            {
                Destroy(AP);
                Destroy(FC);
                MissNum = 1;
                if (GreatNum == 0)
                {
                    Miss = SetGameObject("Miss", Silver, MissNum + "M");
                }
                else
                {
                    Miss = SetGameObject("Miss", Silver, MissNum + "M " + GreatNum + "G");
                }
            }

            //if FC or Miss is set and great number are not correct then +1
            if (GreatNum < Singleton<TaskStageTarget>.instance.m_GreatResult)
            {
                GreatNum++;
                if (FC != null)
                {
                    FC.GetComponent<Text>().text = "FC " + GreatNum + "G";
                }
                if (Miss != null)
                {
                    Miss.GetComponent<Text>().text = MissNum + "M " + GreatNum + "G";
                }
            }

            // if Miss is set and miss number are not correct then +1
            if (MissNum < CurrentMissNum)
            {
                MissNum++;
                if (Miss != null)
                {
                    if (GreatNum == 0)
                    {
                        Miss.GetComponent<Text>().text = MissNum + "M";
                    }
                    else
                    {
                        Miss.GetComponent<Text>().text = MissNum + "M " + GreatNum + "G";
                    }
                }
            }

            //Destroy gameobject in result screen
            if (GameObject.Find("PnlVictory_2D") != null)
            {
                Destroy(AP);
                Destroy(FC);
                Destroy(Miss);
            }

            // if chart review is canceled after enabled
            if (!Save.Settings.ChartReviewEnabled && (LastCharacter != -1 || LastElfin != -1) && !Reset)
            {
                SetCharacter(LastCharacter);
                SetElfin(LastElfin);
                SetOffset(LastOffset);
                SetAutoFever(true);
                Set = false;
                Reset = true;
            }

            // if chart review is enabled
            if (Save.Settings.ChartReviewEnabled)
            {
                Reset = false;

                // if not set character and elfin
                if (!Set)
                {
                    LastCharacter = SelectedCharacterIndex;
                    LastElfin = SelectedElfinIndex;
                    LastOffset = Offset;

                    SetCharacter(2);
                    SetElfin(-1);
                    SetOffset(0);
                    SetAutoFever(false);
                    Set = true;
                }

                // if is in game scene and objects are not disabled
                if (!ObjectDisabled)
                {
                    GameObject.Find("Below").SetActive(false);
                    GameObject.Find("Score").SetActive(false);
                    GameObject.Find("HitPointRoad").SetActive(false);
                    GameObject.Find("HitPointAir").SetActive(false);
                    ObjectDisabled = true;
                }
            }
        }

        private static GameObject SetGameObject(string name, Color color, string text)
        {
            var gameobject = CreateTextGameObject("Indicator Canvas", name, text, TextAnchor.UpperLeft, true, new Vector3(-102.398f, 367.2864f, 0f), new Vector2(960, 216), 100, SnapsTasteFont, color);
            return gameobject;
        }
    }
}