using Godot;
using System;

public partial class AudioGlobal : Node
{
	public String currScene; // OverWorld, GC, PG, GL, Stadium, Boss
	public int musicVolume;
	public int sfxVolume;
	private Eventbus eventbus;

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
			// Apply defaults if no settings file exists
			ApplyVolume(80, "Master");
			ApplyVolume(70, "Music");
			ApplyVolume(85, "PlayerSFX");
			ApplyVolume(85, "EnemySFX");
			return;
		}

		// Load and apply saved volumes
		float masterVolume = (float)config.GetValue("audio", "master_volume", 80);
		float musicVolume = (float)config.GetValue("audio", "music_volume", 70);
		float playerSFXVolume = (float)config.GetValue("audio", "playerSFX_volume", 85);
		float enemySFXVolume = (float)config.GetValue("audio", "enemySFX_volume", 85);

		ApplyVolume(masterVolume, "Master");
		ApplyVolume(musicVolume, "Music");
		ApplyVolume(playerSFXVolume, "PlayerSFX");
		ApplyVolume(enemySFXVolume, "EnemySFX");
		
		GD.Print($"Loaded settings - Music: {musicVolume}, Master: {masterVolume}");
	}

	public void ApplyVolume(float value, string busName)
	{
		float linear = value / 100.0f;
		float volumeDb = linear > 0 ? Mathf.LinearToDb(linear) : -80f;
		AudioServer.SetBusVolumeDb(AudioServer.GetBusIndex(busName), volumeDb);
	}
}