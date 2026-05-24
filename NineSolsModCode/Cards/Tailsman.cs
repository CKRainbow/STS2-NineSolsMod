using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using NineSolsMod.NineSolsModCode.Character;
using NineSolsMod.NineSolsModCode.Powers;
using NineSolsMod.NineSolsModCode.Variables;
using STS2RitsuLib.Interop.AutoRegistration;

namespace NineSolsMod.NineSolsModCode.Cards;

[RegisterCard(typeof(YiCardPool))]
[RegisterCharacterStarterCard(typeof(Yi), 1)]
public class Tailsman() : NineSolsModCard(0, CardType.Skill,
    CardRarity.Basic, TargetType.AnyEnemy)
{
    // 只是颜色，并不影响能否被打出
    // protected override bool ShouldGlowRedInternal => !Owner.Creature.HasPower<QiPower>();
    // protected override bool IsPlayable => Owner.Creature.HasPower<QiPower>();

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        ArgumentNullException.ThrowIfNull(play.Target);
        await CardPileCmd.Draw(choiceContext, DynamicVars.Cards.BaseValue, Owner, false);
        await PowerCmd.Apply<InternalDamagePower>(choiceContext, play.Target, DynamicVars[NineSolsModVarsFactory.InternalDamageKey].BaseValue, Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        DynamicVars[NineSolsModVarsFactory.InternalDamageKey].UpgradeValueBy(2m);
    }

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new CardsVar(1),
        NineSolsModVarsFactory.InternalDamageVar(4)
    ];
}