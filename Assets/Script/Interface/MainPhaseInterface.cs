public interface IMainPhase
{
	void OnLoad();
	void OnStatus();
	void OnUpdate();
	void OnClear();
}


public interface IPlayLevel
{
	void OnLoad();
	void OnLevel();
	void OnUpdate();
	void OnClear();
}