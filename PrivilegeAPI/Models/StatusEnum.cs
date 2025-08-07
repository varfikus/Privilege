using System.ComponentModel.DataAnnotations;

namespace PrivilegeAPI.Models
{
    public enum StatusEnum
    {
        [Display(Name = "Новая заявка")]
        Add = 0,
        [Display(Name = "Отправлено уведомление о доставке")]
        Delivered = 1,
        [Display(Name = "Принята в работу")]
        Apply = 2,
        [Display(Name = "Заявка отклонена")]
        Denial = 3,
        [Display(Name = "Заявка отработана")]
        Final = 4,
        [Display(Name = "Отказ")]
        DenialFinal = 5
    }
}
