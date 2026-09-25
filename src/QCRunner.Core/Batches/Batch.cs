using QCRunner.Core.Methods;

namespace QCRunner.Core.Batches;

/// <summary>
/// One execution of a method against a production lot.
/// </summary>
public class Batch
{
    public int Id { get; set; }

    public string Reference { get; set; } = string.Empty;

    public DateTime? StartTime { get; set; }

    public DateTime? EndTime { get; set; }

    public string KitLotNumber { get; set; } = string.Empty;

    public DateTime? KitExpiryDate { get; set; }

    /// <summary>Identifier of the user who ran the batch.</summary>
    public int UserId { get; set; }

    public string OperatorName { get; set; } = string.Empty;

    public BatchStatus Status { get; set; } = BatchStatus.Unreleased;

    public BatchResultType Result { get; set; } = BatchResultType.InProgress;

    public string? Comments { get; set; }

    public string? ReaderSerialNumber { get; set; }

    public string? RobotSerialNumber { get; set; }

    public int MethodId { get; set; }

    public virtual Method Method { get; set; } = null!;

    public override string ToString() => $"Batch {Reference}";
}
