using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using NineSolsMod.NineSolsModCode.Character;
using NineSolsMod.NineSolsModCode.Powers;
using NineSolsMod.NineSolsModCode.Utils;
using NineSolsMod.NineSolsModCode.Variables;
using STS2RitsuLib.Cards.DynamicVars;
using STS2RitsuLib.Interop.AutoRegistration;

namespace NineSolsMod.NineSolsModCode.Cards;

[RegisterCard(typeof(YiCardPool))]
public class TailsmanQiBlast() : NineSolsModCard(0, CardType.Skill,
    CardRarity.Ancient, TargetType.AnyEnemy)
{
    // 只是颜色，并不影响能否被打出
    protected override bool ShouldGlowRedInternal => !Owner.Creature.HasPower<QiPower>();
    protected override bool IsPlayable => Owner.Creature.HasPower<QiPower>();

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        ArgumentNullException.ThrowIfNull(play.Target);
        await CardPileCmd.Draw(choiceContext, DynamicVars.Cards.BaseValue, Owner, false);
        await PowerCmd.Apply<InternalDamagePower>(choiceContext, play.Target, DynamicVars[InternalDamageVar.Key].BaseValue, Owner.Creature, this);
        await NineSolsModCmd.CostQi(3, this, choiceContext, false);
        await NineSolsModCmd.Finish(0, this, play.Target, choiceContext);
    }

    protected override void OnUpgrade()
    {
        DynamicVars[InternalDamageVar.Key].UpgradeValueBy(6m);
        DynamicVars["MultiplierBonus"].UpgradeValueBy(10m);
    }

    // FIXME: 没有将一开始的 12 点内伤考虑进去
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new CardsVar(1),
        new InternalDamageVar(12),
        ModCardVars.Int("MultiplierBonus", 20),
        ..NineSolsModVarsFactory.FinishVar(
            0,
            (card) => {
                if (card is null) return 100m;
                if (!card.IsInCombat) return 100m;
                var qiPowerAmount = Math.Min(3, card.Owner.Creature.GetPowerAmount<QiPower>());
                var value = 100m;
                for (int i = 0; i < qiPowerAmount; i++)
                {
                    value *= 1 + DynamicVars["MultiplierBonus"].BaseValue / 100m;
                }
                return value;
            }),
    ];
}