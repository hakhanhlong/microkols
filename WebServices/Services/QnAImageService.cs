using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WebServices.Interfaces;
using WebServices.ViewModels;
using System.Linq;

namespace WebServices.Services
{
    public class QnAImageService : IQnAImageService
    {
        private readonly IQnAImageRepository _IQnAImageRepository;
        public QnAImageService(IQnAImageRepository __IQnAImageRepository) {
            _IQnAImageRepository = __IQnAImageRepository;
        }

        public async Task<List<QnAImageViewModel>> GetByQnAId(int QnAid)
        {
            var filter = new QnAImageByQnAIDSpecification(QnAid);
            var results = await _IQnAImageRepository.ListAsync(filter);

            return results.Select(i => new QnAImageViewModel(i)).ToList();

        
        }

        public async Task<int> Create(QnAImageViewModel model)
        {
            try {                              
                QnAImage qnAImage = new QnAImage()
                {
                    ImageURL = model.ImageURL,
                    IsActive = true,
                    QAId = model.QAId,
                    Title = model.Title
                };
                return (await _IQnAImageRepository.AddAsync(qnAImage)).Id;
            }
            catch(Exception ex) {
                return 0;
            }                        
        }

        public async Task<int> Delete(int id)
        {
            var objDelete = await _IQnAImageRepository.GetByIdAsync(id);
            await _IQnAImageRepository.DeleteAsync(objDelete);
            return 1;
        }
    }
}
