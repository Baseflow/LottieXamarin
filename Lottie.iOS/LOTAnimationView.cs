using System;
using System.Threading.Tasks;
using Foundation;

namespace Airbnb.Lottie
{
    public partial class LOTAnimationView
    {
        /// <summary>
        /// Asynchronously play the animation.
        /// </summary>
        public Task<bool> PlayAsync()
        {
            var tcs = new TaskCompletionSource<bool>();

            this.PlayWithCompletion((bool animationFinished) => tcs.SetResult(animationFinished));

            return tcs.Task;
        }

		/// <summary>
		/// Plays the animation from its current position to a specific progress.
		/// The animation will start from its current position.
		/// </summary>
		public Task<bool> PlayToProgressAsync(nfloat toProgress)
		{
			var tcs = new TaskCompletionSource<bool>();

			this.PlayToProgress(toProgress, (bool animationFinished) => tcs.SetResult(animationFinished));

			return tcs.Task;
		}

		public Task<bool> PlayFromProgressAsync(nfloat fromStartProgress, nfloat toEndProgress)
		{
			var tcs = new TaskCompletionSource<bool>();

			this.PlayFromProgress(fromStartProgress, toEndProgress, (bool animationFinished) => tcs.SetResult(animationFinished));

			return tcs.Task;
		}

		public Task<bool> PlayToFrameAsync(NSNumber toFrame)
		{
			var tcs = new TaskCompletionSource<bool>();

			this.PlayToFrame(toFrame, (bool animationFinished) => tcs.SetResult(animationFinished));

			return tcs.Task;
		}

		public Task<bool> PlayFromFrameAsync(NSNumber fromStartFrame, NSNumber toEndFrame)
		{
			var tcs = new TaskCompletionSource<bool>();

			this.PlayFromFrame(fromStartFrame, toEndFrame, (bool animationFinished) => tcs.SetResult(animationFinished));

			return tcs.Task;
		}

    }
}
