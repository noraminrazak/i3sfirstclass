using SmartSchoolsV2.Models;
using SmartSchoolsV2.Resources;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartSchoolsV2.ViewModels
{
    public class OrderCartViewModel
    {
        private OrderHistory _orderCart;
        public OrderCartViewModel(OrderHistory order)
        {
            this._orderCart = order;
        }
        public int class_id { get { return _orderCart.class_id; } }
        public string class_name { get { return _orderCart.class_name; } }
        public string total_order { get { return _orderCart.total_order + AppResources._OrderText_; } }
        public string order_status { get { return _orderCart.order_status; } }
        //public TextDecorations decoration { get { return TextDecorations.Strikethrough; } }
        public OrderHistory order
        {
            get => _orderCart;
        }
    }
}
