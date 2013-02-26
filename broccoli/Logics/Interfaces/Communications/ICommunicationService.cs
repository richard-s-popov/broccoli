using System;
using System.Collections.Generic;
using BroccoliTrade.Domain;

namespace BroccoliTrade.Logics.Interfaces.Communications
{
    /// <summary>
    /// Сервис для связи, комментариев
    /// </summary>
    public interface ICommunicationService : IDisposable
    {
        /// <summary>
        /// Возвращает все одобренные комментарии
        /// </summary>
        /// <returns></returns>
        IEnumerable<Comment> GetAllConfirmedComments();

        /// <summary>
        /// Добавляет в БД сущность отзыва
        /// </summary>
        /// <param name="entity"></param>
        void AddComment(Comment entity);
    }
}
