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
    
    [ExportGroup("UI Textures")]
    [Export] public Texture2D TableTexture;
    [Export] public Texture2D SlotTexture;
    [Export] public Texture2D BannerTexture; 
    
    [ExportGroup("World Textures")]
    [Export] public Texture2D WorldShopSprite;
}
