using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Collections.ObjectModel;

namespace WpfKb.Controls
{
    public class OnScreenKeyboardSection : Grid
    {
        private ObservableCollection<OnScreenKey> _keys;
        private List<Grid> _buttonRows;

        public ObservableCollection<OnScreenKey> Keys
        {
            get { return _keys; }
            set
            {
                if (value == _keys) return;

                Reset();
                _keys = value;
                LayoutKeys();
            }
        }

        public OnScreenKeyboardSection()
        {
            Margin = new Thickness(5);
            _buttonRows = new List<Grid>();
            _keys = new ObservableCollection<OnScreenKey>();
            _keys.CollectionChanged += Keys_CollectionChanged;
        }

        private void Keys_CollectionChanged(object sender,
                                            System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case System.Collections.Specialized.NotifyCollectionChangedAction.Add:
                case System.Collections.Specialized.NotifyCollectionChangedAction.Remove:
                    LayoutKeys();
                    break;
                case System.Collections.Specialized.NotifyCollectionChangedAction.Replace:
                    throw new NotSupportedException("You cannot currently replace keys.");
                case System.Collections.Specialized.NotifyCollectionChangedAction.Reset:
                    Reset();
                    break;
                default:
                    break;
            }
        }

        private void LayoutKeys()
        {
            if (_keys == null || _keys.Count == 0) return;

            ResizeGrid(_keys);
            for (var buttonRowIndex = 0; buttonRowIndex < _buttonRows.Count; buttonRowIndex++)
            {
                var grid = _buttonRows[buttonRowIndex];
                grid.Children.Clear();
                foreach (var key in _keys.Where(x => x.GridRow == buttonRowIndex))
                {
                    grid.Children.Add(key);
                }
            }
        }

        private void ResizeGrid(IEnumerable<OnScreenKey> keys)
        {
            if (keys == null) throw new ArgumentNullException("keys");

            // Make sure there's enough rows
            var rowCount = keys.Max(x => x.GridRow) + 1;
            for (var extraRowIndex = RowDefinitions.Count; extraRowIndex < rowCount; extraRowIndex++)
            {
                // Button Row
                RowDefinitions.Add(new RowDefinition());

                // Add a grid for the buttons
                var g = new Grid();
                _buttonRows.Add(g);
                Children.Add(g);
                g.SetValue(RowProperty, extraRowIndex);
            }

            // Make sure each row has enough columns
            for (var buttonRowIndex = 0; buttonRowIndex < rowCount; buttonRowIndex++)
            {
                var grid = _buttonRows[buttonRowIndex];
                var colCount = keys.Where(x => x.GridRow == buttonRowIndex).Max(x => x.GridColumn) + 1;
                for (var colsToAdd = colCount - grid.ColumnDefinitions.Count; colsToAdd > 0; colsToAdd--)
                {
                    // Add the extra Column
                    grid.ColumnDefinitions.Add(new ColumnDefinition());
                }
                for (var colsToRemove = grid.ColumnDefinitions.Count - colCount; colsToRemove > 0; colsToRemove--)
                {
                    // Remove the extra Column
                    grid.ColumnDefinitions.RemoveAt(0);
                }

                // Set the width of each column according to the key's GridWidth definition
                keys.Where(x => x.GridRow == buttonRowIndex && x.GridWidth.Value != 1).ToList()
                    .ForEach(x => grid.ColumnDefinitions[x.GridColumn].Width = x.GridWidth);

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