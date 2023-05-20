using Assets.Scripts.UI.Panels;
using MuseDashMirror.UICreate;
using UnityEngine;
using UnityEngine.UI;

namespace FC_AP;

internal static class Patch
{
    internal static GameObject IndicatorToggle { get; private set; }

    internal static void SwitchLanguagesPostfix()
    {
        IndicatorToggle.transform.Find("Txt").GetComponent<Text>().text = "FC/AP On/Off";
    }

    internal static unsafe void PnlMenuPostfix(PnlMenu __instance)
    {
        GameObject vSelect = null;
        foreach (var @object in __instance.transform.parent.parent.Find("Forward"))
        {
            var transform = @object.Cast<Transform>();
            if (transform.name == "PnlVolume") vSelect = transform.gameObject;
        }

        fixed (bool* indicatorEnabled = &Save.Settings.IndicatorEnabled)
        {
            if (IndicatorToggle == null && vSelect != null)
                IndicatorToggle = ToggleCreate.CreatePnlMenuToggle("FC AP Indicator Toggle", indicatorEnabled, "FC/AP On/Off");
        }
    }
}