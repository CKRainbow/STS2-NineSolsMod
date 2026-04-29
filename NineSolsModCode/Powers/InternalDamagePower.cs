using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using STS2RitsuLib.Interop.AutoRegistration;

namespace NineSolsMod.NineSolsModCode.Powers;

[RegisterPower]
public class InternalDamagePower : NineSolsModPower
{
    private class Data
    {
        public decimal effectiveAmount = 0m;
    }

    public override PowerType Type => PowerType.Debuff;
    public override PowerStackType StackType => PowerStackType.Counter;
    public override Color AmountLabelColor => _normalAmountLabelColor;

    protected override object? InitInternalData()
    {
        return new Data();
    }

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DynamicVar("DamagePerAmount", 1m)
    ];

    public override decimal ModifyDamageAdditive(Creature? target, decimal amount, ValueProp props, Creature? dealer, CardModel? cardSource)
    {
        if (target != Owner)
        {
            return 0m;
        }
        var damagePerAmount = DynamicVars["DamagePerAmount"].BaseValue;
        var internalDamageAmount = Amount;
        GetInternalData<Data>().effectiveAmount = Math.Min(internalDamageAmount, amount);
        return GetInternalData<Data>().effectiveAmount * damagePerAmount;
    }

    public override async Task AfterDamageReceived(PlayerChoiceContext choiceContext, Creature target, DamageResult result, ValueProp props, Creature? dealer, CardModel? cardSource)
    {
        if (target == Owner)
        {
            if (result.TotalDamage != 0)
            {
                Flash();
                await PowerCmd.ModifyAmount(choiceContext, this, -GetInternalData<Data>().effectiveAmount, null, null);
                GetInternalData<Data>().effectiveAmount = 0m;
            }
        }
    }

}