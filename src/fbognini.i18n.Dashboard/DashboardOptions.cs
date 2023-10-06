using fbognini.i18n.Dashboard.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fbognini.i18n.Dashboard
{
    public class DashboardOptions
    {
        private static readonly IDashboardAuthorizationFilter[] DefaultAuthorization =
            new[] { new LocalRequestsOnlyAuthorizationFilter() };

        private IEnumerable<IDashboardAsyncAuthorizationFilter> asyncAuthorization;

        public DashboardOptions()
        {
            Authorization = DefaultAuthorization;
            asyncAuthorization = Array.Empty<IDashboardAsyncAuthorizationFilter>();
        }

        public IEnumerable<IDashboardAuthorizationFilter> Authorization { get; set; }
        public IEnumerable<IDashboardAsyncAuthorizationFilter> AsyncAuthorization 
        { 
            get => asyncAuthorization;
            set 
            {
                asyncAuthorization = value;

                if (ReferenceEquals(Authorization, DefaultAuthorization))
                {
                    Authorization = Array.Empty<IDashboardAuthorizationFilter>();
                }
            } 
        }
    }
}
