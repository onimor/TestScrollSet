using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace TestScrollSet.Infrastructure
{
    [ContentProperty(nameof(DataTemplates))]
    public class TypeTemplateSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            DataTemplate template = GetDataTemplate(item?.GetType());

            return template == null
                ? base.SelectTemplate(item, container)
                : template;
        }

        public Dictionary<Type, DataTemplate> DataTemplates { get; set; }
            = new Dictionary<Type, DataTemplate>();

        public DataTemplate GetDataTemplate(Type type)
        {
            Type baseType = null;
            DataTemplate template = null;

            if (type != null)
            {
                foreach (var pair in DataTemplates)
                {
                    if (pair.Key.IsAssignableFrom(type))
                    {
                        if (baseType == null || baseType.IsAssignableFrom(pair.Key))
                        {
                            baseType = pair.Key;
                            template = pair.Value;
                        }
                    }
                }
            }

            return template;
        }

    }
}
