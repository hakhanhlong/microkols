using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackOffice.Models
{
    public class ListWalletViewModel
    {
        public List<WalletViewModel> Wallets { get; set; }
        public PagerViewModel Pager { get; set; }

        public string keyword { get; set; }
        public EntityType EntityType { get; set; }

        public AccountType AccountType { get; set; }
    }


    public class WalletViewModel
    {
        public WalletViewModel() { }

        public WalletViewModel(Wallet _wallet)
        {
            EntityType = _wallet.EntityType;
            EntityId = _wallet.EntityId;
            Balance = _wallet.Balance;
            DateCreated = _wallet.DateCreated;
            DateModified = _wallet.DateModified;
            UserCreated = _wallet.UserCreated;
            UserModified = _wallet.UserModified;
            Id = _wallet.Id;
        }

        public int Id { get; set; }

        public EntityType EntityType { get; set; }
        public int EntityId { get; set; }
        public long Balance { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public string UserCreated { get; set; }
        public string UserModified { get; set; }

        public string Name { get; set; }
        public string Type { get; set; }

    }


}
