using FitnessMVC201.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FitnessMVC201.Configurations
{
    public class TrainerConfiguration : IEntityTypeConfiguration<Trainer>
    {
        public void Configure(EntityTypeBuilder<Trainer> builder)
        {
            builder.Property(x => x.Name).IsRequired().HasMaxLength(100);
            builder.Property(x => x.Description).IsRequired().HasMaxLength(1024);
            builder.Property(x => x.ImageUrl).IsRequired().HasMaxLength(1024);


            builder.HasOne(x => x.Category).WithMany(x => x.Trainer).HasForeignKey(x => x.CategoryId).HasPrincipalKey(x => x.Id).OnDelete(DeleteBehavior.Cascade);

        }
    }
}
