
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using NineSolsMod.NineSolsModCode.Powers;
using NineSolsMod.NineSolsModCode.Variables;

namespace NineSolsMod.NineSolsModCode.Utils;

public static class MechanismUtils
{
    public static async Task Finish(decimal baseAttack, CardModel model, Creature target, PlayerChoiceContext choiceContext)
    {
        var internalDamageAmount = target.GetPowerAmount<InternalDamagePower>();
        await PowerCmd.Remove<InternalDamagePower>(target);
        await DamageCmd.Attack(model.DynamicVars[FinishVar.Key].BaseValue * internalDamageAmount + baseAttack).FromCard(model).Targeting(target)
            .WithHitFx("vfx/vfx_attack_slash", null, null)
            .Execute(choiceContext);
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

        await PowerCmd.ModifyAmount(qiPower, -finalCost, model.Owner.Creature, model);
        return cost;
    }
}