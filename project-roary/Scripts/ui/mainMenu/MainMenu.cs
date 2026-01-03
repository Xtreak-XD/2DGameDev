using Godot;
using System;

public partial class MainMenu : Control
{

    public Button  playContinue;
    public Button Options;
    public Button Exit;
    public Button Reset;

    public Eventbus eventbus;
    public SceneManager sceneManager;
    public SaveManager saveManager;

    private bool hasSavedFile = false;

    public override void _Ready()
    {
        saveManager = GetNode<SaveManager>("/root/SaveManager");
        Options = GetNode<Button>("buttons/Options");
        Exit = GetNode<Button>("buttons/Exit");
        playContinue = GetNode<Button>("buttons/playContinue");
        Reset = GetNode<Button>("buttons/Reset");
        eventbus = GetNode<Eventbus>("/root/Eventbus");
        sceneManager = GetNode<SceneManager>("/root/SceneManager");

        var dayNight = GetNode<DayNightCycle>("/root/DayNightCycle");
        dayNight.Visible = false;

        checkForSaveFile();

        eventbus.leftSettings += onLeftSettings;
        eventbus.deleted += saveDeleted;
        playContinue.Pressed += onPlayContinuePressed;
        Options.Pressed += onOptionsPressed;
        Exit.Pressed += onExitPressed;
        Reset.Pressed += onResetPressed;
    }

    void checkForSaveFile()
    {
        hasSavedFile = saveManager.SaveFileExists();

        if (hasSavedFile)
        {
            playContinue.Text = "Continue";
            Reset.Show();
            GD.Print("file exists");
        }
        else
        {
            playContinue.Text = "Play";
            Reset.Hide();
            GD.Print("file doesn't exist");
        }
    }

    void onPlayContinuePressed()
    {
        if (hasSavedFile)
        {
            eventbus.EmitSignal("load");
        }
        else
        {
            StartNewGame();
        }
    }

    void onResetPressed()
    {
        eventbus.EmitSignal(Eventbus.SignalName.deleteSave);
    }

    void saveDeleted()
    {
        _ExitTree();
        _Ready();
    }

    void StartNewGame()
    {
        string firstScenePath = "res://Scenes/map/ParkingGarage/ParkingGarage.tscn";

        if (!ResourceLoader.Exists(firstScenePath))
        {
            GD.PrintErr($"First scene not found: {firstScenePath}");
            return;
        }

        sceneManager.player?.QueueFree();
        sceneManager.player = null;

        saveManager.CreateNewSave(firstScenePath);
        sceneManager.goToScene(this, firstScenePath, false);
    }

    void onOptionsPressed()
    {
        Hide();
        eventbus.EmitSignal("showSettings");
    }

    void onLeftSettings()
    {
        Show();
    }

    void onExitPressed()
    {
        //bye bye
        GetTree().Quit();
    }

    public override void _ExitTree()
    {
        eventbus.leftSettings -= onLeftSettings;
        playContinue.Pressed -= onPlayContinuePressed;
        Options.Pressed -= onOptionsPressed;
        Exit.Pressed -= onExitPressed;
        Reset.Pressed -= onResetPressed;
        eventbus.deleted -= saveDeleted;

        var dayNight = GetNode<DayNightCycle>("/root/DayNightCycle");
        dayNight.Visible = true;
    }


}
