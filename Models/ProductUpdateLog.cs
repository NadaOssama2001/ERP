using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class ProductUpdateLog
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public DateTime UpdateDateTime { get; set; }
        public string UpdatedByUserId { get; set; }
    }
}
