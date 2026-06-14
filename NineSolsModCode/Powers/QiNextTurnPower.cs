using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.HoverTips;
using NineSolsMod.NineSolsModCode.HoverTips;
using NineSolsMod.NineSolsModCode.Utils;
using STS2RitsuLib.Interop.AutoRegistration;

namespace NineSolsMod.NineSolsModCode.Powers;

[RegisterPower]
public class QiNextTurnPower : NineSolsModPower
{
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    protected override IEnumerable<IHoverTip> AdditionalHoverTips => [
        NineSolsModHoverTipFactory.Qi(this)
    ];

    public override async Task AfterSideTurnStart(CombatSide side, IReadOnlyList<Creature> participants, ICombatState combatState)
    {
        if (participants.Contains(Owner) && AmountOnTurnStart != 0 && Owner.IsPlayer && Owner.Player != null)
        {
            await NineSolsModCmd.GainQi(Owner.Player, AmountOnTurnStart);
            await PowerCmd.Remove(this);
        }
    }
}