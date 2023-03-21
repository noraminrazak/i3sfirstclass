using SmartSchoolsV2.Resources;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartSchoolsV2.Models
{
    public class Order
    {
        public string pickup_date { get; set; }
        public string total_order { get; set; }
        public string footer { get; set; }
        public List<OrderHistory> listOrder { get; set; }

        public bool is_visible { get; set; } = false;

        public Order()
        {
        }

        public Order(string pickupDate, string totalOrder, string totalAmount, List<OrderHistory> listOrders, string fooTer)
        {
            pickup_date = pickupDate;
            if (Convert.ToInt16(totalOrder) > 0)
            {
                total_order = totalOrder + AppResources._OrderTextComma_ + totalAmount;
            }
            else
            {
                total_order = totalOrder + AppResources._OrderText_;
            }
            listOrder = listOrders;

            footer = fooTer;
        }
    }
}
