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

	public override void _Ready()
    {
		eventbus = GetNode<Eventbus>("/root/Eventbus");
		sceneManager = GetNode<SceneManager>("/root/SceneManager");
		audioGlobal = GetNode<AudioGlobal>("/root/AudioGlobal");

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

		//eventbus.loadSettings += LoadSettings;
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

	public void ShowSettings()
	{
		LoadSettingsToUI();
		Show();
	}

	private void SaveSettings()
    {
		audioGlobal.SaveSettings(
			(float)masterSlider.Value,
			(float)musicSlider.Value,
			(float)playerSFXSlider.Value,
			(float)enemySFXSlider.Value
		);

        var config = new ConfigFile();
		config.Load("user://settings.cfg");
		config.SetValue("display", "fullscreen", fullscreenCheck.ButtonPressed);
		config.SetValue("display", "vsync", vsyncCheck.ButtonPressed);
		config.Save("user://settings.cfg");
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
		var err = config.Load("user://settings.cfg");
		
		if (err == Error.Ok)
		{
			fullscreenCheck.ButtonPressed = (bool)config.GetValue("display", "fullscreen", true);
			vsyncCheck.ButtonPressed = (bool)config.GetValue("display", "vsync", true);
		}
		else
		{
			fullscreenCheck.ButtonPressed = true;
			vsyncCheck.ButtonPressed = true;
		}
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
		eventbus.EmitSignal("leftSettings");
	}
}
