using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace WR.Model
{
    public class User : IdentityUser<Guid>
    {
        public virtual ICollection<WarDrobe> WarDrobes { get; set; }

        public virtual ICollection<Article> Articles { get; set; }

        public virtual ICollection<InvitedUser> Invited { get; set; }
    }
}
