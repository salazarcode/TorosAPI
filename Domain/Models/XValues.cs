using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class XValues
    {
        public int ID { get; set; }
        public int ObjectID { get; set; }
        public int PropertyID { get; set; }
        public string Value { get; set; } = "";
        public XObject? XObject { get; set; }
        public XProperty? XProperty { get; set; }
    }
}
