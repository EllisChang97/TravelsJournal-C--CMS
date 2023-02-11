using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TravelsJournal.Models
{
    public class Destination
    {

        [Key]
        public int DestinationID { get; set; }
        public string DestinationName { get; set; }
        public string DestinationSummary { get; set; }

        //A destination can have only one rating
        //A rating can be applied to many destinations
        [ForeignKey("Rating")]
        public int RatingID { get; set; }
        public virtual Rating Rating { get; set; }


        //A destination can have more than one companion
        public ICollection<Companion> Companions { get; set; } //here we are implicitly describing a
                                                               //bridging table (the only way we can represent
                                                               //this relationship is with a bridging table that
                                                               //reords every instance of an association between
                                                               //the destinations and companions

    }

    //data transfer object
    public class DestinationDto
    {
        public int DestinationID { get; set; }
        public string DestinationName { get; set; }
        public string DestinationSummary { get; set; }
        public int RatingID { get; set; }

    }


}