using Godot;
using System;
using System.Threading.Tasks;

public partial class HurtBox : Area2D
{
    public Eventbus eventbus;
    public Node parent;

    public override void _EnterTree()
    {
        eventbus = GetNode<Eventbus>("/root/Eventbus");
        eventbus.applyDamage += onApplyDmg;
    }

    public override void _Ready()
    {
        AddToGroup("hurtbox");

        parent = GetParent();

        if (!(parent is Player || parent is Enemy))
        {
            GD.Print("Hurtbox parent is not a 'Character' type!");
        }
    }

    public void onApplyDmg(Node dmgReceiver, Node dmgDealer, int dmg)
    {
        if (parent == null || parent != dmgReceiver) return;

        if (parent.IsInGroup("enemy") && dmgDealer.IsInGroup("enemy")) return;

        GenericData targetData = null;
        if (parent is Player p) targetData = p.data;
        else if (parent is Enemy e) targetData = e.data;

        if (targetData == null) return;

        flash(); //hitflash

        targetData.Health -= dmg;
        if (parent.IsInGroup("player"))
        {
            eventbus.EmitSignal("updateHealth", targetData.Health);
        }
        else if (parent.IsInGroup("enemy"))
        {
            eventbus.EmitSignal("updateBossHealth", targetData.Health);
        }
        GD.Print($"{parent.Name} took {dmg} damage, from {dmgDealer.Name},remaining health: {targetData.Health}");

        if (targetData.Health <= 0 && IsInstanceValid(parent))
        {

            ResetFlash();

            if (Owner is Enemy enemy) { enemy.Die(); }
            else if(Owner is Player player) { player.Die(); }
        }
    }

    void ResetFlash()
    {
        CanvasItem sprite = parent.GetNodeOrNull<AnimatedSprite2D>("AnimatedSprite2D");
        if(sprite == null ) sprite = parent.GetNodeOrNull<Sprite2D>("Sprite2D");

        if (sprite?.Material is ShaderMaterial shader)
        {
            shader.SetShaderParameter("flash", false);
        }
    }

    public async void flash()
    {
        float flashDuration = 0.15f;
        CanvasItem sprite = parent.GetNodeOrNull<AnimatedSprite2D>("AnimatedSprite2D");
        if (sprite == null) sprite = parent.GetNodeOrNull<Sprite2D>("Sprite2D");

        if (sprite?.Material is ShaderMaterial shader)
        {
            shader.SetShaderParameter("flash", true);

            GetTree().CreateTween().TweenCallback(Callable.From(() =>
            {
                if (IsInstanceValid(sprite) && sprite.Material is ShaderMaterial s)
                {
                    s.SetShaderParameter("flash", false);
                }
            })).SetDelay(flashDuration);
        }
    }

    public override void _ExitTree()
    {
        eventbus.applyDamage -= onApplyDmg;
    }

}
