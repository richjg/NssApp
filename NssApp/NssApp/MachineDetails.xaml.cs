﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NssApp
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MachineDetails : ContentPage
	{
		public MachineDetails(int id)
		{
			InitializeComponent();

		    MachineId.Text = id.ToString();
		}
	}
}