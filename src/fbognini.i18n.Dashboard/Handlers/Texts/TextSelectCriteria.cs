using fbognini.Core.Domain.Query;
using fbognini.i18n.Persistence.Entities;
using System.Linq.Expressions;

namespace fbognini.i18n.Dashboard.Handlers.Texts
{
    internal class TextSelectCriteria : QueryableCriteria<Text>
    {
        public string? TextId { get; set; }
        public string? ResourceId { get; set; }

        public override List<Expression<Func<Text, bool>>> ToWhereClause()
        {
            var list = new List<Expression<Func<Text, bool>>>();

            if (!string.IsNullOrEmpty(TextId))
            {
                list.Add(x => x.TextId == TextId);
            }

            if (!string.IsNullOrEmpty(ResourceId))
            {
                list.Add(x => x.ResourceId == ResourceId);
            }

            return list;
        }
    }
}
