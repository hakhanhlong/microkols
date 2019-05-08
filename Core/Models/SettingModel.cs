using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Models
{
    public class SettingModel
    {
        public SettingModel()
        {

        }

        public SettingModel(IEnumerable<Setting> settings)
        {
            ServiceCharge = int.Parse(settings.FirstOrDefault(m => m.Name == SettingName.ServiceCharge)?.Value);
            ExtraOptionCharge = int.Parse(settings.FirstOrDefault(m => m.Name == SettingName.ExtraOptionCharge)?.Value);

            Phone = settings.FirstOrDefault(m => m.Name == SettingName.ServiceCharge)?.Value
        }
        public int ServiceCharge { get; set; } = 0;
        public int ExtraOptionCharge { get; set; } = 0;
        public string Phone { get; set; }

        public string Address { get; set; }
    }
}
