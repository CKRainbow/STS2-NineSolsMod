using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using NineSolsMod.NineSolsModCode.Combat;
using STS2RitsuLib.Combat.HealthBars;
using STS2RitsuLib.Interop.AutoRegistration;

namespace NineSolsMod.NineSolsModCode.Powers;

[RegisterPower]
public class InternalDamagePower : NineSolsModPower, IHealthBarForecastSource
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

    // public override decimal ModifyDamageAdditive(Creature? target, decimal amount, ValueProp props, Creature? dealer, CardModel? cardSource)
    // {
    //     if (target != Owner)
    //     {
    //         return 0m;
    //     }
    //     var damageperamount = dynamicvars["damageperamount"].basevalue;
    //     // 只对直接作用在血量上的攻击提供加成
    //     var internaldamageamount = amount;
    //     getinternaldata<data>().effectiveamount = math.min(internaldamageamount, amount);
    //     return getinternaldata<data>().effectiveamount * damageperamount;
    // }

    public override decimal ModifyHpLostAfterOsty(Creature target, decimal amount, ValueProp props, Creature? dealer, CardModel? cardSource)
    {
        if (target != Owner)
        {
            return amount;
        }

        var damagePerAmount = DynamicVars["DamagePerAmount"].BaseValue;
        var internalDamageAmount = Amount;
        var effectiveAmount = Math.Min(internalDamageAmount, amount);
        GetInternalData<Data>().effectiveAmount = effectiveAmount;
        return amount + effectiveAmount * damagePerAmount;
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

    /// <inheritdoc/>
    /// <remarks>
    /// TODO: 使用暗红色表示内伤预告
    /// </remarks>
    public IEnumerable<HealthBarForecastSegment> GetHealthBarForecastSegments(HealthBarForecastContext context)
    {
        if (context.Creature != Owner)
            return [];


        var order = HealthBarForecastOrder.ForSideTurnStart(context.Creature, Owner.Side);
        return HealthBarForecasts
            .FromRight(context, new(1f, 0.478f, 0f), Colors.White)
            .Add(Amount, order, InternalDamagePowerHealthBarForecastMaterials.ForecastMaterial)
            .Build();
    }
}