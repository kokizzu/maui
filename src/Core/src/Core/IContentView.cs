using Microsoft.Maui.Graphics;

namespace Microsoft.Maui
{
	/// <summary>
	/// A View that contains another View.
	/// </summary>
	public interface IContentView : IView
	{
		/// <summary>
		/// Gets the view that contains the actual contents of this view.
		/// </summary>
		IView? Content { get; }

		IView? Root { get; }

		// TODO ezhart Document thiss
		Size CrossPlatformMeasure(double widthConstraint, double heightConstraint);
		Size CrossPlatformArrange(Rectangle bounds);
	}
}