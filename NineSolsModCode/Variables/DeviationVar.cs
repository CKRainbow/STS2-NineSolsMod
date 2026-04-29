using MegaCrit.Sts2.Core.Localization.DynamicVars;
using STS2RitsuLib.Cards.DynamicVars;

namespace NineSolsMod.NineSolsModCode.Variables;

public class DeviationVar : DynamicVar
{
    // 在描述中用作占位符的键，推荐添加前缀避免撞车
    public const string Key = "NineSolsMod-Deviation";
    // 本地化键，这里设置为大写的Key
    public static readonly string LocKey = Key.ToUpperInvariant();

    public DeviationVar(decimal baseValue) : base(Key, baseValue)
    {
        this.WithSharedTooltip(LocKey);
        // this.WithTooltip("NINESOLSMOD-INTERNAL_DAMAGE_POWER", "powers");
    }
}