using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models.Powers;
using NineSolsMod.NineSolsModCode.Utils;
using STS2RitsuLib.Interop.AutoRegistration;

namespace NineSolsMod.NineSolsModCode.Powers;

[RegisterPower]
public class HarnessForceJadePower : NineSolsModPower, IParryListener
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    public override Color AmountLabelColor => _normalAmountLabelColor;

    protected override IEnumerable<IHoverTip> AdditionalHoverTips =>
    [
        HoverTipFactory.FromPower<ParryPower>(),
        HoverTipFactory.FromPower<StrengthPower>()
    ];

    public async Task AfterParry(AfterParryContext context)
    {
        if (context.ParryingCreature != Owner)
            return;

        await PowerCmd.Apply<StrengthPower>(context.ChoiceContext, Owner, Amount, context.AttackingCreature, context.SourceCard, false);
    }

}