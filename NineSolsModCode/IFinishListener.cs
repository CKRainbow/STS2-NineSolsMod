using NineSolsMod.NineSolsModCode.Utils;

namespace NineSolsMod.NineSolsModCode;

public interface IFinishListener
{
    Task BeforeFinish(BeforeFinishContext context)
    {
        return Task.CompletedTask;
    }

    Task AfterFinish(AfterFinishContext context)
    {
        return Task.CompletedTask;
    }
}
