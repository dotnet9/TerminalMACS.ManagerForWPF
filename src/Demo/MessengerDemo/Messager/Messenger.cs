using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Messager
{
	public class Messenger : IMessenger
	{
		private static readonly object CreationLock = new object();
		private static Messenger _defaultInstance;
		private readonly object _registerLock = new object();
		private SynchronizationContext _synchronizationContext;

		private Dictionary<Type, List<WeakActionAndToken>> _recipientsOfSubclassesAction;

		public static Messenger Default
		{
			get
			{
				if (_defaultInstance == null)
				{
					lock (CreationLock)
					{
						if (_defaultInstance == null)
						{
							_defaultInstance = new Messenger();
						}
					}
				}

				return _defaultInstance;
			}
		}
		public Messenger()
		{
			_synchronizationContext = SynchronizationContext.Current;
		}

		public void Subscribe<TMessage>(object recipient, Action<TMessage> action, ThreadOption threadOption, string tag) where TMessage : Message
		{
			lock (_registerLock)
			{
				var messageType = typeof(TMessage);

				if (_recipientsOfSubclassesAction == null)
				{
					_recipientsOfSubclassesAction = new Dictionary<Type, List<WeakActionAndToken>>();
				}

				lock (_recipientsOfSubclassesAction)
				{
					List<WeakActionAndToken> list;

					if (!_recipientsOfSubclassesAction.ContainsKey(messageType))
					{
						list = new List<WeakActionAndToken>();
						_recipientsOfSubclassesAction.Add(messageType, list);
					}
					else
					{
						list = _recipientsOfSubclassesAction[messageType];
					}


					var item = new WeakActionAndToken
					{
						Recipient = recipient,
						ThreadOption = threadOption,
						Action = action,
						Tag = tag
					};

					list.Add(item);
				}
			}
		}

		public void Unsubscribe<TMessage>(object recipient, Action<TMessage> action) where TMessage : Message
		{
			var messageType = typeof(TMessage);

			if (recipient == null
				|| _recipientsOfSubclassesAction == null
				|| _recipientsOfSubclassesAction.Count == 0
				|| !_recipientsOfSubclassesAction.ContainsKey(messageType))
			{
				return;
			}

			lock (_recipientsOfSubclassesAction)
			{
				var lstActions = _recipientsOfSubclassesAction[messageType];
				for (int i = lstActions.Count - 1; i >= 0; i--)
				{
					var item = lstActions[i];
					var weakActionCasted = item.Action;

					if (weakActionCasted != null
						&& recipient == weakActionCasted.Target
						&& (action == null
							|| action.Method.Name == weakActionCasted.Method.Name))
					{
						lstActions.Remove(item);
					}
				}
			}
		}
		public void Publish<TMessage>(object sender, TMessage message, string tag) where TMessage : Message
		{
			var messageType = typeof(TMessage);

			if (_recipientsOfSubclassesAction != null)
			{
				var listClone =
					_recipientsOfSubclassesAction.Keys.Take(_recipientsOfSubclassesAction.Count()).ToList();

				foreach (var type in listClone)
				{
					List<WeakActionAndToken> list = null;

					if (messageType == type
						|| messageType.IsSubclassOf(type)
						|| type.IsAssignableFrom(messageType))
					{
						lock (_recipientsOfSubclassesAction)
						{
							list = _recipientsOfSubclassesAction[type].Take(_recipientsOfSubclassesAction[type].Count()).Where(subscription => tag == null || subscription.Tag == tag).ToList();
						}
					}
					if (list != null && list.Count > 0)
					{
						SendToList(message, list);
					}
				}
			}
		}

		private void SendToList<TMessage>(
			TMessage message,
			IEnumerable<WeakActionAndToken> weakActionsAndTokens) where TMessage : Message
		{
			if (weakActionsAndTokens != null)
			{
				var list = weakActionsAndTokens.ToList();
				var listClone = list.Take(list.Count()).ToList();

				foreach (var item in listClone)
				{
					if (item.Action != null
						&& item.Action.Target != null)
					{
						switch (item.ThreadOption)
						{
							case ThreadOption.BackgroundThread:
								Task.Run(() =>
								{
									item.ExecuteWithObject(message);
								});
								break;
							case ThreadOption.UIThread:
								_synchronizationContext.Post((o) =>
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
	}

	class WeakActionAndToken
	{
		public object Recipient;
		public ThreadOption ThreadOption;

		public Delegate Action;

		public string Tag;
		public void ExecuteWithObject<TMessage>(TMessage message) where TMessage : Message
		{
			((Action<TMessage>)Action)(message);
		}
	}

	public enum ThreadOption
	{
		PublisherThread,
		BackgroundThread,
		UIThread
	}
}
