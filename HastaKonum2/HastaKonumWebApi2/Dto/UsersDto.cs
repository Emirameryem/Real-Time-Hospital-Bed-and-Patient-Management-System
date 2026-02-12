namespace HastaKonumWebApi2.Dto
{
    public class UsersDto
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Role { get; set; }
        // Şifreyi dahil etmiyoruz!
    }
}
