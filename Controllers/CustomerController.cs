using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using Consume_WebAPI.Models;

namespace Consume_WebAPI.Controllers
{
    public class CustomerController : Controller
    {
        // GET: /Customer
        public ActionResult Index()
        {
            IEnumerable<CustomerViewModel> customer = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:44350/api/");
                var responseTask = client.GetAsync("customer");
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readJob = result.Content.ReadAsAsync<IList<CustomerViewModel>>();
                    readJob.Wait();
                    customer = readJob.Result;

                }
                else
                {
                    //Return the erroe code here
                    customer = Enumerable.Empty<CustomerViewModel>();
                    ModelState.AddModelError(string.Empty, "Server error occured. Please contact admin dor help");

                }

            }

            return View(customer);
        }



        // Post: /Customer
        public ActionResult Create()
        {
            return View();

        }

        [HttpPost]
        public ActionResult Create(CustomerViewModel customer)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:44350/api/customer");
                var postJob = client.PostAsJsonAsync<CustomerViewModel>("customer", customer);
                postJob.Wait();

                var postResult = postJob.Result;
                if (postResult.IsSuccessStatusCode)
                    return RedirectToAction("Index");
            }

            ModelState.AddModelError(string.Empty, "Server occured error. Please check with admin");

            return View(customer);

        }
        // create an action fot getting data by id for edit form

        public ActionResult Edit(int id)
        {
            CustomerViewModel customer = null;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:44350/api/");
                var responseTask = client.GetAsync("customer/" + id.ToString());
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<CustomerViewModel>();
                    readTask.Wait();

                    customer = readTask.Result;
                }
            }
            return View(customer);

        }


        // Create a post method to update the data

        [HttpPost]

        public ActionResult Edit(CustomerViewModel customer)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:44350/api/customer");
                var putTask = client.PutAsJsonAsync<CustomerViewModel>("customer", customer);
                putTask.Wait();


                var result = putTask.Result;
                if (result.IsSuccessStatusCode)
                    return RedirectToAction("Index");
                return View(customer);
            }



        }


        // Delete Action

        public ActionResult Delete(int id)
        {
            using  (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:44350/api/");
                var deleteTask = client.DeleteAsync("customer/" + id.ToString());

                var result = deleteTask.Result;
                if (result.IsSuccessStatusCode)
                    return RedirectToAction("Index");
                    
            }
            return RedirectToAction("Index");
        }





    }
}

