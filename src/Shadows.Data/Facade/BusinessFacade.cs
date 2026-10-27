using System;
using System.Collections.Generic;
using NHibernate;
using NHibernate.Criterion;
using System.Reflection;
using Shadows.Data.Model;

namespace Shadows.Data.Facade
{
    public class BusinessFacade<T> where T : BaseEntity
    {
        public static int MAX_SEARCH_TEXT_LENGTH = 64;

        #region Fields

        private readonly ISession nhibernateSession;
        protected ISession NHibernateSession
        {
            get
            {
                return nhibernateSession;
            }
        }

        #endregion

        #region Constructor

        public BusinessFacade(ISession nhibernateSession)
        {
            this.nhibernateSession = nhibernateSession ?? throw new Exception("Spatna incializace fasady");
        }

        #endregion

        #region Get entity
        /// <summary>
        /// Get entity
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual T GetEntity(int id)
        {
            ISession session = NHibernateSession;
            T entity;
            try
            {
                entity = (T)session.Load(typeof(T), id);
                return entity;
            }
            catch (Exception ex)
            {
                ProcessException(ex);
                entity = Activator.CreateInstance<T>();
                entity.Id = id;
                return entity;
            }
            finally
            {
                if (session != null) NHibernateHelper.ClearSession(session);
            }
        }
        #endregion

        #region Save entity
        /// <summary>
        /// Save entity
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual void SaveEntity(T entity)
        {
            ISession session = NHibernateSession;
            ITransaction transaction = null;
            try
            {
                transaction = session.BeginTransaction();
                session.SaveOrUpdate(entity);
                transaction.Commit();
            }
            catch (Exception ex)
            {
                transaction?.Rollback();

                ProcessException(ex);
            }
            finally
            {
                if (session != null) NHibernateHelper.FlushSession(session);
            }
        }

        #endregion

        #region GetListEntities
        /// <summary>
        /// Return list of all entites
        /// </summary>
        /// <returns></returns>
        public virtual List<T> GetListEntities()
        {
            return GetListEntities(String.Empty, true, String.Empty, false);
        }
        public virtual List<T> GetListEntities(string order, bool ascending)
        {
            return GetListEntities(order, ascending, String.Empty, false);
        }
        public virtual List<T> GetListEntities(string order, bool ascending, string filter)
        {
            return GetListEntities(order, ascending, filter, false);
        }
        //public virtual List<T> GetListEntities(string order, bool ascending, string filter, int maxRows)
        //{
        //    return GetListEntities(order, ascending, filter, maxRows, false);
        //}
        public virtual List<T> GetListEntities(string order, bool ascending, string filter, bool addEmpty)
        {
            ISession session = NHibernateSession;
            try
            {
                ICriteria crit = session.CreateCriteria(typeof(T));
                if (!String.IsNullOrEmpty(filter)) crit.Add(Expression.Sql(filter));

                if (!String.IsNullOrEmpty(order))
                {
                    string[] orderSplitted = order.Split(',');
                    foreach (string s in orderSplitted)
                    {
                        if (!String.IsNullOrEmpty(s.Trim()))
                        {
                            crit.AddOrder(new Order(s.Trim(), ascending));
                        }
                    }
                }

                //if (maxRows > 0) crit.SetMaxResults(maxRows);
                List<T> list = (List<T>)crit.List<T>();

                if (addEmpty)
                {
                    T emptyItem = (T)Activator.CreateInstance(typeof(T));
                    emptyItem.Id = BaseEntity.NEW_OBJECT_ID;
                    list.Insert(0, emptyItem);
                }

                return list;

            }
            catch (Exception ex)
            {
                ProcessException(ex);
                return new List<T>();
            }
            finally
            {
                if (session != null) NHibernateHelper.ClearSession(session);
            }
        }

        #endregion

        #region Delete entity
        /// <summary>
        /// Delete entity
        /// </summary>
        /// <param name="entitz"></param>
        /// <returns></returns>
        public virtual void DeleteEntitiy(T entity)
        {
            ISession session = NHibernateSession;
            ITransaction transaction = null;
            try
            {
                transaction = session.BeginTransaction();
                session.Delete(entity);
                transaction.Commit();
            }
            catch (Exception ex)
            {
                transaction?.Rollback();
                ProcessException(ex);
            }
            finally
            {
                if (session != null) NHibernateHelper.FlushSession(session);
            }
        }
        #endregion 

        #region Find entity in list
        public virtual T FindEntityInList(int id, List<T> list)
        {
            foreach (T item in list)
            {
                if (item.Id == id) return item;
            }
            return null;
        }

        public virtual T FindEntityInList(string guid, List<T> list)
        {
            if (!typeof(T).IsSerializable)
                throw new Exception("Class must be serializabled!");

            foreach (T item in list)
            {
                if (item.Guid == guid) return item;
            }
            return null;
        }
        #endregion

        #region ProcessException
        public static void ProcessException(Exception ex)
        {
            throw new Exception("Chyba ve fasadě " + MethodBase.GetCurrentMethod().DeclaringType.ToString(), ex);
        }
        #endregion

        #region ExistByField
        public bool ExistByField(object val, string field)
        {
            ISession session = NHibernateSession;
            try
            {
                bool result = false;
                List<T> list = new List<T>();
                ICriteria crit = session.CreateCriteria(typeof(T));
                crit.Add(Expression.Eq(field, val));
                crit.Add(Expression.Eq("Valid", true));
                list = (List<T>)crit.List<T>();
                if (list != null && list.Count > 0)
                {
                    result = true;
                }
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (session != null)
                {
                    NHibernateHelper.FlushSession(session);
                }
            }
        }
        #endregion

        #region GetByField
        public T GetByField(object val, string field)
        {
            ISession session = NHibernateSession;
            try
            {
                T result = null;
                List<T> list = new List<T>();
                ICriteria crit = session.CreateCriteria(typeof(T));
                crit.Add(Expression.Eq(field, val));
                crit.Add(Expression.Eq("Valid", true));
                list = (List<T>)crit.List<T>();
                if (list != null && list.Count > 0)
                {
                    result = list[0];
                }
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (session != null)
                {
                    NHibernateHelper.FlushSession(session);
                }
            }
        }
        #endregion
    }
}
