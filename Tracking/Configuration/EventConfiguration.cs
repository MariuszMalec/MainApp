using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tracking.Models;

namespace Tracking.Configuration
{
    public class EventConfiguration : IEntityTypeConfiguration<Event>
    {

        public void Configure(EntityTypeBuilder<Event> builder)
        {
            //builder.HasOne(e => e.Id).WithMany().HasForeignKey("Event").OnDelete(DeleteBehavior.SetNull);
            //builder.HasOne(e => e.Id).WithMany(e => e.UserId).OnDelete(DeleteBehavior.Cascade);
            //builder.HasKey(e => e.Id);
        }
    }
}
