﻿using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Feipder.Entities
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum OrderStatus
    {
        Created,
        Approved,
        InDelivery,
        Done
    }
}
