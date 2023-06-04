using System.ComponentModel.DataAnnotations.Schema;

namespace AzureFunctionApp.Models
{
    [Table("SalesRequest")]
    public class SalesRequestModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string Status { get; set; }
    }
}
