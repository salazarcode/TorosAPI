namespace API.Responses.Domain
{
    public class XPropertyResumeDTO
    {
        public int ID { get; set; }
        public string? Key { get; set; }
        public string? Name { get; set; }
        public bool IsNullable { get; set; }
        public int Lenght { get; set; }
        public string? PropertyClassKey { get; set; }
    }
}
