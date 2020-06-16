using Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Specifications
{
    public class QnASpecification : BaseSpecification<QnA>
    {
        public QnASpecification(QnAType? type, bool isActive) : base(m => m.IsActive == isActive && ((!type.HasValue) || m.Type == type))
        {
            AddInclude(q => q.QnAImage);
            AddInclude(q => q.QnAVideo);
        }
    }
}
