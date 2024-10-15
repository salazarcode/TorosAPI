namespace Infra.DTOs.Responses.Classes
{
    public class ClassDetailRS
    {
        public int ID { get; set; }
        public string Key { get; set; } = "";
        public string Name { get; set; } = "";
        public List<PropertyDTO> Properties { get; set; } = new();
        public List<ParentDTO> Parents { get; set; } = new();

    }

    public class PropertyDTO
    {
        public int ID { get; set; }
        public string Name { get; set; } = "";
        public string Key { get; set; } = "";
        public string ClassKey { get; set; } = "";
    }
    public class ParentDTO
    {
        public string Key { get; set; } = "";
        public string Name { get; set; } = "";
        public bool IsPrimitive { get; set; } = false;
    }
}
