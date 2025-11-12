using Godot;
using System;


[GlobalClass]
public partial class MetaData : Resource
{
    //player main metaData
    [Export] public int Money;

    //dialogue flags pls make these booleans
    //ex: [Export] public bool TalkedToWiseTurtleAboutBrother = false; //then later when he does switch this to true so when he goes ot talk to him again, it uses a diff set of speech.
}
