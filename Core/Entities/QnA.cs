using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Entities
{
    public class QnA: BaseEntityWithDate
    {

        public QnA()
        {
            QnAImage = new HashSet<QnAImage>();
            QnAVideo = new HashSet<QnAVideo>();
        }

        public string Question { get; set; }
        public string Answer { get; set; }
        public bool IsActive { get; set; }
        public int Order { get; set; }

        public QnAType Type { get; set; }


        public virtual ICollection<QnAImage> QnAImage { get; set; }

        public virtual ICollection<QnAVideo> QnAVideo { get; set; }


     

    }

    public enum QnAType
    {
        General = 1,
        Merchant = 2,
        Influencer = 3
    }
}
