namespace API.Requests.Classes
{
    public class AddPropertyRQ
    {
        public int PropertyClassID { get; set; }
        public string Name { get; set; } = "";
        public string Key { get; set; } = "";
        public int Min { get; set; }
        public int Max { get; set; }
    }
}
