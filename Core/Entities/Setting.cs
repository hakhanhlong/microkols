using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Entities
{
    public class Setting : BaseEntity
    {
        public SettingName Name { get; set; }
        public string Value { get; set; }

    }
    public enum SettingName
    {
        ServiceCharge,
        ExtraOptionCharge,
        Phone,
        Address,
        
    }
}
