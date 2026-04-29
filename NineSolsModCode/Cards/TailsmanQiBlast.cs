using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
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
    // protected override bool ShouldGlowRedInternal => !Owner.Creature.HasPower<QiPower>();
    // protected override bool IsPlayable => Owner.Creature.HasPower<QiPower>();

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        ArgumentNullException.ThrowIfNull(play.Target);
        await CardPileCmd.Draw(choiceContext, DynamicVars.Cards.BaseValue, Owner, false);
        await PowerCmd.Apply<InternalDamagePower>(choiceContext, play.Target, DynamicVars[InternalDamageVar.Key].BaseValue, Owner.Creature, this);
        await NineSolsModCmd.CostQi(3, this, choiceContext, false);
    }

    protected override void OnUpgrade()
    {
        DynamicVars[InternalDamageVar.Key].UpgradeValueBy(3m);
    }

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new CardsVar(1),
        new InternalDamageVar(12),
        new DynamicVar("qiMult", 25),
        NineSolsModVarsFactory.FinishVar(
            100,
            card => {
                var qiPower = card?.Owner.Creature.GetPower<QiPower>();
                var value = DynamicVars["Finish"].BaseValue;
                if (qiPower is null)
                {
                    return value;
                }
                value += Math.Max(qiPower.Amount - 1, 0) * DynamicVars["qiMult"].BaseValue;
                return value;
            }),
        new CalculationBaseVar(0m),
        new ExtraDamageVar(1m),
        new CalculatedDamageVar(ValueProp.Move).WithMultiplier(
            (card, target) => {
                var b = card.DynamicVars.CalculationBase.BaseValue;
                var creature = card.Owner.Creature;
                if (creature is null) return 0m;
                if (card.DynamicVars["Finish"] is not CalculatedVar finishVar) return 0m;
                if (target is null) return 0m;
                var finishMult = finishVar.Calculate(target);
                var internalAmount = target.GetPowerAmount<InternalDamagePower>();
                return finishMult * internalAmount;
            }
        )
    ];
}