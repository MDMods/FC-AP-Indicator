using Assets.Scripts.GameCore.HostComponent;
using Assets.Scripts.PeroTools.Commons;
using Assets.Scripts.PeroTools.Managers;
using FormulaBase;
using MelonLoader;
using UnityEngine;
using UnityEngine.UI;

namespace FC_AP
{
    public class Main : MelonMod
    {
        public static bool isPlayScene;
        public static bool Set;
        public static bool GhostMiss;
        public static bool CollectableNoteMiss;
        public static bool IsRestarted;
        public static bool MainScene;
        public static bool Preparation;
        public static GameObject FC;
        public static GameObject AP;
        private static bool IsAP;
        private static bool IsTrueFC;
        private static bool IsFC;
        private static GameObject canvas;
        private static Canvas mycanvas;

        //public static Main instance;

        public override void OnApplicationStart()
        {
            //instance = this;
            LoggerInstance.Msg(Screen.height);
            ToggleSave.Load();
        }

        public override void OnSceneWasLoaded(int buildIndex, string sceneName)
        {
            if (sceneName == "GameMain")
            {
                isPlayScene = true;
                // not in preparation screen
                Preparation = false;
            }
            else
            {
                isPlayScene = false;
            }
            if (sceneName == "UISystem_PC")
            {
                MainScene = true;
            }
            else
            {
                MainScene = false;
            }
        }

        public override void OnUpdate()
        {
            if (isPlayScene)
            {
                // if gameobject not created and is in game and FC_AP is on
                if (!Set && ToggleManager.FC_AP && Singleton<StageBattleComponent>.instance.isInGame)
                {
                    SetCanvas();
                    SetAP_GameObject();
                }
                // if still AP and get a great
                if (IsAP && IsTrueFC && ToggleManager.FC_AP && Singleton<TaskStageTarget>.instance.m_GreatResult != 0)
                {
                    IsAP = false;
                    Object.Destroy(AP);
                    SetFC_GameObject();
                }
                // if still AP/FC and get a miss
                if (IsTrueFC && ToggleManager.FC_AP)
                {
                    if (IsFC && ((GhostMiss && !ToggleSave.GhostMissEnabled) || (CollectableNoteMiss && !ToggleSave.CollectableMissEnabled)))
                    {
                        IsAP = false;
                        IsFC = false;
                        Object.Destroy(AP);
                        SetFC_GameObject();
                    }
                    if (Singleton<TaskStageTarget>.instance.m_MissCombo != 0 || (GhostMiss && ToggleSave.GhostMissEnabled) || (CollectableNoteMiss && ToggleSave.CollectableMissEnabled))
                    {
                        IsAP = false;
                        IsTrueFC = false;
                        Object.Destroy(AP);
                        Object.Destroy(FC);
                    }
                }
                // Destroy gameobject in result screen
                if (GameObject.Find("PnlVictory_2D") != null)
                {
                    Object.Destroy(AP);
                    Object.Destroy(FC);
                }
                // Restart when get a great
                if (ToggleManager.Restart && Singleton<TaskStageTarget>.instance.m_GreatResult != 0 && !IsRestarted && ToggleSave.GreatRestartEnabled)
                {
                    IsRestarted = true;
                    Singleton<EventManager>.instance.Invoke("Game/Restart", null);
                }
                // Restart when get a miss
                if (ToggleManager.Restart && (GhostMiss || CollectableNoteMiss || Singleton<TaskStageTarget>.instance.m_MissCombo != 0) && !IsRestarted && ToggleSave.MissRestartEnabled)
                {
                    IsRestarted = true;
                    Singleton<EventManager>.instance.Invoke("Game/Restart", null);
                }
            }
            // if not in play scene
            if (!isPlayScene)
            {
                Set = false;
                GhostMiss = false;
                CollectableNoteMiss = false;
                IsAP = true;
                IsTrueFC = true;
                IsFC = true;
                IsRestarted = false;
            }
            // if in main scene
            if (MainScene)
            {
                // remove toggles on preparation screen
                if (!Preparation && GameObject.Find("PnlPreparation"))
                {
                    Preparation = true;
                    ToggleManager.FC_APToggle.SetActive(false);
                    ToggleManager.RestartToggle.SetActive(false);
                }
                else if (Preparation && !GameObject.Find("PnlPreparation"))
                {
                    Preparation = false;
                    ToggleManager.FC_APToggle.SetActive(true);
                    ToggleManager.RestartToggle.SetActive(true);
                }
            }
        }

        public static void SetCanvas()
        {
            canvas = new GameObject();
            canvas.name = "Indicator Canvas";
            canvas.AddComponent<Canvas>();
            canvas.AddComponent<CanvasScaler>();
            canvas.AddComponent<GraphicRaycaster>();
            mycanvas = canvas.GetComponent<Canvas>();
            mycanvas.renderMode = RenderMode.ScreenSpaceOverlay;
        }

        private static void SetAP_GameObject()
        {
            Set = true; // Set to true to prevent infinite loop
            GameObject icanvas = GameObject.Find("Indicator Canvas");
            // AP Gameobject
            AP = new GameObject("AP");
            AP.transform.SetParent(icanvas.transform);
            AP.transform.position = new Vector3(Screen.width * 17 / 80 + 25, Screen.height * 80 / 90 - 5, 0f);
            Text AP_text = AP.AddComponent<Text>();
            AP_text.text = "AP";
            AP_text.font = Font.CreateDynamicFontFromOSFont("Snaps Taste", 100);
            AP_text.fontSize = 100 * Screen.height / 1080;
            AP_text.color = Color.yellow;
            AP_text.transform.position = new Vector3(Screen.width * 17 / 80 + 25, Screen.height * 80 / 90 - 5, 0f);
        }

        private static void SetFC_GameObject()
        {
            GameObject icanvas = GameObject.Find("Indicator Canvas");
            // FC Gameobject
            FC = new GameObject("FC");
            FC.transform.SetParent(icanvas.transform);
            FC.transform.position = new Vector3(Screen.width * 17 / 80 + 20, Screen.height * 80 / 90 - 5, 0f);
            Text FC_text = FC.AddComponent<Text>();
            FC_text.text = "FC";
            FC_text.font = Font.CreateDynamicFontFromOSFont("Snaps Taste", 100);
            FC_text.fontSize = 100 * Screen.height / 1080;
            FC_text.color = new Color(0 / 255f, 136 / 255f, 255 / 255f, 255 / 255f);
            FC_text.transform.position = new Vector3(Screen.width * 17 / 80 + 25, Screen.height * 80 / 90 - 5, 0f);
        }

        /*public void Log(object log)
        {
            LoggerInstance.Msg(log);
        }*/
    }
}