using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GoCardless.Internals
{
    class TaskEnumerable<T, TCursor> : IEnumerable<Task<T>>
    {
        private readonly Func<TCursor, Task<Tuple<T, TCursor>>> _getPage;

        internal TaskEnumerable(Func<TCursor, Task<Tuple<T, TCursor>>> getPage)
        {
            _getPage = getPage;
        }

        public IEnumerator<Task<T>> GetEnumerator()
        {
            return GetTasks().GetEnumerator();
        }

        private IEnumerable<Task<T>> GetTasks()
        {
            var after = default(TCursor);
            do
            {
                var page = _getPage(after);

                yield return page.ContinueWith((t) =>
                {
                    after = t.Result.Item2;
                    return t.Result.Item1;
                });
            } while (after != null);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
