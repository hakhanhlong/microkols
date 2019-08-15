using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackOffice.Models
{
    public class EntityViewModel
    {
        public EntityViewModel()
        {

        }


        public EntityViewModel(City entity)
        {


            Id = entity.Id;
            Name = entity.Name;

        }

        public static List<EntityViewModel> GetList(List<City> entities)
        {
            var result = new List<EntityViewModel>();
            foreach (var entity in entities)
            {

                result.Add(new EntityViewModel(entity));
            }
            return result;

        }
        public EntityViewModel(District entity)
        {


            Id = entity.Id;
            Name = entity.Name;

        }
        public static List<EntityViewModel> GetList(List<District> entities)
        {
            var result = new List<EntityViewModel>();
            foreach (var entity in entities)
            {

                result.Add(new EntityViewModel(entity));
            }
            return result;

        }


        public EntityViewModel(Category entity)
        {


            Id = entity.Id;
            Name = entity.Name;

        }
        public static List<EntityViewModel> GetList(List<Category> entities)
        {
            var result = new List<EntityViewModel>();
            foreach (var entity in entities)
            {

                result.Add(new EntityViewModel(entity));
            }
            return result;

        }




        public int Id { get; set; }
        public string Name { get; set; }
    }
}
