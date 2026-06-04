using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using STS2RitsuLib.Interop.AutoRegistration;

namespace NineSolsMod.NineSolsModCode.Powers;

[RegisterPower]
public class RevivalJadePower : NineSolsModPower
{
    private class Data
    {
        public bool isReviving;
    }

    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    public override PowerInstanceType InstanceType => PowerInstanceType.Instanced;
    public override Color AmountLabelColor => _normalAmountLabelColor;
    private bool IsReviving => GetInternalData<Data>().isReviving;

    protected override object InitInternalData()
    {
        return new Data();
    }

    public override decimal ModifyDamageMultiplicative(Creature? target, decimal amount, ValueProp props, Creature? dealer, CardModel? cardSource)
    {
        if (target != Owner)
        {
            return 1m;
        }

        if (!IsReviving)
        {
            return 1m;
        }

        return 0m;
    }

    /// <summary>
    /// 这个是为了干什么？
    /// </summary>
    /// <param name="canonicalPower"></param>
    /// <param name="target"></param>
    /// <param name="amount"></param>
    /// <param name="applier"></param>
    /// <param name="modifiedAmount"></param>
    /// <returns></returns>
    public override bool TryModifyPowerAmountReceived(PowerModel canonicalPower, Creature target, decimal amount, Creature? applier, out decimal modifiedAmount)
    {
        modifiedAmount = amount;
        if (target != Owner)
        {
            return false;
        }

        if (!IsReviving)
        {
            return false;
        }

        return true;
    }

    public override bool ShouldAllowHitting(Creature creature)
    {
        return !IsReviving;
    }

    public override bool ShouldDie(Creature creature)
    {
        if (creature != Owner)
        {
            return true;
        }

        return false;
    }

    public override bool ShouldStopCombatFromEnding()
    {
        return true;
    }

    public override async Task AfterPreventingDeath(Creature creature)
    {
        if (creature == Owner)
        {
            GetInternalData<Data>().isReviving = true;
            var healAmount = Math.Max(Owner.MaxHp * Amount / 100, 1m);
            await CreatureCmd.Heal(Owner, healAmount, playAnim: false);
            await PowerCmd.Remove(this);
            foreach (var power in Owner.Powers)
            {
                if (power.Type == PowerType.Debuff)
                {
                    await PowerCmd.Remove(power);
                }
            }
        }
    }
}