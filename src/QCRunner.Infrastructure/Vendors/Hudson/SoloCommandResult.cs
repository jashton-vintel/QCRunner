namespace QCRunner.Infrastructure.Vendors.Hudson;

public sealed record SoloCommandResult(bool Succeeded, int ErrorCode, string Message, string Response)
{
    public static SoloCommandResult Ok(string response = "") => new(true, 0, string.Empty, response);

    public static SoloCommandResult Failed(int errorCode, string message) => new(false, errorCode, message, string.Empty);
}
