using System;

namespace com.ktgame.foundation.animation
{
    public class AnimationPlayer : IUpdatable
    {
        public IAnimation Animation { get; private set; }

        public bool IsPlaying { get; private set; }
        
        public bool IsLooping { get; set; }

        public float Time { get; private set; }

        public bool IsFinished => !IsLooping && Time >= Animation.Duration;

        public AnimationPlayer(IAnimation animation, bool isLooping = false)
        {
            Animation = animation;
            IsLooping = isLooping;
            IsPlaying = false;
            SetTime(0f);
        }

        public void Update(float deltaTime)
        {
            if (!IsPlaying)
            {
                return;
            }

            SetTime(Time + deltaTime);
        }

        public void Play()
        {
            IsPlaying = true;
        }

        public void Stop()
        {
            IsPlaying = false;
        }

        public void Reset()
        {
            SetTime(0.0f);
        }

        public void SetTime(float time)
        {
            if (Animation.Duration > 0)
            {
                if (IsLooping && time >= Animation.Duration)
                {
                    time %= Animation.Duration;
                }
            }
            
            time = Math.Max(0, Math.Min(Animation.Duration, time));
            Animation.SetTime(time);

            if (IsPlaying && !IsLooping && time >= Animation.Duration)
            {
                Stop();
            }

            Time = time;
        }
    }
}