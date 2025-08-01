using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrivilegeUI.Models
{
    public class Application
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string ServiceName { get; set; } = string.Empty;
        public DateTime ApplicationDate { get; set; }
        public string BenefitCategory { get; set; } = string.Empty;
        public string CardNumber { get; set; } = string.Empty;
        public string ServiceId { get; set; } = string.Empty;
    }
}
