using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using NineSolsMod.NineSolsModCode.Cards;
using STS2RitsuLib.Interop.AutoRegistration;

namespace NineSolsMod.NineSolsModCode.Powers;

[RegisterPower]
public class SwiftRunPower : NineSolsModTemporaryPower
{
    public override AbstractModel OriginModel => ModelDb.Card<SwiftRun>();

    public override PowerModel InternallyAppliedPower => ModelDb.Power<DexterityPower>();

}