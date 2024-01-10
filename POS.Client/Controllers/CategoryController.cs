using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using POS.Client.Models;
using static System.Net.Mime.MediaTypeNames;
using System.Text;

namespace POS.Client.Controllers
{
    public class CategoryController : Controller
    {
        private string _categoryEndpoint = "https://localhost:7087/api/Category";

        #region Get Categories
        public async Task<ViewResult> Index()
        {
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync(_categoryEndpoint);

            if (response.IsSuccessStatusCode)
            {
                string jsonStr = await response.Content.ReadAsStringAsync();
                List<CategoryDataModel>? lst = JsonConvert.DeserializeObject<List<CategoryDataModel>>(jsonStr);

                if (lst is not null)
                {
                    TempData["lst"] = lst;
                }
            }
            return View();
        }
        #endregion

        #region Create Category Page
        [HttpGet("/Category/Create")]
        public IActionResult CreateCategoryPage()
        {
            return View();
        }
        #endregion

        #region Create
        [HttpPost("/Category/Create")]
        public async Task<RedirectToActionResult> Create(CategoryDataModel categoryDataModel)
        {
            string jsonStr = JsonConvert.SerializeObject(categoryDataModel);
            HttpContent httpContent = new StringContent(jsonStr, Encoding.UTF8, Application.Json);

            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.PostAsync(_categoryEndpoint, httpContent);

            if (response.IsSuccessStatusCode)
            {
                string message = await response.Content.ReadAsStringAsync();
                TempData["message"] = message;
            }
            else
            {
                TempData["error"] = "Creating Fail!";
            }

            return RedirectToAction("CreateCategoryPage", "Category");
        }
        #endregion

        #region Eidt Category Page
        [HttpGet("/Category/Edit")]
        public async Task<ViewResult> EditCategoryPage(int categoryID)
        {
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync($"{_categoryEndpoint}/{categoryID}");

            if (response.IsSuccessStatusCode)
            {
                string jsonStr = await response.Content.ReadAsStringAsync();
                CategoryDataModel? item = JsonConvert.DeserializeObject<CategoryDataModel>(jsonStr);

                if (item is not null)
                {
                    TempData["item"] = item;
                }
                else
                {
                    string message = await response.Content.ReadAsStringAsync();
                    TempData["error"] = message;
                }
            }
            return View();
        }
        #endregion
    }
}
