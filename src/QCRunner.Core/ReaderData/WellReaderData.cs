using QCRunner.Core.Operations.Calculation;

namespace QCRunner.Core.ReaderData;

/// <summary>
/// The readings collected for one well a calculation asked for, in the order they were taken.
/// </summary>
public sealed record WellReaderData(CalculationWell Well, IReadOnlyList<IReaderData> Readings);
