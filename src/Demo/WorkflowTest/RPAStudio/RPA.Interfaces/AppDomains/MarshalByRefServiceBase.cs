using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting;
using System.Text;
using System.Threading.Tasks;

namespace RPA.Interfaces.AppDomains
{
    public class MarshalByRefServiceBase : MarshalByRefObject, IDisposable
    {
        protected bool _isDisposed;

        public override object InitializeLifetimeService()
        {
            return null;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this._isDisposed)
            {
                if (disposing)
                {
                    this.OnDisposing();
                }
                RemotingServices.Disconnect(this);
                this._isDisposed = true;
            }
        }


        protected virtual void OnDisposing()
        {
        }


        ~MarshalByRefServiceBase()
        {
            this.Dispose(false);
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

    }
}
