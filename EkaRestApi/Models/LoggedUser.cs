namespace RestApiCore.Models
{
    public class LoggedUser
    {
        public string UserName { get; set; }
        public int AccesslevelId { get; set; }
        public string? Token { get; set; }
    }
}
