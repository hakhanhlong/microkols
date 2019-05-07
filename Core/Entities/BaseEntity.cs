using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Entities
{
    public class BaseEntity
    {
        public int Id { get; set; }
    }


    public class BaseEntityWithMeta : BaseEntityWithDate
    {

        public bool Published { get; set; }
        public bool Deleted { get; set; }
    }

    public class BaseEntityWithDate : BaseEntity
    {

     
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public string UserCreated { get; set; }
        public string UserModified { get; set; }
    }
}
