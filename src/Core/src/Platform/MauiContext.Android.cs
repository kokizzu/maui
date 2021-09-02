using System;
using Android.Content;
using Android.Views;
using AndroidX.Fragment.App;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui.Animations;

namespace Microsoft.Maui
{
	internal partial class ScopedMauiContext : MauiContext, IScopedMauiContext
	{
		IAnimationManager? _animationManager;
		readonly IMauiContext _mauiContext;
		readonly IScopedMauiContext? _scopedMauiContext;

		public ScopedMauiContext(IMauiContext mauiContext, IServiceProvider? services, Context? context, LayoutInflater? layoutInflater, FragmentManager? fragmentManager) :
			base(services ?? mauiContext.Services,
				context ?? mauiContext.Context!,
				layoutInflater,
				fragmentManager)
		{
			_mauiContext = mauiContext;
			_scopedMauiContext = _mauiContext as IScopedMauiContext;
		}

		LayoutInflater? IScopedMauiContext.LayoutInflater => base.LayoutInflater ?? _mauiContext.GetLayoutInflater();
		FragmentManager? IScopedMauiContext.FragmentManager => base.FragmentManager ?? _mauiContext.GetFragmentManager();
		IAnimationManager IScopedMauiContext.AnimationManager => _animationManager ??= 
			(_scopedMauiContext?.AnimationManager ?? Services.GetRequiredService<IAnimationManager>());
	}

	public partial class MauiContext
	{
		readonly WeakReference<Context>? _context;
		readonly WeakReference<LayoutInflater>? _layoutInflater;
		readonly WeakReference<FragmentManager>? _fragmentManager;

		public MauiContext(IServiceProvider services, Context context)
			: this(services)
		{
			_context = new WeakReference<Context>(context ?? throw new ArgumentNullException(nameof(context)));
		}

		public MauiContext(IServiceProvider services, Context context, LayoutInflater? layoutInflater, FragmentManager? fragmentManager)
			: this(services)
		{
			_context = new WeakReference<Context>(context ?? throw new ArgumentNullException(nameof(context)));

			if (layoutInflater != null)
				_layoutInflater = new WeakReference<LayoutInflater>(layoutInflater);

			if (fragmentManager != null)
				_fragmentManager = new WeakReference<FragmentManager>(fragmentManager);
		}

		internal MauiContext(Context context)
			: this()
		{
			_context = new WeakReference<Context>(context ?? throw new ArgumentNullException(nameof(context)));
		}

		public Context? Context
		{
			get
			{
				if (_context == null)
					return null;

				return _context.TryGetTarget(out Context? context) ? context : null;
			}
		}

		LayoutInflater? IScopedMauiContext.LayoutInflater => LayoutInflater;
		FragmentManager? IScopedMauiContext.FragmentManager => FragmentManager;

		private protected LayoutInflater? LayoutInflater
		{
			get
			{
				if (_layoutInflater == null)
					return null;

				return _layoutInflater.TryGetTarget(out LayoutInflater? layoutInflater) ? layoutInflater : null;
			}
		}

		private protected FragmentManager? FragmentManager
		{
			get
			{
				if (_fragmentManager == null)
					return null;

				return _fragmentManager.TryGetTarget(out FragmentManager? fragmentManager) ? fragmentManager : null;
			}
		}
	}
}
