using Godot;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Rewards;
using MegaCrit.Sts2.Core.Rooms;
using NineSolsMod.NineSolsModCode.Utils;
using STS2RitsuLib.Interop.AutoRegistration;

namespace NineSolsMod.NineSolsModCode.Powers;

[RegisterPower]
public class TransmuteUntoPowerPower : NineSolsModPower, IFinishListener
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    public override Color AmountLabelColor => _normalAmountLabelColor;

    public async Task AfterFinish(AfterFinishContext context)
    {
        if (Owner == null || context.SourceCard.Owner.Creature != Owner) return;
        if (Owner.Player is null) return;

        if (context.TriggeredFatalNum > 0)
        {
            var player = context.SourceCard.Owner;
            Flash();
            if (CombatManager.Instance.IsOverOrEnding)
            {
                if (CombatState.RunState.CurrentRoom is CombatRoom combatRoom)
                {
                    combatRoom.AddExtraReward(Owner.Player, new PotionReward(Owner.Player));
                }
            }
            else
            {
                await PowerCmd.Apply<TransmuteUntoPowerRewardPower>(context.ChoiceContext ?? new ThrowingPlayerChoiceContext(), player.Creature, Amount * context.TriggeredFatalNum, player.Creature, context.SourceCard, false);
            }
        }
    }
}
