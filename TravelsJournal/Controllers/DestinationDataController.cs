using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using TravelsJournal.Models;
using System.Diagnostics;
using TravelsJournal.Migrations;

namespace TravelsJournal.Controllers
{

    //$1
    public class DestinationDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        /*They are GET requests when we want to READ information from our system*/

        /*They are POST requests when we want to MANIPULATE data in our system*/

        // GET: api/DestinationData/ListDestinations
        [HttpGet]
        [ResponseType(typeof(DestinationDto))]

        //used to be this ##
        //public IEnumerable<DestinationDto> ListDestinations()
        public IHttpActionResult ListDestinations()

        {
            List<Destination> Destinations = db.Destinations.ToList();
            List<DestinationDto> DestinationDtos = new List<DestinationDto>();

            Destinations.ForEach(a => DestinationDtos.Add(new DestinationDto()
            {
                DestinationID = a.DestinationID,
                DestinationName = a.DestinationName,
                DestinationSummary = a.DestinationSummary,
                RatingID = a.Rating.RatingID
            }));

            //##notice how there is no companion 'Keeper' column

            return Ok(DestinationDtos);

        }


        //$2
        [HttpGet]
        [ResponseType(typeof(DestinationDto))]
        public IHttpActionResult ListDestinationsForRating(int id)  
        {
            List<Destination> Destinations = db.Destinations.Where(a => a.RatingID == id).ToList();
            List<DestinationDto> DestinationDtos = new List<DestinationDto>();

            Destinations.ForEach(a => DestinationDtos.Add(new DestinationDto()
            {
                DestinationID = a.DestinationID,
                DestinationName = a.DestinationName,
                DestinationSummary = a.DestinationSummary,
                RatingID = a.Rating.RatingID
                //RatingDescription = a.Rating.RatingDescription    ##Not sure how to add a column to the destination table for the rating description
            }));

            return Ok(DestinationDtos);
        }



        //$3
        [HttpGet]
        [ResponseType(typeof(DestinationDto))]
        public IHttpActionResult ListDestinationsForCompanion(int id)
        {
            //all destinations that have companions which match with our ID
            List<Destination> Destinations = db.Destinations.Where(
                a => a.Companions.Any(
                    c => c.CompanionID == id
                )).ToList();
            List<DestinationDto> DestinationDtos = new List<DestinationDto>();

            Destinations.ForEach(a => DestinationDtos.Add(new DestinationDto()
            {
                DestinationID = a.DestinationID,
                DestinationName = a.DestinationName,
                DestinationSummary = a.DestinationSummary,
                RatingID = a.Rating.RatingID
                //RatingDescription = a.Rating.RatingDescription    ##Not sure how to add a column to the destination table for the rating description
            }));

            return Ok(DestinationDtos);
        }


        //$4
        [HttpPost]
        [Route("api/DestinationData/AssociateDestinationWithCompanion/{destinationid}/{companionid}")]
        public IHttpActionResult AssociateDestinationWithCompanion(int destinationid, int companionid)
        {

            Destination SelectedDestination = db.Destinations.Include(a => a.Companions).Where(a => a.DestinationID == destinationid).FirstOrDefault();
            Companion SelectedCompanion = db.Companions.Find(companionid); 

            if (SelectedDestination == null || SelectedCompanion == null)
            {
                return NotFound();
            }

            Debug.WriteLine("input destination id is: " + destinationid);
            Debug.WriteLine("selected destination name is: " + SelectedDestination.DestinationName);
            Debug.WriteLine("input companion id is: " + companionid);
            Debug.WriteLine("selected companion name is: " + SelectedCompanion.CompanionFirstName);


            SelectedDestination.Companions.Add(SelectedCompanion);
            db.SaveChanges();

            return Ok();
        }



        //$5
        [HttpPost]
        [Route("api/DestinationData/UnAssociateDestinationWithCompanion/{destinationid}/{companionid}")]
        public IHttpActionResult UnAssociateDestinationWithCompanion(int destinationid, int companionid)
        {

            Destination SelectedDestination = db.Destinations.Include(a => a.Companions).Where(a => a.DestinationID == destinationid).FirstOrDefault();
            Companion SelectedCompanion = db.Companions.Find(companionid); //## come back to this error (Companions is in red)

            if (SelectedDestination == null || SelectedCompanion == null)
            {
                return NotFound();
            }

            Debug.WriteLine("input destination id is: " + destinationid);
            Debug.WriteLine("selected destination name is: " + SelectedDestination.DestinationName);
            Debug.WriteLine("input companion id is: " + companionid);
            Debug.WriteLine("selected companion name is: " + SelectedCompanion.CompanionFirstName);


            SelectedDestination.Companions.Remove(SelectedCompanion);
            db.SaveChanges();

            return Ok();
        }


        // GET: api/DestinationData/FindDestination/5
        //$6
        [HttpGet]
        [ResponseType(typeof(DestinationDto))] 
        public IHttpActionResult FindDestination(int id)
        {
            Destination Destination = db.Destinations.Find(id);
            DestinationDto DestinationDto = new DestinationDto()
            {
                DestinationID = Destination.DestinationID,
                DestinationName = Destination.DestinationName,
                DestinationSummary = Destination.DestinationSummary,
                RatingID = Destination.Rating.RatingID
                //RatingDescription = a.Rating.RatingDescription    ##Not sure how to add a column to the destination table for the rating description

            };

            if (Destination == null)
            {
                return NotFound();
            }

            return Ok(DestinationDto);
        }

        //here i think i need to use the JSON data
        // POST: api/DestinationData/UpdateDestination/5

        //$7
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateDestination(int id, Destination destination)
        {

            Debug.WriteLine("I have reached the update destination method");
            if (!ModelState.IsValid)
            {
                Debug.WriteLine("Model State is invalid");
                return BadRequest(ModelState);
            }

            if (id != destination.DestinationID)
            {
                Debug.WriteLine("ID mismatch");
                Debug.WriteLine("GET parameter" + id);
                Debug.WriteLine("POST parameter" + destination.DestinationID);

                return BadRequest();
            }

            db.Entry(destination).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DestinationExists(id))
                {
                    Debug.WriteLine("Destination not found");
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            Debug.WriteLine("None of the conditions triggered");
            return StatusCode(HttpStatusCode.NoContent);
        }


        // POST: api/DestinationData/AddDestination

        //$8
        [ResponseType(typeof(Destination))]
        [HttpPost]
        public IHttpActionResult AddDestination(Destination destination)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Destinations.Add(destination);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = destination.DestinationID }, destination);
        }



        //$9
        // POST: api/DestinationData/DeleteDestination/5
        [ResponseType(typeof(Destination))]
        [HttpPost]
        public IHttpActionResult DeleteDestination(int id)
        {
            Destination destination = db.Destinations.Find(id);
            if (destination == null)
            {
                return NotFound();
            }

            db.Destinations.Remove(destination);
            db.SaveChanges();

            return Ok();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool DestinationExists(int id)
        {
            return db.Destinations.Count(e => e.DestinationID == id) > 0;
        }
    }
}