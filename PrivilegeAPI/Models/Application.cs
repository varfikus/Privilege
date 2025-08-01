using System.ComponentModel.DataAnnotations;

namespace PrivilegeAPI.Models
{
    public class Application
    {
        [Key]
        public int Id { get; set; }

        [StringLength(100)]
        public string Organization { get; set; }

        [StringLength(200)]
        public string FullName { get; set; }

        [StringLength(500)]
        public string Address { get; set; }

        [StringLength(10)]
        public string PassportSeries { get; set; }

        [StringLength(20)]
        public string PassportNumber { get; set; }

        [StringLength(100)]
        public string IssuedBy { get; set; }

        [StringLength(20)]
        public string ContactPhone { get; set; }

        [StringLength(100)]
        public string Email { get; set; }

        [StringLength(20)]
        public string CardNumber { get; set; }

        [StringLength(200)]
        public string BenefitCategory { get; set; }

        public DateTime ApplicationDate { get; set; }

        [StringLength(50)]
        public string ServiceId { get; set; }
    }
}
