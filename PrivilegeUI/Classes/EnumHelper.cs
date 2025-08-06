using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace PrivilegeUI.Classes
{
    public class EnumHelper
    {
        public static string GetEnumDisplayName(Enum value)
        {
            return value.GetType()
                        .GetMember(value.ToString())
                        .FirstOrDefault()?
                        .GetCustomAttribute<DisplayAttribute>()?
                        .Name ?? value.ToString();
        }
    }
}
