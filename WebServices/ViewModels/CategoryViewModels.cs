using Core.Entities;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebServices.Code;

namespace WebServices.ViewModels
{

    public class CategoryViewModel
    {
        public CategoryViewModel()
        {

        }
        public CategoryViewModel(Category category)
        {
            Id = category.Id;


            Name = category.Name;
        }
        public static List<CategoryViewModel> GetList(List<Category> categorys)
        {
            var result = new List<CategoryViewModel>();
            foreach (var category in categorys)
            {

                result.Add(new CategoryViewModel(category));
            }
            return result;

        }
        public int Id { get; set; }
      
        public string Name { get; set; }
      

    }



}
