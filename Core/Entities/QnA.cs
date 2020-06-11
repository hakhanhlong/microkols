using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Entities
{
    public class QnA: BaseEntityWithDate
    {
        public string Question { get; set; }
        public string Answer { get; set; }
        public bool IsActive { get; set; }
        public int Order { get; set; }

        public QnAType Type { get; set; }

        private List<QnAImage> _QnAImage = new List<QnAImage>();
        public IEnumerable<QnAImage> QnAImage => _QnAImage.AsReadOnly();

        private List<QnAVideo> _QnAVideo = new List<QnAVideo>();
        public IEnumerable<QnAVideo> QnAVideo => _QnAVideo.AsReadOnly();

    }

    public enum QnAType
    {
        General = 1,
        Merchant = 2,
        Influencer = 3
    }
}
