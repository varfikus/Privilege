namespace PrivilegeAPI.Models
{
    public class ResponseModel
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public string ProcessedOrderId { get; set; }
    }
}
