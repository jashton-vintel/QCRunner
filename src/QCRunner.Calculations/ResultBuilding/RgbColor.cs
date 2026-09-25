using QCRunner.Calculations.Common.Results;

namespace QCRunner.Calculations.ResultBuilding;

public sealed class RgbColor : IRgbColor
{
    public static readonly RgbColor Empty = new();
    public static readonly RgbColor Black = new(0xFF, 0x00, 0x00, 0x00);
    public static readonly RgbColor Blue = new(0xFF, 0x00, 0x00, 0xFF);
    public static readonly RgbColor DarkGray = new(0xFF, 0xA9, 0xA9, 0xA9);
    public static readonly RgbColor Green = new(0xFF, 0x00, 0x80, 0x00);
    public static readonly RgbColor LightBlue = new(0xFF, 0xAD, 0xD8, 0xE6);
    public static readonly RgbColor LightGray = new(0xFF, 0xD3, 0xD3, 0xD3);
    public static readonly RgbColor Red = new(0xFF, 0xFF, 0x00, 0x00);
    public static readonly RgbColor Yellow = new(0xFF, 0xFF, 0xFF, 0x00);
    public static readonly RgbColor White = new(0xFF, 0xFF, 0xFF, 0xFF);

    public RgbColor(byte a, byte r, byte g, byte b)
    {
        A = a;
        R = r;
        G = g;
        B = b;
    }

    private RgbColor()
    {
        IsEmpty = true;
    }

    public byte A { get; }

    public byte R { get; }

    public byte G { get; }

    public byte B { get; }

    public bool IsEmpty { get; }

    public override string ToString() => IsEmpty ? "(empty)" : $"#{A:X2}{R:X2}{G:X2}{B:X2}";
}
