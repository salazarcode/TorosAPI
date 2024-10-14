using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class XObject
    {
        public int ID { get; set; }
        public int ClassID { get; set; }
        public List<XProperty> Values { get; set; } = new();
        public XClass? XClass { get; set; }

    }
}
