namespace DaisExam.Models.ViewModels.Account
{
    public class AccountInfosListViewModel
    {
        public List<AccountInfoVM> AccountVMs { get; set; }
    }
    public class AccountInfoVM
    {
        public int AccountId { get; set; }
        public List<string> OtherOwners { get; set; }
        public string AccountNumber { get; set; }
    }

}
