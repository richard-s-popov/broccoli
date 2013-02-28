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
        /// Возвращает отзыв по id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Comment GetById(int id);

        /// <summary>
        /// Возвращает все одобренные комментарии
        /// </summary>
        /// <returns></returns>
        IEnumerable<Comment> GetAllConfirmedComments();

        /// <summary>
        /// Возвращает все новые необработанные комментарии
        /// </summary>
        /// <returns></returns>
        IEnumerable<Comment> GetAllNewComments();

        /// <summary>
        /// Добавляет в БД сущность отзыва
        /// </summary>
        /// <param name="entity"></param>
        void AddComment(Comment entity);

        /// <summary>
        /// Сохранение
        /// </summary>
        void SaveChanges();
    }
}
