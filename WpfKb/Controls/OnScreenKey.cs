using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using WpfKb.LogicalKeys;

namespace WpfKb.Controls
{
    [TemplatePart(Name = ElementSurface, Type = typeof(Border))]
    [TemplatePart(Name = ElementMouseDownSurface, Type = typeof(Border))]
    [TemplatePart(Name = ElementKeyText, Type = typeof(TextBlock))]
    public class OnScreenKey : Control
    {
        private const string ElementSurface = "PART_Surface";
        private const string ElementMouseDownSurface = "PART_MouseDownSurface";
        private const string ElementKeyText = "PART_KeyText";
        
        public static readonly DependencyProperty KeyProperty = DependencyProperty.Register("Key", typeof(ILogicalKey), typeof(OnScreenKey), new UIPropertyMetadata(null, OnKeyChanged));

        public static readonly DependencyProperty AreAnimationsEnabledProperty = DependencyProperty.Register("AreAnimationsEnabled", typeof(bool), typeof(OnScreenKey), new UIPropertyMetadata(true));
        public static readonly DependencyProperty IsMouseOverAnimationEnabledProperty = DependencyProperty.Register("IsMouseOverAnimationEnabled", typeof(bool), typeof(OnScreenKey), new UIPropertyMetadata(false));
        public static readonly DependencyProperty IsOnScreenKeyDownProperty = DependencyProperty.Register("IsOnScreenKeyDown", typeof(bool), typeof(OnScreenKey), new UIPropertyMetadata(false));
        public static readonly DependencyProperty GridWidthProperty = DependencyProperty.Register("GridWidth", typeof(GridLength), typeof(OnScreenKey), new UIPropertyMetadata(new GridLength(1, GridUnitType.Star)));

        public static readonly DependencyProperty TextBrushProperty = DependencyProperty.Register("TextBrush", typeof(Brush), typeof(OnScreenKey), new PropertyMetadata(default(Brush)));
        public static readonly DependencyProperty OutsideBorderBrushProperty = DependencyProperty.Register("OutsideBorderBrush", typeof(Brush), typeof(OnScreenKey), new PropertyMetadata(default(Brush)));
        public static readonly DependencyProperty OutsideBorderThicknessProperty = DependencyProperty.Register("OutsideBorderThickness", typeof(Thickness), typeof(OnScreenKey), new PropertyMetadata(default(Thickness)));

        public static readonly DependencyProperty MouseOverBrushProperty = DependencyProperty.Register("MouseOverBrush", typeof(Brush), typeof(OnScreenKey), new PropertyMetadata(default(Brush)));
        public static readonly DependencyProperty MouseOverBorderBrushProperty = DependencyProperty.Register("MouseOverBorderBrush", typeof(Brush), typeof(OnScreenKey), new PropertyMetadata(default(Brush)));

        public static readonly RoutedEvent PreviewOnScreenKeyDownEvent = EventManager.RegisterRoutedEvent("PreviewOnScreenKeyDown", RoutingStrategy.Direct, typeof(OnScreenKeyEventHandler), typeof(OnScreenKey));
        public static readonly RoutedEvent PreviewOnScreenKeyUpEvent = EventManager.RegisterRoutedEvent("PreviewOnScreenKeyUp", RoutingStrategy.Direct, typeof(OnScreenKeyEventHandler), typeof(OnScreenKey));
        public static readonly RoutedEvent OnScreenKeyDownEvent = EventManager.RegisterRoutedEvent("OnScreenKeyDown", RoutingStrategy.Direct, typeof(OnScreenKeyEventHandler), typeof(OnScreenKey));
        public static readonly RoutedEvent OnScreenKeyUpEvent = EventManager.RegisterRoutedEvent("OnScreenKeyUp", RoutingStrategy.Direct, typeof(OnScreenKeyEventHandler), typeof(OnScreenKey));
        public static readonly RoutedEvent OnScreenKeyPressEvent = EventManager.RegisterRoutedEvent("OnScreenKeyPress", RoutingStrategy.Direct, typeof(OnScreenKeyEventHandler), typeof(OnScreenKey));


        private Border _keySurface;
        private Border _mouseDownSurface;
        private TextBlock _keyText;
        private Brush _keySurfaceBorderBrush;
        private Brush _keySurfaceBackground;
        private Brush _keyTextBrush;


        private readonly GradientBrush _keySurfaceMouseOverBrush = new LinearGradientBrush(

            new GradientStopCollection

                {

                    new GradientStop(Color.FromRgb(0x78, 0x78, 0x78), 0),

                    new GradientStop(Color.FromRgb(0x78, 0x78, 0x78), 0.6),

                    new GradientStop(Color.FromRgb(0x50, 0x50, 0x50), 1)

                }, 90);

        public ILogicalKey Key
        {
            get { return (ILogicalKey)GetValue(KeyProperty); }
            set { SetValue(KeyProperty, value); }
        }
        
        #region NewTemplateBindings
        public Brush TextBrush
        {
            get { return (Brush)GetValue(TextBrushProperty); }
            set { SetValue(TextBrushProperty, value); }
        }

        public Brush OutsideBorderBrush
        {
            get { return (Brush)GetValue(OutsideBorderBrushProperty); }
            set { SetValue(OutsideBorderBrushProperty, value); }
        }

        public Thickness OutsideBorderThickness
        {
            get { return (Thickness)GetValue(OutsideBorderThicknessProperty); }
            set { SetValue(OutsideBorderThicknessProperty, value); }
        }

        public Brush MouseOverBrush
        {
            get { return (Brush)GetValue(MouseOverBrushProperty); }
            set { SetValue(MouseOverBrushProperty, value); }
        }

        public Brush MouseOverBorderBrush
        {
            get { return (Brush)GetValue(MouseOverBorderBrushProperty); }
            set { SetValue(MouseOverBorderBrushProperty, value); }
        }
        #endregion



        public bool AreAnimationsEnabled
        {
            get { return (bool)GetValue(AreAnimationsEnabledProperty); }
            set { SetValue(AreAnimationsEnabledProperty, value); }
        }

        public bool IsMouseOverAnimationEnabled
        {
            get { return (bool)GetValue(IsMouseOverAnimationEnabledProperty); }
            set { SetValue(IsMouseOverAnimationEnabledProperty, value); }
        }

        public bool IsOnScreenKeyDown
        {
            get { return (bool)GetValue(IsOnScreenKeyDownProperty); }
            private set { SetValue(IsOnScreenKeyDownProperty, value); }
        }

        public int GridRow
        {
            get { return (int)GetValue(Grid.RowProperty); }
            set { SetValue(Grid.RowProperty, value); }
        }

        public int GridColumn
        {
            get { return (int)GetValue(Grid.ColumnProperty); }
            set { SetValue(Grid.ColumnProperty, value); }
        }

        public GridLength GridWidth
        {
            get { return (GridLength)GetValue(GridWidthProperty); }
            set { SetValue(GridWidthProperty, value); }
        }


        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            
            _keySurface = Template.FindName(ElementSurface, this) as Border;
            _mouseDownSurface = Template.FindName(ElementMouseDownSurface, this) as Border;
            _keyText = Template.FindName(ElementKeyText, this) as TextBlock;

            _keySurfaceBorderBrush = _keySurface?.BorderBrush;
            _keySurfaceBackground = _keySurface?.Background;
            _keyTextBrush = _keyText?.Foreground;
            _keyText?.SetBinding(TextBlock.TextProperty, new Binding("DisplayName") { Source = this.Key });
        }


        protected static void OnKeyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            ((OnScreenKey)sender).SetupControl((ILogicalKey)e.NewValue);
        }

        public event OnScreenKeyEventHandler PreviewOnScreenKeyDown
        {
            add { AddHandler(PreviewOnScreenKeyDownEvent, value); }
            remove { RemoveHandler(PreviewOnScreenKeyDownEvent, value); }
        }

        protected OnScreenKeyEventArgs RaisePreviewOnScreenKeyDownEvent()
        {
            var args = new OnScreenKeyEventArgs(PreviewOnScreenKeyDownEvent, this);
            RaiseEvent(args);
            return args;
        }

        public event OnScreenKeyEventHandler PreviewOnScreenKeyUp
        {
            add { AddHandler(PreviewOnScreenKeyUpEvent, value); }
            remove { RemoveHandler(PreviewOnScreenKeyUpEvent, value); }
        }

        protected OnScreenKeyEventArgs RaisePreviewOnScreenKeyUpEvent()
        {
            var args = new OnScreenKeyEventArgs(PreviewOnScreenKeyUpEvent, this);
            RaiseEvent(args);
            return args;
        }

        public event OnScreenKeyEventHandler OnScreenKeyDown
        {
            add { AddHandler(OnScreenKeyDownEvent, value); }
            remove { RemoveHandler(OnScreenKeyDownEvent, value); }
        }

        protected OnScreenKeyEventArgs RaiseOnScreenKeyDownEvent()
        {
            var args = new OnScreenKeyEventArgs(OnScreenKeyDownEvent, this);
            RaiseEvent(args);
            return args;
        }

        public event OnScreenKeyEventHandler OnScreenKeyUp
        {
            add { AddHandler(OnScreenKeyUpEvent, value); }
            remove { RemoveHandler(OnScreenKeyUpEvent, value); }
        }

        protected OnScreenKeyEventArgs RaiseOnScreenKeyUpEvent()
        {
            var args = new OnScreenKeyEventArgs(OnScreenKeyUpEvent, this);
            RaiseEvent(args);
            return args;
        }

        public event OnScreenKeyEventHandler OnScreenKeyPress
        {
            add { AddHandler(OnScreenKeyPressEvent, value); }
            remove { RemoveHandler(OnScreenKeyPressEvent, value); }
        }

        protected OnScreenKeyEventArgs RaiseOnScreenKeyPressEvent()
        {
            var args = new OnScreenKeyEventArgs(OnScreenKeyPressEvent, this);
            RaiseEvent(args);
            return args;
        }

        private void SetupControl(ILogicalKey key)
        {
            _keyText?.SetBinding(TextBlock.TextProperty, new Binding("DisplayName") {Source = key});
            key.PropertyChanged += Key_PropertyChanged;
            key.LogicalKeyPressed += Key_VirtualKeyPressed;
        }

        void Key_VirtualKeyPressed(object sender, LogicalKeyEventArgs e)
        {
            RaiseOnScreenKeyPressEvent();
        }

        void Key_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (Key is ModifierKeyBase && e.PropertyName == "IsInEffect")
            {
                var key = ((ModifierKeyBase)Key);
                if (key.IsInEffect)
                {
                    AnimateMouseDown();
                }
                else
                {
                    AnimateMouseUp();
                }
            }
        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            HandleKeyDown();
            base.OnMouseDown(e);
        }

        protected void HandleKeyDown()
        {
            var args = RaisePreviewOnScreenKeyDownEvent();
            if (args.Handled == false)
            {
                IsOnScreenKeyDown = true;
                AnimateMouseDown();
                Key.Press();
            }
            RaiseOnScreenKeyDownEvent();
        }

        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            HandleKeyUp();
            base.OnMouseUp(e);
        }

        private void HandleKeyUp()
        {
            var args = RaisePreviewOnScreenKeyUpEvent();
            if (args.Handled == false)
            {
                IsOnScreenKeyDown = false;
                AnimateMouseUp();
            }
            RaiseOnScreenKeyUpEvent();
        }

        private void AnimateMouseDown()
        {
            _mouseDownSurface.BeginAnimation(OpacityProperty, new DoubleAnimation(1, new Duration(TimeSpan.Zero)));
            _keyText.Foreground = OutsideBorderBrush;
        }

        private void AnimateMouseUp()
        {
            if ((Key is TogglingModifierKey || Key is InstantaneousModifierKey) && ((ModifierKeyBase)Key).IsInEffect) return;
            _keySurface.BorderBrush = _keySurfaceBorderBrush;
            _keyText.Foreground = _keyTextBrush;
            if (!AreAnimationsEnabled || Key is TogglingModifierKey || Key is InstantaneousModifierKey)
            {
                _mouseDownSurface.BeginAnimation(OpacityProperty, new DoubleAnimation(0, new Duration(TimeSpan.Zero)));
            }
            else
            {
                _mouseDownSurface.BeginAnimation(OpacityProperty, new DoubleAnimation(0, Duration.Automatic));
            }
        }

        protected override void OnMouseEnter(MouseEventArgs e)
        {
            if (IsMouseOverAnimationEnabled)
            {
                if (MouseOverBrush != null)
                    _keySurface.Background = MouseOverBrush;

                if (MouseOverBorderBrush != null)
                    _keySurface.BorderBrush = MouseOverBorderBrush;
            }
            base.OnMouseEnter(e);
        }

        protected override void OnMouseLeave(MouseEventArgs e)
        {
            if (IsMouseOverAnimationEnabled)
            {
                if (Key is TogglingModifierKey && ((ModifierKeyBase)Key).IsInEffect) return;

                _keySurface.Background = _keySurfaceBackground;
                _keySurface.BorderBrush = _keySurfaceBorderBrush;
            }
            if (IsOnScreenKeyDown)
            {
                HandleKeyUp();
            }
            base.OnMouseLeave(e);
        }
    }
}