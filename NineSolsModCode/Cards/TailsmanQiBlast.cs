using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using NineSolsMod.NineSolsModCode.Character;
using NineSolsMod.NineSolsModCode.Powers;
using NineSolsMod.NineSolsModCode.Utils;
using NineSolsMod.NineSolsModCode.Variables;
using STS2RitsuLib.Cards.DynamicVars;
using STS2RitsuLib.Combat.SecondaryResources;
using STS2RitsuLib.Interop.AutoRegistration;

namespace NineSolsMod.NineSolsModCode.Cards;

// FIXME: 暂时无法简单实现最高使用 3 费的效果
[RegisterCard(typeof(YiCardPool))]
public class TailsmanQiBlast() : NineSolsModQiCard(0, CardType.Skill,
    CardRarity.Ancient, TargetType.AnyEnemy, 0, qiRequired: true), ISecondaryResourceHookListener
{
    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        ArgumentNullException.ThrowIfNull(play.Target);
        await CardPileCmd.Draw(choiceContext, DynamicVars.Cards.BaseValue, Owner, false);
        await PowerCmd.Apply<InternalDamagePower>(choiceContext, play.Target, DynamicVars[NineSolsModVarsFactory.InternalDamageKey].BaseValue, Owner.Creature, this);
        var ledger = play.SecondaryResources();
        var qiValue = ledger.Value(QiResource.RequiredQiUseId);
        await NineSolsModCmd.Finish(0, this, play.Target, choiceContext, inputFinishMult: CalcFinishMult(qiValue) / 100m);
    }

    // public int ModifySecondaryResourceXValue(SecondaryResourceXContext context, int value)
    // {
    //     if (context.Card == this)
    //     {
    //         value = Math.Min(3, value);
    //     }
    //     return value;
    // }

    protected override void OnUpgrade()
    {
        DynamicVars[NineSolsModVarsFactory.InternalDamageKey].UpgradeValueBy(6m);
        DynamicVars["MultiplierBonus"].UpgradeValueBy(10m);
    }

    private decimal CalcFinishMult(int qiAmount)
    {
        var value = 100m;
        for (int i = 0; i < qiAmount; i++)
        {
            value *= 1 + DynamicVars["MultiplierBonus"].BaseValue / 100m;
        }
        return value;
    }

    // FIXME: 没有将一开始的 12 点内伤考虑进去
    // FIXME: 可能要调整基类
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new CardsVar(1),
        NineSolsModVarsFactory.InternalDamageVar(12),
        ModCardVars.Int("MultiplierBonus", 20),
        NineSolsModVarsFactory.QiCostVar(3),
        ..NineSolsModVarsFactory.FinishVar(
            100m,
            (card) => {
                if (card is null) return 100m;
                if (!card.IsInCombat) return 100m;
                var qiAmount = Math.Min(5, SecondaryResourceCmd.Get(card.Owner, QiResource.QiId));
                return CalcFinishMult(qiAmount);
            }),
    ];
}