using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messager
{
	public class WeakAction<T> : WeakAction, IExecuteWithObject
	{
		private Action<T> _staticAction;

		public override string MethodName
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

		public override bool IsAlive
		{
			get
			{
				if (_staticAction == null
					&& Reference == null)
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

				return Reference.IsAlive;
			}
		}

		public WeakAction(Action<T> action, bool keepTargetAlive = false)
			: this(action == null ? null : action.Target, action, keepTargetAlive)
		{
		}

		public WeakAction(object target, Action<T> action, bool keepTargetAlive = false)
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

		public new void Execute()
		{
			Execute(default(T));
		}

		public void Execute(T parameter)
		{
			if (_staticAction != null)
			{
				_staticAction(parameter);
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
					Method.Invoke(
						actionTarget,
						new object[]
						{
							parameter
						});
				}

			}
		}

		public void ExecuteWithObject(object parameter)
		{
			var parameterCasted = (T)parameter;
			Execute(parameterCasted);
		}

		public new void MarkForDeletion()
		{
			_staticAction = null;
			base.MarkForDeletion();
		}
	}
}
