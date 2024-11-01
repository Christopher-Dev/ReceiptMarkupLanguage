using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RmlCommon.ServerModels
{
    public class CompletedRender
    {
        public CompletedRender(Guid id, byte[] receipt) 
        { 
            Id = id;
            Receipt = receipt;
        }

        public Guid Id { get; set; }

        public byte[] Receipt { get; set; }

        public string TotalTime { get; set; }
    }
}
