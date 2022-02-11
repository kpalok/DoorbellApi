using System;

namespace DoorbellApi.Models
{
    public class AlertItem
    {
        public int ID { get; set; }

        public DateTime ServerTime { get; set; }

        [System.ComponentModel.DataAnnotations.Required]
        public string Message { get; set; }

        public bool Approved { get; set; }

        public bool Declined { get; set; }
    }
}
