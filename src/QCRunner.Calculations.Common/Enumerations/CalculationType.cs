using System.ComponentModel;

namespace QCRunner.Calculations.Common.Enumerations;

/// <summary>
/// The QC tests the calculation engine knows how to perform.
/// </summary>
public enum CalculationType
{
    [Description("Undefined")]
    Undefined = 0,

    [Description("Colour")]
    Colour = 1,

    [Description("Clarity")]
    Clarity = 2,

    [Description("pH")]
    Ph = 3,

    [Description("Endotoxin")]
    Endotoxin = 4,

    [Description("Ethanol")]
    Ethanol = 5,

    [Description("Kryptofix")]
    Kryptofix = 6,

    [Description("Half-life")]
    HalfLife = 7,

    [Description("Radioactive concentration")]
    Concentration = 8,

    [Description("Radiochemical purity")]
    RadiochemicalPurity = 9,

    [Description("Radiochemical identity")]
    RadiochemicalIdentity = 10,

    [Description("FES")]
    Fes = 11
}
