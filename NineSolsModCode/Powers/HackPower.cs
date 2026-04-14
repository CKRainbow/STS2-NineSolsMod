using MegaCrit.Sts2.Core.Models;
using NineSolsMod.NineSolsModCode.Cards;

namespace NineSolsMod.NineSolsModCode.Powers;

public abstract class HackPower : CustomTemporaryStrenthPower
{
    public override AbstractModel OriginModel => ModelDb.Card<Hack>();
}