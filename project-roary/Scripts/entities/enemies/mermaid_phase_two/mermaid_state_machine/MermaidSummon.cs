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
        GD.Print("The mermaid is summoning three starfish");

        for(int i = 0; i < 3; i++)
        {
            Marker2D spawnPoint = ActiveEnemy.GetRandomStarfishSpawn();

            if(!(spawnPoint == null))
            {
                Starfish starfish = (Starfish)ActiveEnemy.starfishEnemy.Instantiate();
                spawnPoint.AddChild(starfish);

                starfish.GlobalPosition = spawnPoint.GlobalPosition;
            }
        }
    }

    public override MermaidState Process(double delta)
    {
        return MermaidChase;
    }
}
