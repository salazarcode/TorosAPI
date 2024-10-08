using Microsoft.AspNetCore.Mvc;

namespace API.Responses.Domain
{
    public class ClassDTO { 
        public int ID { get; set; }
        public string Name { get; set; }
        public bool IsPrimitive { get; set; } = false;
    }
}
