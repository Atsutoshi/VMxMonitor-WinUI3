using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace VMxMonitor.controls
{
	public sealed partial class PeakIndicator : UserControl
	{
		public PeakIndicator()
		{
			this.InitializeComponent();
		}

		public bool On
		{
			set { setState( value ); }
		}
		public string Label
		{
			set { wText.Text = value; }
		}
		private bool on;

		private void setState( bool state )
		{
			on = state;
			if( on )
			{
				wBorder.Background = new SolidColorBrush( Microsoft.UI.Colors.Green ); // Microsoft.UIのColorsを使用
			}
			else
			{
				wBorder.Background = new SolidColorBrush( Microsoft.UI.Colors.LightGray ); // Microsoft.UIのColorsを使用
			}
		}
	}
}

