using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using NineSolsMod.NineSolsModCode.Powers;
using NineSolsMod.NineSolsModCode.Hooks;
using NineSolsMod.NineSolsModCode.Variables;
using STS2RitsuLib.Cards.DynamicVars;
using STS2RitsuLib.Combat.SecondaryResources;
using MegaCrit.Sts2.Core.Entities.Players;

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
    /// <returns></returns>
    public static async Task Finish(decimal baseAttack, CardModel model, Creature target, PlayerChoiceContext? choiceContext = null, decimal? inputFinishMult = null)
    {
        var internalDamageAmount = target.GetPowerAmount<InternalDamagePower>();

        decimal finishMult;
        if (inputFinishMult is not null)
        {
            finishMult = (int)inputFinishMult;
        }
        else
        {
            var finishVar = model.DynamicVars[NineSolsModVarsFactory.FinishKey];
            if (finishVar is ComputedDynamicVar calculatedFinishVar)
            {
                finishMult = calculatedFinishVar.Calculate(target) / 100m;
            }
            else
            {
                finishMult = finishVar.BaseValue / 100m;
            }
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
        // 不受任何加成影响的伤害
        VfxCmd.PlayOnCreatureCenters(beforeContext.TargetsToDamage, "vfx/vfx_attack_blunt");

        var fatalEligibleTargets = beforeContext.TargetsToDamage.Where(t => t.Powers.All(p => p.ShouldOwnerDeathTriggerFatal())).ToList();

        var damageResults = await CreatureCmd.Damage(damageContext, beforeContext.TargetsToDamage, finishMult * internalDamageAmount, ValueProp.Unpowered, model.Owner.Creature, model);

        int triggeredFatalNum = damageResults.Count(r => fatalEligibleTargets.Contains(r.Receiver) && r.WasTargetKilled);

        var afterContext = new AfterFinishContext
        {
            ChoiceContext = choiceContext,
            SourceCard = model,
            Target = target,
            FinishMult = finishMult,
            InternalDamageAmount = internalDamageAmount,
            DamagedTargets = beforeContext.TargetsToDamage,
            TriggeredFatalNum = triggeredFatalNum
        };

        await FinishHook.AfterFinish(model.Owner.Creature.CombatState, afterContext);

    }

    public static async Task Deviation(CardModel model, Creature target, PlayerChoiceContext? choiceContext = null)
    {
        var deviationAmount = model.DynamicVars[NineSolsModVarsFactory.DeviationKey].BaseValue;
        var context = choiceContext ?? new ThrowingPlayerChoiceContext();
        await PowerCmd.Apply<InternalDamagePower>(context, target, deviationAmount, model.Owner.Creature, model, false);
    }

    public static async Task Deviation(Creature target, decimal amount, PlayerChoiceContext? choiceContext = null)
    {
        var context = choiceContext ?? new ThrowingPlayerChoiceContext();
        await PowerCmd.Apply<InternalDamagePower>(context, target, amount, target, null, false);
    }

    public static async Task GainQi(Player player, int amount)
    {
        await SecondaryResourceCmd.Gain(player, QiResource.QiId, amount);
    }
}