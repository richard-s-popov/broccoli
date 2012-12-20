using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BroccoliTrade.Domain;

namespace BroccoliTrade.Logics.Interfaces.TradingSystem
{
    /// <summary>
    /// Интерфейс для работы с торговыми системами
    /// </summary>
    public interface ITradingSystemService : IDisposable
    {
        /// <summary>
        /// Возвращает торговую систему пользователя по id
        /// </summary>
        /// <param name="id">id торговой системы пользователя</param>
        /// <returns></returns>
        TradingSystems GetById(int id);

        /// <summary>
        /// Возвращает сущность торговой системы
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Systems GetSystemById(int id);

        /// <summary>
        /// Возвращает список торговых систем пользователя
        /// </summary>
        /// <param name="id">id пользователя</param>
        /// <returns></returns>
        IEnumerable<TradingSystems> GetTradingSystemsByUserId(long id);

        /// <summary>
        /// Добавляет новую торговую систему для пользователя
        /// </summary>
        /// <param name="entity"></param>
        void Add(TradingSystems entity);

        /// <summary>
        /// Добавляет заявку в пул (очередь обработки)
        /// </summary>
        /// <param name="entity"></param>
        void AddToPool(TradingSystemPool entity);

        /// <summary>
        /// Удаляет торговую систему
        /// </summary>
        /// <param name="entity"></param>
        void Delete(TradingSystems entity);

        /// <summary>
        /// Сохранение
        /// </summary>
        void Save();

        /// <summary>
        /// Проверка заявок на торговые системы в пуле
        /// </summary>
        void CheckTradingSystemInPool();
    }
}
