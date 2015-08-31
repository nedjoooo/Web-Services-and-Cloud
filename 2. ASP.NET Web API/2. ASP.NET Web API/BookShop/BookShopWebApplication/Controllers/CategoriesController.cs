using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using BookShopSystem.Data;
using BookShopSystem.Models;
using BookShopWebApplication.Models.ViewModels;
using BookShopWebApplication.Models.BindingModels;

namespace BookShopWebApplication.Controllers
{
    public class CategoriesController : ApiController
    {
        private BookShopContext db = new BookShopContext();

        // GET: api/Categories
        public IEnumerable<CategoryViewModel> GetCategories()
        {
            var dbCategories = db.Categories.ToList();
            List<CategoryViewModel> categories = new List<CategoryViewModel>();

            foreach (var category in dbCategories)
            {
                categories.Add(new CategoryViewModel(category));
            }

            return categories;
        }

        // GET: api/Categories/5
        [HttpGet]
        public IHttpActionResult GetCategory(int id)
        {
            Category category = db.Categories.Find(id);
            if (category == null)
            {
                return NotFound();
            }

            var viewModelCategory = new CategoryViewModel(category);

            return Ok(viewModelCategory);
        }

        // PUT: api/Categories/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutCategory(int id, CategoryBindingModel chandegCategory)
        {
            var dbCategory = db.Categories.FirstOrDefault(c => c.Id == id);
            if(dbCategory == null)
            {
                return this.NotFound();
            }

            if(chandegCategory.Name == String.Empty)
            {
                return this.BadRequest("Category name can not be empty");
            }

            var checkDuplicateCategory = db.Categories.FirstOrDefault(c => c.Name == chandegCategory.Name);
            if(checkDuplicateCategory != null)
            {
                return this.BadRequest("Category exists");
            }

            dbCategory.Name = chandegCategory.Name;
            db.SaveChanges();

            return this.Ok("Category changed succesfully");
        }

        // POST: api/Categories
        public IHttpActionResult PostCategory(CategoryBindingModel categoryModel)
        {
            if (categoryModel == null)
            {
                this.ModelState.AddModelError("categoryModel", "Category model can not be empty!");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var checkExistCategory = db.Categories.FirstOrDefault(c => c.Name == categoryModel.Name);
            if(checkExistCategory != null)
            {
                return this.BadRequest("Category exists");
            }

            var category = new Category()
            {
                Name = categoryModel.Name
            };

            db.Categories.Add(category);
            db.SaveChanges();

            return this.Ok("Category successfully added");
        }

        // DELETE: api/Categories/5
        [ResponseType(typeof(Category))]
        public IHttpActionResult DeleteCategory(int id)
        {
            Category category = db.Categories.Find(id);
            if (category == null)
            {
                return NotFound();
            }

            db.Categories.Remove(category);
            db.SaveChanges();

            return Ok(category);
        }

        

        

        

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CategoryExists(int id)
        {
            return db.Categories.Count(e => e.Id == id) > 0;
        }
    }
}