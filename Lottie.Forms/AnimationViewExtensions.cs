using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
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
                Assembly assembly = null;
                string resourceName = null;

                if (embeddedAnimation.StartsWith("resource://", StringComparison.OrdinalIgnoreCase))
                {
                    var uri = new Uri(embeddedAnimation);

                    var parts = uri.OriginalString.Substring(11).Split('?');
                    resourceName = parts.First();

                    if (parts.Count() > 1)
                    {
                        var name = Uri.UnescapeDataString(uri.Query.Substring(10));
                        var assemblyName = new AssemblyName(name);
                        assembly = Assembly.Load(assemblyName);
                    }

                    if (assembly == null)
                    {
                        var callingAssemblyMethod = typeof(Assembly).GetTypeInfo().GetDeclaredMethod("GetCallingAssembly");
                        assembly = (Assembly)callingAssemblyMethod.Invoke(null, new object[0]);
                    }
                }

                if (assembly == null)
                    assembly = Xamarin.Forms.Application.Current.GetType().Assembly;

                if (string.IsNullOrEmpty(resourceName))
                    resourceName = $"{assembly.GetName().Name}.{embeddedAnimation}";

                var stream = assembly.GetManifestResourceStream(resourceName);

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
