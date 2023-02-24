using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace TravelsJournal.Models.ViewModels
{
    public class DetailsDestination
    {
        public DestinationDto SelectedDestination { get; set; }

        public RatingDto RatingForDestination { get; set; } //This is not working
        public IEnumerable<CompanionDto> AccompaniedCompanions { get; set; }

        //the AvailableCompanions below is for a drop down for editing and adding other companions in an edit field
        public IEnumerable<CompanionDto> AvailableCompanions { get; set; }
    }
}