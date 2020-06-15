using Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace WebServices.ViewModels
{

    public class QnAImageEntityViewModel
    {
        public int Id { get; set; }
    }
    public class QnAImageViewModel
    {

        public QnAImageViewModel() { }

        public QnAImageViewModel(QnAImage q)
        {
            Id = q.Id;
            QAId = q.QAId;
            Title = q.Title;
            ImageURL = q.ImageURL;
            IsActive = q.IsActive;
        }

        public int Id { get; set; }
        public int QAId { get; set; }

        public string Title { get; set; }

        public string ImageURL { get; set; }

        public bool IsActive { get; set; }

    }

     public class QnAImageCreateViewModel
    {
               
        public int QAId { get; set; }

        public string Title { get; set; }

        public string ImageURL { get; set; }
        

    }


}
