namespace RestaurantApplication.Api.Models
{
    using RestaurantApplication.Api.Common;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    /// <summary>
    /// A generic Maybe Monad implementation
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="System.Collections.Generic.IEnumerable{T}" />
    public class Option<T> : IEnumerable<T>
    {
        /// <summary>
        /// An async action delegate
        /// </summary>
        /// <param name="element">The element.</param>
        /// <returns></returns>
        public delegate Task ActionAsync(T element);

        /// <summary>
        /// The data
        /// </summary>
        private readonly T[] data;

        /// <summary>
        /// Initializes a new instance of the <see cref="Option{T}"/> class.
        /// </summary>
        /// <param name="data">The data.</param>
        private Option(T[] data)
        {
            this.data = data;
        }

        public void Apply(Action<T> action) => data?.ForEach(action);

        public async Task ApplyAsync(ActionAsync action)
        {
            if (data?.Any() ?? false)
            {
                await action?.Invoke(data[0]); 
            }
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// An enumerator that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<T> GetEnumerator() => ((IEnumerable<T>)this.data).GetEnumerator();

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="System.Collections.IEnumerator"></see> object that can be used to iterate through the collection.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        /// <summary>
        /// Gets the empty.
        /// </summary>
        /// <value>
        /// The empty.
        /// </value>
        public static Option<T> Empty => new Option<T>(new T[0]);

        /// <summary>
        /// Ofs the specified element.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <returns></returns>
        public static Option<T> Of(T element) => element == null ? Empty : new Option<T>(new[] { element });
    }
}
