using AW_lab10.ViewModels;
using System.Collections.Generic;

namespace Lista11.ViewModels
{
    public enum PaymentMethod
    {
        Cash,
        Transfer,
        Blik
    }

    public class ConfirmViewModel
    {
        

        public string name { get; set; }
        public string surname { get; set; }
        public string address { get; set; }

        public PaymentMethod paymentMethod { get; set; }

        public List<CartItemViewModel> basket { get; set; }
    }
}
