using System.Threading;
using Cysharp.Threading.Tasks;

namespace com.ktgame.foundation.animation
{
    public static class AnimationEngineExtensions
    {
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
            
            var tcs = new UniTaskCompletionSource();
            var completed = false;
            
            AnimationDelegate onComplete = (track) =>
            {
                if (track.Name == animName)
                {
                    completed = true;
                    tcs.TrySetResult();
                }
            };

            engine.SubscribeComplete(onComplete);

            try
            {
                engine.Play(animName, loop, restart, trackIndex, mixDuration);

                if (trackIndex == 0)
                {
                    var interrupted = UniTask.WaitUntil(() => completed || engine.CurrentAnimationName != animName,
                        cancellationToken: cancellationToken);
                    await UniTask.WhenAny(tcs.Task, interrupted);
                }
                else
                {
                    await tcs.Task.AttachExternalCancellation(cancellationToken);
                }
            }
            finally
            {
                engine.UnsubcribeComplete(onComplete);
            }
        }
    }
}
