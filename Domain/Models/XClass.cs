namespace Domain.Models
{
    public class XClass
    {
        public int ID { get; set; }
        public string? Name { get; set; }
        public bool IsPrimitive { get; set; } = false;
        public List<XProperty> XProperties { get; set; } = new();
        public List<XAncestry> XAncestries { get; set; } = new();
    }
}
