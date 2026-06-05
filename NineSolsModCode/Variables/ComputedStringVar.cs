
using MegaCrit.Sts2.Core.Localization.DynamicVars;

public sealed class ComputedStringVar(string name, Func<string> stringFactory) : DynamicVar(name, 0m)
{
    public override string ToString()
    {
        return stringFactory();
    }
}