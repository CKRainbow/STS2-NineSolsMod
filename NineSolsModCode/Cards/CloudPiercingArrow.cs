using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.ValueProps;
using NineSolsMod.NineSolsModCode.Tags;
using STS2RitsuLib.CardTags;
using STS2RitsuLib.Interop.AutoRegistration;

namespace NineSolsMod.NineSolsModCode.Cards;

[RegisterCard(typeof(TokenCardPool))]
public class CloudPiercingArrow() : NineSolsModCard(1, CardType.Attack,
    CardRarity.Token, TargetType.AllEnemies)
{
    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this).TargetingAllOpponents(CombatState!)
                    .WithHitFx("vfx/vfx_attack_blunt", null, "heavy_attack.mp3")
                    .Execute(choiceContext);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(4m);
    }

    public override IEnumerable<CardKeyword> CanonicalKeywords => [
        CardKeyword.Exhaust,
        CardKeyword.Retain,
    ];

    protected override HashSet<CardTag> CanonicalTags => [
        NineSolsModTags.AzureSandCraft.GetModCardTag()
    ];

    protected override IEnumerable<IHoverTip> AdditionalHoverTips => [
    ];

    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DamageVar(14m, ValueProp.Move)
    ];
}