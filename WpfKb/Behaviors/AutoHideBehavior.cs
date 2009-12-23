using System;
using System.Linq;
using System.Windows.Input;
using System.Windows.Interactivity;
using System.Windows;
using System.Windows.Media.Animation;
using System.Windows.Threading;

namespace WpfKb.Behaviors
{
    public class AutoHideBehavior : Behavior<UIElement>
    {
        public enum ClickAction
        {
            None,
            Show,
            AcceleratedHide,
        }

        public static readonly DependencyProperty ActionWhenClickedProperty = DependencyProperty.Register("ActionWhenClicked", typeof(ClickAction), typeof(AutoHideBehavior), new UIPropertyMetadata(ClickAction.Show));
        public static readonly DependencyProperty AreAnimationsEnabledProperty = DependencyProperty.Register("AreAnimationsEnabled", typeof(bool), typeof(AutoHideBehavior), new UIPropertyMetadata(true));
        public static readonly DependencyProperty HideDelayProperty = DependencyProperty.Register("HideDelay", typeof(double), typeof(AutoHideBehavior), new UIPropertyMetadata(5d));
        public static readonly DependencyProperty HideDurationProperty = DependencyProperty.Register("HideDuration", typeof(double), typeof(AutoHideBehavior), new UIPropertyMetadata(0.5d));
        public static readonly DependencyProperty IsAllowedToHideProperty = DependencyProperty.Register("IsAllowedToHide", typeof(bool), typeof(AutoHideBehavior), new UIPropertyMetadata(true, OnIsAllowedToHidePropertyChanged));
        public static readonly DependencyProperty IsAllowedToShowProperty = DependencyProperty.Register("IsAllowedToShow", typeof(bool), typeof(AutoHideBehavior), new UIPropertyMetadata(true));
        public static readonly DependencyProperty IsShownProperty = DependencyProperty.Register("IsShown", typeof(bool), typeof(AutoHideBehavior), new UIPropertyMetadata(true, OnIsShownPropertyChanged));
        public static readonly DependencyProperty MaxOpacityProperty = DependencyProperty.Register("MaxOpacity", typeof(double), typeof(AutoHideBehavior), new UIPropertyMetadata(1d));
        public static readonly DependencyProperty MinOpacityProperty = DependencyProperty.Register("MinOpacity", typeof(double), typeof(AutoHideBehavior), new UIPropertyMetadata(0d));
        public static readonly DependencyProperty ShowDurationProperty = DependencyProperty.Register("ShowDuration", typeof(double), typeof(AutoHideBehavior), new UIPropertyMetadata(0d));
        public static readonly DependencyProperty TimerIntervalProperty = DependencyProperty.Register("TimerInterval", typeof(double), typeof(AutoHideBehavior), new UIPropertyMetadata(0.3d));

        private DispatcherTimer _timer;
        private DateTime _lastActivityTime;

        public ClickAction ActionWhenClicked
        {
            get { return (ClickAction)GetValue(ActionWhenClickedProperty); }
            set { SetValue(ActionWhenClickedProperty, value); }
        }

        public bool AreAnimationsEnabled
        {
            get { return (bool)GetValue(AreAnimationsEnabledProperty); }
            set { SetValue(AreAnimationsEnabledProperty, value); }
        }

        public double HideDelay
        {
            get { return (double)GetValue(HideDelayProperty); }
            set { SetValue(HideDelayProperty, value); }
        }

        public double HideDuration
        {
            get { return (double)GetValue(HideDurationProperty); }
            set { SetValue(HideDurationProperty, value); }
        }

        public bool IsAllowedToHide
        {
            get { return (bool)GetValue(IsAllowedToHideProperty); }
            set { SetValue(IsAllowedToHideProperty, value); }
        }

        public bool IsAllowedToShow
        {
            get { return (bool)GetValue(IsAllowedToShowProperty); }
            set { SetValue(IsAllowedToShowProperty, value); }
        }

        public bool IsShown
        {
            get { return (bool)GetValue(IsShownProperty); }
            set { SetValue(IsShownProperty, value); }
        }

        public double MaxOpacity
        {
            get { return (double)GetValue(MaxOpacityProperty); }
            set { SetValue(MaxOpacityProperty, value); }
        }

        public double MinOpacity
        {
            get { return (double)GetValue(MinOpacityProperty); }
            set { SetValue(MinOpacityProperty, value); }
        }
        
        public double ShowDuration
        {
            get { return (double)GetValue(ShowDurationProperty); }
            set { SetValue(ShowDurationProperty, value); }
        }

        public double TimerInterval
        {
            get { return (double)GetValue(TimerIntervalProperty); }
            set { SetValue(TimerIntervalProperty, value); }
        }

        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.PreviewMouseDown += HandlePreviewMouseDown;
            Show();
            PrepareToHide();
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.PreviewMouseDown -= HandlePreviewMouseDown;
        }

        private static void OnIsAllowedToHidePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var behavior = (AutoHideBehavior)d;
            behavior.PingActivity();
        }

        private static void OnIsShownPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var behavior = (AutoHideBehavior) d;
            if ((bool)e.NewValue) behavior.Show();
            else behavior.Hide();
        }

        private void HandlePreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            _lastActivityTime = DateTime.Now;
            switch (ActionWhenClicked)
            {
                case ClickAction.Show:
                    Show();
                    break;

                case ClickAction.AcceleratedHide:
                    HideFast();
                    break;
            }
        }

        private void PrepareToHide()
        {
            PingActivity();
            if (_timer == null)
            {
                _timer = new DispatcherTimer(TimeSpan.FromSeconds(TimerInterval), DispatcherPriority.Background,
                                             Tick, Dispatcher);
            }
        }

        private void Tick(object sender, EventArgs e)
        {
            AssociatedObject.IsHitTestVisible = AssociatedObject.Opacity > 0;
            if (DateTime.Now - _lastActivityTime > TimeSpan.FromSeconds(HideDelay))
            {
                if (AssociatedObject.Opacity >= MaxOpacity && IsAllowedToHide) Hide();
                else _lastActivityTime = DateTime.Now;
            }
        }

        public void PingActivity()
        {
            _lastActivityTime = DateTime.Now;
        }

        public void Show()
        {
            if (AssociatedObject == null) return;
            PingActivity();
            PrepareToHide();
            if (IsAllowedToShow)
            {
                var duration = AreAnimationsEnabled
                                   ? new Duration(TimeSpan.FromSeconds(ShowDuration))
                                   : new Duration(TimeSpan.Zero);

                IsShown = true;
                AssociatedObject.BeginAnimation(UIElement.OpacityProperty, new DoubleAnimation(MaxOpacity, duration));
            }
        }

        public void Hide()
        {
            if (AssociatedObject == null) return;

            var duration = AreAnimationsEnabled
                               ? new Duration(TimeSpan.FromSeconds(HideDuration))
                               : new Duration(TimeSpan.Zero);
            
            IsShown = false;
            AssociatedObject.BeginAnimation(UIElement.OpacityProperty, new DoubleAnimation(MinOpacity, duration));
        }

        public void HideFast()
        {
            if (AssociatedObject == null) return;
            IsShown = false;
            AssociatedObject.BeginAnimation(UIElement.OpacityProperty, null);
            AssociatedObject.BeginAnimation(UIElement.OpacityProperty,
                                            new DoubleAnimation(MinOpacity, new Duration(TimeSpan.Zero)));
        }
    }
}