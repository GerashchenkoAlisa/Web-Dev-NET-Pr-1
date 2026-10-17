using System.Globalization;
using BankSystem.Services.Helpers;

namespace BankSystem.Services.Generators;

public class SimpleGenerator : IUniqueNumberGenerator
{
    private static readonly SimpleGenerator InstanceValue = new();
    private long counter = 1234567890;

    static SimpleGenerator()
    {
    }

    private SimpleGenerator()
    {
    }

    public static SimpleGenerator Instance => InstanceValue;

    public string Generate()
    {
        this.counter++;
        return this.counter.ToString(CultureInfo.InvariantCulture).GenerateHash();
    }
}
