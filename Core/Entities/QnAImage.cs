using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Entities
{
    public class QnAImage: BaseEntity
    {
        

        public int QAId { get; set; }

        public string Title { get; set; }

        public string ImageURL { get; set; }

        public QnA QnA { get; set; }

        public bool IsActive { get; set; }

    }
}
