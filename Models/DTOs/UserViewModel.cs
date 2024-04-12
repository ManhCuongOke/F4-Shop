using System.ComponentModel.DataAnnotations;

public class UserViewModel
{
    [Required(ErrorMessage = "Tên không được trống!")]
    public string sName { get; set; }
    [Required(ErrorMessage = "Email không được trống!")]
    public string sEmail { get; set; }
    [Required(ErrorMessage = "Mật khẩu không được trống")]
    public string sPassword { get; set; }
}