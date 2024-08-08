using System.ComponentModel.DataAnnotations;

namespace LMSweb_v3.ViewModels;
public class LoginViewModel
{
    public required string ID { get; set; }

    [DataType(DataType.Password)]
    public required string Password { get; set; }
}