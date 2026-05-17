using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using NineSolsMod.NineSolsModCode.Cards;
using STS2RitsuLib.Interop.AutoRegistration;

namespace NineSolsMod.NineSolsModCode.Powers;

[RegisterPower]
public class HackPower : NineSolsModTemporaryPower
{
    public override AbstractModel OriginModel => ModelDb.Card<Hack>();

    public override PowerModel InternallyAppliedPower => ModelDb.Power<StrengthPower>();
}