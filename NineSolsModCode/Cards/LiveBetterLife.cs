using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using NineSolsMod.NineSolsModCode.Character;
using STS2RitsuLib.Cards.DynamicVars;
using STS2RitsuLib.Interop.AutoRegistration;

namespace NineSolsMod.NineSolsModCode.Cards;

[RegisterCard(typeof(YiCardPool))]
public class LiveBetterLife() : NineSolsModCard(1, CardType.Skill,
    CardRarity.Rare, TargetType.AnyAlly)
{
    public override CardMultiplayerConstraint MultiplayerConstraint => CardMultiplayerConstraint.MultiplayerOnly;


    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        ArgumentNullException.ThrowIfNull(play.Target);
        ArgumentNullException.ThrowIfNull(play.Target.Player);
        var healthLoss = ((ComputedDynamicVar)DynamicVars["TotalHealthLoss"]).Calculate();
        await CreatureCmd.Damage(choiceContext, Owner.Creature, healthLoss, DamageProps.cardHpLoss, this);
        await PowerCmd.Apply<StrengthPower>(choiceContext, play.Target, DynamicVars["StrengthPower"].BaseValue, Owner.Creature, this);
        await PowerCmd.Apply<DexterityPower>(choiceContext, play.Target, DynamicVars["DexterityPower"].BaseValue, Owner.Creature, this);

        foreach (CardModel cardModel in PileType.Hand.GetPile(play.Target.Player).Cards)
        {
            if (!cardModel.EnergyCost.CostsX)
            {
                cardModel.SetToFreeThisTurn();
            }
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars["StrengthPower"].UpgradeValueBy(1);
        DynamicVars["DexterityPower"].UpgradeValueBy(1);
    }

    public override IEnumerable<CardKeyword> CanonicalKeywords => [
        CardKeyword.Exhaust
    ];

    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new PowerVar<StrengthPower>(3m),
        new PowerVar<DexterityPower>(3m),
        ModCardVars.Int("HealthLossPercentage", 30m),
        ModCardVars.Computed("TotalHealthLoss", 0m, (card) => {
            if (card is null) return 0m;
            var maxHealth = card.Owner.Creature.MaxHp;
            return maxHealth * DynamicVars["HealthLossPercentage"].IntValue / 100m;
        })
    ];
}