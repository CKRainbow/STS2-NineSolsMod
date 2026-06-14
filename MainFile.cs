using System.Reflection;
using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Modding;
using NineSolsMod.NineSolsModCode;
using NineSolsMod.NineSolsModCode.Cards;
using NineSolsMod.NineSolsModCode.Settings;
using STS2RitsuLib;
using STS2RitsuLib.Interop;

namespace NineSolsMod;

[ModInitializer(nameof(Initialize))]
public partial class MainFile : Node
{
    public const string ModId = "NineSolsMod"; //Used for resource filepath

    public static MegaCrit.Sts2.Core.Logging.Logger Logger { get; } =
        new(ModId, MegaCrit.Sts2.Core.Logging.LogType.Generic);

    public static void Initialize()
    {
        var assembly = Assembly.GetExecutingAssembly();
        RitsuLibFramework.EnsureGodotScriptsRegistered(assembly, Logger);
        ModTypeDiscoveryHub.RegisterModAssembly(ModId, assembly);

        var content = RitsuLibFramework.GetContentRegistry(ModId);

        // 设置先古卡升级
        RitsuLibFramework.RegisterArchaicToothTranscendenceMapping<Tailsman, TailsmanQiBlast>();
        // 设置先古遗物升级
        // RitsuLibFramework.RegisterTouchOfOrobasRefinementMapping<TestRelic, Akabeko>();

        // 注册设置
        SettingsPage.Register();

        // 注册次级资源
        QiResource.Register();

        Harmony harmony = new(ModId);
        harmony.PatchAll();
    }
}