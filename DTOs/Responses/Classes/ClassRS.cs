namespace Infra.DTOs.Responses.Classes
{
    public class ClassRS
    {
        public int ID { get; set; }
        public string Key { get; set; } = "";
        public string Name { get; set; } = "";
        public bool IsPrimitive { get; set; } = false;
    }
}
