using System.ComponentModel;
using System.Runtime.Serialization;
using Microsoft.OpenApi.Attributes;
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
