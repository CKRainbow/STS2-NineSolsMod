using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using NineSolsMod.NineSolsModCode.Variables;
using STS2RitsuLib.Combat.SecondaryResources;

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
public abstract class NineSolsModQiCard :
    NineSolsModCard
{
    protected int qiCost;
    protected bool qiRequired;

    public NineSolsModQiCard(int cost, CardType type, CardRarity rarity, TargetType target, int qiCost, bool showInLibrary = true, bool qiRequired = false) :
        base(cost, type, rarity, target, showInLibrary)
    {
        this.qiCost = qiCost;
        this.qiRequired = qiRequired;
        if (qiRequired)
        {
            this.SecondaryResourceUses().Set(
                "qi_required",
                QiResource.QiId,
                cost: new(
                    Amount: qiCost,
                    CostsX: qiCost == 0
                ),
                kind: SecondaryResourceUseKind.RequiredCost
            );

        }
        else
        {
            this.SecondaryResourceUses().Set(
                "qi_optional",
                QiResource.QiId,
                cost: new(
                    Amount: qiCost,
                    CostsX: qiCost == 0
                ),
                kind: SecondaryResourceUseKind.OptionalSpend
            );
        }

    }

    protected override IEnumerable<DynamicVar> CanonicalVars => [
        NineSolsModVarsFactory.QiCostVar(qiCost)
    ];
}