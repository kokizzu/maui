using Microsoft.Maui.Graphics;

namespace Microsoft.Maui
{
	public static class SizeExtensions 
	{
		public static Size AddPadding(this Size size, Thickness padding) 
		{
			if (padding == Thickness.Zero)
			{
				return size;
			}

			return new Size(size.Width + padding.HorizontalThickness, size.Height + padding.VerticalThickness);
		}
	}

	public static class RectangleExtensions 
	{
		// TODO ezhart Is this being used
		public static Rectangle PadInside(this Rectangle rectangle, Thickness padding) 
		{
			if (padding == Thickness.Zero)
			{
				return rectangle;
			}

			return new Rectangle(rectangle.Left + padding.Left, rectangle.Top + padding.Top,
				rectangle.Right - padding.Right, rectangle.Bottom - padding.Bottom);
		}
	}
}
