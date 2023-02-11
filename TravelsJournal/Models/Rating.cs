using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace TravelsJournal.Models
{
    public class Rating
    {

        [Key]
        public int RatingID { get; set; }

        public string RatingDescription { get; set; }
    }
}