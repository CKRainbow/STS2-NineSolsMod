using BaseLib.Extensions;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace NineSolsMod.NineSolsModCode.Variables;

public class FinishVar : DynamicVar
{
    // 在描述中用作占位符的键，推荐添加前缀避免撞车
    public const string Key = "NineSolsMod-Finish";
    // 本地化键，这里设置为大写的Key
    public static readonly string LocKey = Key.ToUpperInvariant();

    public FinishVar(decimal baseValue) : base(Key, baseValue)
    {
        this.WithTooltip(LocKey);
    }
}