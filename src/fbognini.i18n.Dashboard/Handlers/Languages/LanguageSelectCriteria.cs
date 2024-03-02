using fbognini.Core.Domain.Query;
using fbognini.i18n.Persistence.Entities;
using System.Linq.Expressions;

namespace fbognini.i18n.Dashboard.Handlers.Languages
{
    internal class LanguageSelectCriteria : QueryableCriteria<Language>
    {

        public override List<Expression<Func<Language, bool>>> ToWhereClause()
        {
            var list = new List<Expression<Func<Language, bool>>>();

            return list;
        }
    }
}
