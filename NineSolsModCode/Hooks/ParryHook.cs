using MegaCrit.Sts2.Core.Combat;
using NineSolsMod.NineSolsModCode.Utils;

namespace NineSolsMod.NineSolsModCode.Hooks;

public static class ParryHook
{
    // public static async Task BeforeParry;

    public static async Task AfterParry(ICombatState combatState, AfterParryContext context)
    {
        foreach (var model in combatState.IterateHookListeners())
        {
            if (model is IParryListener listener)
                await listener.AfterParry(context);

            model.InvokeExecutionFinished();
        }
    }

}