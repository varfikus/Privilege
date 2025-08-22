namespace PrivilegeAPI.Models
{
    public class Recipient
    {
        public string OrganizationCode { get; set; }      // <kodorgout>
        public string OrganizationName { get; set; }      // <nameorg>
        public string Subdivision { get; set; }          // <structuralsubdivision>
        public string SubOrgName { get; set; }           // <nameorgsub>
        public string Post { get; set; }                 // <post>
    }
}
