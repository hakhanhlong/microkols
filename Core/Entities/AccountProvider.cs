﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Entities
{
    public class AccountProvider : BaseEntity
    {
        public int AccountId { get; set; }
        public Account Account { get; set; }
        public string Provider { get; set; }
        public string ProviderId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }

    }

    public enum AccountProviderNames
    {
        Facebook
    }



}
