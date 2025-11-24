using Godot;

public partial class SBAttack : Node
{
    [Export] public PackedScene SpiralBubble;  
    [Export] public int Count = 10;            
    [Export] public float AngleStep = 15f;   
    [Export] public float Speed = 1500f;        

    private float currentAngle = 0f;

    public void Fire(Node2D owner)
    {
        if (SpiralBubble == null)
        {
            return;
        }

        for (int i = 0; i < Count; i++)
        {
            float angle = currentAngle + (i * AngleStep);
            Vector2 dir = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));

            var bubble = SpiralBubble.Instantiate<Node2D>();
            bubble.Name = "Bubble";
            owner.GetParent().AddChild(bubble);
            bubble.GlobalPosition = owner.GlobalPosition;

            if (bubble.HasMethod("SetVelocity"))
                bubble.Call("SetVelocity", dir * Speed);
        }

        currentAngle += 1; 
    }
}