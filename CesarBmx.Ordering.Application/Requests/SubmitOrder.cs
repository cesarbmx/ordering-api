using CesarBmx.Ordering.Domain.Types;
using System.ComponentModel.DataAnnotations;


namespace CesarBmx.Ordering.Application.Requests
{
    public class SubmitOrder
    {
        [Required] public string UserId { get; set; }
        [Required] public string CurrencyId { get; set; }
        [Required] public decimal Price { get; set; }
        [Required] public OrderType OrderType { get; set; }
        [Required] public decimal Quantity { get; set; }
    }
}
