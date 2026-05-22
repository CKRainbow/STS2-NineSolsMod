using Godot;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using NineSolsMod.NineSolsModCode.Variables;
using NineSolsMod.NineSolsModCode.Utils;
using STS2RitsuLib.Interop.AutoRegistration;

namespace NineSolsMod.NineSolsModCode.Powers;

[RegisterPower]
public class MobQuellJadePower : NineSolsModPower, IFinishListener
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Single;
    public override Color AmountLabelColor => _normalAmountLabelColor;

    protected override IEnumerable<DynamicVar> CanonicalVars => [
        ..NineSolsModVarsFactory.FinishVar(100)
    ];

    public Task BeforeFinish(BeforeFinishContext context)
    {
        if (context.SourceCard.Owner.Creature == Owner)
        {
            context.TargetsToDamage = Owner.CombatState!.HittableEnemies.ToList();
        }
        return Task.CompletedTask;
    }
}