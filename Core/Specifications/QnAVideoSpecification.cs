using Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Specifications
{
    public class QnAVideoSpecification : BaseSpecification<QnAVideo>
    {
        public QnAVideoSpecification(int id) : base(m => m.Id == id)
        {}
    }

    public class QnAVideoByQnAIDSpecification : BaseSpecification<QnAVideo>
    {
        public QnAVideoByQnAIDSpecification(int id) : base(m => m.QAId == id)
        { }
    }
}
