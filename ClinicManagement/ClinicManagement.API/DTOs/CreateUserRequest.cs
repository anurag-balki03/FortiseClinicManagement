namespace ClinicManagement.Api.DTOs
{
    public class CreateUserRequest
    {
        public string FullName { get; set; } = string.Empty;

        public string Username { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;

        public int Role { get; set; }
    }
}