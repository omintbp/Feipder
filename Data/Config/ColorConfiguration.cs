using Feipder.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Drawing;
using DrColor = System.Drawing.Color;

namespace Feipder.Data.Config
{
    public class ColorConfiguration : IEntityTypeConfiguration<Models.Color>
    {
        public void Configure(EntityTypeBuilder<Models.Color> builder)
        {
            for (var i = 1; i < 20; i++)
            {
                builder.HasData(new Models.Color(RandomColor()) { Id = i });
            }
        }

        private DrColor RandomColor()
        {
            var rnd = new Random();
            var randomGen = new Random();
            var names = (KnownColor[])Enum.GetValues(typeof(KnownColor));

            var randomColorName = names[randomGen.Next(names.Length)];
            var randomColor = DrColor.FromKnownColor(randomColorName);

            return randomColor;
        }
    }
}
