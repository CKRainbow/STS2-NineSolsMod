using BaseLib.Hooks;
using Godot;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;

namespace NineSolsMod.NineSolsModCode.Powers;

public class MedicalJadePower : NineSolsModPower, IHealAmountModifier
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    public override Color AmountLabelColor => _normalAmountLabelColor;

    public decimal ModifyHealMultiplicative(Creature creature, decimal amount)
    {
        if (creature != Owner)
        {
            return 1m;
        }
        return 1m + Amount / 100m;
    }

}