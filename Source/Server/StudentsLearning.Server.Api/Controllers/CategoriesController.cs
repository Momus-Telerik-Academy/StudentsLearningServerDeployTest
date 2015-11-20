﻿namespace StudentsLearning.Server.Api.Controllers
{
    #region

    using System.Linq;
    using System.Web.Http;
    using System.Web.Http.Cors;

    using StudentsLearning.Data;
    using StudentsLearning.Data.Models;
    using StudentsLearning.Data.Repositories;
    using StudentsLearning.Server.Api.Infrastructure.Filters;
    using StudentsLearning.Server.Api.Models.CategoryTransferModels;
    using StudentsLearning.Services.Data;
    using StudentsLearning.Services.Data.Contracts;

    #endregion

    [RoutePrefix("api/Categories")]
    [EnableCors("*", "*", "*")]
    public class CategoriesController : ApiController
    {
        private readonly ICategoriesService categories;

        // for integration tests
        public CategoriesController()
            : this(new CategoriesService(new EfGenericRepository<Category>(new StudentsLearningDbContext())))
        {
        }

        public CategoriesController(ICategoriesService categoryService)
        {
            this.categories = categoryService;
        }

        public IHttpActionResult Get()
        {
            var result = this.categories.All().ToList();
            return this.Ok(result);
        }

        public IHttpActionResult Get(int id)
        {
            var result = this.categories.GetById(id);
            if (result == null)
            {
                return this.NotFound();
            }

            return this.Ok(result);
        }

        // TODO: [note] The update of the sections list will be done in post / delete in SectionsController through the foreign key automaticly
        [ValidateModelState]
        [CheckNull]
        public IHttpActionResult Put(int id, [FromBody] CategoryRequestModel updates)
        {
            if (!this.ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var updatedCategory = this.categories.GetById(id);

            if (this.categories.GetId(updates.Name) != 0)
            {
                return this.BadRequest();
            }

            updatedCategory.Name = updates.Name;
            this.categories.Update(updatedCategory);

            return this.Ok();
        }

        [HttpPost]
        [ValidateModelState]
        [CheckNull]
        public IHttpActionResult Post([FromBody] CategoryRequestModel categoryModel)
        {
            if (!this.ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            this.categories.Add(categoryModel.Name);
            return this.Ok(this.categories.GetId(categoryModel.Name));
        }

        [Authorize]
        [HttpDelete]
        [CheckNull]
        public IHttpActionResult Delete(int id)
        {
            var category = this.categories.GetById(id);

            if(category == null)
            {
                return this.NotFound();
            }

            this.categories.Delete(category);
            return this.Ok();
        }
    }
}