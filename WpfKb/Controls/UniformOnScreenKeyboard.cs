using System.Linq;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows.Controls;
using System.Windows;

namespace WpfKb.Controls
{
    public class UniformOnScreenKeyboard : Grid
    {
        public static readonly DependencyProperty AreAnimationsEnabledProperty = DependencyProperty.Register("AreAnimationsEnabled", typeof(bool), typeof(UniformOnScreenKeyboard), new UIPropertyMetadata(true, OnAreAnimationsEnabledPropertyChanged));

        private ObservableCollection<OnScreenKey> _keys;

        public bool AreAnimationsEnabled
        {
            get { return (bool)GetValue(AreAnimationsEnabledProperty); }
            set { SetValue(AreAnimationsEnabledProperty, value); }
        }

        public ObservableCollection<OnScreenKey> Keys
        {
            get { return _keys; }
            set
            {
                if (value == _keys) return;

                Reset();
                _keys = value;
                if (_keys != null)
                    foreach (var key in _keys)
                    {
                        Children.Add(key);
                    }
                ResizeGrid();
            }
        }

        public UniformOnScreenKeyboard()
        {
            _keys = new ObservableCollection<OnScreenKey>();
            _keys.CollectionChanged += Keys_CollectionChanged;
        }

        private static void OnAreAnimationsEnabledPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var keyboard = (UniformOnScreenKeyboard)d;
            keyboard.Keys.ToList().ForEach(x => x.AreAnimationsEnabled = (bool)e.NewValue);
        }

        private void Keys_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    // Add the keys to the Grid's Children collection and resize the grid
                    foreach (var key in e.NewItems.OfType<OnScreenKey>())
                    {
                        key.AreAnimationsEnabled = AreAnimationsEnabled;
                        Children.Add(key);
                    }
                    ResizeGrid();
                    break;
                case NotifyCollectionChangedAction.Remove:
                    // Remove the keys from the Grid's Children collection and resize the grid
                    foreach (var key in e.OldItems.OfType<OnScreenKey>())
                    {
                        Children.Remove(key);
                    }
                    ResizeGrid();
                    break;
                case NotifyCollectionChangedAction.Replace:
                    // Throw everything away and start again
                    Children.Clear();
                    foreach (var key in _keys)
                    {
                        Children.Add(key);
                    }
                    ResizeGrid();
                    break;
                case NotifyCollectionChangedAction.Reset:
                    Reset();
                    break;
                default:
                    break;
            }
        }


        private void ResizeGrid()
        {
            if (_keys == null)
            {
                Reset();
                return;
            }

            // Make sure there's the right number of rows
            var rowCount = _keys.Max(x => x.GridRow) + 1;
            for (var rowsToAdd = rowCount - RowDefinitions.Count; rowsToAdd > 0; rowsToAdd--)
            {
                // Add the extra Row
                RowDefinitions.Add(new RowDefinition());
            }
            for (var rowsToRemove = RowDefinitions.Count - rowCount; rowsToRemove > 0; rowsToRemove--)
            {
                // Remove the extra Row
                RowDefinitions.RemoveAt(0);
            }

            // Make sure there's the right number of cols
            var colCount = _keys.Max(x => x.GridColumn) + 1;
            for (var colsToAdd = colCount - ColumnDefinitions.Count; colsToAdd > 0; colsToAdd--)
            {
                // Add the extra Column
                ColumnDefinitions.Add(new ColumnDefinition());
            }
            for (var colsToRemove = ColumnDefinitions.Count - colCount; colsToRemove > 0; colsToRemove--)
            {
                // Remove the extra Column
                ColumnDefinitions.RemoveAt(0);
            }
        }

        private void Reset()
        {
            _keys = null;
            Children.Clear();
            RowDefinitions.Clear();
            ColumnDefinitions.Clear();
        }
    }
}