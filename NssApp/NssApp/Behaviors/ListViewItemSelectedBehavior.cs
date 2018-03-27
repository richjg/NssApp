using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace NssApp.Behaviors
{
    public class ListViewItemSelectedBehavior : Behavior<ListView>
    {
        public static readonly BindableProperty CommandProperty = BindableProperty.Create("Command", typeof(ICommand), typeof(ListViewItemSelectedBehavior), null);

        public ListView AssociatedObject { get; private set; }

        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        protected override void OnAttachedTo(ListView bindable)
        {
            base.OnAttachedTo(bindable);

            AssociatedObject = bindable;
            bindable.BindingContextChanged += OnBindingContextChanged;
            bindable.ItemSelected += Listview_ItemSelected;
        }

        private void Listview_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (Command == null)
            {
                return;
            }

            //object parameter = Converter.Convert(e, typeof(object), null, null);
            if (Command.CanExecute(e.SelectedItem))
            {
                Command.Execute(e.SelectedItem);
            }
        }

        protected override void OnDetachingFrom(ListView bindable)
        {
            base.OnDetachingFrom(bindable);

            bindable.BindingContextChanged -= OnBindingContextChanged;
            bindable.ItemSelected -= Listview_ItemSelected;
            AssociatedObject = null;
        }

        protected void OnBindingContextChanged(object sender, EventArgs e)
        {
            OnBindingContextChanged();
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            BindingContext = AssociatedObject.BindingContext;
        }

    }
}
