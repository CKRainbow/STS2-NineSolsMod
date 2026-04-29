using Godot;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Models;
using STS2RitsuLib.Interop.AutoRegistration;

namespace NineSolsMod.NineSolsModCode.Powers;

[RegisterPower]
public class ComputingPowerOverclockPower : NineSolsModPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    public override Color AmountLabelColor => _normalAmountLabelColor;

    public override bool TryModifyEnergyCostInCombat(CardModel card, decimal originalCost, out decimal modifiedCost)
    {
        modifiedCost = originalCost;
        if (card.Owner.Creature != Owner)
        {
            return false;
        }

        if (card.Type != CardType.Power)
        {
            return false;
        }

        if (originalCost <= 0m)
        {
            return false;
        }

        modifiedCost = originalCost - Amount;
        if (modifiedCost < 0m)
        {
            modifiedCost = default;
        }

        return true;
    }
}