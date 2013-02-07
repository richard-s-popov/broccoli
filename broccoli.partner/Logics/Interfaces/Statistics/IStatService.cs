using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BroccoliTrade.Domain;

namespace BroccoliTrade.Logics.Interfaces.Statistics
{
    public interface IStatService : IDisposable
    {
        /// <summary>
        /// Возвращает список реферальных хостов
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        IEnumerable<Referrer> GetReferrersByUserId(long userId);

        /// <summary>
        /// Возвращает список реферальных хостов за период
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        IEnumerable<Referrer> GetReferrersByUserIdAndPeriod(long userId, DateTime? start, DateTime? end);
            
        /// <summary>
        /// Возвращает реферальный хост по пользователю и имени хоста
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="host"></param>
        /// <returns></returns>
        Referrer GetReferrerByUserAndHost(long userId, string host);

        /// <summary>
        /// Возвращает реферальный хост по идентификатору
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Referrer GetById(int id);

        /// <summary>
        /// Добавляет новую сущность в контекст
        /// </summary>
        /// <param name="entity"></param>
        void AddReferrer(Referrer entity);

        /// <summary>
        /// Сохраняе контекст в БД
        /// </summary>
        void Save();
    }
}
