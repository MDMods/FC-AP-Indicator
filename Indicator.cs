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
        internal static int GreatNum { get; set; }
        internal static int MissNum { get; set; }
        internal static int CurrentMissNum { get; set; }
        internal static int GhostMiss { get; set; }
        internal static int CollectableNoteMiss { get; set; }
        private static GameObject AP { get; set; }
        private static GameObject FC { get; set; }
        private static GameObject Miss { get; set; }

        private static Color blue = new Color(0 / 255f, 136 / 255f, 255 / 255f, 255 / 255f);

        private static Color silver = new Color(192 / 255f, 192 / 255f, 192 / 255f, 255 / 255f);

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

        /// <summary>
        /// Get font
        /// </summary>
        private void Start()
        {
            var asset = Addressables.LoadAssetAsync<Font>("Snaps Taste");
            font = asset.WaitForCompletion();
        }

        private void Update()
        {
            // if indicator is enabled and not set, also is in game
            if (Save.Settings.FC_APEnabled && AP == null && FC == null && Miss == null && Singleton<StageBattleComponent>.instance.isInGame)
            {
                SetCanvas();
                AP = SetGameObject("AP", Color.yellow, "AP");
            }

            //if AP is set, FC is not set and get a great
            if (AP != null && FC == null && Miss == null && Singleton<TaskStageTarget>.instance.m_GreatResult != 0)
            {
                Destroy(AP);
                //if not AP then it must be 1 Great
                GreatNum = 1;
                FC = SetGameObject("FC", blue, "FC " + GreatNum + "G");
            }

            //if AP is set, FC is not set and ghost not count as miss
            if (AP != null && FC == null && GhostMiss != 0)
            {
                Destroy(AP);
                FC = SetGameObject("FC", blue, "FC");
            }

            //if AP is set, FC is not set and collectable note not count as miss
            if (AP != null && FC == null && CollectableNoteMiss != 0)
            {
                Destroy(AP);
                FC = SetGameObject("FC", blue, "FC");
            }

            //if AP or FC is set, Miss doesn't set and get normal miss or ghost miss when enabled or collectable note miss when enabled
            if (Miss == null && (AP != null || FC != null) && CurrentMissNum != 0)
            {
                Destroy(AP);
                Destroy(FC);
                MissNum = 1;
                if (GreatNum == 0)
                {
                    Miss = SetGameObject("Miss", silver, MissNum + "M");
                }
                else
                {
                    Miss = SetGameObject("Miss", silver, MissNum + "M " + GreatNum + "G");
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
                VariableUtils.SetResult(Singleton<DataManager>.instance["Account"]["SelectedRoleIndex"], new Il2CppSystem.Int32() { m_value = LastCharacter }.BoxIl2CppObject());
                VariableUtils.SetResult(Singleton<DataManager>.instance["Account"]["SelectedElfinIndex"], new Il2CppSystem.Int32() { m_value = LastElfin }.BoxIl2CppObject());
                VariableUtils.SetResult(Singleton<DataManager>.instance["GameConfig"]["Offset"], new Il2CppSystem.Int32() { m_value = LastOffset }.BoxIl2CppObject());
                VariableUtils.SetResult(Singleton<DataManager>.instance["Account"]["IsAutoFever"], new Il2CppSystem.Boolean() { m_value = true }.BoxIl2CppObject());
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
                    LastCharacter = VariableUtils.GetResult<int>(Singleton<DataManager>.instance["Account"]["SelectedRoleIndex"]);
                    LastElfin = VariableUtils.GetResult<int>(Singleton<DataManager>.instance["Account"]["SelectedElfinIndex"]);
                    LastOffset = VariableUtils.GetResult<int>(Singleton<DataManager>.instance["GameConfig"]["Offset"]);

                    VariableUtils.SetResult(Singleton<DataManager>.instance["Account"]["SelectedRoleIndex"], new Il2CppSystem.Int32() { m_value = 2 }.BoxIl2CppObject());
                    VariableUtils.SetResult(Singleton<DataManager>.instance["Account"]["SelectedElfinIndex"], new Il2CppSystem.Int32() { m_value = -1 }.BoxIl2CppObject());
                    VariableUtils.SetResult(Singleton<DataManager>.instance["GameConfig"]["Offset"], new Il2CppSystem.Int32() { m_value = 0 }.BoxIl2CppObject());
                    VariableUtils.SetResult(Singleton<DataManager>.instance["Account"]["IsAutoFever"], new Il2CppSystem.Boolean() { m_value = false }.BoxIl2CppObject());
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
            GameObject canvas = GameObject.Find("Indicator Canvas");
            GameObject gameobject = new GameObject(name);
            gameobject.transform.SetParent(canvas.transform);
            Text gameobject_text = gameobject.AddComponent<Text>();
            gameobject_text.text = text;
            gameobject_text.alignment = TextAnchor.UpperLeft;
            gameobject_text.font = font;
            gameobject_text.color = color;
            gameobject_text.fontSize = 100;
            gameobject_text.transform.localPosition = new Vector3(-102.398f, 367.2864f, 0f);
            RectTransform rectTransform = gameobject_text.GetComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(960, 216);
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
            canvas.GetComponent<CanvasScaler>().referenceResolution = new Vector2(1920, 1080);
            canvas.GetComponent<CanvasScaler>().uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            canvas.GetComponent<CanvasScaler>().screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
        }
    }
}