using Godot;
using Godot.Collections;
using System;

[GlobalClass]
public partial class ShopResource : Resource
{
    [Export] public string ShopName;
    [Export] public Array<IndividualItem> Items;
    [Export] public Texture2D ShopKeeperPortrait;
    [Export] public string GreetingMessage;
}
