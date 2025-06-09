using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaisExam.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Required(ErrorMessage = "Username is required.")]
        [StringLength(64, ErrorMessage = "Username cannot exceed 64 characters.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password hash is required.")]
        [StringLength(255, ErrorMessage = "Password hash cannot exceed 255 characters.")]
        public string PasswordHash { get; set; }

        [Required(ErrorMessage = "Full name is required.")]
        [StringLength(128, ErrorMessage = "Full name cannot exceed 128 characters.")]
        public string FullName { get; set; }
    }
}
