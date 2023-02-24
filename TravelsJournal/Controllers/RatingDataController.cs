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
    public class RatingDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        
        [HttpGet]
        [ResponseType(typeof(RatingDto))]  
        public IHttpActionResult ListRatings()
        {
            List<Rating> Ratings = db.Ratings.ToList();
            List<RatingDto> RatingDtos = new List<RatingDto>();

            Ratings.ForEach(r => RatingDtos.Add(new RatingDto()
            {
                RatingID = r.RatingID,
                RatingDescription = r.RatingDescription,
            }));

            return Ok(RatingDtos);
        }

        
        [ResponseType(typeof(RatingDto))]
        [HttpGet]
        public IHttpActionResult FindRating(int id)
        {
            Rating Rating = db.Ratings.Find(id);
            RatingDto RatingDto = new RatingDto()
            {
                RatingID = Rating.RatingID,
                RatingDescription = Rating.RatingDescription,
            };
            if (Rating == null)
            {
                return NotFound();
            }

            return Ok(RatingDto);
        }

      
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateRating(int id, Rating Rating)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != Rating.RatingID)
            {

                return BadRequest();
            }

            db.Entry(Rating).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RatingExists(id))  
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return StatusCode(HttpStatusCode.NoContent);
        }

       
        [ResponseType(typeof(Rating))]
        [HttpPost]
        public IHttpActionResult AddRating(Rating Rating)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Ratings.Add(Rating);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = Rating.RatingID }, Rating);
        }

        
        [ResponseType(typeof(Rating))]
        [HttpPost]
        public IHttpActionResult DeleteRating(int id)
        {
            Rating Rating = db.Ratings.Find(id);
            if (Rating == null)
            {
                return NotFound();
            }

            db.Ratings.Remove(Rating);
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

        private bool RatingExists(int id)
        {
            return db.Ratings.Count(e => e.RatingID == id) > 0;
        }
    }
}