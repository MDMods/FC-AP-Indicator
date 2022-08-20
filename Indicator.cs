using Assets.Scripts.GameCore.HostComponent;
using Assets.Scripts.PeroTools.Commons;
using Assets.Scripts.PeroTools.Nice.Datas;
using Assets.Scripts.PeroTools.Nice.Interface;
using FormulaBase;
using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

namespace FC_AP

{
    internal class Indicator : MonoBehaviour
    {
        // indicator

        internal static Font font { get; set; }
        internal static bool SetAP { get; set; }
        internal static bool SetFC { get; set; }
        internal static bool SetMiss { get; set; }
        internal static int GreatNum { get; set; }
        internal static int GhostMiss { get; set; }
        internal static int CollectableNoteMiss { get; set; }
        private static GameObject AP { get; set; }
        private static GameObject FC { get; set; }
        private static GameObject Miss { get; set; }

        private static Color blue = new Color(0 / 255f, 136 / 255f, 255 / 255f, 255 / 255f);

        //chart review

        internal static bool ObjectDisabled { get; set; }
        private static bool Set { get; set; }
        private static bool Reset { get; set; }
        private static int LastOffset { get; set; }

        private static int LastCharacter = -1;

        private static int LastElfin = -1;

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
            if (!Save.Settings.ChartReviewEnabled && (LastCharacter != -1 || LastElfin != -1) && !Reset)
            {
                VariableUtils.SetResult(Singleton<DataManager>.instance["Account"]["SelectedRoleIndex"], new Il2CppSystem.Int32() { m_value = LastCharacter }.BoxIl2CppObject());
                VariableUtils.SetResult(Singleton<DataManager>.instance["Account"]["SelectedElfinIndex"], new Il2CppSystem.Int32() { m_value = LastElfin }.BoxIl2CppObject());
                VariableUtils.SetResult(Singleton<DataManager>.instance["GameConfig"]["Offset"], new Il2CppSystem.Int32() { m_value = LastOffset }.BoxIl2CppObject());
                VariableUtils.SetResult(Singleton<DataManager>.instance["Account"]["IsAutoFever"], new Il2CppSystem.Boolean() { m_value = true }.BoxIl2CppObject());
                Set = false;
                Reset = true;
            }

            if (Save.Settings.ChartReviewEnabled)
            {
                Reset = false;
                // if not set character and elfin
                if (!Set)
                {
                    LastCharacter = VariableUtils.GetResult<int>(Singleton<DataManager>.instance["Account"]["SelectedRoleIndex"]);
                    LastElfin = VariableUtils.GetResult<int>(Singleton<DataManager>.instance["Account"]["SelectedElfinIndex"]);
                    LastOffset = VariableUtils.GetResult<int>(Singleton<DataManager>.instance["GameConfig"]["Offset"]);

                    VariableUtils.SetResult(Singleton<DataManager>.instance["Account"]["SelectedRoleIndex"], new Il2CppSystem.Int32() { m_value = 2 }.BoxIl2CppObject());
                    VariableUtils.SetResult(Singleton<DataManager>.instance["Account"]["SelectedElfinIndex"], new Il2CppSystem.Int32() { m_value = -1 }.BoxIl2CppObject());
                    VariableUtils.SetResult(Singleton<DataManager>.instance["GameConfig"]["Offset"], new Il2CppSystem.Int32() { m_value = 0 }.BoxIl2CppObject());
                    VariableUtils.SetResult(Singleton<DataManager>.instance["Account"]["IsAutoFever"], new Il2CppSystem.Boolean() { m_value = false }.BoxIl2CppObject());
                    VariableUtils.SetResult(Singleton<DataManager>.instance["GameConfig"]["FullScreen"], new Il2CppSystem.Boolean() { m_value = false }.BoxIl2CppObject());
                    VariableUtils.SetResult(Singleton<DataManager>.instance["GameConfig"]["HasBorder"], new Il2CppSystem.Boolean() { m_value = false }.BoxIl2CppObject());
                    Set = true;
                }

                // if is game scene and objects are not disabled
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
            GameObject canvas = GameObject.Find("Indicator Canvas");
            GameObject gameobject = new GameObject(name);
            gameobject.transform.SetParent(canvas.transform);
            gameobject.transform.position = new Vector3(-2.6111f, 2.5506f, 90);
            Text gameobject_text = gameobject.AddComponent<Text>();
            gameobject_text.text = text;
            gameobject_text.font = font;
            gameobject_text.fontSize = 100 * Screen.height / 1080;
            gameobject_text.color = color;
            gameobject_text.transform.position = new Vector3(-2.6111f, 2.5506f, 90);
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