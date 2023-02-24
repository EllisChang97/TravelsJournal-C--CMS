using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TravelsJournal.Models.ViewModels
{
    public class DetailsCompanion 
    {
        public CompanionDto SelectedCompanion { get; set; }
        public IEnumerable<DestinationDto> AccompaniedDestinations { get; set; }
    }
}