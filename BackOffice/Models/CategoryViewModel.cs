﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BackOffice.Models
{
    public class CategoryCreateEditModel
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }

        public string UserCreated { get; set; }

        public string UserModified { get; set; }

        public bool Published { get; set; }
        public bool Deleted { get; set; }

    }
}
