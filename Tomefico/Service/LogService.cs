using System;
using Tomefico.Data;
using Tomefico.Enums;
using Tomefico.Models;

namespace Tomefico.Service;

public class LogService
{
    private readonly IServiceProvider serviceProvider;
    public LogService(IServiceProvider serviceProvider)
    {
        this.serviceProvider = serviceProvider;
    }

    public async Task<bool> OnLog(string title, string Description, DateTime? dateandtime, LogStatus status)
    {
        try
        {
            using var scope = serviceProvider.CreateScope();
            var tomeContext = scope.ServiceProvider.GetRequiredService<TomeContext>();

            var log = new LogModel
            {
                Id = Guid.NewGuid(),
                Title = title,
                Description = Description,
                CreatedAt = dateandtime.HasValue ? dateandtime.Value : DateTime.Now,
                LogStatus = status
            };

            await tomeContext.Logs.AddAsync(log);
            await tomeContext.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Fehler - {ex.Message}");
            return false;
        }
    }

}
