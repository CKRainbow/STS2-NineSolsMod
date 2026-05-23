using Godot;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Rewards;
using STS2RitsuLib.Interop.AutoRegistration;

namespace NineSolsMod.NineSolsModCode.Powers;

[RegisterPower]
public class TransmuteUntoWealthRewardPower : NineSolsModPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    public override Color AmountLabelColor => _normalAmountLabelColor;

    public override Task AfterCombatEnd(CombatRoom room)
    {
        if (Owner != null && Owner.Player != null)
        {
            room.AddExtraReward(Owner.Player, new GoldReward(Amount, Owner.Player, false));
        }
        return Task.CompletedTask;
    }
}
