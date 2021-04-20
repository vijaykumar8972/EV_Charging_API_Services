using System;
using System.Collections.Generic;
using System.Text;

namespace EVCharging.Models
{
    public class EmailConfig : IEmailConfig
    {
        public string FormEmail { get; set; }
        public string Password { get; set; }
        public string EmailUrl { get; set; }
    }
    public interface IEmailConfig
    {
        public string FormEmail { get; set; }
        public string EmailUrl { get; set; }
        public string Password { get; set; }
    }
}
