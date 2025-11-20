using Godot;

public partial class MermaidSummon : MermaidState
{
	public MermaidChase MermaidChase;

 	public override void _Ready()
    {
        MermaidChase = GetParent().GetNode<MermaidChase>("MermaidChase");
    }

    public override void EnterState()
    {
        GD.Print("The mermaid is summoning a starfish");
        // Summon starfish
    }

    public override MermaidState Process(double delta)
    {
        return MermaidChase;
    }
}
