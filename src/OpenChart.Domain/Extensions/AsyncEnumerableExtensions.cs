using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace OpenChart.Domain.Extensions
{
    public static class AsyncEnumerableExtensions
    {
        public static async ValueTask<LinkedList<T>> ToLinkedListAsync<T>(this IAsyncEnumerable<T> enumerable, CancellationToken cancellationToken)
        {
            if (enumerable == null)
                throw new InvalidOperationException();

            var linkedList = new LinkedList<T>();

            await using var enumerator = enumerable.GetAsyncEnumerator(cancellationToken);

            while (await enumerator.MoveNextAsync())
            {
                var current = enumerator.Current;
                linkedList.AddLast(current);
            }

            return linkedList;
        }

        public static IOrderedAsyncEnumerable<T> OrderBy<T>(this IAsyncEnumerable<T> enumerable) where T: IComparable<T>
        {
            return enumerable.OrderBy(x => x);
        }

        public static IOrderedAsyncEnumerable<T> OrderByDescending<T>(this IAsyncEnumerable<T> enumerable) where T: IComparable<T>
        {
            return enumerable.OrderByDescending(x => x);
        }

        public static LinkedList<T> ToLinkedList<T>(this IEnumerable<T> enumerable)
        {
            var linkedList = new LinkedList<T>(enumerable);
            return linkedList;
        }
    }
}