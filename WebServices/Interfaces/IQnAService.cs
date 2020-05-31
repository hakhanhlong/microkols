using Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WebServices.ViewModels;

namespace WebServices.Interfaces
{
    public interface IQnAService
    {

        Task<ListQnAViewModel> GetByType(QnAType? type, bool isActive, int pageindex);

        Task<int> Create(QnAViewModel model);

        Task<int> Update(QnAViewModel model);
    }
}
