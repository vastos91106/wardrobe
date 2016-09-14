using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WR.Model
{
    public class WarDrobe
    {
        public Guid ID { get; set; }

        [Required]
        public string Name { get; set; }

        public Guid UserID { get; set; }

        public virtual User Owner { get; set; }

        public bool IsPrivate { get; set; }

        public virtual ICollection<Article> Articles { get; set; }

        public virtual ICollection<InvitedUser> InvitedUsers { get; set; }
    }
}
