using Assets.Scripts.GameCore.HostComponent;
using Assets.Scripts.PeroTools.Commons;
using Assets.Scripts.PeroTools.Managers;
using FormulaBase;
using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

namespace FC_AP
{
    internal class Indicator : MonoBehaviour
    {
        internal static bool Set { get; set; }
        internal static bool IsAP { get; set; } //FC with ghost and collectable note miss
        internal static bool IsTrueFC { get; set; } //FC with no ghost and collectable note miss
        internal static bool IsFC { get; set; }
        internal static int GhostMiss = 0;
        internal static int CollectableNoteMiss = 0;
        internal static bool IsRestarted { get; set; }
        private static GameObject FC { get; set; }
        private static GameObject AP { get; set; }

        //private static GameObject Miss { get; set; }
        internal static Font font { get; set; }

        private static Color blue = new Color(0 / 255f, 136 / 255f, 255 / 255f, 255 / 255f);

        private static int GreatNum;

        //private static int MissNum;

        public Indicator(IntPtr intPtr) : base(intPtr)
        {
        }

        private void Start()
        {
            var asset = Addressables.LoadAssetAsync<Font>("Snaps Taste");
            font = asset.WaitForCompletion();
        }

        private void Update()
        {
            // if gameobject not created and is in game and FC_AP is on
            if (!Set && Save.Settings.FC_APEnabled && Singleton<StageBattleComponent>.instance.isInGame && GreatNum == 0) //&& MissNum == 0)
            {
                SetCanvas();
                Set = true;
                AP = SetGameObject("AP", Color.yellow, "AP");
            }
            // if still AP and get a great
            if (IsAP && IsTrueFC && Save.Settings.FC_APEnabled && Singleton<TaskStageTarget>.instance.m_GreatResult != 0)// && MissNum == 0)
            {
                IsAP = false;
                Destroy(AP);
                GreatNum = 1;
                FC = SetGameObject("FC", blue, "FC " + GreatNum + "G");
            }
            // if still AP/FC and get a miss
            if (IsTrueFC && Save.Settings.FC_APEnabled)
            {
                if (IsFC && ((GhostMiss != 0 && !Save.Settings.GhostMissEnabled) || (CollectableNoteMiss != 0 && !Save.Settings.CollectableMissEnabled)))
                {
                    IsAP = false;
                    IsFC = false;
                    Destroy(AP);
                    FC = SetGameObject("FC", blue, "FC");
                }
                if (Singleton<TaskStageTarget>.instance.m_Miss != 0 || (GhostMiss != 0 && Save.Settings.GhostMissEnabled) || (CollectableNoteMiss != 0 && Save.Settings.CollectableMissEnabled))
                {
                    IsAP = false;
                    IsTrueFC = false;
                    Destroy(AP);
                    Destroy(FC);
                    /*MissNum = 1;
                    if (GreatNum != 0)
                    {
                        Miss = Miss = SetGameObject("Miss", Color.grey, GreatNum + "G " + MissNum + "M");
                    }
                    else
                    {
                        Miss = SetGameObject("Miss", Color.grey, MissNum + "M");
                    }*/
                }
            }
            if (Singleton<TaskStageTarget>.instance.m_GreatResult != GreatNum && IsTrueFC)// && MissNum == 0)
            {
                GreatNum = Singleton<TaskStageTarget>.instance.m_GreatResult;
                Destroy(FC);
                FC = SetGameObject("FC", blue, "FC " + GreatNum + "G");
            }
            /*if ((Singleton<TaskStageTarget>.instance.m_Miss + GhostMiss + CollectableNoteMiss) != MissNum)
            {
                MissNum = Singleton<TaskStageTarget>.instance.m_MissCombo + GhostMiss + CollectableNoteMiss;
                Destroy(Miss);
                if (GreatNum != 0)
                {
                    Miss = Miss = SetGameObject("Miss", Color.grey, GreatNum + "G " + MissNum + "M");
                }
                else
                {
                    Miss = SetGameObject("Miss", Color.grey, MissNum + "M");
                }
            }*/
            // Destroy gameobject in result screen
            if (GameObject.Find("PnlVictory_2D") != null)
            {
                Destroy(AP);
                Destroy(FC);
            }
            // Restart when getting a great
            if (Save.Settings.RestartEnabled && Singleton<TaskStageTarget>.instance.m_GreatResult != 0 && !IsRestarted && Save.Settings.GreatRestartEnabled)
            {
                IsRestarted = true;
                Singleton<EventManager>.instance.Invoke("Game/Restart", null);
            }
            // Restart when getting a miss
            if (Save.Settings.RestartEnabled && (GhostMiss != 0 || CollectableNoteMiss != 0 || Singleton<TaskStageTarget>.instance.m_MissCombo != 0) && !IsRestarted && Save.Settings.MissRestartEnabled)
            {
                IsRestarted = true;
                Singleton<EventManager>.instance.Invoke("Game/Restart", null);
            }
        }

        private static GameObject SetGameObject(string name, Color color, string text)
        {
            GameObject icanvas = GameObject.Find("Indicator Canvas");
            GameObject gameobject = new GameObject(name);
            gameobject.transform.SetParent(icanvas.transform);
            gameobject.transform.position = new Vector3(Screen.width * 17 / 80 + 25, Screen.height * 80 / 90 - 5, 0f);
            Text gameobject_text = gameobject.AddComponent<Text>();
            gameobject_text.text = text;
            gameobject_text.font = font;
            gameobject_text.fontSize = 85;
            gameobject_text.color = color;
            gameobject_text.transform.position = new Vector3(Screen.width * 5 / 16, Screen.height * 38 / 45 - 15, 0f);
            RectTransform rectTransform = gameobject_text.GetComponent<RectTransform>();
            //rectTransform.localPosition = new Vector3(-435, 345, 0);
            rectTransform.sizeDelta = new Vector2(400, 200);
            return gameobject;
        }

        public static void SetCanvas()
        {
            GameObject canvas = new GameObject();
            canvas.name = "Indicator Canvas";
            canvas.AddComponent<Canvas>();
            canvas.AddComponent<CanvasScaler>();
            canvas.AddComponent<GraphicRaycaster>();
            canvas.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;
        }
    }
}