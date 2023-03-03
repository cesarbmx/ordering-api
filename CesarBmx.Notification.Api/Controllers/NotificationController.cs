using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CesarBmx.Shared.Application.Responses;
using CesarBmx.Notification.Application.Responses;
using CesarBmx.Notification.Application.Services;
using CesarBmx.Shared.Api.ActionFilters;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace CesarBmx.Notification.Api.Controllers
{
    [SwaggerResponse(500, Type = typeof(InternalServerError))]
    [SwaggerResponse(401, Type = typeof(Unauthorized))]
    [SwaggerResponse(403, Type = typeof(Forbidden))]
    [SwaggerOrder(orderPrefix: "G")]
    public class NotificationController : Controller
    {
        private readonly MessageService _messageService;

        public NotificationController(MessageService messageService)
        {
            _messageService = messageService;
        }

        /// <summary>
        /// Get messages
        /// </summary>
        [HttpGet]
        [Route("api/messages")]
        [SwaggerResponse(200, Type = typeof(List<Message>))]
        [SwaggerOperation(Tags = new[] { "Messages" }, OperationId = "Messages_GetMessages")]
        public async Task<IActionResult> GetMessages(string userId)
        {
            // Reponse
            var response = await _messageService.GetMessages(userId);

            // Return
            return Ok(response);
        }

        /// <summary>
        /// Get message
        /// </summary>
        [HttpGet]
        [Route("api/messages/{messageId}", Name = "Messages_GetMessage")]
        [SwaggerResponse(200, Type = typeof(Message))]
        [SwaggerResponse(404, Type = typeof(NotFound))]
        [SwaggerOperation(Tags = new[] { "Messages" }, OperationId = "Messages_GetMessage")]
        public async Task<IActionResult> GetMessage(Guid messageId)
        {
            // Reponse
            var response = await _messageService.GetMessage(messageId);

            // Return
            return Ok(response);
        }
    }
}

