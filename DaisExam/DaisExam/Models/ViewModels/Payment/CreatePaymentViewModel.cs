using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace DaisExam.Models.ViewModels.Payment
{

    public class CreatePaymentViewModel
    {
        public int FromAccountId { get; set; }
        public string FromAccountNumber { get; set; }

        [Required]
        public int ToAccountId { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than 0.")]
        public decimal Amount { get; set; }

        [StringLength(32)]
        public string? Reason { get; set; }

        public List<SelectListItem> ToAccountOptions { get; set; } = new();
    }

}
