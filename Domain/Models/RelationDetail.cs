using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class RelationDetail
    {
        public int PropertyID { get; set; }
        public int Min { get; set; }
        public int Max { get; set; }
        public string OnDelete { get; set; } = "";
        public XProperty? XProperty { get; set; }
    }
}
