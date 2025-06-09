using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaisExam.Models
{
    public class Payment
    {
        [Key]
        public int PaymentId { get; set; }

        [Required(ErrorMessage = "From account is required.")]
        public int FromAccountId { get; set; }

        [Required(ErrorMessage = "To account is required.")]
        public int ToAccountId { get; set; }

        [Required(ErrorMessage = "Amount is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than 0.")]
        public decimal Amount { get; set; }

        [StringLength(32, ErrorMessage = "Reason cannot exceed 32 characters.")]
        public string Reason { get; set; }

        [Required(ErrorMessage = "Date and time of payment is required.")]
        public DateTime DateTimeMade { get; set; }

        [Required(ErrorMessage = "User is required for the payment.")]
        public int UserId { get; set; }
        [Required]
        public string Status { get; set; }

        public DateTime? DateApproved { get; set; }
    }
   
}
