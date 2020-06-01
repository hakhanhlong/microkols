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
    public class QnAService: IQnAService
    {
        private readonly IQnARepository _IQnARepository;
        public QnAService(IQnARepository __IQnARepository) {
            _IQnARepository = __IQnARepository;
        }

        public async Task<ListQnAViewModel> GetByType(QnAType? type, bool isActive, int pageindex)
        {
            var filter = new QnASpecification(type, isActive);
            var list = await _IQnARepository.ListPagedAsync(filter, "", pageindex, 50);
            var total = await _IQnARepository.CountAsync(filter);

            return new ListQnAViewModel()
            {
                List_QnA = list.Select(q => new QnAViewModel(q)).OrderBy(o => o.Order).ToList(),
                Pager = new PagerViewModel()
                {
                    PageSize = 25,
                    Page = pageindex,
                    Total = total
                }
            };
        }

        public async Task<int> Create(QnAViewModel model)
        {
            try {
                var list = _IQnARepository.ListAll().Where(o => o.Type == model.Type && o.IsActive == model.IsActive).Select(q => q.Order).ToList();
                int maxOrder = 0;
                if(list.Count > 0)
                {
                    maxOrder = list.Max() + 1;                    
                }
                else
                {
                    maxOrder = 1;
                }
                
                QnA qnA = new QnA()
                {
                    Order = maxOrder,
                    IsActive = model.IsActive,
                    Question = model.Question,
                    Answer = model.Answer,
                    DateCreated = DateTime.Now,
                    DateModified = DateTime.Now,
                    Type = model.Type,
                    UserCreated = model.UserCreated,
                    UserModified = model.UserModified
                };

                return (await _IQnARepository.AddAsync(qnA)).Id;
            }
            catch(Exception ex) {
                return 0;
            }                        
        }

        public async Task<int> Update(QnAViewModel model)
        {
            try {
                var qna = await _IQnARepository.GetByIdAsync(model.Id);
                if(qna != null)
                {
                    qna.Question = model.Question;
                    qna.Answer = model.Answer;
                    qna.Type = model.Type;
                    qna.IsActive = model.IsActive;
                    qna.DateModified = DateTime.Now;
                    qna.UserModified = model.UserModified;

                    await _IQnARepository.UpdateAsync(qna);

                    return qna.Id;
                }
            }
            catch
            {
                return 0;
            }

            return 0;
        }
    }
}
