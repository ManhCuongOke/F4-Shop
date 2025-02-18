﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project.Models
{
    public class User
    {
        public int PK_iUserID { get; set; }
        [Required(ErrorMessage = "Bạn chưa nhập tên!")]
        public string sName { get; set; }
        // [NotMapped]
        public string sAddress { get; set; }
        [Required(ErrorMessage = "Bạn chưa nhập email")]
        public string sEmail { get; set; }
        public int FK_iRoleID { get; set; }
        [Required(ErrorMessage = "Bạn chưa nhập mật khẩu")]
        public string sPassword { get; set; }
        [NotMapped]
        public DateTime dDateBirth {  get; set; }
        [NotMapped]
        public string sPhone {  get; set; }
        [NotMapped]
        public DateTime dCreatedTime { get; set; }
        [NotMapped]
        public DateTime dUpdatedTime { get; set;}
        [NotMapped]
        public DateTime? dDeletedTime { get; set; }

    }
}
