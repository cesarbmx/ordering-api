﻿using CesarBmx.Shared.Application.Responses;
using CesarBmx.Ordering.Application.Responses;
using CesarBmx.Ordering.Application.Services;
using CesarBmx.Shared.Api.ActionFilters;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using CesarBmx.Ordering.Application.Requests;
using CesarBmx.Ordering.Application.Conflicts;

namespace CesarBmx.Ordering.Api.Controllers
{
    [SwaggerResponse(500, Type = typeof(InternalServerError))]
    [SwaggerResponse(401, Type = typeof(Unauthorized))]
    [SwaggerResponse(403, Type = typeof(Forbidden))]
    [SwaggerOrder(orderPrefix: "G")]
    public class OrderController : Controller
    {
        private readonly OrderService _orderService;

        public OrderController(OrderService orderService)
        {
            _orderService = orderService;
        }

        /// <summary>
        /// Get orders
        /// </summary>
        [HttpGet]
        [Route("api/orders")]
        [SwaggerResponse(200, Type = typeof(List<Order>))]
        [SwaggerOperation(Tags = new[] { "Orders" }, OperationId = "Orders_GetOrders")]
        public async Task<IActionResult> GetOrders(string userId)
        {
            // Reponse
            var response = await _orderService.GetOrders(userId);

            // Return
            return Ok(response);
        }

        /// <summary>
        /// Get order
        /// </summary>
        [HttpGet]
        [Route("api/orders/{orderId}", Name = "Orders_GetOrder")]
        [SwaggerResponse(200, Type = typeof(Order))]
        [SwaggerResponse(404, Type = typeof(NotFound))]
        [SwaggerOperation(Tags = new[] { "Orders" }, OperationId = "Orders_GetOrder")]
        public async Task<IActionResult> GetOrder(Guid orderId)
        {
            // Reponse
            var response = await _orderService.GetOrder(orderId);

            // Return
            return Ok(response);
        }

        /// <summary>
        /// Submit order
        /// </summary>
        [HttpPost]
        [Route("api/users")]
        [SwaggerResponse(201, Type = typeof(Order))]
        [SwaggerResponse(400, Type = typeof(BadRequest))]
        [SwaggerResponse(409, Type = typeof(Conflict<SubmitOrderConflict>))]
        [SwaggerResponse(422, Type = typeof(Validation))]
        [SwaggerOperation(Tags = new[] { "Users" }, OperationId = "Users_AddUser")]
        public async Task<IActionResult> AddUser([FromBody] SubmitOrder request)
        {
            // Reponse
            var response = await _orderService.SubmitOrder(request);

            // Return
            return CreatedAtRoute("Users_GetUser", new { response.UserId }, response);
        }
    }
}

