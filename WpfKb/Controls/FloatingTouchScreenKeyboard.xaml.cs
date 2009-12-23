using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace WpfKb.Controls
{
    public partial class FloatingTouchScreenKeyboard
    {
        public static readonly DependencyProperty AreAnimationsEnabledProperty = DependencyProperty.Register("AreAnimationsEnabled", typeof(bool), typeof(FloatingTouchScreenKeyboard), new UIPropertyMetadata(true));
        public static readonly DependencyProperty IsAllowedToFadeProperty = DependencyProperty.Register("IsAllowedToFade", typeof(bool), typeof(FloatingTouchScreenKeyboard), new UIPropertyMetadata(true));
        public static readonly DependencyProperty IsDraggingProperty = DependencyProperty.Register("IsDragging", typeof(bool), typeof(FloatingTouchScreenKeyboard), new UIPropertyMetadata(false));
        public static readonly DependencyProperty IsDragHelperAllowedToHideProperty = DependencyProperty.Register("IsDragHelperAllowedToHide", typeof(bool), typeof(FloatingTouchScreenKeyboard), new UIPropertyMetadata(false));
        public static readonly DependencyProperty IsKeyboardShownProperty = DependencyProperty.Register("IsKeyboardShown", typeof(bool), typeof(FloatingTouchScreenKeyboard), new UIPropertyMetadata(true));
        public static readonly DependencyProperty MaximumKeyboardOpacityProperty = DependencyProperty.Register("MaximumKeyboardOpacity", typeof(double), typeof(FloatingTouchScreenKeyboard), new UIPropertyMetadata(0.9d));
        public static readonly DependencyProperty MinimumKeyboardOpacityProperty = DependencyProperty.Register("MinimumKeyboardOpacity", typeof(double), typeof(FloatingTouchScreenKeyboard), new UIPropertyMetadata(0.2d));
        public static readonly DependencyProperty KeyboardHideDelayProperty = DependencyProperty.Register("KeyboardHideDelay", typeof(double), typeof(FloatingTouchScreenKeyboard), new UIPropertyMetadata(5d));
        public static readonly DependencyProperty KeyboardHideAnimationDurationProperty = DependencyProperty.Register("KeyboardHideAnimationDuration", typeof(double), typeof(FloatingTouchScreenKeyboard), new UIPropertyMetadata(0.5d));
        public static readonly DependencyProperty KeyboardShowAnimationDurationProperty = DependencyProperty.Register("KeyboardShowAnimationDuration", typeof(double), typeof(FloatingTouchScreenKeyboard), new UIPropertyMetadata(0.5d));
        public static readonly DependencyProperty DeadZoneProperty = DependencyProperty.Register("DeadZone", typeof(double), typeof(FloatingTouchScreenKeyboard), new UIPropertyMetadata(5d));

        private Point _mouseDownPosition;
        private Point _mouseDownOffset;
        private bool _isAllowedToFadeValueBeforeDrag;

        public FloatingTouchScreenKeyboard()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Gets or sets a value indicating whether animations are enabled.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if animations are enabled; otherwise, <c>false</c>.
        /// </value>
        public bool AreAnimationsEnabled
        {
            get { return (bool)GetValue(AreAnimationsEnabledProperty); }
            set { SetValue(AreAnimationsEnabledProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value that indicates whether the Keyboard is allowed to fade. This is a dependency property.
        /// </summary>
        public bool IsAllowedToFade
        {
            get { return (bool)GetValue(IsAllowedToFadeProperty); }
            set { SetValue(IsAllowedToFadeProperty, value); }
        }

        /// <summary>
        /// Gets a value that indicates if the keyboard is currently being dragged. This is a dependency property.
        /// </summary>
        public bool IsDragging
        {
            get { return (bool)GetValue(IsDraggingProperty); }
            private set { SetValue(IsDraggingProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value that indicates if the drag helper text is allowed to hide. This is a dependency property.
        /// </summary>
        public bool IsDragHelperAllowedToHide
        {
            get { return (bool)GetValue(IsDragHelperAllowedToHideProperty); }
            set { SetValue(IsDragHelperAllowedToHideProperty, value); }
        }

        /// <summary>
        /// Gets a value that indicates that the keyboard is shown (not faded). This is a dependency property.
        /// </summary>
        public bool IsKeyboardShown
        {
            get { return (bool)GetValue(IsKeyboardShownProperty); }
            private set { SetValue(IsKeyboardShownProperty, value); }
        }

        /// <summary>
        /// Gets or sets the maximum opacity for a fully displayed keyboard. This is a dependency property.
        /// </summary>
        public double MaximumKeyboardOpacity
        {
            get { return (double)GetValue(MaximumKeyboardOpacityProperty); }
            set { SetValue(MaximumKeyboardOpacityProperty, value); }
        }

        /// <summary>
        /// Gets or sets the opacity to use for a partially hidden keyboard. This is a dependency property.
        /// </summary>
        public double MinimumKeyboardOpacity
        {
            get { return (double)GetValue(MinimumKeyboardOpacityProperty); }
            set { SetValue(MinimumKeyboardOpacityProperty, value); }
        }

        /// <summary>
        /// Gets or sets the number of seconds to wait after the last keyboard activity before hiding the keyboard. This is a dependency property.
        /// </summary>
        public double KeyboardHideDelay
        {
            get { return (double)GetValue(KeyboardHideDelayProperty); }
            set { SetValue(KeyboardHideDelayProperty, value); }
        }

        /// <summary>
        /// Gets or sets the duration in seconds for the keyboard hide animation. This is a dependency property.
        /// </summary>
        public double KeyboardHideAnimationDuration
        {
            get { return (double)GetValue(KeyboardHideAnimationDurationProperty); }
            set { SetValue(KeyboardHideAnimationDurationProperty, value); }
        }

        /// <summary>
        /// Gets or sets the duration in seconds for the keyboard show animation. This is a dependency property.
        /// </summary>
        public double KeyboardShowAnimationDuration
        {
            get { return (double)GetValue(KeyboardShowAnimationDurationProperty); }
            set { SetValue(KeyboardShowAnimationDurationProperty, value); }
        }

        /// <summary>
        /// Gets or sets the maximum opacity for a fully displayed keyboard. This is a dependency property.
        /// </summary>
        public double DeadZone
        {
            get { return (double)GetValue(DeadZoneProperty); }
            set { SetValue(DeadZoneProperty, value); }
        }

        protected override void OnOpened(EventArgs e)
        {
            IsKeyboardShown = true;
            base.OnOpened(e);
        }

        protected override void OnClosed(EventArgs e)
        {
            IsKeyboardShown = false;
            base.OnClosed(e);
        }

        private void DragHandle_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            _mouseDownPosition = e.GetPosition(PlacementTarget);
            _mouseDownOffset = new Point(HorizontalOffset, VerticalOffset);
        }

        private void DragHandle_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                var delta = e.GetPosition(PlacementTarget) - _mouseDownPosition;
                if (!IsDragging && delta.Length > DeadZone)
                {
                    IsDragging = true;
                    IsDragHelperAllowedToHide = true;
                    _isAllowedToFadeValueBeforeDrag = IsAllowedToFade;
                    IsAllowedToFade = false;
                    DragHandle.CaptureMouse();
                }

                if (IsDragging)
                {
                    HorizontalOffset = _mouseDownOffset.X + delta.X;
                    VerticalOffset = _mouseDownOffset.Y + delta.Y;
                }
            }
        }

        private void DragHandle_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            DragHandle.ReleaseMouseCapture();
            IsDragging = false;
            IsAllowedToFade = _isAllowedToFadeValueBeforeDrag;
        }

    }
}