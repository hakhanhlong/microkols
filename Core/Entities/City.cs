using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Entities
{
    public partial class City : BaseEntity
    {
        public string Name { get; set; }
        public int? DisplayOrder { get; set; }

        private List<District> _District = new List<District>();
        public IEnumerable<District> District => _District.AsReadOnly();
    }

    public partial class District : BaseEntity
    {
        public int? CityId { get; set; }
        public virtual City City { get; set; }
        public string Name { get; set; }
        public int? DisplayOrder { get; set; }
    }
}
