using Godot;
using System;

public partial class AudioGlobal : Node
{
	public String currScene; // OverWorld, GC, PG, GL, Stadium, Boss
	private Eventbus eventbus;
	private String currentClip = "";
	public float MasterVolume { get; private set; }
	public float MusicVolume { get; private set; }
	public float PlayerSFXVolume { get; private set; }
	public float EnemySFXVolume { get; private set; }

	private AudioStreamPlayer musicPlayer;

	public override void _Ready()
    {
		LoadAndApplySettings();

        eventbus = GetNode<Eventbus>("/root/Eventbus");
		eventbus.sceneChanged += OnSceneChanged;

		// Get or create the AudioStreamPlayer child
		musicPlayer = GetNodeOrNull<AudioStreamPlayer>("MusicPlayer");

		if (musicPlayer == null)
		{
			GD.PrintErr("AudioGlobal: MusicPlayer child not found. Please add an AudioStreamPlayer named 'MusicPlayer' as a child of AudioGlobal in the Godot editor.");
			return;
		}

		string initialScene = GetTree().CurrentScene.Name;
		string clipName = GetClipNameForScene(initialScene);

		GD.Print("Volumes - Music 2: " + MusicVolume + ", Master: " + MasterVolume);

		if (!string.IsNullOrEmpty(clipName))
		{
			musicPlayer.Set("parameters/switch_to_clip", clipName);
			currentClip = clipName;
		}

		// Start playing the music
		if (!musicPlayer.Playing)
		{
			musicPlayer.Play();
		}
    }

	public override void _ExitTree()
	{
		if (eventbus != null)
		{
			eventbus.sceneChanged -= OnSceneChanged;
		}
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

	// public void SaveSettings(float master, float music, float playerSFX, float enemySFX)
    // {
    //     var config = new ConfigFile();
	// 	var err = config.Load("user://settings.cfg");

	// 	// If config doesn't exist, it will be created when we save
	// 	if (err != Error.Ok)
	// 	{
	// 		GD.Print("AudioGlobal: Creating new settings.cfg file");
	// 	}

	// 	config.SetValue("audio", "master_volume", master);
	// 	config.SetValue("audio", "music_volume", music);
	// 	config.SetValue("audio", "playerSFX_volume", playerSFX);
	// 	config.SetValue("audio", "enemySFX_volume", enemySFX);

	// 	var saveErr = config.Save("user://settings.cfg");
	// 	if (saveErr != Error.Ok)
	// 	{
	// 		GD.PrintErr($"AudioGlobal: Failed to save settings.cfg - Error: {saveErr}");
	// 	}
	// 	else
	// 	{
	// 		GD.Print($"AudioGlobal: Settings saved - Master: {master}, Music: {music}, PlayerSFX: {playerSFX}, EnemySFX: {enemySFX}");
	// 	}
    // }

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
	private void OnSceneChanged(string sceneName)
	{
		currScene = sceneName;
        GD.Print($"AudioGlobal: Scene changed to {sceneName}");

		SwitchMusicForScene(sceneName);
	}

	private void SwitchMusicForScene(string sceneName)
	{
		if (musicPlayer == null || musicPlayer.Stream is not AudioStreamInteractive)
		{
			GD.PrintErr("AudioGlobal: MusicPlayer is not set up with AudioStreamInteractive");
			return;
		}

		string clipName = GetClipNameForScene(sceneName);
		GD.Print("ClipName: " + clipName + "CurrentClip: " + currentClip);

		if (currentClip == clipName)
		{
			GD.Print("AudioGlobal: Music clip is already playing for this scene, no switch needed.");
			return;
		}

		musicPlayer.Set("parameters/switch_to_clip", clipName);
		currentClip = clipName;
		GD.Print($"AudioGlobal: Switched to music clip {clipName} for scene {sceneName}");
	}

	private String GetClipNameForScene(String sceneName)
    {
        return sceneName switch
		{
			"Overworld" => "OverworldMusic", 
            "GreenLibrary" => "GreenLibraryMusic",
            "GreenLibraryBoss" => "GreenLibraryBoss",
            "GrahamCenter" => "GrahamCenterMusic",
            "NaturePreserve" => "NaturePreserveMusic",
            "ParkingGarage" => "ParkingGarageMusic",
            "Stadium" => "StadiumMusic",
			"DiedScreen" => "RespawnScreenMusic",
            _ => "MenuMusic"
		};
    }
}