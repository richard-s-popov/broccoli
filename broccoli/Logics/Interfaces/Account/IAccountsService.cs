using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BroccoliTrade.Domain;

namespace BroccoliTrade.Logics.Interfaces.Account
{
    /// <summary>
    /// Интерфейс для работы с сервисом счетов
    /// </summary>
    public interface IAccountsService : IDisposable
    {
        /// <summary>
        /// Возвращает счет по id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Accounts GetById(int id);

        /// <summary>
        /// Возвращает счет по его номеру
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        Accounts GetByAccountNumber(string account);

        /// <summary>
        /// Возвращает счета по userId
        /// </summary>
        /// <param name="id">Id пользователя</param>
        /// <returns>Список счетов</returns>
        IEnumerable<Accounts> GetAccountsByUserId(long id);

        /// <summary>
        /// Создание счета
        /// </summary>
        /// <param name="entity">Сущность счета</param>
        void CreateAccount(Accounts entity);

        /// <summary>
        /// Проверка, что счет активирован
        /// </summary>
        /// <param name="id">Номер счета</param>
        /// <returns></returns>
        bool AccountIsActivated(int id);

        /// <summary>
        /// Проверка, находится ли счет в обработке
        /// </summary>
        /// <param name="id">Номер счета</param>
        /// <returns></returns>
        bool AccountIsBusy(int id);

        /// <summary>
        /// Обновление сущности счет
        /// </summary>
        /// <param name="entity"></param>
        void UpdateAccount(Accounts entity);

        /// <summary>
        /// Сохранение контекста в БД
        /// </summary>
        void Save();

        /// <summary>
        /// Добавление счета в пулл заявок
        /// </summary>
        /// <param name="entity"></param>
        void AddAccountToApplicationsPool(AccountPool entity);

        /// <summary>
        /// Проверка счетов у брокера
        /// </summary>
        void CheckAccountsInPool();

        /// <summary>
        /// Удаление счета по номеру
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        void DeleteAccount(Accounts account);

        /// <summary>
        /// Возвращает количество новых уведомлений по пользователю
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        int GetNewNotoficationsCountByUserId(long userId);

        /// <summary>
        /// Обработчик на пополненные счета
        /// </summary>
        void AccountCountWorker();
    }
}
