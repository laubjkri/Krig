using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Krig
{
    public class Player
    {
        public int Score { get; set; }        
        public Card? CurrentCard { get; set; }
        public int PlayerId { get; set; }

    }
}
