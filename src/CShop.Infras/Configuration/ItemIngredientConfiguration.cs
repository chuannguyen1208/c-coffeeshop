using CShop.Domain.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CShop.Infras.Configuration;
internal class ItemIngredientConfiguration : IEntityTypeConfiguration<ItemIngredient>
{
    public void Configure(EntityTypeBuilder<ItemIngredient> builder)
    {
        builder.HasKey(s => s.Id);
        builder.HasOne(s => s.Item).WithMany(s => s.ItemIngredients).HasForeignKey(s => s.ItemId);
        builder.HasOne(s => s.Ingredient).WithMany().HasForeignKey(s => s.IngredientId);
    }
}
