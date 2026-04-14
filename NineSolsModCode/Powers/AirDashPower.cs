using MegaCrit.Sts2.Core.Models;
using NineSolsMod.NineSolsModCode.Cards;

namespace NineSolsMod.NineSolsModCode.Powers;

public abstract class AirDashPower : CustomTemporaryDexterityPower
{
    public override AbstractModel OriginModel => ModelDb.Card<AirDash>();
}