using QCRunner.Core.ReaderData;

namespace QCRunner.Core.Instruments.Readers;

public sealed class ReaderDataReceivedEventArgs : EventArgs
{
    public ReaderDataReceivedEventArgs(IReaderData data)
    {
        ArgumentNullException.ThrowIfNull(data);
        Data = data;
    }

    public IReaderData Data { get; }
}
