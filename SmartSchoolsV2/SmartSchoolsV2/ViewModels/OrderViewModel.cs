using MvvmHelpers;
using SmartSchoolsV2.Models;
using System.ComponentModel;

namespace SmartSchoolsV2.ViewModels
{
    public class OrderViewModel : ObservableRangeCollection<OrderCartViewModel>, INotifyPropertyChanged
    {
        private ObservableRangeCollection<OrderCartViewModel> orderCart = new ObservableRangeCollection<OrderCartViewModel>();
        public OrderViewModel(Order order, bool expanded = true) //expanded = false by default
        {
            this.orders = order;
            this._expanded = expanded;

            foreach (OrderHistory oc in order.listOrder)
            {
                orderCart.Add(new OrderCartViewModel(oc));
            }
            if (expanded)
                this.AddRange(orderCart);
        }

        public OrderViewModel()
        {
        }
        private bool _expanded;
        public bool Expanded
        {
            get { return _expanded; }
            set
            {
                if (_expanded != value)
                {
                    _expanded = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("Expanded"));
                    OnPropertyChanged(new PropertyChangedEventArgs("StateIcon"));
                    if (_expanded)
                    {
                        this.AddRange(orderCart);
                    }
                    else
                    {
                        this.Clear();
                    }
                }
            }
        }

        public string StateIcon
        {
            get
            {
                if (Expanded)
                {
                    return "arrow_a.png";
                }
                else
                { return "arrow_b.png"; }
            }
        }
        public string pickup_date { get { return orders.pickup_date; } }
        public string total_order { get { return orders.total_order; } }
        public string footer { get { return orders.footer; } }
        public Order orders { get; set; }
    }
}
