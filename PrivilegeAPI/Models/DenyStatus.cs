using System.ComponentModel.DataAnnotations;

namespace PrivilegeAPI.Models
{
    public enum DenyStatus
    {
        [Display(Name = "Новая")]
        Add = 0,
        [Display(Name = "Отправлена")]
        Delivered = 1,
    }
}
