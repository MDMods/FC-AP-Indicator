using Assets.Scripts.GameCore.HostComponent;
using Assets.Scripts.PeroTools.Commons;
using Assets.Scripts.PeroTools.Managers;
using FormulaBase;
using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;
using MelonLoader;

namespace FC_AP

{
    internal class Indicator : MonoBehaviour
    {
        internal static Font font { get; set; }
        internal static bool SetAP { get; set; }
        internal static bool SetFC { get; set; }
        internal static bool SetMiss { get; set; }
        internal static bool Restarted { get; set; }
        internal static int GreatNum { get; set; }
        internal static int GhostMiss { get; set; }
        internal static int CollectableNoteMiss { get; set; }
        private static GameObject AP { get; set; }
        private static GameObject FC { get; set; }
        private static GameObject Miss { get; set; }

        private static Color blue = new Color(0 / 255f, 136 / 255f, 255 / 255f, 255 / 255f);

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
            // if indicator is enabled and not set, also is in game
            if (Save.Settings.FC_APEnabled && !SetAP && Singleton<StageBattleComponent>.instance.isInGame)
            {
                SetCanvas();
                SetAP = true;
                AP = SetGameObject("AP", Color.yellow, "AP");
            }

            //if AP is set, FC is not set and get a great
            if (AP != null && !SetFC && Singleton<TaskStageTarget>.instance.m_GreatResult != 0)
            {
                Destroy(AP);
                //if not AP then it must be 1 Great
                GreatNum = 1;
                FC = SetGameObject("FC", blue, "FC " + GreatNum + "G");
                SetFC = true;
            }

            //if is AP and FC is not set, ghost and collectable notes are not count as miss when missed
            if (AP != null && !SetFC && ((!Save.Settings.GhostMissEnabled && GhostMiss != 0) || (!Save.Settings.CollectableMissEnabled && CollectableNoteMiss != 0)))
            {
                Destroy(AP);
                FC = SetGameObject("FC", blue, "FC");
                SetFC = true;
            }

            //if AP or FC is set and get normal miss or ghost miss when enabled or collectable note miss when enabled
            if (!SetMiss && (AP != null || FC != null) && (Singleton<TaskStageTarget>.instance.m_MissCombo != 0 || (Save.Settings.GhostMissEnabled && GhostMiss != 0) || (Save.Settings.CollectableMissEnabled && CollectableNoteMiss != 0)))
            {
                Destroy(AP);
                Destroy(FC);
                if (Singleton<TaskStageTarget>.instance.m_GreatResult == 0)
                {
                    Miss = SetGameObject("Miss", Color.grey, "");
                }
                else
                {
                    Miss = SetGameObject("Miss", Color.grey, GreatNum + "G");
                }
                SetMiss = true;
            }

            //if FC is set and great number are not correct then just +1
            if (GreatNum < Singleton<TaskStageTarget>.instance.m_GreatResult)
            {
                GreatNum++;
                if (FC != null)
                {
                    FC.GetComponent<Text>().text = "FC " + GreatNum + "G";
                }
                if (Miss != null)
                {
                    Miss.GetComponent<Text>().text = GreatNum + "G";
                }
            }

            //Destroy gameobject in result screen
            if (GameObject.Find("PnlVictory_2D") != null)
            {
                Destroy(AP);
                Destroy(FC);
                Destroy(Miss);
            }

            //if Restart is enabled and not restarted, get a great when great restart is enabled or get a miss when miss restart is enabled will restart the game
            //if you set GhostMissEnabled or CollectableMissEnabled as false, then miss these notes will not be count as either Great or Miss, means it wont restart the game
            if (Save.Settings.RestartEnabled && !Restarted && ((Save.Settings.GreatRestartEnabled && Singleton<TaskStageTarget>.instance.m_GreatResult != 0) || (Save.Settings.MissRestartEnabled && (Singleton<TaskStageTarget>.instance.m_MissCombo != 0 || (Save.Settings.GhostMissEnabled && GhostMiss != 0) || (Save.Settings.CollectableMissEnabled && CollectableNoteMiss != 0)))))
            {
                Singleton<EventManager>.instance.Invoke("Game/Restart", null);
                Restarted = true;
            }
        }

        private static GameObject SetGameObject(string name, Color color, string text)
        {
            GameObject canvas = GameObject.Find("Indicator Canvas");
            GameObject gameobject = new GameObject(name);
            GameObject Score = GameObject.Find("Score");
            gameobject.transform.SetParent(canvas.transform);
            gameobject.transform.position = new Vector3(Score.transform.position.x + 3.5f, Score.transform.position.y - 1.13f, Score.transform.position.z);
            Text gameobject_text = gameobject.AddComponent<Text>();
            gameobject_text.text = text;
            gameobject_text.font = font;
            gameobject_text.fontSize = 100 * Screen.height / 1080;
            gameobject_text.color = color;
            gameobject_text.transform.position = new Vector3(Score.transform.position.x + 3.5f, Score.transform.position.y - 1.13f, Score.transform.position.z);
            RectTransform rectTransform = gameobject_text.GetComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(Screen.width * 1 / 5, Screen.height * 1 / 5);
            rectTransform.localScale = new Vector3(1, 1, 1);
            return gameobject;
        }

        public static void SetCanvas()
        {
            GameObject canvas = new GameObject();
            canvas.name = "Indicator Canvas";
            canvas.AddComponent<Canvas>();
            canvas.AddComponent<CanvasScaler>();
            canvas.AddComponent<GraphicRaycaster>();
            canvas.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceCamera;
            canvas.GetComponent<Canvas>().worldCamera = GameObject.Find("Camera_2D").GetComponent<Camera>();
            canvas.GetComponent<Canvas>().sortingOrder = 0;
            canvas.GetComponent<CanvasScaler>().referenceResolution = new Vector2(1920, 1080);
            canvas.GetComponent<CanvasScaler>().uiScaleMode = CanvasScaler.ScaleMode.ConstantPixelSize;
        }
    }
}