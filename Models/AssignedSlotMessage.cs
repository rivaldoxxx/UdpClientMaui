using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UdpClientMaui.Models
{
    public class AssignedSlotMessage
    {
        public string ProductName { get; set; }
        public int[] Slots { get; set; }
    }
}
