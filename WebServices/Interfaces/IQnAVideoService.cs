using Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WebServices.ViewModels;

namespace WebServices.Interfaces
{
    public interface IQnAVideoService
    {

        Task<List<QnAVideoViewModel>> GetByQnAId(int QnAid);

        Task<int> Create(QnAVideoViewModel model);

        Task<int> Delete(int id);
    }
}
