using System;
using System.Collections.Concurrent;

namespace ClientsCommon
{
    public class ObservableConcurrentQueue<T> : ConcurrentQueue<T>
    {
        public event EventHandler<ItemEnqueuedEventArgs<T>> ItemEnqueued;

        public new void Enqueue(T item)
        {
            base.Enqueue(item);

            OnItemEnqueued(new ItemEnqueuedEventArgs<T>(item));
        }

        private void OnItemEnqueued(ItemEnqueuedEventArgs<T> item)
        {
            ItemEnqueued?.Invoke(this, item);
        }
    }

    public class ItemEnqueuedEventArgs<T> : EventArgs
    {
        public ItemEnqueuedEventArgs(T item)
        {
            Item = item;
        }

        public T Item { get; }
    }
}