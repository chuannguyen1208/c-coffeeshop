using CShop.Domain.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CShop.Infras.Configuration;
internal class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.HasKey(s => s.Id);
        builder.HasOne(s => s.Order).WithMany(o => o.OrderItems).HasForeignKey(o => o.OrderId);
        builder.HasOne(s => s.Item).WithMany().HasForeignKey(o => o.ItemId);
    }
}
