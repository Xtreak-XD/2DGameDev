using Godot;
using System;

public partial class SettingsMenu : Control
{
	// Audio sliders
	private HSlider masterSlider;
	private Label masterValueLabel;
	private HSlider musicSlider;
	private Label musicValueLabel;
	private HSlider sfxSlider;
	private Label sfxValueLabel;

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
		sfxSlider = GetNode<HSlider>("%SFXSlider");
		sfxValueLabel = GetNode<Label>("%SFXValue");
		fullscreenCheck = GetNode<CheckBox>("%FullscreenCheck");
		vsyncCheck = GetNode<CheckBox>("%VsyncCheck");

		menuScreen = GetNode<MenuScreen>("%Menu Screen");

		masterSlider.ValueChanged += OnMasterVolumeChanged;
		musicSlider.ValueChanged += OnMusicVolumeChanged;
		sfxSlider.ValueChanged += OnSFXVolumeChanged;
		fullscreenCheck.Toggled += OnFullscreenToggled;
		vsyncCheck.Toggled += OnVSyncToggled;

		GetNode<Button>("%BackButton").Pressed += OnBackPressed;
		GetNode<Button>("%ApplyButton").Pressed += SaveSettings;	

		LoadSettings();
    }

	public override void _ExitTree()
	{
		masterSlider.ValueChanged -= OnMasterVolumeChanged;
		musicSlider.ValueChanged -= OnMusicVolumeChanged;
		sfxSlider.ValueChanged -= OnSFXVolumeChanged;
		fullscreenCheck.Toggled -= OnFullscreenToggled;
		vsyncCheck.Toggled -= OnVSyncToggled;

		GetNode<Button>("%BackButton").Pressed -= OnBackPressed;
		GetNode<Button>("%ApplyButton").Pressed -= SaveSettings;	
	}

	public void ShowSettings()
	{
		Show();
		LoadSettings();
	}

	private void SaveSettings()
    {
        var config = new ConfigFile();
		config.SetValue("audio", "master_volume", masterSlider.Value);
		config.SetValue("audio", "music_volume", musicSlider.Value);
		config.SetValue("audio", "sfx_volume", sfxSlider.Value);

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
			sfxSlider.Value = 85;
			return;
		}

		masterSlider.Value = (float)config.GetValue("audio", "master_volume", 80);
		musicSlider.Value = (float)config.GetValue("audio", "music_volume", 70);
		sfxSlider.Value = (float)config.GetValue("audio", "sfx_volume", 85);

		fullscreenCheck.ButtonPressed = (bool)config.GetValue("display", "fullscreen", true);
		vsyncCheck.ButtonPressed = (bool)config.GetValue("display", "vsync", true);
	}

	//ToDo: Make each slider actually change volume in game
    private void OnMasterVolumeChanged(double value)
    {
        masterValueLabel.Text = $"{(int)value}%";
    }

    private void OnMusicVolumeChanged(double value)
    {
        musicValueLabel.Text = $"{(int)value}%";
    }

    private void OnSFXVolumeChanged(double value)
    {
        sfxValueLabel.Text = $"{(int)value}%";
    }

	private void OnFullscreenToggled(bool toggledOn)
    {
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
}
