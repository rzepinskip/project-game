using System;
using System.Collections.Concurrent;

namespace Common
{
    public class ObservableConcurrentQueue<T> : ConcurrentQueue<T>
    {
        public event EventHandler<ItemQueuedEventArgs<T>> FirstItemEnqueued;

        public new void Enqueue(T item)
        {
            base.Enqueue(item);

            if (base.Count == 1)
                OnFirstItemEnqueued(new ItemQueuedEventArgs<T>(item));
        }

        private void OnFirstItemEnqueued(ItemQueuedEventArgs<T> item)
        {
            FirstItemEnqueued?.Invoke(this, item);
        }
    }

    public class ItemQueuedEventArgs<T> : EventArgs
    {
        public T Item { get; private set; }

        public ItemQueuedEventArgs(T item)
        {
            this.Item = item;
        }
    }
}