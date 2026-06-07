using STS2RitsuLib;
using STS2RitsuLib.Data;
using STS2RitsuLib.Settings;
using STS2RitsuLib.Utils;
using STS2RitsuLib.Utils.Persistence;

namespace NineSolsMod.NineSolsModCode.Settings;

public sealed class Settings
{
    public bool ManualParryEnabled { get; set; } = false;
}

public static class SettingsPage
{
    private const string DataKey = "settings";

    // 设置绑定，可调用来查询值和改动
    private static readonly ModSettingsValueBinding<Settings, bool> ManualParryEnabledBinding = new(
        MainFile.ModId, DataKey, SaveScope.Profile,
        static s => s.ManualParryEnabled,
        static (s, v) => s.ManualParryEnabled = v);

    private static readonly I18N I18NText = RitsuLibFramework.CreateModLocalization(
        MainFile.ModId,
        instanceName: "settings",
        pckFolders: [$"res://{MainFile.ModId}/localization/settings"]
    );

    public static void Register()
    {
        // 注册 DataStore
        ModDataStore.For(MainFile.ModId).Register(
            key: DataKey, // 持久化数据ID，需要和别人防撞
            fileName: "settings.json", // 你的数据文件名
            scope: SaveScope.Profile, // Profile 表示每个存档独立，可改成 Global 表示所有存档共享
            defaultFactory: () => new Settings(),
            autoCreateIfMissing: true);

        // 注册页面UI
        RitsuLibFramework.RegisterModSettings(MainFile.ModId, page => page
            .WithTitle(ModSettingsText.I18N(I18NText, "title", "标题"))
            .WithModDisplayName(ModSettingsText.I18N(I18NText, "modDisplayName", "九日"))
            // .WithVisibleOnHostSurfaces(ModSettingsHostSurface.MainMenu | ModSettingsHostSurface.RunPause)
            .AddSection("gameplay", section => section
                .WithTitle(ModSettingsText.I18N(I18NText, "gameplay.title", "游戏玩法"))
                .AddToggle(
                    "manual_parry",
                    ModSettingsText.I18N(I18NText, "gameplay.manualParryEnabled.label", "手动格挡"),
                    ManualParryEnabledBinding,
                    description: ModSettingsText.I18N(I18NText, "gameplay.manualParryEnabled.description", "启用后玩家可以在怪物攻击时按下格挡键来格挡，成功格挡后会获得一个格挡标记，格挡标记会在回合结束时消失。"),
                    visibleWhen: () => true // 需要有协同 Mod
                )
            )
        );
    }
}