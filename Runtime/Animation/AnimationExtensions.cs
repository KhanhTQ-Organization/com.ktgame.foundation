using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace com.ktgame.foundation.animation
{
    public static class AnimationExtensions
    {
        public static async UniTask PlayAsync(
            this IAnimation self, 
            IProgress<float> progress = null, 
            bool isLooping = false, 
            bool ignoreTimeScale = true, 
            CancellationToken cancellationToken = default)
        {
            var player = new AnimationPlayer(self, isLooping);
            progress?.Report(0.0f);
            player.Play();

            while (!player.IsFinished)
            {
                cancellationToken.ThrowIfCancellationRequested();
                
                await UniTask.NextFrame(cancellationToken);
                
                float deltaTime = ignoreTimeScale ? Time.unscaledDeltaTime : Time.deltaTime;
                player.Update(deltaTime);
                
                float progressValue = self.Duration > 0 ? player.Time / self.Duration : 1f;
                progress?.Report(progressValue);
            }
        }
    }
}
