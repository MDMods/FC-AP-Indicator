using Il2CppSystem.IO;
using MelonLoader;
using Tomlet;
using UnhollowerRuntimeLib;
using UnityEngine;
using static MuseDashMirror.CommonPatches.PatchEvents;
using static MuseDashMirror.BattleComponent;
using static MuseDashMirror.SceneInfo;
using static FC_AP.Indicator;

namespace FC_AP;

public class Main : MelonMod
{
    public override void OnInitializeMelon()
    {
        Save.Load();
        PnlMenuEvent += Patch.PnlMenuPostfix;
        SwitchLanguagesEvent += Patch.SwitchLanguagesPostfix;
        MenuSelectEvent += DisableToggle;
        EnterGameScene += RegisterGameObject;
        GameStartEvent += SetAP;
        OnVictoryEvent += DestroyGameObject;
        AddScoreEvent += UpdateFCGameObject;
        MissCubeEvent += UpdateMissGameObject;
        LoggerInstance.Msg("FC/AP indicator is loaded!");
    }

    public override void OnDeinitializeMelon()
    {
        File.WriteAllText(Path.Combine("UserData", "FC AP.cfg"), TomletMain.TomlStringFrom(Save.Settings));
    }

    private void RegisterGameObject()
    {
        if (!Save.Settings.FC_APEnabled) return;
        ClassInjector.RegisterTypeInIl2Cpp<Indicator>();
        var gameObject = new GameObject("Indicator");
        gameObject.AddComponent<Indicator>();
    }

    private void DisableToggle(int listIndex, int index, bool isOn)
    {
        if (listIndex == 0 && index == 0 && isOn)
        {
            Patch.FC_APToggle.SetActive(true);
        }
        else
        {
            Patch.FC_APToggle.SetActive(false);
        }
    }
}