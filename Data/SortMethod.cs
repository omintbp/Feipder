using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Feipder.Data
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum SortMethod
    {
        Newest,
        ByPriceAsc,
        ByPriceDesc,
    }
}
