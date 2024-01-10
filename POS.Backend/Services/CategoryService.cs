using Dapper;
using POS.Backend.Models;
using System.Data;
using System.Data.SqlClient;

namespace POS.Backend.Services
{
    public class CategoryService
    {
        SqlConnectionStringBuilder sqlConnectionStringBuilder = new SqlConnectionStringBuilder()
        {
            DataSource = "LAPTOP-DR9SFJ1C",
            InitialCatalog = "POS",
            UserID = "sa",
            Password = "sa@123"
        };

        public List<CategoryDataModel> GetCategoriesService()
        {
            try
            {
                string query = @"SELECT [CategoryId]
      ,[CategoryName]
      , [IsDeleted]
  FROM [dbo].[Tbl_Category]";

                using IDbConnection db = new SqlConnection(sqlConnectionStringBuilder.ConnectionString);
                List<CategoryDataModel> lst = db.Query<CategoryDataModel>(query).Where(category => category.IsDeleted == false).ToList();

                return lst;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public CategoryDataModel GetCategoryService(int categoryID)
        {
            try
            {
                string query = "SELECT CategoryId,CategoryName,IsDeleted FROM Tbl_Category WHERE CategoryId = @CategoryId AND IsDeleted = @IsDeleted";
                using IDbConnection db = new SqlConnection(sqlConnectionStringBuilder.ConnectionString);

                CategoryDataModel categoryDataModel = new()
                {
                    CategoryId = categoryID,
                    IsDeleted = false,
                };

                CategoryDataModel? item = db.Query<CategoryDataModel>(query, categoryDataModel).FirstOrDefault();
                return item;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public int CreateService(CategoryDataModel categoryDataModel)
        {
            try
            {
                string query = "INSERT INTO Tbl_Category (CategoryName) VALUES (@CategoryName)";

                using IDbConnection db = new SqlConnection(sqlConnectionStringBuilder.ConnectionString);
                int result = db.Execute(query, categoryDataModel);

                return result;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public int UpdateService(CategoryDataModel categoryDataModel)
        {
            try
            {
                string query = "UPDATE Tbl_Category SET CategoryName = @CategoryName WHERE CategoryId = @CategoryId";
                using IDbConnection db = new SqlConnection(sqlConnectionStringBuilder.ConnectionString);

                int result = db.Execute(query, categoryDataModel);
                return result;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public int PatchCategoryService(int categoryID)
        {
            try
            {
                string query = "UPDATE Tbl_Category SET IsDeleted = @IsDeleted WHERE CategoryId = @CategoryId";
                using IDbConnection db = new SqlConnection(sqlConnectionStringBuilder.ConnectionString);
                CategoryDataModel categoryDataModel = new CategoryDataModel()
                {
                    CategoryId = categoryID,
                    IsDeleted = true
                };

                int reuslt = db.Execute(query, categoryDataModel);
                return reuslt;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
    }
}
