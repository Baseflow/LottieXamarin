using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Lottie.Forms.ViewModels
{
	public class LottieFormsViewModel : INotifyPropertyChanged
	{
		private string _animationResource;
		public string AnimationResource
		{
			get
			{
				return _animationResource;
			}

			set
			{
				_animationResource = value;
				NotifyChanged();
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;

		protected void NotifyChanged([CallerMemberName] string name = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
		}
	}
}