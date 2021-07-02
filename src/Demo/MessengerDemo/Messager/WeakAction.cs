using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Messager
{
    public class WeakAction
    {
        private Action _staticAction;

        protected MethodInfo Method
        {
            get;
            set;
        }

        public virtual string MethodName
        {
            get
            {
                if (_staticAction != null)
                {
#if NETFX_CORE
                    return _staticAction.GetMethodInfo().Name;
#else
                    return _staticAction.Method.Name;
#endif
                }

                return Method.Name;
            }
        }

        protected WeakReference ActionReference
        {
            get;
            set;
        }

        protected object LiveReference
        {
            get;
            set;
        }

        protected WeakReference Reference
        {
            get;
            set;
        }

        public bool IsStatic
        {
            get
            {
                return _staticAction != null;
            }
        }

        protected WeakAction()
        {
        }

        public WeakAction(Action action, bool keepTargetAlive = false)
            : this(action == null ? null : action.Target, action, keepTargetAlive)
        {
        }

        public WeakAction(object target, Action action, bool keepTargetAlive = false)
        {
#if NETFX_CORE
            if (action.GetMethodInfo().IsStatic)
#else
            if (action.Method.IsStatic)
#endif
            {
                _staticAction = action;

                if (target != null)
                {
                    // Keep a reference to the target to control the
                    // WeakAction's lifetime.
                    Reference = new WeakReference(target);
                }

                return;
            }

#if SILVERLIGHT
            
#else
#if NETFX_CORE
            Method = action.GetMethodInfo();
#else
            Method = action.Method;
#endif
            ActionReference = new WeakReference(action.Target);
#endif

            LiveReference = keepTargetAlive ? action.Target : null;
            Reference = new WeakReference(target);

#if DEBUG
            if (ActionReference != null
                && ActionReference.Target != null
                && !keepTargetAlive)
            {
                var type = ActionReference.Target.GetType();

                if (type.Name.StartsWith("<>")
                    && type.Name.Contains("DisplayClass"))
                {
                    System.Diagnostics.Debug.WriteLine(
                        "You are attempting to register a lambda with a closure without using keepTargetAlive. Are you sure? Check http://galasoft.ch/s/mvvmweakaction for more info.");
                }
            }
#endif
        }

        public virtual bool IsAlive
        {
            get
            {
                if (_staticAction == null
                    && Reference == null
                    && LiveReference == null)
                {
                    return false;
                }

                if (_staticAction != null)
                {
                    if (Reference != null)
                    {
                        return Reference.IsAlive;
                    }

                    return true;
                }

                // Non static action

                if (LiveReference != null)
                {
                    return true;
                }

                if (Reference != null)
                {
                    return Reference.IsAlive;
                }

                return false;
            }
        }

        public object Target
        {
            get
            {
                if (Reference == null)
                {
                    return null;
                }

                return Reference.Target;
            }
        }

        protected object ActionTarget
        {
            get
            {
                if (LiveReference != null)
                {
                    return LiveReference;
                }

                if (ActionReference == null)
                {
                    return null;
                }

                return ActionReference.Target;
            }
        }

        public void Execute()
        {
            if (_staticAction != null)
            {
                _staticAction();
                return;
            }

            var actionTarget = ActionTarget;

            if (IsAlive)
            {
                if (Method != null
                    && (LiveReference != null
                        || ActionReference != null)
                    && actionTarget != null)
                {
                    Method.Invoke(actionTarget, null);

                    // ReSharper disable RedundantJumpStatement
                    return;
                    // ReSharper restore RedundantJumpStatement
                }

            }
        }

        /// <summary>
        /// Sets the reference that this instance stores to null.
        /// </summary>
        public void MarkForDeletion()
        {
            Reference = null;
            ActionReference = null;
            LiveReference = null;
            Method = null;
            _staticAction = null;
        }
    }
}
