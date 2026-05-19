using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using Godot;
using NineSolsMod.NineSolsModCode.Variables;
using System.Collections.Generic;
using STS2RitsuLib.Scaffolding.Content;
using NineSolsMod.NineSolsModCode.Powers;

namespace NineSolsMod.NineSolsModCode.Cards;

/// <summary>
/// 使用气的卡牌基类，提供气量检查和红光效果。
/// </summary>
/// <param name="cost"></param>
/// <param name="type"></param>
/// <param name="rarity"></param>
/// <param name="target"></param>
/// <param name="qiCost"></param> 为 0 时代表使用 X 气
/// <param name="showInLibrary"></param>
public abstract class NineSolsModQiCard(int cost, CardType type, CardRarity rarity, TargetType target, int qiCost, bool showInLibrary = true, bool qiOnly = false) :
    NineSolsModCard(cost, type, rarity, target, showInLibrary)
{
    protected int qiCost = qiCost;

    protected bool HasEnoughQi
    {
        get
        {
            if (qiCost == 0) return true;
            return Owner.Creature.GetPower<QiPower>()?.Amount >= qiCost;
        }
    }

    protected override bool ShouldGlowRedInternal => !HasEnoughQi && qiOnly;

    protected override bool ShouldGlowGoldInternal => HasEnoughQi && !qiOnly;

    protected override bool IsPlayable => (HasEnoughQi || !qiOnly) && base.IsPlayable;

    protected override IEnumerable<DynamicVar> CanonicalVars => [
        NineSolsModVarsFactory.QiCostVar(qiCost)
    ];
}