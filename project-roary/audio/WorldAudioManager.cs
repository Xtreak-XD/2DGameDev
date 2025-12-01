using Godot;
using System;

public partial class WorldAudioManager : Node
{
	[Export] public AudioStreamPlayer2D bgMusicPlayer;
	private AudioGlobal audioGlobal;
	public override void _Ready()
    {
		audioGlobal = GetNode<AudioGlobal>("/root/AudioGlobal");

		if (bgMusicPlayer != null && bgMusicPlayer.Stream != null)
        {
            bgMusicPlayer.Play();
        }
	}
}
