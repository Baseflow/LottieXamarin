using System.Windows.Input;

namespace Lottie.Maui
{
    public static class LottieExtensions
    {
        public static void ExecuteCommandIfPossible(this ICommand command, object parameter = null)
        {
            if (command?.CanExecute(parameter) == true)
            {
                command.Execute(parameter);
            }
        }
    }
}
