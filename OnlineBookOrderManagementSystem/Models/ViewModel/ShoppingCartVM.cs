namespace OnlineBookOrderManagementSystem.Models.ViewModel
{
    public class ShoppingCartVM
    {
        public IEnumerable<ShoppingCartVM> ShoppingCartList { get; set; }
        public double OrderTotal {  get; set; } 
    }
}
