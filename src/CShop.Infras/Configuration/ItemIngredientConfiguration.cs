using CShop.UseCases.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CShop.Infras.Configuration;
internal class ItemIngredientConfiguration : IEntityTypeConfiguration<ItemIngredient>
{
    public void Configure(EntityTypeBuilder<ItemIngredient> builder)
    {
        builder.HasKey(s => s.Id);
        builder.HasOne(s => s.Item).WithMany().HasForeignKey(s => s.ItemId);
        builder.HasOne(s => s.Ingredient).WithMany().HasForeignKey(s => s.IngredientId);
    }
}
