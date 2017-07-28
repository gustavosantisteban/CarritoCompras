using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SeguridadWebv2.Helper
{
    public class PreferencesMP
    {
        public List<Items> items { get; set; }
    }

    public class Items
    {
        public string title { get; set; }
        public int quantity { get; set; }
        public string currency_id { get; set; }
        public decimal unit_price { get; set; }
    }

    public class StatusMP
    {
        public StatusMP()
        {
            Items = new List<Items>();
        }

        public string MPRefID { get; set; }
        public string MPCollectionID { get; set; }
        public IList<Items> Items { get; set; }
        public string Status { get; set; }

    }
}