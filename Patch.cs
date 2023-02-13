using Assets.Scripts.UI.Panels;
using MuseDashMirror.UICreate;
using UnityEngine;
using UnityEngine.UI;

namespace FC_AP;

internal static class Patch
{
    internal static GameObject FC_APToggle { get; set; }

    internal static void SwitchLanguagesPostfix()
    {
        FC_APToggle.transform.Find("Txt").GetComponent<Text>().text = "FC/AP On/Off";
    }

    internal static unsafe void PnlMenuPostfix(PnlMenu __instance)
    {
        GameObject vSelect = null;
        foreach (var @object in __instance.transform.parent.parent.Find("Forward"))
        {
            var transform = @object.Cast<Transform>();
            if (transform.name == "PnlVolume")
            {
                vSelect = transform.gameObject;
            }
        }

        fixed (bool* fc_apEnabled = &Save.Settings.FC_APEnabled)
        {
            if (FC_APToggle == null && vSelect != null)
            {
                FC_APToggle = ToggleCreate.CreatePnlMenuToggle("FC AP Indicator Toggle", fc_apEnabled, "FC/AP On/Off");
            }
        }
    }
}