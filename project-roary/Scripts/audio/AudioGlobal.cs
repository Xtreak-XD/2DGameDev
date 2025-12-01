using Godot;
using System;

public partial class AudioGlobal : Node
{
	public String currScene; // OverWorld, GC, PG, GL, Stadium, Boss
	private Eventbus eventbus;
	public float MasterVolume { get; private set; }
	public float MusicVolume { get; private set; }
	public float PlayerSFXVolume { get; private set; }
	public float EnemySFXVolume { get; private set; }

	public override void _Ready()
    {
        eventbus = GetNode<Eventbus>("/root/Eventbus");
        
        LoadAndApplySettings();
    }

	public void LoadAndApplySettings()
	{
		var config = new ConfigFile();
		var err = config.Load("user://settings.cfg");

		if (err != Error.Ok)
		{
			SetVolume(80, "Master");
			SetVolume(70, "Music");
			SetVolume(85, "PlayerSFX");
			SetVolume(85, "EnemySFX");
			return;
		}

		// Load and apply saved volumes
		float masterVolume = (float)config.GetValue("audio", "master_volume", 80);
		float musicVolume = (float)config.GetValue("audio", "music_volume", 70);
		float playerSFXVolume = (float)config.GetValue("audio", "playerSFX_volume", 85);
		float enemySFXVolume = (float)config.GetValue("audio", "enemySFX_volume", 85);

		SetVolume(masterVolume, "Master");
		SetVolume(musicVolume, "Music");
		SetVolume(playerSFXVolume, "PlayerSFX");
		SetVolume(enemySFXVolume, "EnemySFX");
		
		GD.Print($"Loaded settings - Music: {musicVolume}, Master: {masterVolume}");
	}

	public void SaveSettings(float master, float music, float playerSFX, float enemySFX)
    {
        var config = new ConfigFile();
		config.Load("user://settings.cfg");
		config.SetValue("audio", "master_volume", master);
		config.SetValue("audio", "music_volume", music);
		config.SetValue("audio", "playerSFX_volume", playerSFX);
		config.SetValue("audio", "enemySFX_volume", enemySFX);
		config.Save("user://settings.cfg");
    }

	public void SetVolume(float value, string busName)
	{
		switch (busName)
		{
			case "Master":
				MasterVolume = value;
				break;
			case "Music":
				MusicVolume = value;
				break;
			case "PlayerSFX":
				PlayerSFXVolume = value;
				break;
			case "EnemySFX":
				EnemySFXVolume = value;
				break;
		}
		
		float linear = value / 100.0f;
		float volumeDb = linear > 0 ? Mathf.LinearToDb(linear) : -80f;
		AudioServer.SetBusVolumeDb(AudioServer.GetBusIndex(busName), volumeDb);
	}
}