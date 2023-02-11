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

namespace TravelsJournal.Controllers
{
    public class DestinationDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        /*They are GET requests when we want to READ information from our system*/

        /*They are POST requests when we want to MANIPULATE data in our system*/

        // GET: api/DestinationData/ListDestinations
        [HttpGet]
        public IEnumerable<DestinationDto> ListDestinations()
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

            return DestinationDtos;

        }

        // GET: api/DestinationData/FindDestination/5
        [ResponseType(typeof(Destination))]
        [HttpGet]
        public IHttpActionResult FindDestination(int id)
        {
            Destination Destination = db.Destinations.Find(id);
            DestinationDto DestinationDto = new DestinationDto()
            {
                DestinationID = Destination.DestinationID,
                DestinationName = Destination.DestinationName,
                DestinationSummary = Destination.DestinationSummary,
                RatingID = Destination.Rating.RatingID
            };

            if (Destination == null)
            {
                return NotFound();
            }

            return Ok(DestinationDto);
        }

        //here i think i need to use the JSON data
        // POST: api/DestinationData/UpdateDestination/5
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


        //here i think i need to use the JSON data
        // POST: api/DestinationData/AddDestination
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