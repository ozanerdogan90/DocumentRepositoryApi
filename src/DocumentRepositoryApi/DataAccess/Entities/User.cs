namespace DocumentRepositoryApi.DataAccess.Entities
{
    public class User : BaseEntity
    {
        public string Email { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string Surname { get; set; }
        public string Birthday { get; set; }
    }
}
