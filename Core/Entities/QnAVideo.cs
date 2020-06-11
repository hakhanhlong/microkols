using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Entities
{
    public class QnAVideo: BaseEntity
    {        
        public int QAId { get; set; }

        public string EmbedKey { get; set; }

        public string EmbedURL { get; set; }

        public string Title { get; set; }

        public QnA QnA { get; set; }

        public bool IsActive { get; set; }
    }
}
