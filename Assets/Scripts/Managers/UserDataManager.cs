public class UserDataManager : Singleton<UserDataManager>
{
    private int _lastLevelCompleted = 0;
    
    private float _soundVolumeSaved = 100f;

    private float _musicVolumeSaved = 100f;
    public int LastLevelCompleted => _lastLevelCompleted;

    public float SoundVolumeSaved => _soundVolumeSaved;

    public float MusicVolumeSaved => _musicVolumeSaved;

    public void SaveVolumeSettings(float soundVolume, float musicVolume)
    {
        _soundVolumeSaved = soundVolume;
        _musicVolumeSaved = musicVolume;
    }

    public void SaveCompletingLevel(int levelNumber)
    {
        _lastLevelCompleted = levelNumber;
    }
}