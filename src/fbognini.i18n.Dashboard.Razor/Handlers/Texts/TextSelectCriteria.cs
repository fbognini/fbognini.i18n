using fbognini.Core.Data;
using fbognini.i18n.Persistence.Entities;
using System.Linq.Expressions;

namespace fbognini.i18n.Dashboard.Handlers.Texts
{
    internal class TextSelectCriteria : SelectCriteria<Text>
    {
        public override List<Expression<Func<Text, bool>>> ToWhereClause()
        {
            var list = new List<Expression<Func<Text, bool>>>();


            return list;
        }
    }
}
