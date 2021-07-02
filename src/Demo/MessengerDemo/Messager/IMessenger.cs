using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messager
{
	interface IMessenger
	{
		void Subscribe<TMessage>(object recipient, Action<TMessage> action);
		void SubscribeOnMainThread<TMessage>(object recipient, Action<TMessage> action);
		void Unsubscribe<TMessage>(object recipient);
		void Unsubscribe<TMessage>(object recipient, Action<TMessage> action);
		void Publish<TMessage>(object sender, TMessage message);
		void Publish<TMessage, TTarget>(object sender, TMessage message);

	}
}
