using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web_153502_Tolstoi.Domain.Entities
{
    public class CartItem
    {
        public Game? Item { get; set; }
        public int Count { get; set; }
    }
}
