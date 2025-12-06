using Godot;
using System;

public partial class SettingsMenu : Control
{
	public Eventbus eventbus;
	private AudioGlobal audioGlobal;
	// Audio sliders
	private HSlider masterSlider;
	private Label masterValueLabel;
	private HSlider musicSlider;
	private Label musicValueLabel;
	private Label enemySFXLabel;
	private HSlider enemySFXSlider;
	private Label playerSFXValueLabel;
	private HSlider playerSFXSlider;

	// Display settings
	private CheckBox fullscreenCheck;
	private CheckBox vsyncCheck;

	public SceneManager sceneManager;
	public Button back;
	public Button apply;

	public override void _EnterTree()
	{
		// Load config fresh (don't store it)
		var config = new ConfigFile();
		var err = config.Load("user://settings.cfg");
		
		// If config doesn't exist, create it with defaults
		if (err != Error.Ok)
		{
			config.SetValue("display", "fullscreen", true);
			config.SetValue("display", "vsync", true);
			config.Save("user://settings.cfg");
		}
		
		// Apply display settings EARLY (before window shows)
		bool fullscreen = (bool)config.GetValue("display", "fullscreen", true);
		bool vsync = (bool)config.GetValue("display", "vsync", true);
		
		DisplayServer.WindowSetMode(fullscreen ? DisplayServer.WindowMode.Fullscreen : DisplayServer.WindowMode.Windowed);
		DisplayServer.WindowSetVsyncMode(vsync ? DisplayServer.VSyncMode.Enabled : DisplayServer.VSyncMode.Disabled);
	}
	public override void _Ready()
    {
		ProcessMode = ProcessModeEnum.Always;
		SetProcess(true);
		eventbus = GetNode<Eventbus>("/root/Eventbus");
		sceneManager = GetNode<SceneManager>("/root/SceneManager");
		audioGlobal = GetNode<AudioGlobal>("/root/AudioGlobal");

        Hide();

		// Audio settings nodes
		masterSlider = GetNode<HSlider>("%MasterSlider");
		masterValueLabel = GetNode<Label>("%MasterValue");
		musicSlider = GetNode<HSlider>("%MusicSlider");
		musicValueLabel = GetNode<Label>("%MusicValue");
		playerSFXSlider = GetNode<HSlider>("%PlayerSFXSlider");
		playerSFXValueLabel = GetNode<Label>("%PlayerSFXValue");
		enemySFXSlider = GetNode<HSlider>("%EnemySFXSlider");
		enemySFXLabel = GetNode<Label>("%EnemySFXValue");
		fullscreenCheck = GetNode<CheckBox>("%FullscreenCheck");
		vsyncCheck = GetNode<CheckBox>("%VsyncCheck");

		back = GetNode<Button>("%BackButton");
		apply = GetNode<Button>("%ApplyButton");

		masterSlider.ValueChanged += OnMasterVolumeChanged;
		musicSlider.ValueChanged += OnMusicVolumeChanged;
		playerSFXSlider.ValueChanged += OnPlayerSFXVolumeChanged;
		enemySFXSlider.ValueChanged += OnEnemySFXVolumeChanged;
		fullscreenCheck.Toggled += OnFullscreenToggled;
		vsyncCheck.Toggled += OnVSyncToggled;

		back.Pressed += OnBackPressed;
		apply.Pressed += SaveSettings;	

		eventbus.showSettings += ShowSettings;

		LoadSettingsToUI();
	}

	public override void _Input(InputEvent @event)
	{
		if (@event.IsActionPressed("Pause") && Visible)
		{
			GetViewport().SetInputAsHandled(); // Prevents the event from propagating further (remove if we want to exit completely from option)
			OnBackPressed();
		}
	}

	public void ShowSettings()
	{
		LoadSettingsToUI();
		Show();
	}

	private void SaveSettings()
	{
		var config = new ConfigFile();
		config.Load("user://settings.cfg");
		
		// Save audio settings
		config.SetValue("audio", "master_volume", (float)masterSlider.Value);
		config.SetValue("audio", "music_volume", (float)musicSlider.Value);
		config.SetValue("audio", "playerSFX_volume", (float)playerSFXSlider.Value);
		config.SetValue("audio", "enemySFX_volume", (float)enemySFXSlider.Value);
		
		// Save display settings
		config.SetValue("display", "fullscreen", fullscreenCheck.ButtonPressed);
		config.SetValue("display", "vsync", vsyncCheck.ButtonPressed);
		
		// Single save operation
		config.Save("user://settings.cfg");
		
		// Update AudioGlobal's internal state (but don't save again)
		audioGlobal.SetVolume((float)masterSlider.Value, "Master");
		audioGlobal.SetVolume((float)musicSlider.Value, "Music");
		audioGlobal.SetVolume((float)playerSFXSlider.Value, "PlayerSFX");
		audioGlobal.SetVolume((float)enemySFXSlider.Value, "EnemySFX");
	}

	private void LoadSettingsToUI()
	{
		// Load audio settings from AudioGlobal
		masterSlider.Value = audioGlobal.MasterVolume;
		musicSlider.Value = audioGlobal.MusicVolume;
		playerSFXSlider.Value = audioGlobal.PlayerSFXVolume;
		enemySFXSlider.Value = audioGlobal.EnemySFXVolume;

		// Update labels (ValueChanged events will fire automatically)
		masterValueLabel.Text = $"{(int)masterSlider.Value}%";
		musicValueLabel.Text = $"{(int)musicSlider.Value}%";
		playerSFXValueLabel.Text = $"{(int)playerSFXSlider.Value}%";
		enemySFXLabel.Text = $"{(int)enemySFXSlider.Value}%";
		
		var config = new ConfigFile();
    	config.Load("user://settings.cfg");
		fullscreenCheck.ButtonPressed = (bool)config.GetValue("display", "fullscreen", true);
		vsyncCheck.ButtonPressed = (bool)config.GetValue("display", "vsync", true);
	}

    private void OnMasterVolumeChanged(double value)
    {
        masterValueLabel.Text = $"{(int)value}%";

		audioGlobal.SetVolume((float)value, "Master");
    }

    private void OnMusicVolumeChanged(double value)
    {
        musicValueLabel.Text = $"{(int)value}%";

		audioGlobal.SetVolume((float)value, "Music");
    }

    private void OnPlayerSFXVolumeChanged(double value)
    {
        playerSFXValueLabel.Text = $"{(int)value}%";
		audioGlobal.SetVolume((float)value, "PlayerSFX");
    }

	private void OnEnemySFXVolumeChanged(double value)
	{
		enemySFXLabel.Text = $"{(int)value}%";
		audioGlobal.SetVolume((float)value, "EnemySFX");
	}

	private void OnFullscreenToggled(bool isToggled)
    {
		DisplayServer.WindowSetMode(isToggled ? DisplayServer.WindowMode.Fullscreen : DisplayServer.WindowMode.Windowed);

    }

	private void OnVSyncToggled(bool isToggled)
	{
		DisplayServer.WindowSetVsyncMode(isToggled ? DisplayServer.VSyncMode.Enabled : DisplayServer.VSyncMode.Disabled);
	}
	
	private void OnBackPressed()
    {
		Hide();
		eventbus.EmitSignal("leftSettings");
	}
}
