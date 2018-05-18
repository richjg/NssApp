using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NssApp.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CustomNavigationPage : NavigationPage
    {
        public CustomNavigationPage() : base()
        {
            InitializeComponent();
        }

        public CustomNavigationPage(Page root) : base(root)
        {
            try
            {
                InitializeComponent();
            }
            catch (Exception e)
            {
                Exception ee = e;
            }
        }

        internal void ApplyNavigationTextColor(Page targetPage)
        {
            //var color = NavigationBarAttachedProperty.GetTextColor(targetPage);
            //BarTextColor = color == Color.Default
            //    ? Color.White
            //    : color;
        }
    }
}