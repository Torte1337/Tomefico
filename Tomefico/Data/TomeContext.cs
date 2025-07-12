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

    public TomeContext(DbContextOptions<TomeContext> options) : base(options) { }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
    }
}
