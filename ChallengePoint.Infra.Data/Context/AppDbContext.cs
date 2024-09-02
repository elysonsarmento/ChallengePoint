using ChallengePoint.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace ChallengePoint.Infra.Data.Context;

public class AppDbContext : DbContext
{


    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<CollaboratorModel> Collaborators { get; set; }

    public DbSet<TimekeepingModel> Timekeeping { get; set; }

}
