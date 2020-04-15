using Microsoft.UI.Xaml.Controls;
using System;
using System.ComponentModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;

namespace RadioButtonsTest
{
    public class RadioButtonsConverter : IValueConverter
    {
        public RadioButtonsConverter()
        {
        }

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return (int)value - 10;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return (int)value + 10;
        }
    }

    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();

            Binding binding = this._bindableValue.CreateBinding(new RadioButtonsConverter());
            this.RadioContainer.SetBinding(RadioButtons.SelectedIndexProperty, binding);
        }

        private BindableValue<int> _bindableValue = new BindableValue<int>();

        private class BindableValue<ValueT> : INotifyPropertyChanged where ValueT : new()
        {
            public BindableValue()
            {
                this.PropName = nameof(this.Value);
            }

            public string PropName { get; private set; }

            public ValueT Value
            {
                get { return this._value; }
                set
                {
                    if (!this._value.Equals(value))
                    {
                        this._value = value;
                        this.NotifyPropertyChanged(this.PropName);
                    }
                }
            }

            public event PropertyChangedEventHandler PropertyChanged;

            public Binding CreateBinding(BindingMode mode = BindingMode.TwoWay)
            {
                return this.CreateBinding(null, mode);
            }

            public Binding CreateBinding(IValueConverter converter, BindingMode mode = BindingMode.TwoWay)
            {
                return new Binding
                {
                    Path = new PropertyPath(this.PropName),
                    Source = this,
                    Mode = mode,
                    Converter = converter
                };
            }

            private void NotifyPropertyChanged(string propName)
            {
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
            }

            private ValueT _value = new ValueT();
        }
    }
}
