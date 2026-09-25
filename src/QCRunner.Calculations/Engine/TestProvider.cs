using System.Reflection;
using QCRunner.Calculations.Common;
using QCRunner.Calculations.Common.Enumerations;

namespace QCRunner.Calculations.Engine;

/// <summary>
/// Discovers every concrete <see cref="ITest"/> in this assembly and hands out a fresh
/// instance per calculation, keyed by the calculation type the test declares.
/// </summary>
internal static class TestProvider
{
    private static readonly IReadOnlyDictionary<CalculationType, Func<ITest>> Factories = Discover();

    public static IEnumerable<CalculationType> SupportedCalculations => Factories.Keys;

    public static ITest Create(CalculationType calculation)
    {
        if (!Factories.TryGetValue(calculation, out Func<ITest>? factory))
        {
            throw new UnsupportedCalculationException(calculation);
        }

        return factory();
    }

    private static Dictionary<CalculationType, Func<ITest>> Discover()
    {
        var factories = new Dictionary<CalculationType, Func<ITest>>();

        IEnumerable<Type> testTypes = typeof(TestProvider).Assembly.GetTypes()
            .Where(type => type.IsClass && !type.IsAbstract && typeof(ITest).IsAssignableFrom(type))
            .Where(type => type.GetConstructor(Type.EmptyTypes) is not null);

        foreach (Type testType in testTypes)
        {
            ITest prototype = (ITest)Activator.CreateInstance(testType)!;

            if (factories.ContainsKey(prototype.CalculationType))
            {
                throw new InvalidOperationException($"More than one test claims the {prototype.CalculationType} calculation.");
            }

            factories.Add(prototype.CalculationType, () => (ITest)Activator.CreateInstance(testType)!);
        }

        return factories;
    }
}
