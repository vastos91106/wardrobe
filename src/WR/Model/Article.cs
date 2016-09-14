using System;
using System.ComponentModel.DataAnnotations;

namespace WR.Model
{
    public class Article
    {
        public Guid ID { get; set; }

        [Required]
        public string Name { get; set; }

        public float Size { get; set; }

        [Required]
        public Guid WardRobeID { get; set; }

        public virtual WarDrobe WarDrobe { get; set; }

        public bool IsBooked { get; set; }

        public Guid UserID { get; set; }

        public DateTime? BookedDate { get; set; }

        public string AvatarPath { get; set; }

        public virtual User BookedByUser { get; set; }
    }
}
