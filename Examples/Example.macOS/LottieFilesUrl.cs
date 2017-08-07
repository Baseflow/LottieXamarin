using System;
using System.Globalization;
using System.Text.RegularExpressions;
using Foundation;

namespace Example.macOS
{
    public class LottieFilesUrl
    {
        private static string LottieFilesHost = "www.lottiefiles.com";
        private const string DownloadUrl = "https://www.lottiefiles.com/download/";

		public int Id { get; }
        public NSUrl JsonUrl { get; }
        public string AnimationName;


        public LottieFilesUrl(NSUrl url)
        {
            var fileIdRegExp = Regex.Match(url.LastPathComponent, @"^\d+", RegexOptions.Singleline);
            if (fileIdRegExp.Success)
            {
                this.Id = Int32.Parse(fileIdRegExp.Value);

                this.JsonUrl = new NSUrl(String.Concat(DownloadUrl, this.Id));

                this.AnimationName = GetAnimationName(
                    url.LastPathComponent.Substring(fileIdRegExp.Length + 1)
                );
            }
        }

        public static bool IsValidUrl(NSUrl url)
        {
            if (url == null)
                return false;

            bool isValidUrl = url.Host.Equals(LottieFilesHost, 
                                    StringComparison.InvariantCultureIgnoreCase);

            isValidUrl &= Regex.Match(url.LastPathComponent, @"^\d+", 
                                      RegexOptions.Singleline).Success;

            return isValidUrl;
        }

        private string GetAnimationName(string title)
        {
            title = title.Replace('-', ' ');
			return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(title);
		}
    }
}
