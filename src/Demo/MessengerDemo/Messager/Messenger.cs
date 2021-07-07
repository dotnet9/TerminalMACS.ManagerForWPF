using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Messager
{
    public class Messenger : IMessenger
    {
        public static readonly Messenger Default = new Messenger();
        private readonly object registerLock = new object();

        private Dictionary<Type, List<WeakActionAndToken>>? recipientsOfSubclassesAction;

        public void Subscribe<TMessage>(object recipient, Action<TMessage> action, ThreadOption threadOption, string? tag) where TMessage : Message
        {
            lock (this.registerLock)
            {
                var messageType = typeof(TMessage);

                this.recipientsOfSubclassesAction ??= new Dictionary<Type, List<WeakActionAndToken>>();

                List<WeakActionAndToken> list;

                if (!this.recipientsOfSubclassesAction.ContainsKey(messageType))
                {
                    list = new List<WeakActionAndToken>();
                    this.recipientsOfSubclassesAction.Add(messageType, list);
                }
                else
                {
                    list = this.recipientsOfSubclassesAction[messageType];
                }

                var item = new WeakActionAndToken { Recipient = recipient, ThreadOption = threadOption, Action = action, Tag = tag };

                list.Add(item);
            }
        }

        public void Unsubscribe<TMessage>(object? recipient, Action<TMessage>? action) where TMessage : Message
        {
            var messageType = typeof(TMessage);

            if (recipient == null || this.recipientsOfSubclassesAction == null || this.recipientsOfSubclassesAction.Count == 0 || !this.recipientsOfSubclassesAction.ContainsKey(messageType))
            {
                return;
            }

            var lstActions = this.recipientsOfSubclassesAction[messageType];
            for (var i = lstActions.Count - 1; i >= 0; i--)
            {
                var item = lstActions[i];
                var pastAction = item.Action;

                if (pastAction != null
                    && recipient == pastAction.Target
                    && (action == null || action.Method.Name == pastAction.Method.Name))
                {
                    lstActions.Remove(item);
                }
            }
        }

        public void Publish<TMessage>(object sender, TMessage message, string? tag) where TMessage : Message
        {
            var messageType = typeof(TMessage);

            if (this.recipientsOfSubclassesAction != null)
            {
                var listClone = this.recipientsOfSubclassesAction.Keys.Take(this.recipientsOfSubclassesAction.Count).ToList();

                foreach (var type in listClone)
                {
                    List<WeakActionAndToken>? list = null;

                    if (messageType == type || messageType.IsSubclassOf(type) || type.IsAssignableFrom(messageType))
                    {
                        list = this.recipientsOfSubclassesAction[type]
                            .Take(this.recipientsOfSubclassesAction[type].Count)
                            .Where(subscription => tag == null || subscription.Tag == tag).ToList();
                    }
                    if (list is { Count: > 0 })
                    {
                        this.SendToList(message, list);
                    }
                }
            }
        }

        private void SendToList<TMessage>(TMessage message, IEnumerable<WeakActionAndToken> weakActionsAndTokens) where TMessage : Message
        {
            var list = weakActionsAndTokens.ToList();
            var listClone = list.Take(list.Count()).ToList();

            foreach (var item in listClone)
            {
                if (item.Action is { Target: { } })
                {
                    switch (item.ThreadOption)
                    {
                        case ThreadOption.BackgroundThread:
                            Task.Run(() =>
                            {
                                item.ExecuteWithObject(message);
                            });
                            break;
                        case ThreadOption.UiThread:
                            SynchronizationContext.Current.Post(_ =>
                            {
                                item.ExecuteWithObject(message);
                            }, null);
                            break;
                        default:
                            item.ExecuteWithObject(message);
                            break;
                    }
                }
            }
        }
    }

    public class WeakActionAndToken
    {
        public object? Recipient { get; set; }

        public ThreadOption ThreadOption { get; set; }

        public Delegate? Action { get; set; }

        public string? Tag { get; set; }

        public void ExecuteWithObject<TMessage>(TMessage message) where TMessage : Message
        {
            if (this.Action is Action<TMessage> factAction)
            {
                factAction.Invoke(message);
            }
        }
    }

    public enum ThreadOption
    {
        PublisherThread,
        BackgroundThread,
        UiThread
    }
}