using Assets.Scripts.PeroTools.Nice.Events;
using Assets.Scripts.PeroTools.Nice.Variables;
using UnityEngine;
using UnityEngine.UI;

namespace FC_AP
{
    internal class ToggleManager
    {
        internal static GameObject FC_APToggle { get; set; }
        internal static GameObject ChartReviewToggle { get; set; }

        /// <summary>
        /// function for setting up toggle
        /// </summary>
        /// <param name="IsEnabled">must use bool pointer to make unityaction convert success</param>
        internal static unsafe void SetupToggle(GameObject Toggle, string name, Vector3 position, bool* IsEnabled, string text)
        {
            Toggle.name = name;

            var txt = Toggle.transform.Find("Txt").GetComponent<UnityEngine.UI.Text>();
            var checkBox = Toggle.transform.Find("Background").GetComponent<Image>();
            var checkMark = Toggle.transform.Find("Background").GetChild(0).GetComponent<Image>();
            var toggleComp = Toggle.GetComponent<Toggle>();

            Toggle.transform.position = position;
            Toggle.GetComponent<OnToggle>().enabled = false;
            Toggle.GetComponent<OnToggleOn>().enabled = false;
            Toggle.GetComponent<OnActivate>().enabled = false;
            Toggle.GetComponent<VariableBehaviour>().enabled = false;

            toggleComp.group = null;
            toggleComp.SetIsOnWithoutNotify(*IsEnabled);
            toggleComp.onValueChanged.AddListener((UnityEngine.Events.UnityAction<bool>)((val) =>
            {
                *IsEnabled = val;
            }));
            txt.text = text;
            txt.fontSize = 40;
            txt.color = new Color(1, 1, 1, 0.298f);
            var rect = txt.transform.Cast<RectTransform>();
            var vect = rect.offsetMax;
            rect.offsetMax = new Vector2(txt.preferredWidth + 10, vect.y);

            checkBox.color = new Color(60 / 255f, 40 / 255f, 111 / 255f);
            checkMark.color = new Color(103 / 255f, 93 / 255f, 130 / 255f);
        }
    }
}