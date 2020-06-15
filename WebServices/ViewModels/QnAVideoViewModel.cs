using Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace WebServices.ViewModels
{

    public class QnAVideoEntityViewModel
    {
        public int Id { get; set; }
    }
    public class QnAVideoViewModel
    {

        public QnAVideoViewModel() { }

        public QnAVideoViewModel(QnAVideo q)
        {
            Id = q.Id;
            QAId = q.QAId;
            Title = q.Title;
            EmbedKey = q.EmbedKey;
            EmbedURL = q.EmbedURL;            
            IsActive = q.IsActive;
        }

        public int Id { get; set; }
        public int QAId { get; set; }


        public string EmbedKey { get; set; }
        public string EmbedURL { get; set; }

        public string Title { get; set; }

        

        public bool IsActive { get; set; }

    }

     public class QnAVideoCreateViewModel
    {
               
        public int QAId { get; set; }

        public string Title { get; set; }

        public string EmbedKey { get; set; }
        public string EmbedURL { get; set; }


    }


}
