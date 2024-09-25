﻿using System.ComponentModel.DataAnnotations;

namespace Go.APIs.Dtos.AccountDtos
{
    public class SignInDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
