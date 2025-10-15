using Godot;
using System;
using System.Formats.Asn1;

public partial class LayersAndMasks : Node
{

    public uint GetCollisionLayerByName(string layerName) //Turned into global variable. Delete this if already created
    {
        for (uint i = 1; i <= 32; ++i) // can change 32 to actual number of layers once figured out
        {
            var layer = ProjectSettings.GetSetting("layer_names/2d_physics/layer_" + i).ToString();
            if (layer.Equals(layerName))
            {
                return i;
            }
        }
        GD.PrintErr("Could not find the " + layerName + "collision layer.");
        GD.PrintErr("Make sure to set the name: " + layerName + " under 'project settings _> layer names' for the collision layer");
        return 0;
    }
    
}
