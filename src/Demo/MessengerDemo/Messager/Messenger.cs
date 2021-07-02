using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messager
{
	public class Messenger : IMessenger
	{
		private static readonly object CreationLock = new object();
		private static Messenger _defaultInstance;
		private readonly object _registerLock = new object();

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

		public void Subscribe<TMessage>(object recipient, Action<TMessage> action)
		{
			Subscribe<TMessage>(recipient, action, false);
		}

		public void SubscribeOnMainThread<TMessage>(object recipient, Action<TMessage> action)
		{
			Subscribe<TMessage>(recipient, action, true);
		}

		private void Subscribe<TMessage>(object recipient, Action<TMessage> action, bool isSubscribeInMainThread)
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

					var weakAction = new WeakAction<TMessage>(recipient, action, true);

					var item = new WeakActionAndToken
					{
						Action = weakAction,
						Token = ""
					};

					list.Add(item);
				}
			}
		}

		public void Unsubscribe<TMessage>(object recipient)
		{
			UnsubscribeFromLists<TMessage>(recipient, null, null, _recipientsOfSubclassesAction);
		}

		public void Unsubscribe<TMessage>(object recipient, Action<TMessage> action)
		{
			UnsubscribeFromLists<TMessage>(recipient, null, action, _recipientsOfSubclassesAction);
		}
		private static void UnsubscribeFromLists<TMessage>(
			object recipient,
			object token,
			Action<TMessage> action,
			Dictionary<Type, List<WeakActionAndToken>> lists)
		{
			var messageType = typeof(TMessage);

			if (recipient == null
				|| lists == null
				|| lists.Count == 0
				|| !lists.ContainsKey(messageType))
			{
				return;
			}

			lock (lists)
			{
				foreach (var item in lists[messageType])
				{
					var weakActionCasted = item.Action as WeakAction<TMessage>;

					if (weakActionCasted != null
						&& recipient == weakActionCasted.Target
						&& (action == null
#if NETFX_CORE
                            || action.GetMethodInfo().Name == weakActionCasted.MethodName)
#else
							|| action.Method.Name == weakActionCasted.MethodName)
#endif
						&& (token == null
							|| token.Equals(item.Token)))
					{
						item.Action.MarkForDeletion();
					}
				}
			}
		}
		public void Publish<TMessage>(object sender, TMessage message)
		{
			Publish(message, typeof(TMessage), null);
		}

		public void Publish<TMessage, TTarget>(object sender, TMessage message)
		{
			Publish(message, typeof(TMessage), null);
		}
		private void Publish<TMessage>(TMessage message, Type messageTargetType, object token)
		{
			var messageType = typeof(TMessage);

			if (_recipientsOfSubclassesAction != null)
			{
				// Clone to protect from people registering in a "receive message" method
				// Correction Messaging BL0008.002
				var listClone =
					_recipientsOfSubclassesAction.Keys.Take(_recipientsOfSubclassesAction.Count()).ToList();

				foreach (var type in listClone)
				{
					List<WeakActionAndToken> list = null;

					if (messageType == type
#if NETFX_CORE
                        || messageType.GetTypeInfo().IsSubclassOf(type)
                        || type.GetTypeInfo().IsAssignableFrom(messageType.GetTypeInfo()))
#else
						|| messageType.IsSubclassOf(type)
						|| type.IsAssignableFrom(messageType))
#endif
					{
						lock (_recipientsOfSubclassesAction)
						{
							list = _recipientsOfSubclassesAction[type].Take(_recipientsOfSubclassesAction[type].Count()).ToList();
						}
					}

					SendToList(message, list, messageTargetType, token);
				}
			}

			if (_recipientsOfSubclassesAction != null)
			{
				List<WeakActionAndToken> list = null;

				lock (_recipientsOfSubclassesAction)
				{
					if (_recipientsOfSubclassesAction.ContainsKey(messageType))
					{
						list = _recipientsOfSubclassesAction[messageType]
							.Take(_recipientsOfSubclassesAction[messageType].Count())
							.ToList();
					}
				}

				if (list != null)
				{
					SendToList(message, list, messageTargetType, token);
				}
			}
		}

		private static void SendToList<TMessage>(
			TMessage message,
			IEnumerable<WeakActionAndToken> weakActionsAndTokens,
			Type messageTargetType,
			object token)
		{
			if (weakActionsAndTokens != null)
			{
				// Clone to protect from people registering in a "receive message" method
				// Correction Messaging BL0004.007
				var list = weakActionsAndTokens.ToList();
				var listClone = list.Take(list.Count()).ToList();

				foreach (var item in listClone)
				{
					var executeAction = item.Action as IExecuteWithObject;

					if (executeAction != null
						&& item.Action.IsAlive
						&& item.Action.Target != null
						&& (messageTargetType == null
							|| item.Action.Target.GetType() == messageTargetType
#if NETFX_CORE
                            || messageTargetType.GetTypeInfo().IsAssignableFrom(item.Action.Target.GetType().GetTypeInfo()))
#else
							|| messageTargetType.IsAssignableFrom(item.Action.Target.GetType()))
#endif
						&& ((item.Token == null && token == null)
							|| item.Token != null && item.Token.Equals(token)))
					{
						executeAction.ExecuteWithObject(message);
					}
				}
			}
		}
	}
}
