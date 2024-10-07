using Microsoft.AspNetCore.Mvc;

namespace API.Responses.Domain
{
    public class ClassesWithPropertiesDTO
    {
        public int ID { get; set; }
        public string Key { get; set; }
        public string Name { get; set; }
        public bool IsPrimitive { get; set; } = false;
        public ICollection<XPropertyResumeDTO>? XProperties { get; set; }
    }


}
