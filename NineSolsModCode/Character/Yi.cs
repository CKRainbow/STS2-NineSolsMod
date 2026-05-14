using Godot;
using MegaCrit.Sts2.Core.Entities.Characters;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Combat;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Characters;
using STS2RitsuLib.Scaffolding.Godot;
using STS2RitsuLib.Scaffolding.Visuals.Definition;
using STS2RitsuLib.Scaffolding.Visuals.StateMachine;

namespace NineSolsMod.NineSolsModCode.Character;

[RegisterCharacter]
public class Yi : ModCharacterTemplate<YiCardPool, YiRelicPool, YiPotionPool>
{
    public const string CharacterId = "Yi";

    public static readonly Color Color = new("4a9b8c");

    public override Color NameColor => Color;
    public override Color EnergyLabelOutlineColor => Color;
    public override CharacterGender Gender => CharacterGender.Masculine;
    public override int StartingHp => 75;
    public override int StartingGold => 99;

    public override string? PlaceholderCharacterId => "Nicrobinder";

    // 角色名称颜色
    public override CharacterAssetProfile AssetProfile => CharacterAssetProfiles.Merge(
        CharacterAssetProfiles.Ironclad(),
        new(
            VisualCues: new VisualCueSet(

            ),
            Scenes: new(
                // 人物模型tscn路径。
                VisualsPath: $"res://{MainFile.ModId}/scenes/creature_visuals/yi.tscn",
                // 能量表盘tscn路径。
                EnergyCounterPath: $"res://{MainFile.ModId}/scenes/combat/energy_counters/yi_energy_counter.tscn"
            // FIXME: 商店人物场景。
            // MerchantAnimPath: SceneHelper.GetScenePath("merchant/characters/necrobinder_merchant"),
            // FIXME: 篝火休息场景。
            // RestSiteAnimPath: SceneHelper.GetScenePath("rest_site/characters/necrobinder_rest_site")
            ),
            Ui: new(
                // 人物头像路径。
                IconTexturePath: $"res://{MainFile.ModId}/images/charui/character_icon_yi.png",
                // 人物头像2号。
                IconPath: $"res://{MainFile.ModId}/scenes/ui/character_icons/yi_icon.tscn",
                // 人物选择背景。
                CharacterSelectBgPath: $"res://{MainFile.ModId}/scenes/ui/char_select/char_select_bg_yi.tscn",
                // 人物选择图标。
                CharacterSelectIconPath: $"res://{MainFile.ModId}/images/charui/char_select_yi.png",
                // 人物选择图标-锁定状态。
                CharacterSelectLockedIconPath: $"res://{MainFile.ModId}/images/charui/char_select_yi_locked.png"
            // FIXME: 人物选择过渡动画。
            // CharacterSelectTransitionPath: "res://materials/transitions/ironclad_transition_mat.tres",
            // FIXME: 地图上的角色标记图标、表情轮盘上的角色头像
            // MapMarkerPath: ImageHelper.GetImagePath("packed/map/icons/map_marker_necrobinder.png")
            ),
            Vfx: new(
            // FIXME: 卡牌拖尾场景。
            // TrailPath: "res://scenes/vfx/card_trail_ironclad.tscn"
            ),
            // FIXME
            Audio: new(
            // 攻击音效
            // AttackSfx: null,
            // 施法音效
            // CastSfx: null,
            // 死亡音效
            // DeathSfx: null,
            // 角色选择音效
            // CharacterSelectSfx: null,
            // 过渡音效
            // CharacterTransitionSfx: "event:/sfx/ui/wipe_ironclad"
            ),
            Multiplayer: new(
            // 多人模式-手指。
            // ArmPointingTexturePath: null,
            // 多人模式剪刀石头布-石头。
            // ArmRockTexturePath: null,
            // 多人模式剪刀石头布-布。
            // ArmPaperTexturePath: null,
            // 多人模式剪刀石头布-剪刀。
            // ArmScissorsTexturePath: null
            )));

    // 攻击和施法动画延迟，以对齐动画
    public override float AttackAnimDelay => 0f;
    public override float CastAnimDelay => 0f;

    // 自动转换人物场景，让你不需要手动挂脚本。复制即可。
    protected override NCreatureVisuals? TryCreateCreatureVisuals() => RitsuGodotNodeFactories.CreateFromScenePath<NCreatureVisuals>(AssetProfile.Scenes!.VisualsPath!);

    // TODO: 考虑修改一下受伤动画，目前有点突兀
    protected override ModAnimStateMachine? SetupCustomCombatAnimationStateMachine(
    Node visualsRoot,
    CharacterModel character)
    {
        return ModAnimStateMachines.StandardCue(visualsRoot, character,
            idleName: "idle_loop",
            deadName: "die", deadLoop: false,
            hitName: "hurt", hitLoop: false,
            attackName: "attack", attackLoop: false,
            castName: "cast", castLoop: false
        );
    }

    // 攻击建筑师的攻击特效列表
    public override List<string> GetArchitectAttackVfx() => [
        "vfx/vfx_attack_blunt",
        "vfx/vfx_heavy_blunt",
        "vfx/vfx_attack_slash",
        "vfx/vfx_bloody_impact",
        "vfx/vfx_rock_shatter"
    ];

    // public override NCreatureVisuals? CreateCustomVisuals()
    // {
    //     var nCreatureVisuals = NodeFactory<NCreatureVisuals>.CreateFromScene(CustomVisualPath);
    //     var visuals = nCreatureVisuals.GetNodeOrNull<AnimatedSprite2D>("%AnimatedSprite2D");
    //     if (visuals is not null)
    //     {
    //         MainFile.Logger.Debug("Set animation transition for AnimatedSprite2D");
    //         visuals.Animation = "idle_loop";
    //         visuals.AnimationFinished += () =>
    //         {
    //             if (visuals.Animation != "die")
    //             {
    //                 visuals.Play("idle_loop");
    //             }
    //         };
    //     }
    //     return nCreatureVisuals;
    // }
}