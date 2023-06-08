using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Collections;
using System.Text.Json;

namespace Feipder.Entities.RequestModels
{
    public class ItemsStatusRequest
    {
        public IEnumerable<BasketItemInfoRequest> Items { get; set; } = new List<BasketItemInfoRequest>();
    }
}
