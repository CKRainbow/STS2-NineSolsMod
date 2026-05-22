using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using NineSolsMod.NineSolsModCode.Powers;
using NineSolsMod.NineSolsModCode.Hooks;
using NineSolsMod.NineSolsModCode.Variables;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace NineSolsMod.NineSolsModCode.Utils;

public static class NineSolsModCmd
{
    /// <summary>
    /// “终结”机制的统一方法，先移除InternalDamagePower，再造成伤害
    /// </summary>
    /// <param name="baseAttack"></param>
    /// <param name="model"></param>
    /// <param name="target"></param>
    /// <param name="choiceContext"></param>
    /// <param name="calculated">是否使用CalculatedFinishVar</param>
    /// <returns></returns>
    public static async Task Finish(decimal baseAttack, CardModel model, Creature target, PlayerChoiceContext? choiceContext = null)
    {
        var internalDamageAmount = target.GetPowerAmount<InternalDamagePower>();

        decimal finishMult;
        var finishVar = model.DynamicVars["Finish"];
        if (finishVar is CalculatedVar calculatedFinishVar)
        {
            finishMult = calculatedFinishVar.Calculate(target) / 100m;
        }
        else
        {
            finishMult = finishVar.BaseValue / 100m;
        }

        await PowerCmd.Remove<InternalDamagePower>(target);

        if (baseAttack > 0)
        {
            await DamageCmd.Attack(baseAttack).FromCard(model).Targeting(target)
                .WithHitFx("vfx/vfx_attack_slash", null, null)
                .Execute(choiceContext);
        }

        if (internalDamageAmount <= 0)
        {
            return;
        }

        var beforeContext = new BeforeFinishContext
        {
            ChoiceContext = choiceContext,
            SourceCard = model,
            Target = target,
            FinishMult = finishMult,
            InternalDamageAmount = internalDamageAmount,
            TargetsToDamage = [target]
        };

        ArgumentNullException.ThrowIfNull(model.Owner.Creature.CombatState);
        await FinishHook.BeforeFinish(model.Owner.Creature.CombatState, beforeContext);

        var damageContext = choiceContext ?? new ThrowingPlayerChoiceContext();
        // // 不受任何加成影响的伤害
        VfxCmd.PlayOnCreatureCenters(beforeContext.TargetsToDamage, "vfx/vfx_attack_blunt");
        await CreatureCmd.Damage(damageContext, beforeContext.TargetsToDamage, finishMult * internalDamageAmount, ValueProp.Unpowered, model.Owner.Creature, model);

        var afterContext = new AfterFinishContext
        {
            ChoiceContext = choiceContext,
            SourceCard = model,
            Target = target,
            FinishMult = finishMult,
            InternalDamageAmount = internalDamageAmount,
            DamagedTargets = beforeContext.TargetsToDamage
        };

        await FinishHook.AfterFinish(model.Owner.Creature.CombatState, afterContext);

    }

    public static async Task Deviation(CardModel model, Creature target, PlayerChoiceContext? choiceContext = null)
    {
        var deviationAmount = model.DynamicVars[DeviationVar.Key].BaseValue;
        var context = choiceContext ?? new ThrowingPlayerChoiceContext();
        await PowerCmd.Apply<InternalDamagePower>(context, target, deviationAmount, model.Owner.Creature, model, false);
    }

    public static async Task Deviation(Creature target, decimal amount, PlayerChoiceContext? choiceContext = null)
    {
        var context = choiceContext ?? new ThrowingPlayerChoiceContext();
        await PowerCmd.Apply<InternalDamagePower>(context, target, amount, target, null, false);
    }

    /// <summary>
    /// 消耗气的统一方法
    /// </summary>
    /// <param name="cost"></param>
    /// <param name="model"></param>
    /// <param name="choiceContext"></param>
    /// <returns>返回实际消耗的气的数值</returns>
    public static async Task<decimal> CostQi(decimal cost, CardModel model, PlayerChoiceContext choiceContext, bool strict = true)
    {
        var qiPower = model.Owner.Creature.GetPower<QiPower>();
        if (qiPower is null)
        {
            return 0;
        }

        if (strict && qiPower.Amount < cost)
        {
            return 0;
        }

        var finalCost = Math.Min(qiPower.Amount, cost);

        await PowerCmd.ModifyAmount(choiceContext, qiPower, -finalCost, model.Owner.Creature, model);
        return cost;
    }

    public static async Task GainQi(decimal amount, Creature source, CardModel? model = null)
    {
        await PowerCmd.Apply<QiPower>(new ThrowingPlayerChoiceContext(), source, amount, source, model, false);
    }
}