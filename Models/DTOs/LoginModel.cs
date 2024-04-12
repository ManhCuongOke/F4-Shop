using System.ComponentModel.DataAnnotations;

public class LoginModel {
    [Required(ErrorMessage = "Email không được trống!")]
    public string sEmail { get; set; }
    [Required(ErrorMessage = "Mật khẩu không được trống")]
    public string sPassword { get; set; }
}