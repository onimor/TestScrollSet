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
                OffsertProxy proxy = (OffsertProxy)d;

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

                proxy.AssociatedObject.SetValue(offsetProperty, e.NewValue);
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
                OffsetInitAsync(this, horizOffset, vertOffset);
            }

            private static async void OffsetInitAsync(OffsertProxy proxy, double horizontal, double vertical)
            {
                HV hv = new HV();
                double horizOld;
                double vertOld;

                await proxy.Dispatcher.BeginInvoke(GetSizeAction, proxy.ScrollViewer, hv);
                do
                {
                    double horiz = horizontal;
                    if (horiz > hv.width)
                        horiz = hv.width;
                    double vert = vertical;
                    if (vert > hv.height)
                        vert = hv.height;
                    await proxy.Dispatcher.BeginInvoke(SetOffsetAction, proxy.ScrollViewer, horiz, vertical);
                    (horizOld, vertOld) = (hv.horiz, hv.vert);
                    await proxy.Dispatcher.BeginInvoke(GetOffsetAction, proxy.ScrollViewer, hv);
                } while (!(hv.IsEnd || hv.OffsetEquals(horizontal, vertical) || (horizOld, vertOld) == (hv.horiz, hv.vert)));

                await proxy.Dispatcher.BeginInvoke(InitEnd, proxy);
            }

            private static readonly Action<OffsertProxy> InitEnd = proxy =>
            {
                Binding verticalBinding = new()
                {
                    Source = proxy.ScrollViewer,
                    Path = new PropertyPath(System.Windows.Controls.ScrollViewer.VerticalOffsetProperty),
                    Mode = BindingMode.OneWay
                };
                Binding horizontalBinding = new()
                {
                    Source = proxy.ScrollViewer,
                    Path = new PropertyPath(System.Windows.Controls.ScrollViewer.HorizontalOffsetProperty),
                    Mode = BindingMode.OneWay
                };

                _ = BindingOperations.SetBinding(proxy, _VerticalOffsetProperty, verticalBinding);
                _ = BindingOperations.SetBinding(proxy, _HorizontalOffsetProperty, horizontalBinding);

            };

            private class HV
            {
                public double width;
                public double height;
                public double horiz;
                public double vert;

                public bool IsEnd => width == horiz && height == vert;
                public bool OffsetEquals(double horizontal, double vertical)
                    => horizontal == horiz && vertical == vert;
                public override string ToString() => $"({width} - {height}); ({horiz} - {vert})";
            }

            private static readonly Action<System.Windows.Controls.ScrollViewer, HV> GetSizeAction = (sv, hv) =>
            {
                hv.width = sv.ActualWidth;
                hv.height = sv.ActualHeight;
            };

            private static readonly Action<System.Windows.Controls.ScrollViewer, HV> GetOffsetAction = (sv, hv) =>
            {
                hv.horiz = sv.HorizontalOffset;
                hv.vert = sv.VerticalOffset;
                GetSizeAction(sv, hv);
            };


            private static readonly Action<System.Windows.Controls.ScrollViewer, double, double> SetOffsetAction = (sv, h, v) =>
            {
                sv.ScrollToHorizontalOffset(h);
                sv.ScrollToVerticalOffset(v);
            };
        }

    }
}