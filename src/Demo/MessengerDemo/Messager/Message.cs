using System;

namespace Messager
{
	public abstract class Message
    {
        protected Message(object sender)
        {
            if (sender == null)
            {
                throw new ArgumentNullException(nameof(sender));
            }

            this.Sender = sender;
        }

        public object Sender { get; }
    }
}
