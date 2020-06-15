using Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Specifications
{
    public class QnAImageSpecification : BaseSpecification<QnAImage>
    {
        public QnAImageSpecification(int id) : base(m => m.Id == id)
        {}
    }

    public class QnAImageByQnAIDSpecification : BaseSpecification<QnAImage>
    {
        public QnAImageByQnAIDSpecification(int id) : base(m => m.QAId == id)
        { }
    }
}
