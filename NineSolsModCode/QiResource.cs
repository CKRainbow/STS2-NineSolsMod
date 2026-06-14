using Godot;
using NineSolsMod.NineSolsModCode.Character;
using NineSolsMod.NineSolsModCode.Nodes;
using STS2RitsuLib;
using STS2RitsuLib.Combat.SecondaryResources;

namespace NineSolsMod.NineSolsModCode;

public static class QiResource
{
    public static SecondaryResourceDefinition QiDefinition { get; private set; } = null!;
    public static string QiId { get; private set; } = string.Empty;
    public static readonly string RequiredQiUseId = "qi_required";
    public static readonly string OptionalQiUseId = "qi_optional";

    public static void Register()
    {
        var registry = RitsuLibFramework.GetSecondaryResourceRegistry(MainFile.ModId);

        QiDefinition = registry.Register("qi", new(
            defaultAmount: 0,
            baseMaxAmount: 5,
            hardMaxAmount: 5,
            turnStartPolicy: SecondaryResourceTurnStartPolicy.None,
            persistencePolicy: SecondaryResourcePersistencePolicy.Combat,
            smallIconPath: $"res://{MainFile.ModId}/images/charui/qi.png",
            largeIconPath: $"res://{MainFile.ModId}/images/charui/qi.png"
        ));
        QiId = QiDefinition.Id;

        // 战斗计数器。使用的图标就是你注册时提供的图标
        registry.RegisterCombatUi(
            "qi_combat_counter",
            parent =>
            {
                // var row = NSecondaryResourceCounter.Create(QiDefinition, new SecondaryResourceCounterStyle
                // {
                //     FontSize = 32,
                //     PositiveColor = Colors.Cyan,
                //     FormatAmount = (amount, max) => amount.ToString(),
                //     IconStyle = SecondaryResourceIconStyle.Default with
                //     {
                //         Size = new Vector2(80, 80),
                //         HoverTip = SecondaryResourceHoverTipStyle.Default,
                //     },
                // });
                var row = NQiCounter.Create(
                    QiDefinition,
                    secondaryResourceIconStyle: SecondaryResourceIconStyle.Default with
                    {
                        Size = new(48, 48),
                        HoverTip = SecondaryResourceHoverTipStyle.Default
                    }
                );
                // 自由指定位置。例如这里我们找到能量计数器的位置，放在它旁边
                var energyCounter = parent.GetNode<Control>("%EnergyCounterContainer");
                row.Position = energyCounter.Position + new Vector2(0, -60);
                return row;
            },
            ctx => ctx.Node.Bind(ctx.Player)
        );

        // 卡牌面上的次级资源费用显示。使用的图标就是你注册时提供的图标
        registry.RegisterCardUi(
            "qi_card_ui",
            parent =>
            {
                var ui = NSecondaryResourceCardCostUi.Create(QiId, new SecondaryResourceCardCostUiStyle
                {
                    IconSize = new Vector2(48, 48),
                    FontSize = 24,
                });
                // 自由指定位置。例如这里我们找到能量图标的位置，放在它旁边
                var energyIcon = parent.GetNode<TextureRect>("%EnergyIcon");
                ui.Position = energyIcon.Position + new Vector2(0, 80);
                return ui;
            },
            ctx => ctx.Node.Refresh(ctx)
        );

        // 限定仅对特定角色始终显示
        registry.AlwaysShowInCombatUiForCharacter<Yi>(QiDefinition.LocalId);
        // 永远显示（不受角色限制）
        // registry.AlwaysShowInCombatUi(QiDefinition.LocalId);
    }
}