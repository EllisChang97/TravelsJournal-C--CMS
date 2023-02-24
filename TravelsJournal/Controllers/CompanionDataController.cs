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
    public class CompanionDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        [HttpGet]
        [ResponseType(typeof(CompanionDto))]
        public IHttpActionResult ListCompanions()
        {
            List<Companion> Companions = db.Companions.ToList();
            List<CompanionDto> CompanionDtos = new List<CompanionDto>();

            Companions.ForEach(c => CompanionDtos.Add(new CompanionDto()
            {
                CompanionID = c.CompanionID,
                CompanionFirstName = c.CompanionFirstName,
                CompanionLastName = c.CompanionLastName
            }));

            return Ok(CompanionDtos);
        }

      
        [HttpGet]
        [ResponseType(typeof(CompanionDto))]
        public IHttpActionResult ListCompanionsForDestination(int id)
        {
            List<Companion> Companions = db.Companions.Where(
                c => c.Destinations.Any(
                    d => d.DestinationID == id)
                ).ToList();
            List<CompanionDto> CompanionDtos = new List<CompanionDto>();

            Companions.ForEach(c => CompanionDtos.Add(new CompanionDto()
            {
                CompanionID = c.CompanionID,
                CompanionFirstName = c.CompanionFirstName,
                CompanionLastName = c.CompanionLastName
            }));

            return Ok(CompanionDtos);
        }


        

        [HttpGet]
        [ResponseType(typeof(CompanionDto))]
        public IHttpActionResult ListCompanionsNotAccompaniedOnDestination(int id) 
        {
            List<Companion> Companions = db.Companions.Where(
                c => !c.Destinations.Any(
                    d => d.DestinationID == id)
                ).ToList();
            List<CompanionDto> CompanionDtos = new List<CompanionDto>();

            Companions.ForEach(c => CompanionDtos.Add(new CompanionDto()
            {
                CompanionID = c.CompanionID,
                CompanionFirstName = c.CompanionFirstName,
                CompanionLastName = c.CompanionLastName
            }));

            return Ok(CompanionDtos);
        }


        [ResponseType(typeof(CompanionDto))]
        [HttpGet]
        public IHttpActionResult FindCompanion(int id)
        {
            Companion Companion = db.Companions.Find(id);
            CompanionDto CompanionDto = new CompanionDto()
            {
               CompanionID = Companion.CompanionID,
                CompanionFirstName = Companion.CompanionFirstName,
                CompanionLastName = Companion.CompanionLastName
            };
            if (Companion == null)
            {
                return NotFound();
            }

            return Ok(CompanionDto);
        }


        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateCompanion(int id, Companion Companion)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != Companion.CompanionID)
            {

                return BadRequest();
            }

            db.Entry(Companion).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CompanionExists(id))
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

        
        [ResponseType(typeof(Companion))]
        [HttpPost]
        public IHttpActionResult AddCompanion(Companion Companion)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Companions.Add(Companion);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = Companion.CompanionID }, Companion);
        }

        
        [ResponseType(typeof(Companion))]
        [HttpPost]
        public IHttpActionResult DeleteCompanion(int id)
        {
            Companion Companion = db.Companions.Find(id);
            if (Companion == null)
            {
                return NotFound();
            }

            db.Companions.Remove(Companion);
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

        private bool CompanionExists(int id)
        {
            return db.Companions.Count(e => e.CompanionID == id) > 0;
        }
    }
}