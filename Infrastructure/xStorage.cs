using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace TestScrollSet.Infrastructure
{
    class xStorage
    {
        public static bool GetHasStorage(DependencyObject obj) { return (bool)obj.GetValue(HasStorageProperty); }
        public static void SetHasStorage(DependencyObject obj, bool value) { obj.SetValue(HasStorageProperty, value); }
        public static readonly DependencyProperty HasStorageProperty = DependencyProperty.RegisterAttached("HasStorage", typeof(bool), typeof(xStorage), new PropertyMetadata(false, StorageChanged));

        //-----------------Storage---------------------
        static Dictionary<string, StoreInternal> Dict = new Dictionary<string, StoreInternal>();
        static StoreInternal GetByName(string name)
        {
            if (Dict.TryGetValue(name, out StoreInternal store))
                return store;
            Dict.Add(name, new StoreInternal());
            return GetByName(name);
        }
        class StoreInternal
        {
            public double VerticalOffset { get; set; }
            //здесь можно добавлять еще свойства и оформлять для них обработчики
        }
        //--------------Update----------------------
        static void StorageChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is DataGrid dg)
                if (!dg.IsInitialized)
                    dg.Initialized += Dg_Initialized;
        }
        static void Dg_Initialized(object sender, EventArgs e)
        {
            DataGrid dg = sender as DataGrid;
            ScrollViewer viewer = null;
            StoreInternal store = null;

            bool updateStore()  //переключение Storage при изменении DataContext
            {
                try
                {
                    store = null;
                    string id = ((dynamic)dg.DataContext).UniqId;
                    store = GetByName(id);
                    return true;
                }
                catch { return false; }
            }

            dg.DataContextChanged += delegate
            {
                if (updateStore() && GetHasStorage(dg))
                    viewer?.ScrollToVerticalOffset(store.VerticalOffset);
            };

            dg.Loaded += delegate
            {
                if (viewer == null)
                {
                    updateStore();
                    viewer = dg.Template.FindName("DG_ScrollViewer", dg) as ScrollViewer;
                    if (viewer != null)
                        viewer.ScrollChanged += delegate
                        {
                            if (store != null)
                                store.VerticalOffset = viewer.VerticalOffset;
                        };
                }
            };
        }
    }
}
