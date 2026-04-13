using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.ValueProps;
using NineSolsMod.NineSolsModCode.Cards;
using NineSolsMod.NineSolsModCode.Character;
using NineSolsMod.NineSolsModCode.Powers;

namespace NineSolsMod.NineSolsModCode.Cards;

[Pool(typeof(TokenCardPool))]
public class UnboundedCounter() : NineSolsModCard(1, CardType.Skill,
    CardRarity.Token, TargetType.Self)
{
    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);
        await PowerCmd.Apply<UnboundedCounterPower>(Owner.Creature, 1m, Owner.Creature, this, false);
        await PowerCmd.Apply<ParryPower>(Owner.Creature, DynamicVars["ParryPower"].IntValue, Owner.Creature, this, false);
    }

    protected override void OnUpgrade()
    {
        DynamicVars["ParryPower"].UpgradeValueBy(3m);
    }

    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new PowerVar<ParryPower>(6m)
    ];

    public override IEnumerable<CardKeyword> CanonicalKeywords => [
        CardKeyword.Exhaust,
        CardKeyword.Ethereal
    ];

    public static async Task CreateInHand(Player owner, CombatState combatState)
    {
        if (CombatManager.Instance.IsOverOrEnding)
        {
            return;
        }
        await CardPileCmd.AddGeneratedCardToCombat(combatState.CreateCard<UnboundedCounter>(owner), PileType.Hand, true, CardPilePosition.Bottom);
    }
}