using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RmlCommon.ServerModels
{
    public class ReceiptModel
    {
        public ReceiptModel(DataContextModel dataContext, ResourcesModel resources, BodyModel body) 
        { 
            _body = body;
            _resources = resources;
            _dataContext = dataContext;
        }

        public Guid Id { get; set; } = Guid.NewGuid();

        private DataContextModel _dataContext { get; set; }

        private ResourcesModel _resources { get; set; }

        private BodyModel _body { get; set; }

        public string AssembledReceipt 
        { 
            get 
            { 
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("<Receipt>");
                sb.AppendLine($"{_dataContext.DataContextContent}");
                sb.AppendLine($"{_resources.ResourceContent}");
                sb.AppendLine($"{_body.BodyContent}");
                sb.AppendLine("</Receipt>");
                return sb.ToString();
            }
        }

    }
}
