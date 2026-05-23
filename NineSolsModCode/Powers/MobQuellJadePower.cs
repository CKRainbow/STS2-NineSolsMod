using Godot;
using MegaCrit.Sts2.Core.Entities.Powers;
using NineSolsMod.NineSolsModCode.Utils;
using STS2RitsuLib.Interop.AutoRegistration;
using MegaCrit.Sts2.Core.HoverTips;
using NineSolsMod.NineSolsModCode.HoverTips;

namespace NineSolsMod.NineSolsModCode.Powers;

[RegisterPower]
public class MobQuellJadePower : NineSolsModPower, IFinishListener
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Single;
    public override Color AmountLabelColor => _normalAmountLabelColor;

    protected override IEnumerable<IHoverTip> AdditionalHoverTips => [
        NineSolsModHoverTipFactory.Finish()
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