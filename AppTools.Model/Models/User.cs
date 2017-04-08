using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppTools.Model
{
    public class User
    {
        [Key]
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        public string UserName { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required]
        public string Partner { get; set; }
        [DisplayFormat(DataFormatString = "{0:###-###-####}")]
        public string Telephone { get; set; }
        [Display(Name = "Has Provided Info?")]
        public bool HasProvidedInfo { get; set; }
        [Display(Name = "Can Create Case?")]
        public bool CanCreateCase { get; set; }
        public string Company { get; set; }
        [Required]
        public int UserType { get; set; }
        public string CN { get; set; }
        public string ObjectClass { get; set; }
        public bool IsAutomationUser { get; set; }
        public bool CaseUserTwoFA { get; set; }
        public bool UserUserTwoFA { get; set; }
        public string Policy { get; set; }
        [Display(Name = "Account Locked Time")]
        [DataType(DataType.DateTime)]
        public DateTime pwdAccountLockedTime { get; set; }
        [Display(Name = "Password Changed Time")]
        [DataType(DataType.DateTime)]
        public DateTime pwdChangedTime { get; set; }
        public string pwdPolicySubentry { get; set; }
        [Display(Name = "Password Needs Reset")]
        public bool pwdReset { get; set; }
    }
}
