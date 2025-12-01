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
    }

	private void OnChangedScene(String newScene)
	{
		currScene = newScene;
	}
}
