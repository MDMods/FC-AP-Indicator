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
				if (!Set && Singleton<StageBattleComponent>.instance.isInGame)
                {
					SetGameObject();
				}
				if (Singleton<TaskStageTarget>.instance.m_GreatResult != 0)
				{
                    UnityEngine.Object.Destroy(AP);
				}
				if (Singleton<TaskStageTarget>.instance.m_MissResult != 0 || Singleton<TaskStageTarget>.instance.m_MissCombo != 0)
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
			}
			// if not in play scene
			if (!isPlayScene)
			{
				Set = false;
			}
		}

		public static void SetGameObject()
		{
			Set = true; // Set to true to prevent infinite loop
			// create canvas
			GameObject canvas = new GameObject();
			Canvas mycanvas;
			canvas.name = "Indicator Canvas";
			canvas.AddComponent<Canvas>();
			canvas.AddComponent<CanvasScaler>();
			canvas.AddComponent<GraphicRaycaster>();
			mycanvas = canvas.GetComponent<Canvas>();
			mycanvas.renderMode = RenderMode.ScreenSpaceOverlay;
			// FC Gameobject
			FC = new GameObject("FC");
			FC.transform.SetParent(canvas.transform);
			FC.transform.position = new Vector3(Screen.width * 17 / 80, Screen.height * 39 / 45, 0f);
			Text FC_text = FC.AddComponent<Text>();
			FC_text.text = "FC";
			GameObject root = GameObject.Find("Forward");
			FC_text.font = root.transform.Find("PnlPause/Bg/ImgBase/ImgBase2/TxtTittle").GetComponent<Text>().font;
			FC_text.fontSize = 45;
			FC_text.color = Color.blue;
			FC_text.transform.position = new Vector3(Screen.width * 17 / 80, Screen.height * 39 / 45, 0f);
			// AP Gameobject
			AP = new GameObject("AP");
			AP.transform.SetParent(canvas.transform);
			AP.transform.position = new Vector3(Screen.width * 21 / 80, Screen.height * 39 / 45, 0f);
			Text AP_text = AP.AddComponent<Text>();
			AP_text.text = "AP";
			AP_text.font = root.transform.Find("PnlPause/Bg/ImgBase/ImgBase2/TxtTittle").GetComponent<Text>().font;
			AP_text.fontSize = 45;
			AP_text.color = Color.yellow;
			AP_text.transform.position = new Vector3(Screen.width * 21 / 80, Screen.height * 39 / 45, 0f);

		}
	}
	/*public class ToggleManager
	{
		public static GameObject FC_APToggle;
		public static GameObject RestartToggle;
		public static GameObject Vselect;

		public static void SetUpFC_APToggle()
        {
			FC_APToggle.name = "FC AP Indicator Toggle";
			UnityEngine.UI.Text component = FC_APToggle.transform.Find("Txt").GetComponent<UnityEngine.UI.Text>();
			Image component2 = FC_APToggle.transform.Find("Background").GetComponent<Image>();
			Image component3 = FC_APToggle.transform.Find("Background").GetChild(0).GetComponent<Image>();
			Toggle component4 = FC_APToggle.GetComponent<Toggle>();
			FC_APToggle.transform.position = new Vector3(20f, -5f, 100f);
			FC_APToggle.GetComponent<OnToggle>().enabled = false;
			FC_APToggle.GetComponent<OnToggleOn>().enabled = false;
			FC_APToggle.GetComponent<OnActivate>().enabled = false;
			FC_APToggle.GetComponent<VariableBehaviour>().enabled = false;
			component4.SetIsOnWithoutNotify(ToggleSave.FC_APEnabled);
			component4.group = null;
			/*component4.onValueChanged.AddListener(delegate (bool val)
			{
				ToggleSave.FC_APEnabled = val;
				if (val)
				{
					
				}
				else
				{
					
				}
			});
			component.text = "FC AP Indicator";
			component.color = new Color(1f, 1f, 1f, 0.298f);
			RectTransform rectTransform = component.transform.Cast<RectTransform>();
			Vector2 offsetMax = rectTransform.offsetMax;
			rectTransform.offsetMax = new Vector2(component.preferredWidth + 10f, offsetMax.y);
			component2.color = new Color(0.23529412f, 0.15686275f, 0.43529412f);
			component3.color = new Color(0.40392157f, 0.3647059f, 0.50980395f);
		}
		public static void SetUpRestartToggle()
		{
			FC_APToggle.name = "Restart Toggle";
			UnityEngine.UI.Text component = FC_APToggle.transform.Find("Txt").GetComponent<UnityEngine.UI.Text>();
			Image component2 = FC_APToggle.transform.Find("Background").GetComponent<Image>();
			Image component3 = FC_APToggle.transform.Find("Background").GetChild(0).GetComponent<Image>();
			Toggle component4 = FC_APToggle.GetComponent<Toggle>();
			FC_APToggle.transform.position = new Vector3(20f, -5f, 100f);
			FC_APToggle.GetComponent<OnToggle>().enabled = false;
			FC_APToggle.GetComponent<OnToggleOn>().enabled = false;
			FC_APToggle.GetComponent<OnActivate>().enabled = false;
			FC_APToggle.GetComponent<VariableBehaviour>().enabled = false;
			component4.SetIsOnWithoutNotify(ToggleSave.RestartEnabled);
			component4.group = null;
			/*component4.onValueChanged.AddListener(delegate (bool val)
			{
				ToggleSave.RestartEnabled = val;
				if (val)
				{
					
				}
				else
				{
					
				}
			});
			component.text = "Restart";
			component.color = new Color(1f, 1f, 1f, 0.298f);
			RectTransform rectTransform = component.transform.Cast<RectTransform>();
			Vector2 offsetMax = rectTransform.offsetMax;
			rectTransform.offsetMax = new Vector2(component.preferredWidth + 10f, offsetMax.y);
			component2.color = new Color(0.23529412f, 0.15686275f, 0.43529412f);
			component3.color = new Color(0.40392157f, 0.3647059f, 0.50980395f);
		}

		public static void Restart()
        {
			if (Singleton<TaskStageTarget>.instance.m_GreatResult != 0)
			{
				Singleton<EventManager>.instance.Invoke("Game/Restart", null);
			}
			if (Singleton<TaskStageTarget>.instance.m_MissResult != 0)
			{
				Singleton<EventManager>.instance.Invoke("Game/Restart", null);
			}
		}
	}

	[HarmonyPatch(typeof(PnlStage), "Awake")]
	internal class Patch
    {
		private static void Postfix(PnlStage __instance)
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
			ToggleManager.Vselect = vSelect;
			if (ToggleManager.FC_APToggle == null && ToggleManager.Vselect != null)
            {
				GameObject FC_APToggle = UnityEngine.Object.Instantiate<GameObject>(ToggleManager.Vselect.transform.Find("LogoSetting").Find("Toggles").Find("TglOn").gameObject, __instance.transform);
				ToggleManager.FC_APToggle = FC_APToggle;
				ToggleManager.SetUpFC_APToggle();
			}
			if (ToggleManager.RestartToggle == null && ToggleManager.Vselect != null)
			{
				GameObject RestartToggle = UnityEngine.Object.Instantiate<GameObject>(ToggleManager.Vselect.transform.Find("LogoSetting").Find("Toggles").Find("TglOn").gameObject, __instance.transform);
				ToggleManager.RestartToggle = RestartToggle;
				ToggleManager.SetUpFC_APToggle();
			}
		}

	}*/

	/*[HarmonyPatch(typeof(VolumeSelect), MethodType.Constructor)]
	internal class VolumeCtorPatch
	{
		// Token: 0x0600000F RID: 15 RVA: 0x000026BA File Offset: 0x000008BA
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
			fc_apEnabled = MelonPreferences.CreateEntry<bool>("FC AP", "fc_apEnabled", false, null, "Whether the FC AP indicator checkbox is enabled.", false, false, null);
			restartEnabled = MelonPreferences.CreateEntry<bool>("Restart", "restartEnabled", false, null, "Whether the restart checkbox is enabled.", false, false, null);
		}

		public static MelonPreferences_Category ToggleCategory;

		public static MelonPreferences_Entry<bool> fc_apEnabled;

		public static MelonPreferences_Entry<bool> restartEnabled;
	}*/
}
