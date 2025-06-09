namespace DaisExam.Models.ViewModels.Payment
{
    public class PaymentListViewModel
    {
        public List<PaymentVM> PaymentVMs { get; set; }
    }
    public class PaymentListViewModelSeperated
    {
        public List<PaymentVM> PendingPayments { get; set; }
        public List<PaymentVM> ApprovedPayments { get; set; }
    }
    public class PaymentVM
    {
        public int PaymentId { get; set; }
        public int FromAccountId { get; set; }
        public string FromAccountNumber { get; set; }
        public int ToAccountId { get; set; }
        public string ToAccountNumber { get; set; }
        public decimal Amount { get; set; }
        public string? Reason { get; set; }
        public DateTime DateTimeMade { get; set; }
        public bool UserIsAccountOwner { get; set; }
        public string Status { get; set; }
        public DateTime? DateApproved { get; set; }
    }
}
