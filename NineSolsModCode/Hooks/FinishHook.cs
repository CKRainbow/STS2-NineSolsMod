using MegaCrit.Sts2.Core.Combat;
using NineSolsMod.NineSolsModCode.Utils;

namespace NineSolsMod.NineSolsModCode.Hooks;

public static class FinishHook
{
    public static async Task BeforeFinish(ICombatState combatState, BeforeFinishContext context)
    {
        foreach (var model in combatState.IterateHookListeners())
        {
            if (model is IFinishListener listener)
                await listener.BeforeFinish(context);

            model.InvokeExecutionFinished();
        }
    }

    public static async Task AfterFinish(ICombatState combatState, AfterFinishContext context)
    {
        foreach (var model in combatState.IterateHookListeners())
        {
            if (model is IFinishListener listener)
                await listener.AfterFinish(context);

            model.InvokeExecutionFinished();
        }
    }
}
