using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Drawing;
using DrColor = System.Drawing.Color;

namespace Feipder.Data.Config
{
    public class ColorConfiguration : IEntityTypeConfiguration<Entities.Models.Color>
    {
        public void Configure(EntityTypeBuilder<Entities.Models.Color> builder)
        {
            for (var i = 1; i < 20; i++)
            {
                builder.HasData(new Entities.Models.Color(RandomColor()) { Id = i });
            }
        }

        private DrColor RandomColor()
        {
            var randomGen = new Random();
            var names = (KnownColor[])Enum.GetValues(typeof(KnownColor));

            var randomColorName = names[randomGen.Next(names.Length)];
            var randomColor = DrColor.FromKnownColor(randomColorName);

            return randomColor;
        }
    }
}
