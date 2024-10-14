using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class XProperty
    {
        public int ID { get; set; }
        public int ClassID { get; set; }
        public int PropertyClassID { get; set; }
        public int? RelationDetailID { get; set; }
        public string Key { get; set; } = "";
        public string Name { get; set; } = "";
        public XClass? XClass { get; set; }
        public XClass? PropertyClass { get; set; }
        public RelationDetail? RelationDetail { get; set; }
    }
}
