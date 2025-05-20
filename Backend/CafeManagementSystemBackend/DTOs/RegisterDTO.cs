namespace CafeManagementSystemBackend.DTOs
{
    public class RegisterDTO
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Role {  get; set; } // (if Admin/if Staff/if Customer)

    }
}
