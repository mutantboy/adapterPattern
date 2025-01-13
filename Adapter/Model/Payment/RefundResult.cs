using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adapter.Model.Payment
{
    public class RefundResult
    {
        public bool Success { get; set; }
        public string RefundId { get; set; }
        public string Message { get; set; }
    }
}
