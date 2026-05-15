using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using STS2RitsuLib.Cards.DynamicVars;
using STS2RitsuLib.Interop.AutoRegistration;

namespace NineSolsMod.NineSolsModCode.Cards;

[RegisterCard(typeof(TokenCardPool))]
public class XuanTie() : NineSolsModCard(0, CardType.Skill,
    CardRarity.Token, TargetType.Self)
{
    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        CardModel? cardModel = (await CardSelectCmd.FromHand(
            prefs: new CardSelectorPrefs(SelectionScreenPrompt, 1),
            context: choiceContext,
            player: Owner,
            filter: c => c.Type == CardType.Attack,
            source: this)).FirstOrDefault();

        if (cardModel != null)
        {
            // 通过查看卡牌中 DynamicVar 中的 DamageVar 来增加其伤害
            decimal attackInc = DynamicVars["AttackInc"].IntValue;
            cardModel.DynamicVars.Damage.BaseValue += attackInc;
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars["AttackInc"].UpgradeValueBy(3);
    }

    public override IEnumerable<CardKeyword> CanonicalKeywords => [
        CardKeyword.Exhaust
    ];

    protected override IEnumerable<DynamicVar> CanonicalVars => [
        ModCardVars.Int("AttackInc", 6)
    ];
}
