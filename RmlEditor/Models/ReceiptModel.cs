using LiteDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RmlEditor.Models
{
    public class ReceiptModel
    {
        [BsonId] // Specifies the primary key for LiteDB
        public Guid Id { get; set; } = Guid.NewGuid();

        [BsonField("projectName")] // Specifies the field name in camel case
        public string ProjectName { get; set; } = "New Project"; // Default value "New Project

        [BsonField("code")] // Specifies the field name in camel case
        public string Code { get; set; } = string.Empty;

        [BsonField("createdOn")] // Specifies the field name in camel case
        public DateTimeOffset CreatedOn { get; set; } = DateTimeOffset.UtcNow;

        [BsonField("lastUpdatedOn")] // Specifies the field name in camel case
        public DateTimeOffset? LastUpdatedOn { get; set; }
    }
}
