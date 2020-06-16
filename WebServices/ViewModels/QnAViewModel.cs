using Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebServices.ViewModels
{


    
    public class QnAViewModel
    {

        public List<QnAImageViewModel> QnAImages = new List<QnAImageViewModel>();
        public List<QnAVideoViewModel> QnAVideos = new List<QnAVideoViewModel>();


        public QnAViewModel() { }

        public QnAViewModel(QnA q) {
            Id = q.Id;
            Question = q.Question;
            Answer = q.Answer;
            Type = q.Type;
            IsActive = q.IsActive;
            Order = q.Order;
            DateCreated = q.DateCreated;
            DateModified = q.DateModified;
            UserCreated = q.UserCreated;
            UserModified = q.UserModified;
            QnAImages = q.QnAImage.Select(qi=> new QnAImageViewModel(qi)).ToList();
            QnAVideos = q.QnAVideo.Select(qi => new QnAVideoViewModel(qi)).ToList();
        }


        public int Id { get; set; }

        [Required(ErrorMessage = "Hãy nhập {0}")]
        [Display(Name = "Câu hỏi")]
        public string Question { get; set; }

        [Required(ErrorMessage = "Hãy nhập {0}")]
        [Display(Name = "Câu trả lời")]
        public string Answer { get; set; }

        [Display(Name = "Thuộc loại")]
        public QnAType Type { get; set; }

        [Display(Name = "Hoạt động/Không hoạt động")]
        public bool IsActive { get; set; }
        
        public int Order { get; set; }

        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public string UserCreated { get; set; }
        public string UserModified { get; set; }

    }

    public class ListQnAViewModel
    {
        public List<QnAViewModel> List_QnA { get; set; }
        public PagerViewModel Pager { get; set; }
    }


}
