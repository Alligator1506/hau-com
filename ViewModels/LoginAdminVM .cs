using System.ComponentModel.DataAnnotations;

namespace webtmdt.ViewModels
{
    public class LoginAdminVM
    {
        [Display(Name = "Tên đăng nhập")]
        [Required(ErrorMessage = "Chưa nhập tên đăng nhập")]
        [MaxLength(50, ErrorMessage = "Tối đa 50 kí tự")]
        public string Username { get; set; }

        [Display(Name = "Mật khẩu")]
        [Required(ErrorMessage = "Chưa nhập mật khẩu")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
