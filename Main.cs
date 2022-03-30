using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MelonLoader;
using Assets.Scripts.GameCore.HostComponent;
using Assets.Scripts.PeroTools.Commons;
using FormulaBase;
using Assets.Scripts.PeroTools.Managers;
using Assets.Scripts.PeroTools.Nice.Events;
using Assets.Scripts.PeroTools.Nice.Variables;
using Assets.Scripts.UI.Panels;
using Assets.Scripts.UI.Specials;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using UnhollowerRuntimeLib;
using HarmonyLib;

namespace FC_AP
{
	public class Main : MelonMod
    {
		public static bool isPlayScene;
		public static bool Set;
		public static GameObject FC;
		public static GameObject AP;
		static bool IsAP;
		static bool IsFC;
		static GameObject canvas;
		static Canvas mycanvas;

        public override void OnApplicationStart()
        {
            base.OnApplicationStart();
			ToggleSave.Load();
        }
        public override void OnSceneWasLoaded(int buildIndex, string sceneName)
		{
			base.OnSceneWasLoaded(buildIndex, sceneName);
			if (sceneName == "GameMain")
			{
				isPlayScene = true;
			}
			else
			{
				isPlayScene = false;
			}
		}

		public override void OnUpdate()
		{
			base.OnUpdate();
			if (isPlayScene)
			{
				 // if gameobject not created and is in game
				if (!Set && ToggleManager.FC_AP && Singleton<StageBattleComponent>.instance.isInGame)
                {
					SetCanvas();
					SetAP_GameObject();
				}
				if (IsAP && Singleton<TaskStageTarget>.instance.m_GreatResult != 0)
				{
					IsAP = false;
                    UnityEngine.Object.Destroy(AP);
					SetFC_GameObject();
				}
				if (IsFC && Singleton<TaskStageTarget>.instance.m_MissResult != 0 || Singleton<TaskStageTarget>.instance.m_MissCombo != 0)
				{
					UnityEngine.Object.Destroy(AP);
					UnityEngine.Object.Destroy(FC);
				}
				// Destroy gameobject in result screen
				if (GameObject.Find("PnlVictory_2D") != null)
                {
					UnityEngine.Object.Destroy(AP);
					UnityEngine.Object.Destroy(FC);
				}
				if (ToggleManager.Restart && Singleton<TaskStageTarget>.instance.m_GreatResult != 0)
					{
						Singleton<EventManager>.instance.Invoke("Game/Restart", null);
					}
				if (ToggleManager.Restart && Singleton<TaskStageTarget>.instance.m_MissResult != 0)
				{
					Singleton<EventManager>.instance.Invoke("Game/Restart", null);
				}
			}
			// if not in play scene
			if (!isPlayScene)
			{
				Set = false;
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
		public static void SetAP_GameObject()
		{
			Set = true; // Set to true to prevent infinite loop
			IsAP = true;
			GameObject icanvas = GameObject.Find("Indicator Canvas");
			// AP Gameobject
			AP = new GameObject("AP");
            AP.transform.SetParent(icanvas.transform);
            AP.transform.position = new Vector3(Screen.width * 17 / 80 + 25, Screen.height * 80 / 90 - 16, 0f);
            UnityEngine.UI.Text AP_text = AP.AddComponent<UnityEngine.UI.Text>();
            AP_text.text = "AP";
            GameObject root = GameObject.Find("Forward");
            AP_text.font = root.transform.Find("PnlPause/Bg/ImgBase/ImgBase2/TxtTittle").GetComponent<UnityEngine.UI.Text>().font;
            AP_text.fontSize = 72 * Screen.height / 1080;
            AP_text.color = Color.yellow;
            AP_text.transform.position = new Vector3(Screen.width * 17 / 80 + 25, Screen.height * 80 / 90 - 16, 0f);
		}

		 public static void SetFC_GameObject()
		{
			IsFC = true;
			GameObject icanvas = GameObject.Find("Indicator Canvas");
			// FC Gameobject
			FC = new GameObject("FC");
            FC.transform.SetParent(canvas.transform);
            FC.transform.position = new Vector3(Screen.width * 17 / 80 + 20, Screen.height * 80 / 90 - 16, 0f);
			UnityEngine.UI.Text FC_text = FC.AddComponent<UnityEngine.UI.Text>();
            FC_text.text = "FC";
            GameObject root = GameObject.Find("Forward");
            FC_text.font = root.transform.Find("PnlPause/Bg/ImgBase/ImgBase2/TxtTittle").GetComponent<UnityEngine.UI.Text>().font;
            FC_text.fontSize = 72 * Screen.height / 1080;
            FC_text.color = new Color(105 / 255f, 211 / 255f, 255 / 255f, 255 / 255f);
            FC_text.transform.position = new Vector3(Screen.width * 17 / 80 + 20, Screen.height * 80 / 90 - 16, 0f);
		} 
	}
	public class ToggleManager
	{
		public static GameObject FC_APToggle;
		public static GameObject RestartToggle;
		public static GameObject vSelect;
		public static PnlStage stage;
		public static bool FC_AP;
		public static bool Restart;

		public static void FC_AP_On()
        {
			FC_AP = true;
        }
		public static void FC_AP_Off()
        {
			FC_AP = false;
        }
		public static void Restart_On()
        {
			Restart = true;
        }
		public static void Restart_Off()
        {
			Restart = false;
        }
		public static void SetupFC_APToggle()
		{
			FC_APToggle.name = "FC AP Indicator Toggle";

			var txt = FC_APToggle.transform.Find("Txt").GetComponent<UnityEngine.UI.Text>();
			var checkBox = FC_APToggle.transform.Find("Background").GetComponent<Image>();
			var checkMark = FC_APToggle.transform.Find("Background").GetChild(0).GetComponent<Image>();
			var toggleComp = FC_APToggle.GetComponent<Toggle>();

			FC_APToggle.transform.position = new Vector3(3.5f, -5f, 100f);
			FC_APToggle.GetComponent<OnToggle>().enabled = false;
			FC_APToggle.GetComponent<OnToggleOn>().enabled = false;
			FC_APToggle.GetComponent<OnActivate>().enabled = false;
			FC_APToggle.GetComponent<VariableBehaviour>().enabled = false;

			toggleComp.group = null;
			toggleComp.SetIsOnWithoutNotify(ToggleSave.FC_APEnabled);
			toggleComp.onValueChanged.AddListener((UnityEngine.Events.UnityAction<bool>)((val) => {
				ToggleSave.FC_APEnabled = val;
				if (val) FC_AP_On();
				else FC_AP_Off();
			}));

			txt.text = "FC/AP On/Off";
			txt.color = new Color(1, 1, 1, 0.298f);
			var rect = txt.transform.Cast<RectTransform>();
			var vect = rect.offsetMax;
			rect.offsetMax = new Vector2(txt.preferredWidth + 10, vect.y);

			checkBox.color = new Color(60 / 255f, 40 / 255f, 111 / 255f);
			checkMark.color = new Color(103 / 255f, 93 / 255f, 130 / 255f);
		}
		public static void SetupRestartToggle()
		{
			RestartToggle.name = "Restart Toggle";

			var txt = RestartToggle.transform.Find("Txt").GetComponent<UnityEngine.UI.Text>();
			var checkBox = RestartToggle.transform.Find("Background").GetComponent<Image>();
			var checkMark = RestartToggle.transform.Find("Background").GetChild(0).GetComponent<Image>();
			var toggleComp = RestartToggle.GetComponent<Toggle>();

			RestartToggle.transform.position = new Vector3(6.5f, -5f, 100f);
			RestartToggle.GetComponent<OnToggle>().enabled = false;
			RestartToggle.GetComponent<OnToggleOn>().enabled = false;
			RestartToggle.GetComponent<OnActivate>().enabled = false;
			RestartToggle.GetComponent<VariableBehaviour>().enabled = false;

			toggleComp.group = null;
			toggleComp.SetIsOnWithoutNotify(ToggleSave.RestartEnabled);
			toggleComp.onValueChanged.AddListener((UnityEngine.Events.UnityAction<bool>)((val) =>
			{
				ToggleSave.RestartEnabled = val;
				if (val) Restart_On();
				else Restart_Off();
			}));

			txt.text = "Auto Restart On/Off";
			txt.color = new Color(1, 1, 1, 0.298f);
			var rect = txt.transform.Cast<RectTransform>();
			var vect = rect.offsetMax;
			rect.offsetMax = new Vector2(txt.preferredWidth + 10, vect.y);

			checkBox.color = new Color(60 / 255f, 40 / 255f, 111 / 255f);
			checkMark.color = new Color(103 / 255f, 93 / 255f, 130 / 255f);
		}
	}

	[HarmonyPatch(typeof(PnlStage), "Awake")]
	internal class Patch
    {
		private static void Postfix(PnlStage __instance)
		{
			ToggleManager.stage = __instance;
			bool fc_apEnabled = ToggleSave.FC_APEnabled;
			bool restartEnabled = ToggleSave.RestartEnabled;
			if (fc_apEnabled)
			{
				ToggleManager.FC_AP_On();
			}
			if (restartEnabled)
            {
				ToggleManager.Restart_On();
            }
			GameObject vSelect = null;
			foreach (Il2CppSystem.Object @object in __instance.transform.parent.parent.Find("Forward"))
			{
				Transform transform = @object.Cast<Transform>();
				if (transform.name == "PnlVolume")
				{
					vSelect = transform.gameObject;
				}
			}
			ToggleManager.vSelect = vSelect;
			if (ToggleManager.FC_APToggle == null && ToggleManager.vSelect != null)
			{
				GameObject fc_apToggle = UnityEngine.Object.Instantiate<GameObject>(ToggleManager.vSelect.transform.Find("LogoSetting").Find("Toggles").Find("TglOn").gameObject, __instance.transform);
				ToggleManager.FC_APToggle = fc_apToggle;
				ToggleManager.SetupFC_APToggle();
			}
			if (ToggleManager.RestartToggle == null && ToggleManager.vSelect != null)
			{
				GameObject restartToggle = UnityEngine.Object.Instantiate<GameObject>(ToggleManager.vSelect.transform.Find("LogoSetting").Find("Toggles").Find("TglOn").gameObject, __instance.transform);
				ToggleManager.RestartToggle = restartToggle;
				ToggleManager.SetupRestartToggle();
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

	public class ToggleSave
	{
		public static bool FC_APEnabled
		{
			get
			{
				return fc_apEnabled.Value;
			}
			set
			{
				fc_apEnabled.Value = value;
			}
		}
		
		public static bool RestartEnabled
        {
            get
            {
				return restartEnabled.Value;
            }
            set
            {
				restartEnabled.Value = value;
            }
        }

		public static void Load()
		{
			ToggleCategory = MelonPreferences.CreateCategory("FC AP");
			ToggleCategory.SetFilePath("UserData/FC AP.cfg", true);
			fc_apEnabled = MelonPreferences.CreateEntry<bool>("FC AP", "fc_apEnabled", false, null, "Whether the FC AP indicator is enabled.", false, false, null);
			restartEnabled = MelonPreferences.CreateEntry<bool>("Restart", "restartEnabled", false, null, "Whether the auto restart is enabled.", false, false, null);
		}

		public static MelonPreferences_Category ToggleCategory;

		public static MelonPreferences_Entry<bool> fc_apEnabled;

		public static MelonPreferences_Entry<bool> restartEnabled;
	}
}
