using System.ComponentModel.DataAnnotations;

namespace Feipder.Entities.ResponseModels.Basket
{
    public class ItemUpdateResponse
    {
        /// <summary>
        /// Обновленная информация о товаре
        /// </summary>
        [Required]
        public BasketItemResponse Item { get; set; } = null!;

        /// <summary>
        /// Количество товаров в корзине
        /// </summary>
        [Range(0, Int32.MaxValue)]
        public int ItemsCount { get; set; }

        /// <summary>
        /// Общая сумма товаров
        /// </summary>
        [Range(0, Int32.MaxValue)]
        public double TotalPrice { get; set; }

        /// <summary>
        /// Сумма скидок
        /// </summary>
        [Range(0, Int32.MaxValue)]
        public double DiscountPrice { get; set; }

        /// <summary>
        /// Сумма заказа с учётом скидок
        /// </summary>
        public double FinalPrice { get => (TotalPrice - DiscountPrice) <= 0 ? 0 : TotalPrice - DiscountPrice; }
    }
}
