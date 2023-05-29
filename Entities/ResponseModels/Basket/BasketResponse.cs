using Feipder.Entities.Models;
using System.ComponentModel.DataAnnotations;

namespace Feipder.Entities.ResponseModels.Basket
{
    public class BasketResponse
    {
        /// <summary>
        /// Список товаров в корзине
        /// </summary>
        public IEnumerable<BasketItemResponse> Items { get; set; } = new List<BasketItemResponse>();

        /// <summary>
        /// Количество товаров в корзине
        /// </summary>
        [Range(0, int.MaxValue)]
        public int ItemsCount { get; set; }

        /// <summary>
        /// Общая сумма товаров
        /// </summary>
        [Range(0, int.MaxValue)]
        public double TotalPrice { get; set; }

        /// <summary>
        /// Сумма скидок
        /// </summary>
        [Range(0, int.MaxValue)]
        public double DiscountPrice { get; set; }

        /// <summary>
        /// Сумма заказа с учётом скидок
        /// </summary>
        public double FinalPrice { get => TotalPrice - DiscountPrice <= 0 ? 0 : TotalPrice - DiscountPrice; }
    }
}
