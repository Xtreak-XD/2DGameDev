using Godot;
using System;

public partial class CarStats : Resource
{
	[Export]
	public float TopSpeed { get; set; }
	[Export]
	public float Acceleration { get; set; }
	[Export]
	public float SteeringSpeed { get; set; }
}
