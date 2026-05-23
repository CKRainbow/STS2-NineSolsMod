
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

public interface IChoosable
{
    Task OnChoose(PlayerChoiceContext choiceContext, CardPlay play);
}