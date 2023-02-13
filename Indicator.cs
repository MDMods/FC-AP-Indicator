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

    private static GameObject AP { get; set; }
    private static GameObject FC { get; set; }
    private static GameObject Miss { get; set; }

    private void Start()
    {
        LoadFonts();
    }

    private void OnDestroy()
    {
        AP = null;
        FC = null;
        Miss = null;
        UnloadFonts();
    }

    internal static void SetAPToggle()
    {
        CreateCanvas("Indicator Canvas", "Camera_2D");
        AP = SetGameObject("AP", Color.yellow, "AP");
    }

    internal static void UpdateFCGameObject()
    {
        // if no great, return
        if (GreatNum == 0) return;

        // if has miss, don't need update FC
        if (TotalMissNum != 0) return;

        if (FC is null)
        {
            Destroy(AP);
            FC = SetGameObject("FC", Blue, "FC " + GreatNum + "G");
        }
        else
        {
            FC.GetComponent<Text>().text = "FC " + GreatNum + "G";
        }
    }

    internal static void UpdateMissGameObject()
    {
        // if no miss, return
        if (TotalMissNum == 0) return;

        Destroy(AP);
        Destroy(FC);

        int currentMiss;
        // if collectable note not count as miss and FC is null
        if (!Save.Settings.CollectableMissEnabled && CollectableNoteMissNum != 0)
        {
            currentMiss = NormalMissNum + GhostMissNum;
            if (Miss is null)
                FC = SetFC();
        }

        // if collectable note not count as miss and FC is null
        else if (!Save.Settings.GhostMissEnabled && GhostMissNum != 0)
        {
            currentMiss = NormalMissNum + CollectableNoteMissNum;
            if (Miss is null)
                FC = SetFC();
        }

        // if collectable note and ghost not count as miss and FC is null
        else if (!Save.Settings.CollectableMissEnabled && CollectableNoteMissNum != 0 && !Save.Settings.GhostMissEnabled && GhostMissNum != 0)
        {
            currentMiss = NormalMissNum;
            if (Miss is null)
                FC = SetFC();
        }
        else
        {
            currentMiss = TotalMissNum;
            Miss ??= SetMiss(currentMiss);
        }

        if (Miss is null)
        {
            if (currentMiss > NormalMissNum || NormalMissNum > 0)
            {
                Destroy(FC);
                Miss = SetMiss(currentMiss);
            }
        }
        else
        {
            Miss.GetComponent<Text>().text = GreatNum == 0 ? currentMiss + "M" : currentMiss + "M " + GreatNum + "G";
        }
    }

    private static GameObject SetFC()
    {
        var fc = GreatNum == 0 ? SetGameObject("FC", Blue, "FC") : SetGameObject("FC", Blue, "FC " + GreatNum + "G");
        return fc;
    }

    private static GameObject SetMiss(int missNum)
    {
        var miss = GreatNum == 0 ? SetGameObject("Miss", Silver, missNum + "M") : SetGameObject("Miss", Silver, missNum + "M " + GreatNum + "G");
        return miss;
    }

    internal static void DestroyGameObject()
    {
        Destroy(AP);
        Destroy(FC);
        Destroy(Miss);
    }

    private static GameObject SetGameObject(string name, Color color, string text)
    {
        var gameobject = CreateTextGameObject("Indicator Canvas", name, text, TextAnchor.UpperLeft, true, new Vector3(-102.398f, 367.2864f, 0f), new Vector2(960, 216), 100, SnapsTasteFont, color);
        return gameobject;
    }
}