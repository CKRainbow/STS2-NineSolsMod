using Godot;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using STS2RitsuLib.Interop.AutoRegistration;

namespace NineSolsMod.NineSolsModCode.Powers;

[RegisterPower]
public class GreatTaoFormPower : NineSolsModPower
{
    private class Data
    {
        public int internalDamageToApply = 0;
        public CardModel? cardSource = null;
        public Creature? dealer = null;
    }

    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Single;
    public override Color AmountLabelColor => _normalAmountLabelColor;

    protected override object? InitInternalData()
    {
        return new Data();
    }


    public override decimal ModifyHpLostAfterOstyLate(Creature? target, decimal amount, ValueProp props, Creature? dealer, CardModel? cardSource)
    {
        if (!CombatManager.Instance.IsInProgress)
        {
            return amount;
        }
        if (target != Owner)
        {
            return amount;
        }
        // 不只影响攻击伤害
        GetInternalData<Data>().internalDamageToApply = (int)amount;
        GetInternalData<Data>().cardSource = cardSource;
        GetInternalData<Data>().dealer = dealer;
        return 0;
    }

    public override async Task AfterModifyingHpLostAfterOsty()
    {
        Flash();
        if (GetInternalData<Data>().internalDamageToApply > 0)
        {
            await PowerCmd.Apply<InternalDamagePower>(
                new ThrowingPlayerChoiceContext(),
                Owner,
                GetInternalData<Data>().internalDamageToApply,
                GetInternalData<Data>().dealer,
                GetInternalData<Data>().cardSource,
                false
            );
        }
        GetInternalData<Data>().internalDamageToApply = 0;
        GetInternalData<Data>().cardSource = null;
        GetInternalData<Data>().dealer = null;
    }

    public override decimal ModifyDamageCap(Creature? target, ValueProp props, Creature? dealer, CardModel? cardSource)
    {
        if (target != Owner)
        {
            return decimal.MaxValue;
        }
        if (props.IsPoweredAttack())
        {
            return decimal.MaxValue;
        }
        return 0;
    }

    public override Task AfterModifyingDamageAmount(CardModel? cardSource)
    {
        Flash();
        return Task.CompletedTask;
    }

    public override async Task BeforeTurnEnd(PlayerChoiceContext choiceContext, CombatSide side)
    {
        if (CombatManager.Instance.IsOverOrEnding)
            return;
        if (side != Owner.Side)
            return;
        if (Owner.IsDead)
            return;

        if (Owner.GetPowerAmount<InternalDamagePower>() >= Owner.CurrentHp)
        {
            await CreatureCmd.Kill(Owner, false);
        }
    }
}