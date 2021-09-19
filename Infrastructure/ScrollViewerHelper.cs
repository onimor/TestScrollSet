using System;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace AttachedProperties
{
    /// <summary>Класс с Attached Property для <see cref="ScrollViewer"/>.</summary>
    public static class ScrollViewer
    {
        /// <summary> Приватный класс для связи <see cref="ScrollViewer"/> с <see cref="AssociatedObject"/>.</summary>
        private class OffsertProxy : DependencyObject
        {

            /// <summary>
            /// Вертикальное смещение.
            /// </summary>
            public double VerticalOffset
            {
                get => (double)GetValue(VerticalOffsetProperty);
                set => SetValue(VerticalOffsetProperty, value);
            }

            /// <summary><see cref="DependencyProperty"/> для свойства <see cref="VerticalOffset"/>.</summary>
            public static readonly DependencyProperty VerticalOffsetProperty =
                DependencyProperty.Register(nameof(VerticalOffset), typeof(double), typeof(OffsertProxy), new PropertyMetadata(double.NaN, OffsetChanged));

            private static void OffsetChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
            {
                DependencyProperty offsetProperty;
                if (e.Property == VerticalOffsetProperty)
                {
                    offsetProperty = AttachedProperties.ScrollViewer.VerticalOffsetProperty;
                }
                else if (e.Property == HorizontalOffsetProperty)
                {
                    offsetProperty = AttachedProperties.ScrollViewer.HorizontalOffsetProperty;
                }
                else
                {
                    throw new Exception("Чёрт знает, что произошло!");
                }
                ((OffsertProxy)d).AssociatedObject.SetValue(offsetProperty, e.NewValue);
            }


            /// <summary>
            /// Горизонтальное смещение.
            /// </summary>
            public double HorizontalOffset
            {
                get => (double)GetValue(HorizontalOffsetProperty);
                set => SetValue(HorizontalOffsetProperty, value);
            }

            /// <summary><see cref="DependencyProperty"/> для свойства <see cref="HorizontalOffset"/>.</summary>
            public static readonly DependencyProperty HorizontalOffsetProperty =
                DependencyProperty.Register(nameof(HorizontalOffset), typeof(double), typeof(OffsertProxy), new PropertyMetadata(double.NaN, OffsetChanged));

            public DependencyObject AssociatedObject { get; }
            public System.Windows.Controls.ScrollViewer ScrollViewer { get; }

            public OffsertProxy(DependencyObject associatedObject, System.Windows.Controls.ScrollViewer scrollViewer)
            {
                AssociatedObject = associatedObject;
                ScrollViewer = scrollViewer;

                Binding verticalBinding = new Binding()
                {
                    Source = scrollViewer,
                    Path = new PropertyPath(System.Windows.Controls.ScrollViewer.VerticalOffsetProperty),
                    Mode = BindingMode.OneWay
                };
                Binding horizontalBinding = new Binding()
                {
                    Source = scrollViewer,
                    Path = new PropertyPath(System.Windows.Controls.ScrollViewer.HorizontalOffsetProperty),
                    Mode = BindingMode.OneWay
                };

                _ = BindingOperations.SetBinding(this, VerticalOffsetProperty, verticalBinding);
                _ = BindingOperations.SetBinding(this, HorizontalOffsetProperty, horizontalBinding);
            }
        }



        /// <summary>Возвращает значение присоединённого свойства ScrollViewer.VerticalOffset для <paramref name="obj"/>.</summary>
        /// <param name="obj"><see cref="DependencyObject"/> значение свойства которого будет возвращено.</param>
        /// <returns><see cref="double"/> значение свойства.</returns>
        public static double GetVerticalOffset(DependencyObject obj)
        {
            return (double)obj.GetValue(VerticalOffsetProperty);
        }

        /// <summary>Задаёт значение присоединённого свойства ScrollViewer.VerticalOffset для <paramref name="obj"/>.</summary>
        /// <param name="obj"><see cref="DependencyObject"/> значение свойства которого будет возвращено.</param>
        /// <param name="value"><see cref="double"/> значение для свойства.</param>
        public static void SetVerticalOffset(DependencyObject obj, double value)
        {
            obj.SetValue(VerticalOffsetProperty, value);
        }

        /// <summary><see cref="DependencyProperty"/> для методов <see cref="GetVerticalOffset(DependencyObject)"/>
        /// и <see cref="SetVerticalOffset(DependencyObject, double)"/>.</summary>
        public static readonly DependencyProperty VerticalOffsetProperty =
            DependencyProperty.RegisterAttached(
                nameof(GetVerticalOffset).Substring(3),
                typeof(double),
                typeof(ScrollViewer),
                new FrameworkPropertyMetadata(double.NaN, OffsetChanged) { BindsTwoWayByDefault = true });




        /// <summary>Возвращает значение присоединённого свойства ScrollViewer.HorizontalOffset для <paramref name="obj"/>.</summary>
        /// <param name="obj"><see cref="DependencyObject"/> значение свойства которого будет возвращено.</param>
        /// <returns><see cref="double"/> значение свойства.</returns>
        public static double GetHorizontalOffset(DependencyObject obj)
        {
            return (double)obj.GetValue(HorizontalOffsetProperty);
        }

        /// <summary>Задаёт значение присоединённого свойства ScrollViewer.HorizontalOffset для <paramref name="obj"/>.</summary>
        /// <param name="obj"><see cref="DependencyObject"/> значение свойства которого будет возвращено.</param>
        /// <param name="value"><see cref="double"/> значение для свойства.</param>
        public static void SetHorizontalOffset(DependencyObject obj, double value)
        {
            obj.SetValue(HorizontalOffsetProperty, value);
        }

        /// <summary><see cref="DependencyProperty"/> для методов <see cref="GetHorizontalOffset(DependencyObject)"/>
        /// и <see cref="SetHorizontalOffset(DependencyObject, double)"/>.</summary>
        public static readonly DependencyProperty HorizontalOffsetProperty =
            DependencyProperty.RegisterAttached(
                nameof(GetHorizontalOffset).Substring(3),
                typeof(double),
                typeof(ScrollViewer),
                new FrameworkPropertyMetadata(double.NaN, OffsetChanged) { BindsTwoWayByDefault = true });

        private static void OffsetChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            OffsertProxy proxy = GetProxy(d);
            if (proxy == null)
            {
                if (d is FrameworkElement element && !element.IsLoaded)
                {
                    element.Loaded += OnLoaded;
                }
                else
                {
                    OnLoaded(d, null);
                }
            }
            else
            {
                if (e.Property == VerticalOffsetProperty)
                {
                    proxy.ScrollViewer.ScrollToVerticalOffset((double)e.NewValue);
                }
                else if (e.Property == HorizontalOffsetProperty)
                {
                    proxy.ScrollViewer.ScrollToHorizontalOffset((double)e.NewValue);
                }
                else
                {
                    throw new Exception("Чёрт знает, что произошло!");
                }
            }
        }

        private static void OnLoaded(object sender, RoutedEventArgs e)
        {
            DependencyObject dObj = (DependencyObject)sender;
            OffsertProxy proxy = GetProxy(dObj);
            if (proxy == null)
            {
                System.Windows.Controls.ScrollViewer scrollViewer = null;
                if (dObj is System.Windows.Controls.DataGrid dataGrid)
                {
                    scrollViewer = dataGrid.Template.FindName("DG_ScrollViewer", dataGrid) as System.Windows.Controls.ScrollViewer;
                }
                if (scrollViewer == null)
                {
                    scrollViewer = GetVisualChild<System.Windows.Controls.ScrollViewer>(dObj);
                }

                if (scrollViewer != null)
                {
                    SetProxy(dObj, proxy = new OffsertProxy(dObj, scrollViewer));

                    object vert = dObj.ReadLocalValue(VerticalOffsetProperty);
                    if (vert != DependencyProperty.UnsetValue)
                    {
                        proxy.ScrollViewer.ScrollToVerticalOffset((double)dObj.GetValue(VerticalOffsetProperty));
                    }

                    object horiz = dObj.ReadLocalValue(HorizontalOffsetProperty);
                    if (vert != DependencyProperty.UnsetValue)
                    {
                        proxy.ScrollViewer.ScrollToVerticalOffset((double)dObj.GetValue(HorizontalOffsetProperty));
                    }
                }
            }
        }

        private static T GetVisualChild<T>(DependencyObject parent) where T : Visual
        {
            T child = null;
            int count = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < count; i++)
            {
                Visual v = (Visual)VisualTreeHelper.GetChild(parent, i);
                child = v as T;
                if (child == null)
                {
                    child = GetVisualChild<T>(v);
                }

                if (child != null)
                {
                    break;
                }
            }
            return child;
        }

        /// <summary>Возвращает значение присоединённого свойства Proxy для <paramref name="obj"/>.</summary>
        /// <param name="obj"><see cref="DependencyObject"/ значение свойства которого будет возвращено.</param>
        /// <returns><see cref="OffsertProxy"/> значение свойства.</returns>
        private static OffsertProxy GetProxy(DependencyObject obj)
        {
            return (OffsertProxy)obj.GetValue(ProxyPropertyKey.DependencyProperty);
        }

        /// <summary>Задаёт значение присоединённого свойства Proxy для <paramref name="obj"/>.</summary>
        /// <param name="obj"><see cref="DependencyObject"/> значение свойства которого будет возвращено.</param>
        /// <param name="value"><see cref="OffsertProxy"/> значение для свойства.</param>
        private static void SetProxy(DependencyObject obj, OffsertProxy value)
        {
            obj.SetValue(ProxyPropertyKey, value);
        }

        /// <summary><see cref="DependencyPropertyKey"/> для метода <see cref="SetProxy(DependencyObject, OffsertProxy)"/>.</summary>
        private static readonly DependencyPropertyKey ProxyPropertyKey =
            DependencyProperty.RegisterAttachedReadOnly(nameof(GetProxy).Substring(3), typeof(OffsertProxy), typeof(ScrollViewer), new PropertyMetadata(null));

    }
}
