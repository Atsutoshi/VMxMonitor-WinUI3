using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using VM_app_221_WAS.services;
using VMxMonitor.models;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace VMxMonitor.views.pages
{
	public sealed partial class HistoryPage : UserControl
	{
		public const int HISTORY_MONITOR = 1;
		public const int HISTORY_INQUIRY = 2;

		public event EventHandler<MonitorEventArgs> SelectMonitor;
		public event EventHandler<InquiryEventArgs> SelectInquiry;
		public event RoutedEventHandler Cancel;

		private DataModel mDataModel;
		private UserModel mUser;
		private List<MonitorModel> mMonitors;
		private List<InquiryModel> mInquiries;
		private int mMode;

		public HistoryPage( DataModel model, UserModel user, int mode )
		{
			this.InitializeComponent();
			mDataModel = model;
			mUser = user;
			mMode = mode;

			if( mMode == HISTORY_MONITOR )
			{
				mMonitors = new List<MonitorModel>();
				IQueryable<MonitorModel> records = from table in mDataModel.MonitorModel
												   where table.UserId == mUser.Id && table.Mode == "VSM"
												   orderby table._FinishedAt descending
												   select table;
				foreach( MonitorModel record in records )
				{
					mMonitors.Add( record );
				}
			}
			else if( mMode == HISTORY_INQUIRY )
			{
				mInquiries = new List<InquiryModel>();
				IQueryable<InquiryModel> records = from table in mDataModel.InquiryModel
												   where table.UserId == mUser.Id
												   orderby table._AnsweredAt descending
												   select table;
				foreach( InquiryModel record in records )
				{
					mInquiries.Add( record );
				}
			}
		}

		private void OnLoad( object sender, RoutedEventArgs e )
		{
			if( mMode == HISTORY_MONITOR )
			{
				foreach( MonitorModel monitor in mMonitors )
				{
					wList.Items.Add( monitor );
				}
			}
			else if( mMode == HISTORY_INQUIRY )
			{
				foreach( InquiryModel inquiry in mInquiries )
				{
					wList.Items.Add( inquiry );
				}
			}

			wResultButton.IsEnabled = false;
		}

		private void OnSelectItem( object sender, SelectionChangedEventArgs e )
		{
			if( mMode == HISTORY_MONITOR )
			{
				wResultButton.IsEnabled = wList.SelectedIndex >= 0 && wList.SelectedIndex < mMonitors.Count;
			}
			else if( mMode == HISTORY_INQUIRY )
			{
				wResultButton.IsEnabled = wList.SelectedIndex >= 0 && wList.SelectedIndex < mInquiries.Count;
			}
		}

		private void OnDoubleClickItem( object sender, DoubleTappedRoutedEventArgs e )
		{
			SelectItem();
		}

		private void OnResultButton( object sender, RoutedEventArgs e )
		{
			SelectItem();
		}

		private void OnCloseButton( object sender, RoutedEventArgs e )
		{
			Cancel?.Invoke( this, e );
		}

		private void SelectItem()
		{
			if( mMode == HISTORY_MONITOR )
			{
				if( wList.SelectedIndex >= 0 && wList.SelectedIndex < mMonitors.Count )
				{
					SelectMonitor?.Invoke( this, new MonitorEventArgs( mMonitors[wList.SelectedIndex] ) );
				}
			}
			else if( mMode == HISTORY_INQUIRY )
			{
				if( wList.SelectedIndex >= 0 && wList.SelectedIndex < mInquiries.Count )
				{
					SelectInquiry?.Invoke( this, new InquiryEventArgs( mInquiries[wList.SelectedIndex] ) );
				}
			}
		}
	}
}