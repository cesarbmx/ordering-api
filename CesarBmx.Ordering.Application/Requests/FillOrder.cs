using System;
using System.ComponentModel.DataAnnotations;


namespace CesarBmx.Ordering.Application.Requests
{
    public class FillOrder
    {
        [Required] public Guid OrderId { get; set; }
    }
}
