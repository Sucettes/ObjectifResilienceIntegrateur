namespace Gwenael.Application.Dtos
{
    public class ResetPasswordDto
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Code { get; set; }
        public string NewPassword { get; set; }
    }
}