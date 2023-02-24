using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TravelsJournal.Models.ViewModels
{
    public class DetailsRating 
    {
        //the Rating itself that we want to display
        public RatingDto SelectedRating { get; set; }

        //all of the related destinations to that particular rating
        public IEnumerable<DestinationDto> RelatedDestinations { get; set; }
    }
}