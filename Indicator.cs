using System;
using UnityEngine;
using UnityEngine.UI;
using static MuseDashMirror.BattleComponent;
using static MuseDashMirror.UICreate.CanvasCreate;
using static MuseDashMirror.UICreate.Colors;
using static MuseDashMirror.UICreate.Fonts;
using static MuseDashMirror.UICreate.TextGameObjectCreate;

namespace FC_AP;

internal class Indicator : MonoBehaviour
{
    public Indicator(IntPtr intPtr) : base(intPtr)
    {
    }

    private static GameObject _Indicator { get; set; }
    private static Text _Text { get; set; }
    private static int _CurrentMiss { get; set; }

    // if collectable note not count as miss
    private static bool NoCollectableNoteMiss => !Save.Settings.CollectableMissEnabled && CollectableNoteMissNum > 0;

    // if collectable note not count as miss
    private static bool NoGhostMiss => !Save.Settings.GhostMissEnabled && GhostMissNum > 0;

    private void OnDestroy()
    {
        _Indicator = default;
        _CurrentMiss = default;
    }

    internal static void DestroyGameObject() => Destroy(_Indicator);

    internal static void SetAP()
    {
        CreateCanvas("Indicator Canvas", "Camera_2D");
        _Indicator = CreateTextGameObject("Indicator Canvas", "Indicator", "AP",
            TextAnchor.UpperLeft, true, new Vector3(-102.398f, 367.2864f, 0f),
            new Vector2(960, 216), 100, SnapsTasteFont, Color.yellow);
        _Text = _Indicator.GetComponent<Text>();
    }

    internal static void UpdateIndicator()
    {
        // if no great and miss, return
        if (GreatNum == 0 && TotalMissNum == 0) return;

        if (TotalMissNum > 0) UpdateMissNum();

        if (GreatNum > 0)
        {
            if (_CurrentMiss == 0)
            {
                _Text.text = "FC " + GreatNum + "G";
                _Text.color = Blue;
                return;
            }

            _Text.text = _CurrentMiss + "M " + GreatNum + "G";
            _Text.color = Silver;
            return;
        }

        if (_CurrentMiss == 0)
        {
            _Text.text = "FC";
            _Text.color = Blue;
        }
        else
        {
            _Text.text = _CurrentMiss + "M";
            _Text.color = Silver;
        }
    }

    private static void UpdateMissNum()
    {
        if (NoCollectableNoteMiss)
            _CurrentMiss = NormalMissNum + GhostMissNum;

        else if (NoGhostMiss)
            _CurrentMiss = NormalMissNum + CollectableNoteMissNum;

        else if (NoCollectableNoteMiss && NoGhostMiss)
            _CurrentMiss = NormalMissNum;

        else
            _CurrentMiss = TotalMissNum;
    }
}