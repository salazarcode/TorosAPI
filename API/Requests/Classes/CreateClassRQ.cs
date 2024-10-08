namespace API.Requests.Classes
{
    public class CreateClassRQ
    {
        public string Name { get; set; }
        public bool IsPrimitive { get; set; } = false;
    }
}
