using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using TravelsJournal.Models;
using TravelsJournal.Models.ViewModels;
using System.Web.Script.Serialization;

namespace TravelsJournal.Controllers
{
    public class RatingController : Controller
    {

        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static RatingController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44346/api/");
        }



        //$1
        // GET: Rating/List
        public ActionResult List()
        {
            //objective: communicate with our Rating data api to retrieve a list of Ratings
            //curl https://localhost:44346/api/Ratingdata/listRatings


            string url = "ratingdata/listratings";
            HttpResponseMessage response = client.GetAsync(url).Result;

            //Debug.WriteLine("The response code is ");
            //Debug.WriteLine(response.StatusCode);

            IEnumerable<RatingDto> ratings = response.Content.ReadAsAsync<IEnumerable<RatingDto>>().Result; //##RatingDto is probably in red bc I haven't coded in the ratingdatacontroller yet
            //Debug.WriteLine("Number of Ratings received : ");
            //Debug.WriteLine(Ratings.Count());


            return View(ratings);
        }


        //$2 
        // GET: Rating/Details/5
        public ActionResult Details(int id)
        {
            //objective: communicate with our Rating data api to retrieve one Rating
            //curl https://localhost:44346/api/Ratingdata/findrating/{id}

            DetailsRating ViewModel = new DetailsRating();

            string url = "ratingdata/findrating/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            Debug.WriteLine("The response code is ");
            Debug.WriteLine(response.StatusCode);

            RatingDto SelectedRating = response.Content.ReadAsAsync<RatingDto>().Result;
            Debug.WriteLine("Rating received : ");
            Debug.WriteLine(SelectedRating.RatingID);

            ViewModel.SelectedRating = SelectedRating;

            //showcase information about destinations related to this rating
            //send a request to gather information about destinations related to a particular rating ID
            url = "destinationdata/listdestinationsforrating/" + id;
            response = client.GetAsync(url).Result;
            IEnumerable<DestinationDto> RelatedDestinations = response.Content.ReadAsAsync<IEnumerable<DestinationDto>>().Result;

            ViewModel.RelatedDestinations = RelatedDestinations;


            return View(ViewModel);
        }


        public ActionResult Error()
        {

            return View();
        }

        // GET: Rating/New
        public ActionResult New()
        {
            return View();
        }



        //$3
        // POST: Rating/Create
        [HttpPost]
        public ActionResult Create(Rating Rating)
        {
            Debug.WriteLine("the json payload is :");
            //Debug.WriteLine(Rating.RatingName);
            //objective: add a new Rating into our system using the API
            //curl -H "Content-Type:application/json" -d @Rating.json https://localhost:44346/api/Ratingdata/addRating 
            string url = "ratingdata/addrating";


            string jsonpayload = jss.Serialize(Rating);
            Debug.WriteLine(jsonpayload);

            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";

            HttpResponseMessage response = client.PostAsync(url, content).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }


        }

        //$4
        // GET: Rating/Edit/5
        public ActionResult Edit(int id)
        {
            string url = "ratingdata/findrating/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            RatingDto selectedRating = response.Content.ReadAsAsync<RatingDto>().Result;
            return View(selectedRating);
        }


        //$5
        // POST: Rating/Update/5
        [HttpPost]
        public ActionResult Update(int id, Rating Rating)
        {

            string url = "ratingdata/updaterating/" + id;
            string jsonpayload = jss.Serialize(Rating);
            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;
            Debug.WriteLine(content);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }


        //$6
        // GET: Rating/Delete/5
        public ActionResult DeleteConfirm(int id)
        {
            string url = "ratingdata/findrating/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            RatingDto selectedRating = response.Content.ReadAsAsync<RatingDto>().Result;
            return View(selectedRating);
        }



        //$7
        // POST: Rating/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            string url = "ratingdata/deleterating/" + id;
            HttpContent content = new StringContent("");
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }
        
    }
}
