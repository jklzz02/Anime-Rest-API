using AnimeApi.Server.Core.Objects.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AnimeApi.Server.DataAccess.Context.Configurations;

public class IdentityProviderConfiguration : IEntityTypeConfiguration<IdentityProvider>
{
    public void Configure(EntityTypeBuilder<IdentityProvider> entity)
    {
        entity.HasKey(e => e.Id);
        entity.ToTable("identity_provider");
        
        entity.Property(e => e.Name)
            .HasMaxLength(50);
    }
}