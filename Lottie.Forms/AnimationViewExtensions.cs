using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lottie.Forms
{
    public static class AnimationViewExtensions
    {
        public static Stream GetStreamFromAssembly(this AnimationView animationView)
        {
            if (animationView.Animation is string embeddedAnimation)
            {
                var assembly = Xamarin.Forms.Application.Current.GetType().Assembly;
                var stream = assembly.GetManifestResourceStream($"{assembly.GetName().Name}.{embeddedAnimation}");

                if (stream == null)
                {
                    return null;
                    //throw new FileNotFoundException("Cannot find file.", embeddedAnimation);
                }
                return stream;
            }
            return null;
        }
    }
}
