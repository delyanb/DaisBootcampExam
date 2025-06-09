using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaisExam.Models
{
    public class Account
    {
        [Key]
        public int AccountId { get; set; }

        [Required(ErrorMessage = "Account number is required.")]
        [StringLength(32, ErrorMessage = "Account number cannot exceed 32 characters.")]
        public string AccountNumber { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Available amount must be non-negative.")]
        public decimal AvailableAmount { get; set; }
    }
}
