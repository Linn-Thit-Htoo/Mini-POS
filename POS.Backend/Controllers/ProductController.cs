using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using POS.Backend.Models;

namespace POS.Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly AppDbContext _appDbContext;
        public ProductController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        #region Get Products
        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            try
            {
                var lst = await _appDbContext.Products.Where(x => x.IsDeleted == false).ToListAsync();
                return Ok(lst);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Get Product
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProduct(int id)
        {
            try
            {
                if (id == 0)
                {
                    return BadRequest("Product ID is required!");
                }

                var item = await _appDbContext.Products.Where(product => product.ProductId == id && product.IsDeleted == false).FirstOrDefaultAsync();

                return item is null ? NotFound("No data found.") : Ok(item);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Create Product
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ProductDataModel productDataModel)
        {
            try
            {
                if (productDataModel is null || productDataModel.CategoryId == 0 || string.IsNullOrEmpty(productDataModel.ProductName) || productDataModel.Quantity == 0 || productDataModel.Price == 0)
                {
                    return BadRequest();
                }

                productDataModel.CreatedDate = DateTime.Now;

                await _appDbContext.Products.AddAsync(productDataModel);

                int result = await _appDbContext.SaveChangesAsync();

                return result > 0 ? StatusCode(StatusCodes.Status201Created, "Product Created!") : BadRequest();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Update Product
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, ProductDataModel productDataModel)
        {
            try
            {
                if (productDataModel is null || id == 0 || productDataModel.CategoryId == 0 || string.IsNullOrEmpty(productDataModel.ProductName) || productDataModel.Quantity == 0 || productDataModel.Price == 0)
                {
                    return BadRequest();
                }

                var item = await _appDbContext.Products.Where(x => x.ProductId == id).FirstOrDefaultAsync();

                if (item is null) return NotFound("No data found.");

                item.CategoryId = productDataModel.CategoryId;
                item.ProductName = productDataModel.ProductName;
                item.Quantity = productDataModel.Quantity;
                item.Price = productDataModel.Price;

                int result = await _appDbContext.SaveChangesAsync();

                return result > 0 ? StatusCode(StatusCodes.Status202Accepted, "Product Updated!") : BadRequest();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Delete Product
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                if (id == 0) return BadRequest();

                var item = _appDbContext.Products.Where(x => x.ProductId == id && x.IsDeleted == false).FirstOrDefault();

                if (item is null) return NotFound("No data found.");

                item.IsDeleted = true;

                int result = await _appDbContext.SaveChangesAsync();

                return result > 0 ? StatusCode(StatusCodes.Status202Accepted, "Product Deleted!") : BadRequest();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion
    }
}
