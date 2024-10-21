using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace VMxMonitor.ViewModels
{
	public class CommunityDialogViewModel : INotifyPropertyChanged
	{
		private string _titleCommunity;
		private string _promptCommunityRegistration;
		private string _promptCommunityModification;
		private string _buttonUpdate;
		private string _buttonRegister;
		private string _buttonDelete;
		private string _labelCommunityName;
		private string _labelPassword;
		private string _errorCommunityNameEmpty;
		private string _errorCommunityNameDuplicated;
		private string _errorPasswordEmpty;
		private string _promptUserDeletion;

		public string TitleCommunity
		{
			get => _titleCommunity;
			set => SetProperty( ref _titleCommunity, value );
		}

		public string PromptCommunityRegistration
		{
			get => _promptCommunityRegistration;
			set => SetProperty( ref _promptCommunityRegistration, value );
		}

		public string PromptCommunityModification
		{
			get => _promptCommunityModification;
			set => SetProperty( ref _promptCommunityModification, value );
		}

		public string ButtonUpdate
		{
			get => _buttonUpdate;
			set => SetProperty( ref _buttonUpdate, value );
		}

		public string ButtonRegister
		{
			get => _buttonRegister;
			set => SetProperty( ref _buttonRegister, value );
		}

		public string ButtonDelete
		{
			get => _buttonDelete;
			set => SetProperty( ref _buttonDelete, value );
		}

		public string LabelCommunityName
		{
			get => _labelCommunityName;
			set => SetProperty( ref _labelCommunityName, value );
		}

		public string LabelPassword
		{
			get => _labelPassword;
			set => SetProperty( ref _labelPassword, value );
		}

		public string ErrorCommunityNameEmpty
		{
			get => _errorCommunityNameEmpty;
			set => SetProperty( ref _errorCommunityNameEmpty, value );
		}

		public string ErrorCommunityNameDuplicated
		{
			get => _errorCommunityNameDuplicated;
			set => SetProperty( ref _errorCommunityNameDuplicated, value );
		}

		public string ErrorPasswordEmpty
		{
			get => _errorPasswordEmpty;
			set => SetProperty( ref _errorPasswordEmpty, value );
		}

		public string PromptUserDeletion
		{
			get => _promptUserDeletion;
			set => SetProperty( ref _promptUserDeletion, value );
		}

		public event PropertyChangedEventHandler PropertyChanged;

		protected bool SetProperty<T>( ref T storage, T value, [CallerMemberName] string propertyName = null )
		{
			if( Equals( storage, value ) )
				return false;

			storage = value;
			OnPropertyChanged( propertyName );
			return true;
		}

		protected void OnPropertyChanged( [CallerMemberName] string propertyName = null )
		{
			PropertyChanged?.Invoke( this, new PropertyChangedEventArgs( propertyName ) );
		}
	}
}
