using Microsoft.Maui.Graphics;
using Microsoft.Maui.HotReload;
using Microsoft.Maui.Layouts;

namespace Microsoft.Maui.Controls
{
	public partial class ContentPage : IContentView, HotReload.IHotReloadableView
	{
		IView IContentView.Content => Content;
		IView IContentView.Root => Content;

		protected override Size MeasureOverride(double widthConstraint, double heightConstraint)
		{
			DesiredSize = this.ComputeDesiredSize(widthConstraint, heightConstraint);
			return DesiredSize;
		}

		protected override Size ArrangeOverride(Rectangle bounds)
		{
			Frame = this.ComputeFrame(bounds);
			Handler?.NativeArrange(Frame);
			return Frame.Size;
		}

		Size IContentView.CrossPlatformMeasure(double widthConstraint, double heightConstraint)
		{
			var root = (this as IContentView).Root;
			var padding = Padding;

			if (root != null)
			{
				_ = root.Measure(widthConstraint - padding.HorizontalThickness, heightConstraint - padding.VerticalThickness);
			}

			return new Size(widthConstraint, heightConstraint);
		}

		Size IContentView.CrossPlatformArrange(Rectangle bounds)
		{
			if ((this as IContentView).Root is IView view)
			{
				var padding = Padding;

				_ = view.Arrange(new Rectangle(padding.Left, padding.Top,
					bounds.Width - padding.HorizontalThickness, bounds.Height - padding.VerticalThickness));
			}

			return bounds.Size;
		}

		protected override void InvalidateMeasureOverride()
		{
			base.InvalidateMeasureOverride();
			if (Content is IView view)
			{
				view.InvalidateMeasure();
			}
		}

		#region HotReload

		IView IReplaceableView.ReplacedView => HotReload.MauiHotReloadHelper.GetReplacedView(this) ?? this;

		HotReload.IReloadHandler HotReload.IHotReloadableView.ReloadHandler { get; set; }

		void HotReload.IHotReloadableView.TransferState(IView newView)
		{
			//TODO: Let you hot reload the the ViewModel
			//TODO: Lets do a real state transfer
			if (newView is View v)
				v.BindingContext = BindingContext;
		}

		void HotReload.IHotReloadableView.Reload()
		{
			Device.BeginInvokeOnMainThread(() =>
			{
				this.CheckHandlers();
				var reloadHandler = ((IHotReloadableView)this).ReloadHandler;
				reloadHandler?.Reload();
				//TODO: if reload handler is null, Do a manual reload?
			});
		}
		#endregion
	}
}
