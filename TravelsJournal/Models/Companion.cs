using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace TravelsJournal.Models
{
    public class Companion
    {
        [Key]
        public int CompanionID { get; set; }

        public string CompanionFirstName { get; set; }
        public string CompanionLastName { get; set; }

        //A Companion can accompany on many destinations
        public ICollection<Destination> Destinations { get; set; } //here we are implicitly describing a
                                                                   //bridging table (the only way we can represent
                                                                   //this relationship is with a bridging table that
                                                                   //records every instance of an association between
                                                                   //the destinations and companions


    }

    public class CompanionDto
    {
        public int CompanionID { get; set; }

        public string CompanionFirstName { get; set; }
        public string CompanionLastName { get; set; }
    }
}