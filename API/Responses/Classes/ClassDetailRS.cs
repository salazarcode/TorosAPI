using Domain.Models;

namespace API.Responses.Classes
{
    public class ClassDetailRS
    {
        public int ID { get; set; }
        public string Key { get; set; } = "";
        public string Name { get; set; } = "";
        public List<PropertyDTO> Properties { get; set; } = new();
        public List<AncestryDTO> Ancestries { get; set; } = new();

    }

    public class PropertyDTO
    {
        public int ID { get; set; }
        public string Name { get; set; } = "";
        public string Key { get; set; } = "";
        public int Min { get; set; }
        public int Max { get; set; }
        public string ClassName { get; set; }
    }
    public class AncestryDTO
    {
        public string Key { get; set; } = "";
        public string Name { get; set; } = "";
        public bool IsPrimitive { get; set; } = false;
    }
}
