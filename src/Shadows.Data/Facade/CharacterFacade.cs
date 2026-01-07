using Shadows.Data.Model;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate.Criterion;

namespace Shadows.Data.Facade
{
    public class CharacterFacade : BusinessFacade<Character>
    {
        public CharacterFacade(ISession nhibernateSession) : base(nhibernateSession) { }

        public Character Login(string userName, string password)
        {
            ISession session = NHibernateSession;
            try
            {
                ICriteria crit = session.CreateCriteria(typeof(Character));
                crit.Add(Expression.Eq("UserName", userName));
                crit.Add(Expression.Eq("Password", password));

                var list = (List<Character>)crit.List<Character>();

                if (list.Count > 0)
                {
                    Character u = list[0];
                    return u;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                ProcessException(ex);
                return null;
            }
            finally
            {
                if (session != null) NHibernateHelper.ClearSession(session);
            }
        }
    }
}
