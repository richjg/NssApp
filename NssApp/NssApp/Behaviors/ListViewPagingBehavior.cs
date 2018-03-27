using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace NssApp.Behaviors
{
    public class ListViewPagingBehavior : Behavior<ListView>
    {
        public static readonly BindableProperty CommandProperty =
            BindableProperty.Create("Command", typeof(ICommand), typeof(ListViewPagingBehavior), null);

        public static readonly BindableProperty InputConverterProperty =
            BindableProperty.Create("Converter", typeof(IValueConverter), typeof(ListViewPagingBehavior), null);

        public ListView AssociatedObject { get; private set; }

        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        public IValueConverter Converter
        {
            get { return (IValueConverter)GetValue(InputConverterProperty); }
            set { SetValue(InputConverterProperty, value); }
        }

        protected override void OnAttachedTo(ListView bindable)
        {
            base.OnAttachedTo(bindable);

            AssociatedObject = bindable;
            bindable.BindingContextChanged += OnBindingContextChanged;
            bindable.ItemAppearing += OnItemAppearing;
        }

        protected override void OnDetachingFrom(ListView bindable)
        {
            base.OnDetachingFrom(bindable);

            bindable.BindingContextChanged -= OnBindingContextChanged;
            bindable.ItemAppearing -= OnItemAppearing;
            AssociatedObject = null;
        }

        protected void OnBindingContextChanged(object sender, EventArgs e)
        {
            OnBindingContextChanged();
        }

        private void OnItemAppearing(object sender, ItemVisibilityEventArgs e)
        {
            var lv = ((ListView)sender);
            if (lv.IsRefreshing)
                return;

            if (Command == null)
            {
                return;
            }

            //object parameter = Converter.Convert(e, typeof(object), null, null);
            if (Command.CanExecute(e.Item))
            {
                Command.Execute(e.Item);
            }
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            BindingContext = AssociatedObject.BindingContext;
        }

    }
}
