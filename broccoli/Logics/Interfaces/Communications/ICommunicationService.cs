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
        /// Возвращает группы
        /// </summary>
        /// <returns></returns>
        IEnumerable<UserGroups> GetAllGroups();

        /// <summary>
        /// Возвращает группу по Id
        /// </summary>
        /// <returns></returns>
        UserGroups GetGroupById(int id);

        /// <summary>
        /// Возвращает письмо по id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Mails GetMailById(int id);

        /// <summary>
        /// Возвращает номер для следующего письма в группе
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        int GetNextNumberInGroup(int groupId);

        /// <summary>
        /// Сохранить письмо
        /// </summary>
        /// <param name="entity"></param>
        void SaveMail(Mails entity);

        /// <summary>
        /// Сохранение
        /// </summary>
        void SaveChanges();
    }
}
