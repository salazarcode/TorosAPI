namespace Infra.DTOs.Requests.Classes
{
    public class UpdateClassRQ
    {
        public string Name { get; set; } = "";
        public string Key { get; set; } = "";
        public bool IsPrimitive { get; set; } = false;
    }
}
