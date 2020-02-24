using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestClient.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace RestClient.Controllers
{
    public class PagesController : Controller
    {

        //GET /Pages 
        public async Task<IActionResult> Index()
        {
            var pages = new List<Page>();

            using (var httpClient = new HttpClient())
            {
                using var request = await httpClient.GetAsync("https://localhost:44352/api/pages");
                var response = await request.Content.ReadAsStringAsync();
                pages = JsonConvert.DeserializeObject<List<Page>>(response);
            }

            return View(pages);
        }

        //GET /Pages 
        public async Task<IActionResult> Edit(Guid id)
        {
            var page = new Page();


            using (var httpClient = new HttpClient())
            {
                using var request = await httpClient.GetAsync($"https://localhost:44352/api/pages/{id}");
                var response = await request.Content.ReadAsStringAsync();
                page = JsonConvert.DeserializeObject<Page>(response);
            }

            return View(page);
        }

        //Post /Pages/edit/id
        [HttpPost]

        public async Task<IActionResult> Edit(Page page)
        {
            page.Slug = page.Title.Replace(" ", "-").ToLower();


            using (var httpClient = new HttpClient())
            {
                var content = new StringContent(JsonConvert.SerializeObject(page), Encoding.UTF8, "application/json");
                using var request = await httpClient.PutAsync($"https://localhost:44352/api/pages/{page.Id}", content);
                var response = await request.Content.ReadAsStringAsync();

            }

            return Redirect(Request.Headers["Referer"].ToString());
        }

        //Get pages/create
        public IActionResult Create() => View();

        //Post /Pages/create
        [HttpPost]
        public async Task<IActionResult> Create(Page page)
        {
            page.Slug = page.Title.Replace(" ", "-").ToLower();
            page.Sorting = 100;

            using (var httpClient = new HttpClient())
            {
                var content = new StringContent(JsonConvert.SerializeObject(page), Encoding.UTF8, "application/json");
                using var request = await httpClient.PostAsync($"https://localhost:44352/api/pages", content);
                var response = await request.Content.ReadAsStringAsync();

            }

            return RedirectToAction("Index");
        }
        //GET /Pages/Delete 
        public async Task<IActionResult> Delete(Guid id)
        {



            using (var httpClient = new HttpClient())
            {
                using var request = await httpClient.DeleteAsync($"https://localhost:44352/api/pages/{id}");

            }

            return RedirectToAction("Index");
        }
    }
}