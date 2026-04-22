using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using NineSolsMod.NineSolsModCode.Cards;
using NineSolsMod.NineSolsModCode.Utils;

namespace NineSolsMod.NineSolsModCode.Powers;

public class QiNextTurnPower : NineSolsModPower
{
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [
        HoverTipFactory.FromPower<QiPower>()
    ];

    public override async Task AfterSideTurnStart(CombatSide side, CombatState combatState)
    {
        if (side == Owner.Side && AmountOnTurnStart != 0)
        {
            await NineSolsModCmd.GainQi(AmountOnTurnStart, Owner);
            await PowerCmd.Remove(this);
        }
    }
}