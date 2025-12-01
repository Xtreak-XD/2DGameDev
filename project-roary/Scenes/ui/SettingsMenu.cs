using Godot;
using System;

public partial class SettingsMenu : Control
{
	public Eventbus eventbus;
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

	public SceneManager sceneManager;
	
	public Button back;
	public Button apply;
	public override void _Ready()
    {
		eventbus = GetNode<Eventbus>("/root/Eventbus");
		sceneManager = GetNode<SceneManager>("/root/SceneManager");
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

		//menuScreen = GetNode<MenuScreen>("%Menu Screen");

		back = GetNode<Button>("%BackButton");
		apply = GetNode<Button>("%ApplyButton");

		masterSlider.ValueChanged += OnMasterVolumeChanged;
		musicSlider.ValueChanged += OnMusicVolumeChanged;
		sfxSlider.ValueChanged += OnSFXVolumeChanged;
		fullscreenCheck.Toggled += OnFullscreenToggled;
		vsyncCheck.Toggled += OnVSyncToggled;

		back.Pressed += OnBackPressed;
		apply.Pressed += SaveSettings;	

		eventbus.loadSettings += LoadSettings;
		eventbus.showSettings += ShowSettings;
    }

	public override void _Input(InputEvent @event)
	{
		if (@event.IsActionPressed("Pause") && Visible)
		{
			GetViewport().SetInputAsHandled(); // Prevents the event from propagating further (remove if we want to exit completely from option)
			OnBackPressed();
		}
	}

	public override void _ExitTree()
	{
		masterSlider.ValueChanged -= OnMasterVolumeChanged;
		musicSlider.ValueChanged -= OnMusicVolumeChanged;
		sfxSlider.ValueChanged -= OnSFXVolumeChanged;
		fullscreenCheck.Toggled -= OnFullscreenToggled;
		vsyncCheck.Toggled -= OnVSyncToggled;

		back.Pressed -= OnBackPressed;
		apply.Pressed -= SaveSettings;
		eventbus.loadSettings -= LoadSettings;
		eventbus.showSettings -= ShowSettings;
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
			masterSlider.Value = 100;
			musicSlider.Value = 100;
			sfxSlider.Value = 100;
			return;
		}

		masterSlider.Value = (float)config.GetValue("audio", "master_volume", 100);
		musicSlider.Value = (float)config.GetValue("audio", "music_volume", 100);
		sfxSlider.Value = (float)config.GetValue("audio", "sfx_volume", 100);

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
		eventbus.EmitSignal("leftSettings");
	}
}
