using NineSolsMod.NineSolsModCode.Utils;

namespace NineSolsMod.NineSolsModCode;

public interface IParryListener
{
    // Task BeforeParry(BeforeParryContext context);
    Task AfterParry(AfterParryContext context)
    {
        return Task.CompletedTask;
    }
}