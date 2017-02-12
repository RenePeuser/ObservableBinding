using System.Windows.Data;

namespace ObservableBinding
{
    public static class BindingExpressionExtensions
    {
        public static T GetBoundObject<T>(this BindingExpression bindingExpression)
        {
            var result = bindingExpression.DataItem;
            if (result == null)
            {
                return default(T);
            }

            var test = bindingExpression.ResolvedSourcePropertyName;
            var notifyCollectionChanged = result.GetType().GetProperty(test).GetValue(result);
            if (notifyCollectionChanged is T)
            {
                return (T)notifyCollectionChanged;
            }
            return default(T);
        }
    }
}
