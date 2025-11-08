using Godot;

public partial class AlligatorLunge : AlligatorState
{
    public AlligatorChase AlligatorChase;
    bool change;

    public override void _Ready()
    {
        AlligatorChase = GetParent().GetNode<AlligatorChase>("AlligatorChase");
        change = false;
    }

    public override void EnterState()
    {
        change = false;
    }

    public override AlligatorState Process(double delta)
    {
        Vector2 direction = ActiveEnemy.target.GlobalPosition - ActiveEnemy.GlobalPosition;
         
        if (change)
        {
            ActiveEnemy.Velocity = -direction * 120;
            ActiveEnemy.MoveAndSlide();

            //GD.Print("Lunge over. Resuming chase.");
            return AlligatorChase;
        }

        ActiveEnemy.Velocity = Vector2.Zero;

        ActiveEnemy.Velocity = direction;
        ActiveEnemy.MoveAndSlide();

        if(ActiveEnemy.IsPlayerInChompRange())
        {
            //GD.Print("Alligator Lunge hit player.");
            // Player needs to take damage

            change = true;
        }

        return null;
    }
}