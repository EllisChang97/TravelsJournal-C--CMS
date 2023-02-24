using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Http;
using System.Diagnostics;
using TravelsJournal.Models;
using TravelsJournal.Models.ViewModels;
using System.Web.Script.Serialization;

namespace TravelsJournal.Controllers
{
    public class DestinationController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static DestinationController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44346/api/");
        }


        // GET: Destination/List
        // https://localhost:44346/destination/list
        public ActionResult List()
        {
            //to communicate with our destination data API to retrieve a list of destinations
            //curl https://localhost:44346/api/destinationdata/listdestinations

            string url = "destinationdata/listdestinations";
            HttpResponseMessage response = client.GetAsync(url).Result;

            //Debug.WriteLine("The response code is ");
            //Debug.WriteLine(response.StatusCode);

            IEnumerable<DestinationDto> destinations = response.Content.ReadAsAsync<IEnumerable<DestinationDto>>().Result;
            //Debug.WriteLine("Number of destinations recieved : ");
            //Debug.WriteLine(destinations.Count());

            return View(destinations);
        }





        // GET: Destination/Details/5
        public ActionResult Details(int id)
        {
            DetailsDestination ViewModel = new DetailsDestination();

            //to communicate with our destination data API to retrieve one destination
            //curl https://localhost:44346/api/destinationdata/finddestination/{id}

            string url = "destinationdata/finddestination/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            //Debug.WriteLine("The response code is ");
            //Debug.WriteLine(response.StatusCode);

            DestinationDto SelectedDestination = response.Content.ReadAsAsync<DestinationDto>().Result;
            //Debug.WriteLine("Destination recieved : ");
            //Debug.WriteLine(selecteddestination.DestinationName);

            ViewModel.SelectedDestination = SelectedDestination;

            //HttpClient companionClient = new HttpClient();
            //companionClient.BaseAddress = new Uri("https://localhost:44346/api/");

            //show associated companions with this destination
            url = "companiondata/listcompanionsfordestination/" + id;
            response = client.GetAsync(url).Result;
            IEnumerable<CompanionDto> AccompaniedCompanions = response.Content.ReadAsAsync<IEnumerable<CompanionDto>>().Result;

            ViewModel.AccompaniedCompanions = AccompaniedCompanions;

            url = "companiondata/listcompanionsnotaccompaniedondestination/" + id;
            response = client.GetAsync(url).Result;
            IEnumerable<CompanionDto> AvailableCompanions = response.Content.ReadAsAsync<IEnumerable<CompanionDto>>().Result;

            ViewModel.AvailableCompanions = AvailableCompanions;


            // ## newest addition - trying to get the rating description on my destination details page
            //url = "ratingforthisdestination/" + id;
            //response = client.GetAsync(url).Result;
            //RatingDto RatingForDestination = response.Content.ReadAsAsync<RatingDto>().Result;

            //ViewModel.RatingForDestination = RatingForDestination;


            return View(ViewModel);
        }



        //POST: Destination/Associate/{destinationid}
        [HttpPost]
        public ActionResult Associate(int id, int CompanionID)
        {
            Debug.WriteLine("Attempting to associate destination :" + id + " with companion " + CompanionID);

            //call our api to associate destination with companion
            string url = "destinationdata/associatedestinationwithcompanion/" + id + "/" + CompanionID;
            HttpContent content = new StringContent("");
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            return RedirectToAction("Details/" + id);
        }


        //Get: Destination/UnAssociate/{id}?CompanionID={CompanionID}
        [HttpGet]
        public ActionResult UnAssociate(int id, int CompanionID)
        {
            Debug.WriteLine("Attempting to unassociate destination :" + id + " with companion: " + CompanionID);

            //call our api to associate destination with companion
            string url = "destinationdata/unassociatedestinationwithcompanion/" + id + "/" + CompanionID;
            HttpContent content = new StringContent("");
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            return RedirectToAction("Details/" + id);
        }

        public ActionResult Error()
        {
            return View();
        }






        // GET: Destination/New
        public ActionResult New()
        {
            //information about all ratings in the system.
            //GET api/ratingdata/listratings

            string url = "ratingdata/listratings";
            HttpResponseMessage response = client.GetAsync(url).Result;
            IEnumerable<RatingDto> RatingOptions = response.Content.ReadAsAsync<IEnumerable<RatingDto>>().Result;


            return View(RatingOptions);
        }


        // POST: Destination/Create
        [HttpPost]
        public ActionResult Create(Destination destination)
        {
            Debug.WriteLine("the json payload is :");
            //Debug.WriteLine(destination.DestinationName);
            //objective: add a new destination into our system using the API
            //curl -H "Content-Type:application/json" -d @destination.json https://localhost:44346/api/destinationdata/adddestination
            string url = "destinationdata/adddestination";

            string jsonpayload = jss.Serialize(destination);

            Debug.WriteLine(jsonpayload);

            HttpContent content= new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";

            HttpResponseMessage response = client.PostAsync(url, content).Result;
            if(response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }

        }


        // GET: Destination/Edit/5
        public ActionResult Edit(int id)
        {
            UpdateDestination ViewModel = new UpdateDestination();

            //the existing destination information
            string url = "destinationdata/finddestination/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            DestinationDto SelectedDestination = response.Content.ReadAsAsync<DestinationDto>().Result;
            
            ViewModel.SelectedDestination = SelectedDestination;

            // all ratings to choose from when updating this destination
            //the existing destination information
            url = "ratingdata/listratings/";
            response = client.GetAsync(url).Result;
            IEnumerable<RatingDto> RatingOptions = response.Content.ReadAsAsync<IEnumerable<RatingDto>>().Result;

            ViewModel.RatingOptions = RatingOptions;

            return View(ViewModel);

            
        }


        // POST: Destination/Edit/5
        [HttpPost]
        public ActionResult Update(int id, Destination destination)
        {

            string url = "destinationdata/updatedestination/" + id;
            string jsonpayload = jss.Serialize(destination);
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


        // GET: Destination/Delete/5
        public ActionResult DeleteConfirm(int id)
        {
            string url = "destinationdata/finddestination/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            DestinationDto selecteddestination = response.Content.ReadAsAsync<DestinationDto>().Result;
            return View(selecteddestination);
        }


        // POST: Destination/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {

            string url = "destinationdata/deletedestination/" + id;
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
