using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BackOffice.Models;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BackOffice.Controllers
{
    [Authorize]
    public class CategoryController : Controller
    {

        private ICategoryRepository _ICategoryRepository;
        public CategoryController(ICategoryRepository __ICategoryRepository)
        {
            _ICategoryRepository = __ICategoryRepository;
        }

        public IActionResult Index(int pageindex = 1)
        {
            var _list = _ICategoryRepository.ListPaging("" , pageindex, 50);
            return View(_list);
        }

        public IActionResult Add()
        {
            return View("Edit", new CategoryCreateEditModel() { Id = 0});
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id = 0)
        {
            if(id != 0)
            {
                var filter = new CategorySpecification(id);
                var category = await _ICategoryRepository.GetSingleBySpecAsync(filter);
                if(category!= null)
                {
                    ViewBag.CountInfluencer = category.AccountCategory.Count();
                    ViewBag.Influencers = category.AccountCategory;



                    return View(new CategoryCreateEditModel() {
                        Id = category.Id,
                        Name = category.Name,
                        Published = category.Published,
                        Deleted = category.Deleted                        
                    });

                    

                }
                else
                {
                    TempData["MessageError"] = string.Format("{0}", "Lĩnh vực không tồn tại!");
                }

            }

            return View(new CategoryCreateEditModel() { Id = 0 });
        }

        [HttpPost]
        public async Task<IActionResult> Edit(CategoryCreateEditModel model)
        {
            if (ModelState.IsValid)
            {
                if(model.Id > 0) //edit
                {
                    var category = await _ICategoryRepository.GetByIdAsync(model.Id);
                    if (category != null)
                    {

                        category.Name = model.Name;
                        category.DateModified = DateTime.Now;
                        category.UserModified = HttpContext.User.Identity.Name;
                        category.Published = model.Published;
                        category.Deleted = model.Deleted;
                        try {
                            await _ICategoryRepository.UpdateAsync(category);
                            TempData["MessageSuccess"] = string.Format("Thay đổi lĩnh vực {0} thành công", model.Id);
                        }
                        catch(Exception ex) {
                            TempData["MessageError"] = string.Format("Lỗi: {0}", ex.Message);
                        }                       
                    }
                    else
                    {
                        TempData["MessageError"] = string.Format("{0}", "Lĩnh vực không tồn tại!");
                    }
                }
                else //insert
                {
                    Category _category = new Category();
                    _category.Name = model.Name;
                    _category.Published = model.Published;
                    _category.Deleted = model.Deleted;
                    _category.DateCreated = DateTime.Now;
                    _category.DateModified = DateTime.Now;
                    _category.UserCreated = HttpContext.User.Identity.Name;
                    _category.UserModified = HttpContext.User.Identity.Name;
                    try
                    {
                        await _ICategoryRepository.AddAsync(_category);
                        TempData["MessageSuccess"] = "Thêm mới lĩnh vực thành công!";
                    }
                    catch (Exception ex)
                    {
                        TempData["MessageError"] = string.Format("Lỗi: {0}", ex.Message);
                    }

                }
            }
            else
            {
                TempData["MessageError"] = string.Format("{0}", "ModelState Invalid");
            }
            return View(model);
        }
    }
}