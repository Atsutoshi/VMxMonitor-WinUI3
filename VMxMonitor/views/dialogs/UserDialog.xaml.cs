using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using System;
using System.Linq;
using VMxMonitor.models;

namespace VMxMonitor.dialogs
{
	public sealed partial class UserDialog : ContentDialog
	{
		private DataModel mDataModel;
		private CommunityModel mCommunity;
		private UserModel mUser;
		private bool mIsIME;

		public UserDialog( DataModel model, CommunityModel community, UserModel user )
		{
			this.InitializeComponent();
			mDataModel = model;
			mCommunity = community;
			mUser = user;
			mIsIME = false;

			// �C�x���g�n���h���̐ݒ�
			this.Loaded += onLoad;
		}

		private void onLoad( object sender, RoutedEventArgs e )
		{
			// �R���{�{�b�N�X�̐ݒ�
			DateTime today = DateTime.Today;
			for( int i = 120; i > 5; --i )
			{
				wYearCombo.Items.Add( ( today.Year - i ).ToString() );
			}
			for( int i = 1; i <= 12; ++i )
			{
				wMonthCombo.Items.Add( i.ToString() );
			}

			// �E�B�W�F�b�g�̐ݒ�
			if( mUser != null )
			{
				wPromptText.Text = "���[�U�[���̕ύX";
				wNameText.Text = mUser.Name;
				wPasswordText.Password = mUser.Password;
				wYearCombo.SelectedItem = mUser.Birthday.ToLocalTime().Year.ToString();
				wMonthCombo.SelectedItem = mUser.Birthday.ToLocalTime().Month.ToString();
				setUpDayCombo();
				wDayCombo.SelectedItem = mUser.Birthday.ToLocalTime().Day.ToString();
				if( mUser.Sex == UserModel.SEX_MALE )
				{
					wSexMale.IsChecked = true;
				}
				else if( mUser.Sex == UserModel.SEX_FEMALE )
				{
					wSexFemale.IsChecked = true;
				}
			}
			else
			{
				wPromptText.Text = "���[�U�[�o�^";
				wYearCombo.SelectedIndex = 85;
				wMonthCombo.SelectedIndex = 0;
				setUpDayCombo();
				wDayCombo.SelectedIndex = 0;
			}
		}

		private void onTextBox( object sender, TextChangedEventArgs e )
		{
			if( !mIsIME )
			{
				validate();
			}
		}

		private void onSelectDate( object sender, SelectionChangedEventArgs e )
		{
			if( wYearCombo.SelectedIndex >= 0 && wMonthCombo.SelectedIndex >= 0 )
			{
				setUpDayCombo();
			}
		}

		private void setUpDayCombo()
		{
			int year = int.Parse( ( string )wYearCombo.SelectedItem );
			int month = int.Parse( ( string )wMonthCombo.SelectedItem );
			int days = DateTime.DaysInMonth( year, month );
			int selected = wDayCombo.SelectedIndex;
			wDayCombo.Items.Clear();
			for( int i = 1; i <= days; ++i )
			{
				wDayCombo.Items.Add( i.ToString() );
			}
			wDayCombo.SelectedIndex = Math.Min( selected, days - 1 );
		}

		private void validate()
		{
			// �ۑ��{�^���̏�Ԃ����Z�b�g
			if( wNameText.Text.Length > 0 )
			{
				PrimaryButtonText = "�ۑ�";
				IsPrimaryButtonEnabled = true;
			}
			else
			{
				PrimaryButtonText = "�ۑ�";
				IsPrimaryButtonEnabled = false;
			}
		}
	}
}
