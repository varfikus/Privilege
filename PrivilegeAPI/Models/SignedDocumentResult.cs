using System.Xml.Linq;

namespace PrivilegeAPI.Models
{
    public class SignedDocumentResult
    {
        public XDocument SignedXml { get; set; }
        public XDocument Signature { get; set; }
    }
}
