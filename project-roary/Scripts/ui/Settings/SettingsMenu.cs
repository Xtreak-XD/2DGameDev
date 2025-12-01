using Godot;
using System;

public partial class SettingsMenu : Control
{
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

	private MenuScreen menuScreen;
	private CheckBox vsyncCheck;
	
	public override void _Ready()
    {
        Hide();
		ProcessMode = ProcessModeEnum.Always;

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

		menuScreen = GetNode<MenuScreen>("%Menu Screen");

		masterSlider.ValueChanged += OnMasterVolumeChanged;
		musicSlider.ValueChanged += OnMusicVolumeChanged;
		playerSFXSlider.ValueChanged += OnPlayerSFXVolumeChanged;
		enemySFXSlider.ValueChanged += OnEnemySFXVolumeChanged;
		fullscreenCheck.Toggled += OnFullscreenToggled;
		vsyncCheck.Toggled += OnVSyncToggled;

		GetNode<Button>("%BackButton").Pressed += OnBackPressed;
		GetNode<Button>("%ApplyButton").Pressed += SaveSettings;	

		LoadSettings();
    }

	public override void _Input(InputEvent @event)
	{
		if (@event.IsActionPressed("ui_cancel") && Visible)
		{
			GetViewport().SetInputAsHandled(); // Prevents the event from propagating further (remove if we want to exit completely from option)
			OnBackPressed();
		}
	}

	public void ShowSettings()
	{
		Show();
	}

	private void SaveSettings()
    {
        var config = new ConfigFile();
		config.SetValue("audio", "master_volume", masterSlider.Value);
		config.SetValue("audio", "music_volume", musicSlider.Value);
		config.SetValue("audio", "playerSFX_volume", playerSFXSlider.Value);
		config.SetValue("audio", "enemySFX_volume", enemySFXSlider.Value);

		config.SetValue("display", "fullscreen", fullscreenCheck.ButtonPressed);
		config.SetValue("display", "vsync", vsyncCheck.ButtonPressed);

		config.Save("user://settings.cfg");
    }

	private void LoadSettings()
	{
		var config = new ConfigFile();
		var err = config.Load("user://settings.cfg");

		if (err != Error.Ok)
		{
			masterSlider.Value = 80;
			musicSlider.Value = 70;
			playerSFXSlider.Value = 85;
			enemySFXSlider.Value = 85;
			return;
		}

		masterSlider.Value = (float)config.GetValue("audio", "master_volume", 80);
		musicSlider.Value = (float)config.GetValue("audio", "music_volume", 70);
		playerSFXSlider.Value = (float)config.GetValue("audio", "playerSFX_volume", 85);
		enemySFXSlider.Value = (float)config.GetValue("audio", "enemySFX_volume", 85);

		masterValueLabel.Text = $"{(int)masterSlider.Value}%";
		musicValueLabel.Text = $"{(int)musicSlider.Value}%";
		playerSFXValueLabel.Text = $"{(int)playerSFXSlider.Value}%";
		enemySFXLabel.Text = $"{(int)enemySFXSlider.Value}%";

		fullscreenCheck.ButtonPressed = (bool)config.GetValue("display", "fullscreen", true);
		vsyncCheck.ButtonPressed = (bool)config.GetValue("display", "vsync", true);
	}

    private void OnMasterVolumeChanged(double value)
    {
        masterValueLabel.Text = $"{(int)value}%";

		ChangeVolume(value, "Master");
    }

    private void OnMusicVolumeChanged(double value)
    {
        musicValueLabel.Text = $"{(int)value}%";

		ChangeVolume(value, "Music");
    }

    private void OnPlayerSFXVolumeChanged(double value)
    {
        playerSFXValueLabel.Text = $"{(int)value}%";
		ChangeVolume(value, "PlayerSFX");
    }

	private void OnEnemySFXVolumeChanged(double value)
	{
		enemySFXLabel.Text = $"{(int)value}%";
		ChangeVolume(value, "EnemySFX");
	}

	private void OnFullscreenToggled(bool toggledOn)
    {
		GD.Print(toggledOn);
        if (toggledOn)
        {
            DisplayServer.WindowSetMode(DisplayServer.WindowMode.Fullscreen);
        }
        else
        {
            DisplayServer.WindowSetMode(DisplayServer.WindowMode.Windowed);
        }
    }

	private void OnVSyncToggled(bool toggledOn)
	{
		if (toggledOn)
		{
			DisplayServer.WindowSetVsyncMode(DisplayServer.VSyncMode.Enabled);		}
		else
		{
			DisplayServer.WindowSetVsyncMode(DisplayServer.VSyncMode.Disabled);
		}
		
	}
	
	private void OnBackPressed()
    {
        Hide();
		menuScreen.Show();
    }

	private void ChangeVolume(double value, String name)
    {
		float linear = (float)value / 100.0f;
		float volumeDb = linear > 0 ? Mathf.LinearToDb(linear) : -80f;
	    AudioServer.SetBusVolumeDb(AudioServer.GetBusIndex(name), volumeDb);
    }
}
