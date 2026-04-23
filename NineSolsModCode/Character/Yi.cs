using BaseLib.Abstracts;
using BaseLib.Utils.NodeFactories;
using NineSolsMod.NineSolsModCode.Extensions;
using Godot;
using MegaCrit.Sts2.Core.Animation;
using MegaCrit.Sts2.Core.Bindings.MegaSpine;
using MegaCrit.Sts2.Core.Entities.Characters;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.Nodes.Combat;
using NineSolsMod.NineSolsModCode.Cards;
using NineSolsMod.NineSolsModCode.Relics;
using Parry = NineSolsMod.NineSolsModCode.Cards.Parry;

namespace NineSolsMod.NineSolsModCode.Character;

public class Yi : PlaceholderCharacterModel
{
    public const string CharacterId = "Yi";

    public static readonly Color Color = new("4a9b8c");

    public override Color NameColor => Color;
    public override Color EnergyLabelOutlineColor => Color;
    public override CharacterGender Gender => CharacterGender.Masculine;
    public override int StartingHp => 75;

    public override IEnumerable<CardModel> StartingDeck =>
    [
        ModelDb.Card<StrikeYi>(),
        ModelDb.Card<StrikeYi>(),
        ModelDb.Card<StrikeYi>(),
        ModelDb.Card<StrikeYi>(),
        ModelDb.Card<DefendYi>(),
        ModelDb.Card<DefendYi>(),
        ModelDb.Card<DefendYi>(),
        ModelDb.Card<DefendYi>(),
        ModelDb.Card<Parry>(),
        ModelDb.Card<Tailsman>(),
    ];

    public override IReadOnlyList<RelicModel> StartingRelics =>
    [
        ModelDb.Relic<Pipe>()
    ];

    public override CardPoolModel CardPool => ModelDb.CardPool<YiCardPool>();
    public override RelicPoolModel RelicPool => ModelDb.RelicPool<YiRelicPool>();
    public override PotionPoolModel PotionPool => ModelDb.PotionPool<YiPotionPool>();

    /*  PlaceholderCharacterModel will utilize placeholder basegame assets for most of your character assets until you
        override all the other methods that define those assets.
        These are just some of the simplest assets, given some placeholders to differentiate your character with.
        You don't have to, but you're suggested to rename these images. */
    public override string CustomIconTexturePath => "character_icon_yi.png".CharacterUiPath();
    public override string CustomIconPath =>
        "res://NineSolsMod/scenes/ui/character_icons/yi_icon.tscn";

    public override string CustomEnergyCounterPath =>
        "res://NineSolsMod/scenes/combat/energy_counters/yi_energy_counter.tscn";
    public override string CustomCharacterSelectIconPath => "char_select_yi.png".CharacterUiPath();
    public override string CustomCharacterSelectLockedIconPath => "char_select_yi_locked.png".CharacterUiPath();
    public override string CustomCharacterSelectBg => "char_select_bg_yi.tscn".CharacterUiPath();

    // 这个应该比较容易做，要不用一个树的根须？
    public override string CustomMapMarkerPath => ImageHelper.GetImagePath("packed/map/icons/map_marker_necrobinder.png");

    public override string CharacterSelectSfx => "event:/sfx/characters/necrobinder/necrobinder_select";
    // public override string CustomVisualPath => SceneHelper.GetScenePath("creature_visuals/necrobinder");
    public override string CustomVisualPath => "res://NineSolsMod/scenes/creature_visuals/yi.tscn";
    public override NCreatureVisuals? CreateCustomVisuals()
    {
        var nCreatureVisuals = NodeFactory<NCreatureVisuals>.CreateFromScene(CustomVisualPath);
        var visuals = nCreatureVisuals.GetNodeOrNull<AnimatedSprite2D>("%AnimatedSprite2D");
        if (visuals is not null)
        {
            MainFile.Logger.Debug("Set animation transition for AnimatedSprite2D");
            visuals.Animation = "idle_loop";
            visuals.AnimationFinished += () =>
            {
                if (visuals.Animation != "die")
                {
                    visuals.Play("idle_loop");
                }
            };
        }
        return nCreatureVisuals;
    }
    public override string CustomTrailPath => SceneHelper.GetScenePath("vfx/card_trail_necrobinder");

    public override string CustomRestSiteAnimPath => SceneHelper.GetScenePath("rest_site/characters/necrobinder_rest_site");
    public override string CustomMerchantAnimPath => SceneHelper.GetScenePath("merchant/characters/necrobinder_merchant");

    // 人物选择过渡动画。
    // public override string CustomCharacterSelectTransitionPath => "res://materials/transitions/ironclad_transition_mat.tres";
    // 攻击音效
    // public override string CustomAttackSfx => null;
    // 施法音效
    // public override string CustomCastSfx => null;
    // 死亡音效
    // public override string CustomDeathSfx => null;
    // 角色选择音效
    // public override string CharacterSelectSfx => null;
    // 过渡音效。这个不能删。
    public override string CharacterTransitionSfx => "event:/sfx/ui/wipe_ironclad";

    public override CreatureAnimator? SetupCustomAnimationStates(MegaSprite controller)
    {
        MainFile.Logger.Info("SetupCustomAnimationStates Success!");
        return SetupAnimationState(controller, "idle_loop", "die", false, "hurt", false, "attack", false, "cast", false,
            "relaxed_loop", true);
    }
}