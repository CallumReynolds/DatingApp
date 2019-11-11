namespace DatingApp.API.Models
{
    // EF needs to know about these models to query data
    public class Value
    {
        public int Id { get; set; }         
        public string Name { get; set; }
    }
}