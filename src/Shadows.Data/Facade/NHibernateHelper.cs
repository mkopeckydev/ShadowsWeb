using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using NHibernate.Cfg;

namespace Shadows.Data.Facade
{
    public sealed class NHibernateHelper
    {
        private NHibernateHelper()
        {
        }

        private static ISessionFactory sessionFactory;

        public static ISessionFactory SessionFactory
        {
            get
            {
                if (sessionFactory == null)
                {
                    Configuration cfg = new Configuration().Configure();
                    sessionFactory = cfg.BuildSessionFactory();
                }
                return sessionFactory;
            }
        }

        public static ISession OpenSession()
        {
            ISession _session = SessionFactory.OpenSession();
            _session.CacheMode = CacheMode.Ignore;
            return _session;
        }

        public static void FlushSession(ISession session)
        {
            if (session != null)
            {
                //vse poslat do databaze
                session.Flush();
                //vycistit vse z cache, aby pri dalsim ancteni bylo aktualni z db
                session.Clear();
            }
        }

        public static void ClearSession(ISession session)
        {
            if (session != null)
            {
                //vycistit vse z cache, aby pri dalsim ancteni bylo aktualni z db
                session.Clear();
            }
        }
    }
}
