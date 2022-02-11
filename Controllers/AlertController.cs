using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using DoorbellApi.Models;
using Microsoft.AspNetCore.Http;

namespace DoorbellApi.Controllers
{
    [ApiController]
    [Produces(System.Net.Mime.MediaTypeNames.Application.Json)]
    [Route("alertitems")]
    public class AlertController : ControllerBase
    {
        private readonly ILogger<AlertItem> _logger;
        private static readonly List<AlertItem> _alerts = new List<AlertItem>();

        public AlertController(ILogger<AlertItem> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public ActionResult<List<AlertItem>> GetAll()
        {
            return _alerts;
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<AlertItem> Get(int id)
        {
            if (_alerts.Any(a => a.ID == id))
            {
                return _alerts.Where(a => a.ID == id).First();
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<AlertItem> Post(AlertItem alert)
        {
            alert.ID = _alerts.Any() ? _alerts.Max(a => a.ID) + 1 : 1;
            alert.ServerTime = DateTime.Now;
            _alerts.Add(alert);
            return CreatedAtAction(nameof(Get), new { id = alert.ID }, alert);
        }
    }
}
