using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class XAncestry
    {
        public int ClassID { get; set; }
        public int ParentID { get; set; }
        public XClass XClass { get; set; }
        public XClass Parent { get; set; }
    }
}
