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
    public class CompanionController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static CompanionController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44346/api/");
        }

        // GET: Companion/List
        public ActionResult List()
        {
            //objective: communicate with our Companion data api to retrieve a list of Companions
            //curl https://localhost:44346/api/Companiondata/listcompanions


            string url = "companiondata/listcompanions";
            HttpResponseMessage response = client.GetAsync(url).Result;

            //Debug.WriteLine("The response code is ");
            //Debug.WriteLine(response.StatusCode);

            IEnumerable<CompanionDto> Companions = response.Content.ReadAsAsync<IEnumerable<CompanionDto>>().Result;
            //Debug.WriteLine("Number of Companions received : ");
            //Debug.WriteLine(Companions.Count());


            return View(Companions);
        }

        // GET: Companion/Details/5
        public ActionResult Details(int id)
        {
            DetailsCompanion ViewModel = new DetailsCompanion();

            //objective: communicate with our Companion data api to retrieve one Companion
            //curl https://localhost:44346/api/Companiondata/findcompanion/{id}

            string url = "companiondata/findCompanion/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            Debug.WriteLine("The response code is ");
            Debug.WriteLine(response.StatusCode);

            CompanionDto SelectedCompanion = response.Content.ReadAsAsync<CompanionDto>().Result;
            Debug.WriteLine("Keeper received : ");
            Debug.WriteLine(SelectedCompanion.CompanionFirstName);

            ViewModel.SelectedCompanion = SelectedCompanion;

            //show all destinations that have been accompanied by this companion
            url = "destinationdata/listdestinationsforcompanion/" + id;
            response = client.GetAsync(url).Result;
            IEnumerable<DestinationDto> AccompaniedDestinations = response.Content.ReadAsAsync<IEnumerable<DestinationDto>>().Result;

            ViewModel.AccompaniedDestinations = AccompaniedDestinations;


            return View(ViewModel);
        }

        public ActionResult Error()
        {

            return View();
        }

        // GET: Companion/New
        public ActionResult New()
        {
            return View();
        }

        // POST: Companion/Create
        [HttpPost]
        public ActionResult Create(Companion Companion)
        {
            Debug.WriteLine("the json payload is :");
            //Debug.WriteLine(Companion.CompanionName);
            //objective: add a new Companion into our system using the API
            //curl -H "Content-Type:application/json" -d @Companion.json https://localhost:44346/api/Companiondata/addCompanion
            string url = "companiondata/addcompanion";


            string jsonpayload = jss.Serialize(Companion);
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

        // GET: Companion/Edit/5
        public ActionResult Edit(int id)
        {
            string url = "companiondata/findcompanion/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            CompanionDto selectedCompanion = response.Content.ReadAsAsync<CompanionDto>().Result;
            return View(selectedCompanion);
        }

        // POST: Companion/Update/5
        [HttpPost]
        public ActionResult Update(int id, Companion Companion)
        {

            string url = "Companiondata/updatecompanion/" + id;
            string jsonpayload = jss.Serialize(Companion);
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

        // GET: Companion/Delete/5
        public ActionResult DeleteConfirm(int id)
        {
            string url = "companiondata/findcompanion/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            CompanionDto selectedCompanion = response.Content.ReadAsAsync<CompanionDto>().Result;
            return View(selectedCompanion);
        }

        // POST: Companion/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            string url = "companiondata/deletecompanion/" + id;
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
