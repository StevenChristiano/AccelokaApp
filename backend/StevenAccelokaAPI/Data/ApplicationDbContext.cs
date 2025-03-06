using Microsoft.EntityFrameworkCore;
using StevenAccelokaAPI.Models;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<Ticket> Tickets { get; set; }

    public DbSet<BookedTicket> BookedTickets { get; set; }
    public DbSet<Booking> Bookings { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<BookedTicket>()
            .HasOne(bt => bt.Ticket)
            .WithMany() // Adjust this if Ticket has a navigation property
            .HasForeignKey(bt => bt.TicketId)
            .OnDelete(DeleteBehavior.NoAction); // Prevent cascade delete
    }

}
