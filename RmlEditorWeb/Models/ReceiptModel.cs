
namespace ReceiptBuilder.Web.Models
{
    // Models/AppState.cs
    public class ReceiptModel
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string ProjectName { get; set; } = "New Project"; // Default value "New Project

        public string Code { get; set; } = string.Empty;

        public DateTimeOffset CreatedOn { get; set; } = DateTimeOffset.UtcNow;

        public DateTimeOffset? LastUpdatedOn { get; set; }
    }

}
