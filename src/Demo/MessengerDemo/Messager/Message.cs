using System;

namespace Messager
{
	public abstract class Message
    {
        protected Message(object sender)
        {
            this.Sender = sender ?? throw new ArgumentNullException(nameof(sender));
        }

        public object Sender { get; }
    }
}
