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
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Partner { get; set; }
        public string Telephone { get; set; }
        public bool HasProvidedInfo { get; set; }
        public bool CanCreateCase { get; set; }
        public string Company { get; set; }
        public string CN { get; set; }
        public DateTime pwdAccountLockedTime { get; set; }
        public DateTime pwdChangedTime { get; set; }
        public string pwdPolicySubentry { get; set; }
        public bool pwdReset { get; set; }
    }
}
