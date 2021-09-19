using System;
using System.Windows;
using System.Windows.Data;

namespace AttachedProperties
{
    public static partial class ScrollViewer
    {
        /// <summary> Приватный класс для связи <see cref="ScrollViewer"/> с <see cref="AssociatedObject"/>.</summary>
        private class OffsertProxy : DependencyObject
        {

            /// <summary>Вертикальное смещение.</summary>
            public double _VerticalOffset
            {
                get => (double)GetValue(_VerticalOffsetProperty);
                set => SetValue(_VerticalOffsetProperty, value);
            }

            /// <summary><see cref="DependencyProperty"/> для свойства <see cref="_VerticalOffset"/>.</summary>
            public static readonly DependencyProperty _VerticalOffsetProperty =
                DependencyProperty.Register(nameof(_VerticalOffset), typeof(double), typeof(OffsertProxy), new PropertyMetadata(double.NaN, OffsetChanged));

            private static void OffsetChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
            {
                DependencyProperty offsetProperty;
                if (e.Property == _VerticalOffsetProperty)
                {
                    offsetProperty = VerticalOffsetProperty;
                }
                else if (e.Property == _HorizontalOffsetProperty)
                {
                    offsetProperty = HorizontalOffsetProperty;
                }
                else
                {
                    throw new Exception("Чёрт знает, что произошло!");
                }
                ((OffsertProxy)d).AssociatedObject.SetValue(offsetProperty, e.NewValue);
            }


            /// <summary>Горизонтальное смещение.</summary>
            public double _HorizontalOffset
            {
                get => (double)GetValue(_HorizontalOffsetProperty);
                set => SetValue(_HorizontalOffsetProperty, value);
            }

            /// <summary><see cref="DependencyProperty"/> для свойства <see cref="_HorizontalOffset"/>.</summary>
            public static readonly DependencyProperty _HorizontalOffsetProperty =
                DependencyProperty.Register(nameof(_HorizontalOffset), typeof(double), typeof(OffsertProxy), new PropertyMetadata(double.NaN, OffsetChanged));

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

                object vert = associatedObject.ReadLocalValue(VerticalOffsetProperty);
                double vertOffset = scrollViewer.VerticalOffset;
                if (vert != DependencyProperty.UnsetValue)
                {
                    vertOffset = (double)associatedObject.GetValue(VerticalOffsetProperty);
                }

                object horiz = associatedObject.ReadLocalValue(HorizontalOffsetProperty);
                double horizOffset = scrollViewer.HorizontalOffset;
                if (horiz != DependencyProperty.UnsetValue)
                {
                    horizOffset = (double)associatedObject.GetValue(HorizontalOffsetProperty);
                }

                _ = BindingOperations.SetBinding(this, _VerticalOffsetProperty, verticalBinding);
                _ = BindingOperations.SetBinding(this, _HorizontalOffsetProperty, horizontalBinding);


                scrollViewer.ScrollToBottom();
                scrollViewer.ScrollToRightEnd();

                scrollViewer.Dispatcher.BeginInvoke(SetOffsetAction, scrollViewer, vertOffset, horizOffset);
            }
        }

        private static readonly Action<System.Windows.Controls.ScrollViewer, double, double> SetOffsetAction = (sv, v, h) =>
        {
            sv.ScrollToVerticalOffset(v);
            sv.ScrollToHorizontalOffset(h);
        };

    }
}