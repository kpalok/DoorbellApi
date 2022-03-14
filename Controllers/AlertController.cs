using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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

        [HttpGet("new")]
        public ActionResult<List<AlertItem>> GetNew()
        {
            return _alerts.Where(a => !a.Approved && !a.Declined).ToList();
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

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult Delete(int id)
        {
            if (_alerts.Any(a => a.ID == id))
            {
                _alerts.Remove(_alerts.Where(a => a.ID == id).First());
                return Ok();
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
            SaveToFile(alert);
            return CreatedAtAction(nameof(Get), new { id = alert.ID }, alert);
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<AlertItem> Put(AlertItem alert)
        {
            if (_alerts.Any(a => a.ID == alert.ID))
            {
                _alerts[_alerts.IndexOf(_alerts.Where(a => a.ID == alert.ID).First())] = alert;
                return AcceptedAtAction(nameof(Get), new { id = alert.ID }, alert);
            }
            else
            {
                return NotFound();
            }
        }

        private async void SaveToFile(AlertItem alert)
        {
            await using FileStream oFs = System.IO.File.Open($"./archive/alertitems{alert.ID}.txt", FileMode.OpenOrCreate);
            await JsonSerializer.SerializeAsync(oFs, alert);
        }
    }
}
