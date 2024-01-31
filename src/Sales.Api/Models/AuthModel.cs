using System.ComponentModel.DataAnnotations;

namespace Sales.Api.Models;

public class AuthModel
{
    [Required(ErrorMessage = "O nome não pode ser vazio.")]
    public required string Email { get; set; }


    [Required(ErrorMessage = "A senha não pode ser vazia")]
    public required string Password { get; set; }
}
