using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Maui.Handlers
{
    public partial class ContentViewHandler : ViewHandler<IContentView, ContentPanel>
    {
        public override void SetVirtualView(IContentView view)
        {
            base.SetVirtualView(view);

            _ = NativeView ?? throw new InvalidOperationException($"{nameof(NativeView)} should have been set by base class.");
            _ = VirtualView ?? throw new InvalidOperationException($"{nameof(VirtualView)} should have been set by base class.");

            NativeView.CrossPlatformMeasure = VirtualView.Measure;
            NativeView.CrossPlatformArrange = VirtualView.Arrange;
        }

        void UpdateContent()
        {
            _ = NativeView ?? throw new InvalidOperationException($"{nameof(NativeView)} should have been set by base class.");
            _ = VirtualView ?? throw new InvalidOperationException($"{nameof(VirtualView)} should have been set by base class.");
            _ = MauiContext ?? throw new InvalidOperationException($"{nameof(MauiContext)} should have been set by base class.");

            NativeView.Children.Clear();

            if (VirtualView is IContentView cv && cv.Content is IView view)
                NativeView.Children.Add(view.ToNative(MauiContext));
        }

        protected override PagePanel CreateNativeView()
        {
            if (VirtualView == null)
            {
                throw new InvalidOperationException($"{nameof(VirtualView)} must be set to create a LayoutView");
            }

            var view = new ContentPanel
			{
                CrossPlatformMeasure = VirtualView.Measure,
                CrossPlatformArrange = VirtualView.Arrange
            };

            return view;
        }

        public static void MapContent(ContentViewHandler handler, IContentView page)
        {
            handler.UpdateContent();
        }
    }
}
