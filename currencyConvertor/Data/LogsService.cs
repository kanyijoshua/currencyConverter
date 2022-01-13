using currencyConvertor.Models;

namespace currencyConvertor.Data;

public class LogsService
{
    private readonly ApplicationDbContext _context;

    public LogsService(ApplicationDbContext context)
    {
        _context = context;
    }

    public Task<Logs[]> GetLogsAsync(DateTime? startDate, DateTime? endDate)
    {
        return Task.FromResult(_context.Logs.OrderByDescending(c=>c.DateTime).ToArray());
    }
}