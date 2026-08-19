namespace com.ktgame.foundation.animation
{
	public readonly struct AnimationEvent
	{
		public string Name { get; }

		public AnimationEvent(string name)
		{
			Name = name;
		}
	}
}
