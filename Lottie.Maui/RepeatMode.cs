namespace Lottie.Maui
{
    public enum RepeatMode
    {
        /// <summary>
        /// When the animation reaches the end and RepeatCount is Infinite or a positive value, the animation restarts from the beginning.
        /// </summary>
        Restart = 0,

        /// <summary>
        /// When the animation reaches the end and RepeatCount is Infinite or a positive value, the animation reverses direction on every iteration.
        /// </summary>
        Reverse = 1,

        /// <summary>
        /// Repeat the animation indefinitely.
        /// </summary>
        Infinite = 2
    }
}
