using System.Threading;
using Cysharp.Threading.Tasks;

namespace com.ktgame.foundation.animation
{
    public static class AnimationEngineExtensions
    {
        /// <summary>
        /// Plays an animation and asynchronously waits for it to complete.
        /// </summary>
        public static async UniTask PlayAsync(
            this IAnimationEngine engine,
            string animName,
            bool loop = false,
            bool restart = false,
            int trackIndex = 0,
            float mixDuration = 0f,
            CancellationToken cancellationToken = default)
        {
            if (engine == null) return;

            // Use a UniTaskCompletionSource to await the exact Complete event callback
            var tcs = new UniTaskCompletionSource();

            // Define the callback
            AnimationDelegate onComplete = (track) =>
            {
                if (track.Name == animName)
                {
                    tcs.TrySetResult();
                }
            };

            engine.SubscribeComplete(onComplete);

            try
            {
                engine.Play(animName, loop, restart, trackIndex, mixDuration);

                // Wait until either the animation finishes or cancellation is requested
                await tcs.Task.AttachExternalCancellation(cancellationToken);
            }
            finally
            {
                // Guarantee cleanup to prevent memory leaks if cancelled or exception thrown
                engine.UnsubcribeComplete(onComplete);
            }
        }
    }
}
