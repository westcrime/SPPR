using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web_153502_Tolstoi.Domain.Entities
{
    public class Cart
    {
        /// <summary>
        /// Список объектов в корзине
        /// key - идентификатор объекта
        /// </summary>
        public Dictionary<int, CartItem> CartItems { get; set; } = new();
        /// <summary>
        /// Добавить объект в корзину
        /// </summary>
        /// <param name="Game">Добавляемый объект</param>
        public virtual void AddToCart(Game Game)
        {
            if (!CartItems.ContainsKey(Game.Id))
            {
                CartItems.Add(Game.Id, new CartItem() { Item = Game, Count = 0 });
            }
            CartItems[Game.Id].Count++;


        }
        /// <summary>
        /// Удалить объект из корзины
        /// </summary>
        /// <param name="id"> id удаляемого объекта</param>
        public virtual void RemoveItems(int id)
        {
            if (CartItems.Any(item => item.Key == id))
            {
                CartItem item = CartItems.First(item => item.Key == id).Value;
                if (item.Count > 1)
                {
                    item.Count -= 1;
                }
                else
                {
                    CartItems.Remove(id);
                }
            }
        }
        /// <summary>
        /// Очистить корзину
        /// </summary>
        public virtual void ClearAll()
        {
            CartItems.Clear();
        }
        /// <summary>
        /// Количество объектов в корзине
        /// </summary>
        public int Count { get => CartItems.Sum(item => item.Value.Count); }
        /// <summary>
        /// Общая сумма
        /// </summary>
        public double TotalCost { get => CartItems.Sum(item => item.Value.Item.Price * item.Value.Count); }
    }
}
