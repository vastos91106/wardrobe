using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WR.Model
{
    public class InvitedUser
    {
        public Guid ID { get; set; }

        public Guid UserID { get; set; }

        public virtual User User { get; set; }

        public Guid WarDrobeID { get; set; }

        public virtual WarDrobe WarDrobe { get; set; }
    }
}
