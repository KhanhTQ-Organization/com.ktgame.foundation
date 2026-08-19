namespace com.ktgame.foundation.animation
{
	public delegate void AnimationDelegate(AnimationTrack track);

	public delegate void AnimationEventDelegate(AnimationTrack track, AnimationEvent e);

	public interface IAnimationEngine : IEngine
	{
		string CurrentAnimationName { get; }

		bool IsCurrentAnimationComplete { get; }

		float FlipX { get; }

		float FlipY { get; }

		float TimeScale { get; }

		void SetFlipX(bool flip);

		void SetFlipY(bool flip);

		void SetTimeScale(float timeScale);

		void SubscribeStart(AnimationDelegate callback);

		void UnsubcribeStart(AnimationDelegate callback);

		void SubscribeComplete(AnimationDelegate callback);

		void UnsubcribeComplete(AnimationDelegate callback);

		void SubscribeEvent(AnimationEventDelegate callback);

		void UnsubcribeEvent(AnimationEventDelegate callback);

		bool HasAnimation(string animName);

		bool IsPlaying(string animName);

		void Play(string animName, bool loop = false, bool restart = false, int trackIndex = 0, float mixDuration = 0f);

		bool TryPlay(string animName, bool loop = false, bool restart = false, int trackIndex = 0, float mixDuration = 0f);

		void Pause();

		void Resume();

		void Stop();

		void SetSkin(string skinName);
	}
}
