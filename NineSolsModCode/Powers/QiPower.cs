using Godot;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Models;
using STS2RitsuLib.Interop.AutoRegistration;

namespace NineSolsMod.NineSolsModCode.Powers;

[RegisterPower]
public class QiPower : NineSolsModPower
{
    public override PowerType Type => PowerType.None;
    public override PowerStackType StackType => PowerStackType.Counter;
    public override Color AmountLabelColor => _normalAmountLabelColor;


    /// <summary>
    /// 上限为 5
    /// FIXME: 第一次施加时无法保证
    /// </summary>
    /// <param name="canonicalPower"></param>
    /// <param name="target"></param>
    /// <param name="amount"></param>
    /// <param name="applier"></param>
    /// <param name="modifiedAmount"></param>
    /// <returns></returns>
    public override bool TryModifyPowerAmountReceived(PowerModel canonicalPower, Creature target, decimal amount, Creature? applier, out decimal modifiedAmount)
    {
        var currentAmount = target.GetPowerAmount<QiPower>();
        modifiedAmount = Math.Max(0, 5 - currentAmount);
        return true;
    }

    // /// <summary>
    // /// 默认上限为 5
    // /// </summary>
    // /// <param name="target"></param>
    // /// <param name="amount"></param>
    // /// <param name="applier"></param>
    // /// <param name="cardSource"></param>
    // /// <returns></returns>
    // public override Task BeforeApplied(Creature target, decimal amount, Creature? applier, CardModel? cardSource)
    // {
    //     var currentAmount = target.GetPowerAmount<QiPower>();

    // }
}