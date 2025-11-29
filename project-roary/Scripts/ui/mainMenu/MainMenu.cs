using Godot;
using System;

public partial class MainMenu : Control
{

    public Button  playContinue;
    public Button Options;
    public Button Exit;

    public Eventbus eventbus;
    public SceneManager sceneManager;
    public SaveManager saveManager;

    private bool hasSavedFile = false;

    public override void _Ready()
    {
        playContinue = GetNode<Button>("buttons/playContinue");
        Options = GetNode<Button>("buttons/Options");
        Exit = GetNode<Button>("buttons/Exit");
        eventbus = GetNode<Eventbus>("/root/Eventbus");
        sceneManager = GetNode<SceneManager>("/root/SceneManager");
        saveManager = GetNode<SaveManager>("/root/SaveManager");

        checkForSaveFile();

        playContinue.Pressed += onPlayContinuePressed;
        Options.Pressed += onOptionsPressed;
        Exit.Pressed += onExitPressed;
    }

    void checkForSaveFile()
    {
        hasSavedFile = saveManager.SaveFileExists();

        if (hasSavedFile)
        {
            playContinue.Text = "Continue";
        }
        else
        {
            playContinue.Text = "New Game";
        }
    }

    void onPlayContinuePressed()
    {
        if (hasSavedFile)
        {
            //load file
            eventbus.EmitSignal("load");
        }
        else
        {
            StartNewGame();
        }
    }

    void StartNewGame()
    {
        string firstScenePath = "res://Scenes/map/ParkingGarage/ParkingGarage.tscn";

        if (!ResourceLoader.Exists(firstScenePath))
        {
            GD.PrintErr($"First scene not found: {firstScenePath}");
            return;
        }
        saveManager.CreateNewSave(firstScenePath);
        sceneManager.goToScene(this, firstScenePath, false);
    }

    void onOptionsPressed()
    {
        //do later
        GD.Print("open options scene!");
    }

    void onExitPressed()
    {
        //bye bye
        GetTree().Quit();
    }

    public override void _ExitTree()
    {
        playContinue.Pressed -= onPlayContinuePressed;
        Options.Pressed -= onOptionsPressed;
        Exit.Pressed -= onExitPressed;
    }


}
