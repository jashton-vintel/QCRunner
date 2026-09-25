namespace QCRunner.Core.Operations.Reader;

public enum HeatMode
{
    /// <summary>Block the method until the reader reports the target temperature.</summary>
    WaitUntilReached = 1,

    /// <summary>Send the set point and carry on with the next operation immediately.</summary>
    SetAndDiscard = 2
}
