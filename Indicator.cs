using Assets.Scripts.GameCore.HostComponent;
using Assets.Scripts.PeroTools.Commons;
using Assets.Scripts.PeroTools.Managers;
using FormulaBase;
using System;
using UnityEngine.AddressableAssets;
using UnityEngine;
using UnityEngine.UI;

namespace FC_AP
{
    internal class Indicator : MonoBehaviour
    {
        internal static bool Set { get; set; }
        internal static bool IsAP { get; set; }
        internal static bool IsTrueFC { get; set; }
        internal static bool IsFC { get; set; }
        internal static bool GhostMiss { get; set; }
        internal static bool CollectableNoteMiss { get; set; }
        internal static bool IsRestarted { get; set; }
        private static GameObject FC { get; set; }
        private static GameObject AP { get; set; }
        internal static Font font { get; set; }

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
            // if gameobject not created and is in game and FC_AP is on
            if (!Set && ToggleManager.FC_AP && Singleton<StageBattleComponent>.instance.isInGame)
            {
                SetCanvas();
                Set = true;
                AP = SetGameObject("AP", Color.yellow);
            }
            // if still AP and get a great
            if (IsAP && IsTrueFC && ToggleManager.FC_AP && Singleton<TaskStageTarget>.instance.m_GreatResult != 0)
            {
                IsAP = false;
                Destroy(AP);
                FC = SetGameObject("FC", blue);
            }
            // if still AP/FC and get a miss
            if (IsTrueFC && ToggleManager.FC_AP)
            {
                if (IsFC && ((GhostMiss && !ToggleSave.GhostMissEnabled) || (CollectableNoteMiss && !ToggleSave.CollectableMissEnabled)))
                {
                    IsAP = false;
                    IsFC = false;
                    Destroy(AP);
                    SetGameObject("FC", blue);
                }
                if (Singleton<TaskStageTarget>.instance.m_MissCombo != 0 || (GhostMiss && ToggleSave.GhostMissEnabled) || (CollectableNoteMiss && ToggleSave.CollectableMissEnabled))
                {
                    IsAP = false;
                    IsTrueFC = false;
                    Destroy(AP);
                    Destroy(FC);
                }
            }
            // Destroy gameobject in result screen
            if (GameObject.Find("PnlVictory_2D") != null)
            {
                Destroy(AP);
                Destroy(FC);
            }
            // Restart when getting a great
            if (ToggleManager.Restart && Singleton<TaskStageTarget>.instance.m_GreatResult != 0 && !IsRestarted && ToggleSave.GreatRestartEnabled)
            {
                IsRestarted = true;
                Singleton<EventManager>.instance.Invoke("Game/Restart", null);
            }
            // Restart when getting a miss
            if (ToggleManager.Restart && (GhostMiss || CollectableNoteMiss || Singleton<TaskStageTarget>.instance.m_MissCombo != 0) && !IsRestarted && ToggleSave.MissRestartEnabled)
            {
                IsRestarted = true;
                Singleton<EventManager>.instance.Invoke("Game/Restart", null);
            }
        }

        private static GameObject SetGameObject(string name, Color color)
        {
            GameObject icanvas = GameObject.Find("Indicator Canvas");
            GameObject gameobject = new GameObject(name);
            gameobject.transform.SetParent(icanvas.transform);
            gameobject.transform.position = new Vector3(Screen.width * 17 / 80 + 25, Screen.height * 80 / 90 - 5, 0f);
            Text gameobject_text = gameobject.AddComponent<Text>();
            gameobject_text.text = name;
            gameobject_text.font = font;
            gameobject_text.fontSize = 85;
            gameobject_text.color = color;
            gameobject_text.transform.position = new Vector3(Screen.width * 17 / 80 + 25, Screen.height * 80 / 90 - 5, 0f);
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