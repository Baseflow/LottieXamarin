using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Lottie.Forms
{
    public static class LottieExtensions
    {
        public static void ExecuteCommandIfPossible(this ICommand command)
        {
            if (command?.CanExecute(null) == true)
            {
                command.Execute(null);
            }
        }
    }
}
