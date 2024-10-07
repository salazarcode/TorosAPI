using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class XAncestry
    {
        public int XClassID { get; set; }
        public int ParentID { get; set; }
        public virtual XClass XClass { get; set; }
        public virtual XClass Parent { get; set; }
    }
}
