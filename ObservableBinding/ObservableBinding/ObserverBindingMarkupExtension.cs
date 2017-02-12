using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace ObservableBinding
{
    [MarkupExtensionReturnType(typeof(object))]
    public class ObserverBindingMarkupExtension : MarkupExtension
    {
        public IValueConverter Converter { get; set; }
        public object ConverterParamter { get; set; }
        public string ElementName { get; set; }
        public RelativeSource RelativeSource { get; set; }
        public object Source { get; set; }
        public bool ValidatesOnDataErrors { get; set; }
        public bool ValidatesOnExceptions { get; set; }
        public PropertyPath Path { get; set; }
        [TypeConverter(typeof(CultureInfoIetfLanguageTagConverter))]
        public CultureInfo ConverterCulture { get; set; }

        private ObserverBinding _observerBinding;

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            var valueProvider = serviceProvider.GetService(typeof(IProvideValueTarget)) as IProvideValueTarget;
            if (valueProvider != null)
            {
                var bindingTarget = valueProvider.TargetObject as FrameworkElement;

                var binding = new Binding();
                binding.Path = Path;
                binding.Converter = Converter;
                binding.ConverterCulture = ConverterCulture;
                binding.ConverterParameter = ConverterParamter;
                if (ElementName != null) binding.ElementName = ElementName;
                if (RelativeSource != null) binding.RelativeSource = RelativeSource;
                if (Source != null) binding.Source = Source;
                binding.ValidatesOnDataErrors = ValidatesOnDataErrors;
                binding.ValidatesOnExceptions = ValidatesOnExceptions;

                var provideValue = binding.ProvideValue(serviceProvider);
                _observerBinding = new ObserverBinding((BindingExpression)provideValue, bindingTarget);
                return provideValue;
            }
            return null;
        }
    }
}
