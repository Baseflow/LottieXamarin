namespace Lottie.Forms
{
    public enum AnimationSource
    {
        /// <summary>
        /// Use when Lottie should load the json file from Asset or Bundle folder.
        /// The Animation input should be a string containing the file name
        /// </summary>
        AssetOrBundle,

        /// <summary>
        /// Url should point to a json file containing the Lottie animation on a remote resource
        /// </summary>
        Url,

        /// <summary>
        /// Use when passing in json directly as a string
        /// </summary>
        Json,

        /// <summary>
        /// Stream the Lottie animation to the view
        /// </summary>
        Stream,

        /// <summary>
        /// When loading from an EmbeddedResource which is compiled into an Assembly
        /// Either set the file name as string to make Lottie read from the calling Assembly
        /// Or use the syntax "resource://LottieLogo1.json?assembly=Example.Forms"
        /// </summary>
        EmbeddedResource
    }
}
