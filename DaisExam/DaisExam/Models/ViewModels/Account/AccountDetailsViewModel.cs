using DaisExam.Models.ViewModels.Payment;

namespace DaisExam.Models.ViewModels.Account
{
    public class AccountDetailsViewModel
    {
        public AccountDetailsVM AccountDetailsInfo { get; set; }
        public PaymentListViewModel PaymentList { get; set; }
    }
    public class AccountDetailsVM
    {
        
            public int AccountId { get; set; }

            public string AccountNumber { get; set; }

            public List<string> OtherOwners { get; set; }

            public decimal AvailableAmount { get; set; }
        
    }
    
}
