using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Models
{
    [Table("Ancestries", Schema = "abstract")]
    public class XAncestry
    {
        public int XClassID { get; set; }
        public int ParentID { get; set; }
        public virtual XClass XClass { get; set; }
        public virtual XClass Parent { get; set; }
    }
}
