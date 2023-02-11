using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Http;
using System.Diagnostics;
using TravelsJournal.Models;
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
            client.BaseAddress = new Uri("https://localhost:44346/api/destinationdata/");
        }


        // GET: Destination/List
        public ActionResult List()
        {
            //to communicate with our destination data API to retrieve a list of destinations
            //curl https://localhost:44346/api/destinationdata/listdestinations

            string url = "listdestinations";
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

            //to communicate with our destination data API to retrieve one destinations
            //curl https://localhost:44346/api/destinationdata/finddestination/{id}

            string url = "finddestination/"+id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            //Debug.WriteLine("The response code is ");
            //Debug.WriteLine(response.StatusCode);

            DestinationDto selecteddestination = response.Content.ReadAsAsync<DestinationDto>().Result;
            //Debug.WriteLine("Destination recieved : ");
            //Debug.WriteLine(selecteddestination.DestinationName);

            return View(selecteddestination);
        }

        public ActionResult Error()
        {
            return View();
        }

        // GET: Destination/New
        public ActionResult New()
        {
            return View();
        }

        // POST: Destination/Create
        [HttpPost]
        public ActionResult Create(Destination destination)
        {
            Debug.WriteLine("the json payload is :");
            //Debug.WriteLine(destination.DestinationName);
            //objective: add a new destination into our system using the API
            //curl -H "Content-Type:application/json" -d @animal.json https://localhost:44346/api/destinationdata/adddestination
            string url = "adddestination";

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
            return View();
        }

        // POST: Destination/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Destination/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Destination/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
