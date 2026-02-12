namespace HastaKonumWebApi2.Dto
{
    public class CreateUsersDto
    {
        public string Username { get; set; }
        public string Password { get; set; } // Şifre alınır ama sadece içte hash'lenir
        public string Role { get; set; }
    }
}
