using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using POS.Backend.Models;
using POS.Backend.Services;
using System.Data;
using System.Data.SqlClient;

namespace POS.Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly CategoryService _categoryService;
        public CategoryController(CategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        #region Get call Categories
        [HttpGet]
        public IActionResult GetCategories()
        {
            try
            {
                return Ok(_categoryService.GetCategoriesService());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion

        #region Get Category by ID
        [HttpGet("{categoryID}")]
        public IActionResult GetCategory(int categoryID)
        {
            try
            {
                if (categoryID == 0)
                {
                    return BadRequest("Category ID is required.");
                }

                CategoryDataModel item = _categoryService.GetCategoryService(categoryID);
                return item is null ? StatusCode(StatusCodes.Status404NotFound, "No data found.") : Ok(item);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion

        #region Create
        [HttpPost]
        public IActionResult Create([FromBody] CategoryDataModel categoryDataModel)
        {
            try
            {
                if (string.IsNullOrEmpty(categoryDataModel.CategoryName))
                {
                    return BadRequest("Category Name cannot be empty.");
                }

                return _categoryService.CreateService(categoryDataModel) > 0 ? StatusCode(StatusCodes.Status201Created, "Category Created!") : BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion

        #region Update (put)
        [HttpPut]
        public IActionResult Update([FromBody] CategoryDataModel categoryDataModel)
        {
            try
            {
                if (categoryDataModel is null)
                {
                    return BadRequest();
                }

                if (categoryDataModel.CategoryId == 0)
                {
                    return BadRequest("Category ID cannot be empty.");
                }

                if (string.IsNullOrEmpty(categoryDataModel.CategoryName))
                {
                    return BadRequest("Category Name cannot be empty.");
                }

                CategoryDataModel item = _categoryService.GetCategoryService(categoryDataModel.CategoryId);

                if (item is null)
                {
                    return NotFound("No data found");
                }

                return _categoryService.UpdateService(categoryDataModel) > 0 ? StatusCode(StatusCodes.Status202Accepted, "Category Updated!") : StatusCode(StatusCodes.Status400BadRequest);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion

        #region Delete Category
        [HttpDelete]
        public IActionResult PatchCategory(int categoryID)
        {
            try
            {
                if (categoryID == 0)
                {
                    return BadRequest("Category ID is required.");
                }

                CategoryDataModel item = _categoryService.GetCategoryService(categoryID);

                if (item is null)
                {
                    return NotFound("No data found.");
                }

                return _categoryService.PatchCategoryService(categoryID) > 0 ? StatusCode(StatusCodes.Status202Accepted, "Category Data Deleted!") : BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion
    }
}
