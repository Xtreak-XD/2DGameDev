using Godot;

public partial class CoffeeShot : EnemyProjectile
{

    Vector2 dir;
    AnimatedSprite2D anim;
    public override void _Ready()
    {
        dir = Velocity/1000;
        anim = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        selectAnim(dir);
    }

    void selectAnim(Vector2 dir)
    {
        switch (dir)
        {
            case (0,-1):
                anim.Play("ShotUp");
                break;
            case (0,1):
                anim.Play("ShotDown");
                break;
            case (1,0):
                anim.Play("ShotRight");
                break;
            case (-1,0):
                anim.Play("ShotLeft");
                break;
        }
    }
}
