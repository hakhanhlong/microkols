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
    public class QnAVideoService : IQnAVideoService
    {
        private readonly IQnAVideoRepository _IQnAVideoRepository;

        public QnAVideoService(IQnAVideoRepository __IQnAVideoRepository) {
            _IQnAVideoRepository = __IQnAVideoRepository;
        }

        public async Task<List<QnAVideoViewModel>> GetByQnAId(int QnAid)
        {
            var filter = new QnAVideoByQnAIDSpecification(QnAid);

            var results = await _IQnAVideoRepository.ListAsync(filter);

            return results.Select(i => new QnAVideoViewModel(i)).ToList();

        
        }

        public async Task<int> Create(QnAVideoViewModel model)
        {
            try {                              
                QnAVideo qnAVideo = new QnAVideo()
                {
                    EmbedURL = model.EmbedURL,
                    EmbedKey = model.EmbedKey,
                    IsActive = true,
                    QAId = model.QAId,
                    Title = model.Title
                };
                return (await _IQnAVideoRepository.AddAsync(qnAVideo)).Id;
            }
            catch(Exception ex) {
                return 0;
            }                        
        }

        public async Task<int> Delete(int id)
        {
            var objDelete = await _IQnAVideoRepository.GetByIdAsync(id);
            await _IQnAVideoRepository.DeleteAsync(objDelete);
            return 1;
        }
    }
}
