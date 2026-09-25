using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using QCRunner.Core.Methods;

namespace QCRunner.Data.Seeding;

/// <summary>
/// Creates the SQLite database on first run and seeds the example methods so the demo needs
/// no setup.
/// </summary>
public sealed class DatabaseInitializer
{
    private readonly QCRunnerDbContext _context;
    private readonly ILogger<DatabaseInitializer> _logger;

    public DatabaseInitializer(QCRunnerDbContext context, ILogger<DatabaseInitializer> logger)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(logger);

        _context = context;
        _logger = logger;
    }

    public async Task InitialiseAsync(CancellationToken cancellationToken = default)
    {
        bool created = await _context.Database.EnsureCreatedAsync(cancellationToken);

        if (created)
        {
            _logger.LogInformation("Created database {DataSource}", _context.Database.GetDbConnection().DataSource);
        }

        if (await _context.Methods.AnyAsync(cancellationToken))
        {
            return;
        }

        Core.Labware.Labware qcPlate = LabwareSeedData.QcPlate();
        Core.Labware.Labware reagentPlate = LabwareSeedData.ReagentPlate();

        _context.Labware.AddRange(qcPlate, reagentPlate);

        List<Method> methods = new()
        {
            FdgMethodSeed.Create(qcPlate, reagentPlate),
            GalliumMethodSeed.Create(qcPlate, reagentPlate)
        };

        _context.Methods.AddRange(methods);
        await _context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Seeded {Count} example methods: {Methods}", methods.Count, string.Join(", ", methods.Select(method => method.Code)));
    }
}
