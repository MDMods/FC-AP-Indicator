using Assets.Scripts.PeroTools.Nice.Events;
using Assets.Scripts.PeroTools.Nice.Variables;
using Assets.Scripts.UI.Panels;
using UnityEngine;
using UnityEngine.UI;

namespace FC_AP
{
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
            toggleComp.onValueChanged.AddListener((UnityEngine.Events.UnityAction<bool>)((val) =>
            {
                ToggleSave.FC_APEnabled = val;
                if (val) FC_AP_On();
                else FC_AP_Off();
            }));

            txt.text = "FC/AP On/Off";
            txt.fontSize = 35;
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
            txt.fontSize = 35;
            txt.color = new Color(1, 1, 1, 0.298f);
            var rect = txt.transform.Cast<RectTransform>();
            var vect = rect.offsetMax;
            rect.offsetMax = new Vector2(txt.preferredWidth + 10, vect.y);

            checkBox.color = new Color(60 / 255f, 40 / 255f, 111 / 255f);
            checkMark.color = new Color(103 / 255f, 93 / 255f, 130 / 255f);
        }
    }
}