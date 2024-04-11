using System.ComponentModel.DataAnnotations;

public class UserViewModel
{
    [Required(ErrorMessage = "Bạn chưa điền tên!")]
    public string sName { get; set; }
    [Required(ErrorMessage = "Bạn chưa điền email!")]
    public string sEmail { get; set; }
    [Required(ErrorMessage = "Bạn chưa điền mật khẩu")]
    public string sPassword { get; set; }
}