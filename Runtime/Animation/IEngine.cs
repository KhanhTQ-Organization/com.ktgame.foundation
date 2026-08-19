namespace com.ktgame.foundation.animation
{
	public interface IEngine
	{
		bool Locked { get; }

		void OnEnter();

		void OnExecute();

		void OnExit();

		void SetLock(bool isLock);
	}
}
