﻿namespace StudentsLearning.Services.Data.Tests
{
    #region

    using System;
    using System.Linq;

    using NUnit.Framework;

    using StudentsLearning.Data.Models;
    using StudentsLearning.Services.Data.Contracts;
    using StudentsLearning.Services.Data.Tests.TestObjects;

    #endregion

    [TestFixture]
    public class CategoriesServiceTest
    {
        // private InMemoryRepository<User> userRepo;
        private InMemoryRepository<Category> categoriesRepo;

        private ICategoriesService categoriesService;

        [TestFixtureSetUp]
        public void Init()
        {
            // this.userRepo = TestObjectFactory.GetUsersRepository();
            this.categoriesRepo = TestObjectFactory.GetCategoriesRepository();

            this.categoriesService = new CategoriesService(this.categoriesRepo);
        }

        [Test]
        public void CategoriesServicesAddShouldInvokeSaveChanges()
        {
            this.categoriesService.Add("Test category");

            Assert.AreEqual(1, this.categoriesRepo.NumberOfSaves);
        }

        [Test]
        public void CategoriesServicesGetByIdShouldReturnCategoryIfIdExists()
        {
            var category = this.categoriesRepo.All().First();

            var result = this.categoriesService.GetById(category.Id);

            Assert.AreSame(result.GetType(), typeof(Category), "The returned object is not of type Category");
            Assert.AreNotSame(result, null, "The returned category is null");
        }

        [Test]
        public void CategoriesServicesGetIdShouldReturnIndexIfCategoryExists()
        {
            var category = this.categoriesRepo.All().First();

            var id = this.categoriesService.GetId(category.Name);

            Assert.AreNotEqual(id, null);
        }

        [Test]
        public void CategoriesServicesUpdateShouldChangeTheCategory()
        {
            var category = this.categoriesRepo.All().First();
            var name = category.Name;
            var id = category.Id;

            category.Name = name + "Changed";
            this.categoriesService.Update(category);

            // TODO: if updated nultiuple times in repo
            var updatedCategory = this.categoriesRepo.UpdatedEntities.LastOrDefault();

            Assert.AreNotEqual(updatedCategory, null);
            Assert.AreNotEqual(name, updatedCategory.Name);
        }
    }
}