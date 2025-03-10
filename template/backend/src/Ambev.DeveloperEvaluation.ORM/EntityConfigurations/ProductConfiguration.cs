using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ambev.DeveloperEvaluation.Domain.Entities.Products;

namespace Ambev.DeveloperEvaluation.ORM.EntityConfigurations;

/// <summary>
/// Entity Framework configuration for the Product entity.
/// </summary>
public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    /// <summary>
    /// Configures the entity mappings for the Product entity.
    /// </summary>
    /// <param name="builder">The entity type builder</param>
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("Products");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(p => p.Description)
            .HasMaxLength(500);

        builder.Property(p => p.Sku)
            .IsRequired()
            .HasMaxLength(20);

        builder.HasIndex(p => p.Sku)
            .IsUnique();

        builder.Property(p => p.Price)
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.Property(p => p.StockQuantity)
            .IsRequired();

        builder.Property(p => p.Category)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(p => p.BranchExternalId)
            .IsRequired();

        builder.Property(p => p.BranchName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(p => p.Status)
            .IsRequired();

        builder.Property(p => p.CreatedAt)
            .IsRequired();

        builder.Property(p => p.UpdatedAt);
    }
} 