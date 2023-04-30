using Feipder.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Feipder.Data.Config
{
    public class BrandConfiguration : IEntityTypeConfiguration<Brand>
    {
        public void Configure(EntityTypeBuilder<Brand> builder)
        {
            var logoHref = "https://static.dailymoscow.ru/uploads/kaliningrad/2015/10/abibas.jpg";
            builder.HasData(
                    new Brand() { Id = 1, Name = "adidas", Logo = logoHref },
                    new Brand() { Id = 2, Name = "lime", Logo = logoHref },
                    new Brand() { Id = 3, Name = "zarina", Logo = logoHref },
                    new Brand() { Id = 4, Name = "befree", Logo = logoHref },
                    new Brand() { Id = 5, Name = "love republic", Logo = logoHref },
                    new Brand() { Id = 6, Name = "guess", Logo = logoHref },
                    new Brand() { Id = 7, Name = "nike", Logo = logoHref },
                    new Brand() { Id = 8, Name = "levis", Logo = logoHref },
                    new Brand() { Id = 9, Name = "pinko", Logo = logoHref },
                    new Brand() { Id = 10, Name = "furla", Logo = logoHref },
                    new Brand() { Id = 11, Name = "puma", Logo = logoHref },
                    new Brand() { Id = 12, Name = "12 storeez", Logo = logoHref },
                    new Brand() { Id = 13, Name = "Calvin Klein", Logo = logoHref },
                    new Brand() { Id = 14, Name = "2xu", Logo = logoHref },
                    new Brand() { Id = 15, Name = "Alef", Logo = logoHref },
                    new Brand() { Id = 16, Name = "Aleksa", Logo = logoHref },
                    new Brand() { Id = 17, Name = "Alerana", Logo = logoHref },
                    new Brand() { Id = 18, Name = "Anis", Logo = logoHref },
                    new Brand() { Id = 19, Name = "Artel", Logo = logoHref },
                    new Brand() { Id = 20, Name = "Artie", Logo = logoHref }
                );
        }
    }
}
