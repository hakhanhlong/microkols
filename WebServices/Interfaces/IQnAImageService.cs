using Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WebServices.ViewModels;

namespace WebServices.Interfaces
{
    public interface IQnAImageService
    {

        Task<List<QnAImageViewModel>> GetByQnAId(int QnAid);

        Task<int> Create(QnAImageViewModel model);

        Task<int> Delete(int id);
    }
}
