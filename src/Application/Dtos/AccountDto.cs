using System;

namespace Gwenael.Application.Dtos
{
    public class AccountDto : UserDto
    {
        public string[] Permissions { get; set; }
    }
}