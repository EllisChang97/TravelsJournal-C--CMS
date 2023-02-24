using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TravelsJournal.Models.ViewModels
{
    public class UpdateDestination 
    {
        //This viewmodel is a class which stores information that we need to present to /Destination/Update/{}
        
        //the existing destination information

        public DestinationDto SelectedDestination { get; set; }

        // all ratings to choose from when updating this destination

        public IEnumerable<RatingDto> RatingOptions { get; set; }
    }
}