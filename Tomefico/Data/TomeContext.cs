using System;
using Microsoft.EntityFrameworkCore;
using Tomefico.Models;
using Tomefico.Service;

namespace Tomefico.Data;

public class TomeContext : DbContext
{
    public DbSet<BookModel> Books { get; set; }
    public DbSet<AuthorModel> Authors { get; set; }
    public DbSet<LogModel> Logs { get; set; }
    private readonly PathService pathService;
    
    public TomeContext(DbContextOptions<TomeContext> options, PathService pathService) : base(options) { this.pathService = pathService; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        optionsBuilder.UseSqlite(pathService.GetSQLiteConnectionString());
    }
}
