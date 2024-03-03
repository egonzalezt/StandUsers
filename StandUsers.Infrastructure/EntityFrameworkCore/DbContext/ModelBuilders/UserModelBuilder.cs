namespace StandUsers.Infrastructure.EntityFrameworkCore.DbContext.ModelBuilders;

using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.User;

internal static class UserModelBuilder
{
    public static void Configure(this EntityTypeBuilder<User> builder)
    {
        builder.Property(p => p.Name)
            .HasMaxLength(128)
            .IsRequired();
        builder.Property(p => p.Id).IsRequired();
        builder.Property(p => p.IdentificationNumber)
            .HasMaxLength(10)
            .IsRequired();
        builder.Property(p => p.Email)
            .HasMaxLength(200)
            .IsRequired();

        builder.HasIndex(p => p.IdentificationNumber).IsUnique();
        builder.HasIndex(p => p.Email).IsUnique();
    }
}
