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

    public override AlligatorState Process(double delta)
    {
        if (change)
        {
            return AlligatorChase;
        }
        
        Vector2 direction = (ActiveEnemy.target.Position - ActiveEnemy.Position).Normalized();
        ActiveEnemy.Velocity = direction * ActiveEnemy.data.Speed * 50f;
        ActiveEnemy.MoveAndSlide();

        if(ActiveEnemy.IsPlayerInChompkRange())
        {
            GD.Print("Alligator Lunge hit player.");
            change = true;

            ActiveEnemy.Velocity = -direction * ActiveEnemy.data.Speed * 50f;
            ActiveEnemy.MoveAndSlide();
        }

        return null;
    }
}