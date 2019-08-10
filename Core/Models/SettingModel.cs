using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Models
{
    public class SettingModel
    {
        private int _serviceCharge = 0;
        private int _extraOptionCharge = 0;

        public SettingModel()
        {

        }

        public SettingModel(IEnumerable<Setting> settings)
        {
            int.TryParse(settings.FirstOrDefault(m => m.Name == SettingName.CampaignServiceChargePercent)?.Value, out _serviceCharge);
            int.TryParse(settings.FirstOrDefault(m => m.Name == SettingName.CampaignExtraOptionChargePercent)?.Value, out _extraOptionCharge);

            Phone = settings.FirstOrDefault(m => m.Name == SettingName.Phone)?.Value;

            Address = settings.FirstOrDefault(m => m.Name == SettingName.Address)?.Value;
            Email = settings.FirstOrDefault(m => m.Name == SettingName.Email)?.Value;
        }
        public int CampaignServiceChargePercent { get => _serviceCharge; set => _serviceCharge = value; }
        public int CampaignExtraOptionChargePercent { get => _extraOptionCharge; set => _extraOptionCharge = value; }

        public string Phone { get; set; }

        public string Address { get; set; }

        public string Email { get; set; }
    }
}
