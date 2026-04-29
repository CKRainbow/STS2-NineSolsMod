using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.ValueProps;
using NineSolsMod.NineSolsModCode.Keywords;
using NineSolsMod.NineSolsModCode.Powers;
using NineSolsMod.NineSolsModCode.Variables;
using STS2RitsuLib.Interop.AutoRegistration;

namespace NineSolsMod.NineSolsModCode.Cards;

[RegisterCard(typeof(TokenCardPool))]
public class ThunderBusterArrow() : NineSolsModCard(1, CardType.Attack,
    CardRarity.Token, TargetType.AllEnemies)
{
    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        ArgumentNullException.ThrowIfNull(play.Target, "cardPlay.Target");
        await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this).Targeting(play.Target)
            .WithHitFx("vfx/vfx_attack_slash", null, null)
            .Execute(choiceContext);
        await PowerCmd.Apply<InternalDamagePower>(choiceContext, play.Target, DynamicVars[InternalDamageVar.Key].BaseValue, Owner.Creature, this, false);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(4m);
        DynamicVars[InternalDamageVar.Key].UpgradeValueBy(4m);
    }

    protected override IEnumerable<string> RegisteredKeywordIds => [
        NineSolsModKeywords.AzureSandCraft
    ];

    public override IEnumerable<CardKeyword> CanonicalKeywords => [
        CardKeyword.Exhaust,
        CardKeyword.Retain
    ];

    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DamageVar(16m, ValueProp.Move),
        new InternalDamageVar(6m)
    ];
}