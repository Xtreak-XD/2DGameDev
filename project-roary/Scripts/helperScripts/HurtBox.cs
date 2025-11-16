using Godot;
using System;
using System.Threading.Tasks;

public partial class HurtBox : Area2D
{
    public Eventbus eventbus;
    public GenericData data;
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

        if (parent is Player playerParent)
        {
            if (playerParent.data != null)
            {
                data = playerParent.data;
                GD.Print($"connected hurtbox to {parent.Name}");
            }
            else
            {
                GD.PushWarning("Parent's 'data' property is null");
            }
        }
        else if (parent is Enemy enemyParent)
        {
            if (enemyParent.data != null)
            {
                data = enemyParent.data;
                GD.Print($"connected hurtbox to {parent.Name}");
            }
            else
            {
                GD.PushWarning("Parent's 'data' property is null");
            }
        }
        else
        {
            GD.Print("Hurtbox parent is not a 'Character' type!");
        }
    }

    public void onApplyDmg(Node dmgReceiver, Node dmgDealer, int dmg)
    {
        if (parent == null || parent != dmgReceiver) return;

        if (parent.IsInGroup("enemy") && dmgDealer.IsInGroup("enemy")) return;

        flash(); //hitflash 
        
        data.Health -= dmg;
        if (parent.IsInGroup("player"))
        {
            eventbus.EmitSignal("updateHealth", data.Health);
        }
        GD.Print($"{parent.Name} took {dmg} damage, from {dmgDealer.Name},remaining health: {data.Health}");

        if (data.Health <= 0 && IsInstanceValid(parent))
        {
            if (Owner is Enemy enemy) { enemy.Die(); }
            else if(Owner is Player player) { GD.Print("player died"); }
        }
    }

    public async Task flash()
    {
        float flashDuration = 0.15f;
        AnimatedSprite2D sprite = parent.GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        if (sprite.Material is ShaderMaterial shader)
        {
            shader.SetShaderParameter("flash", true);
        }
        else{ return; }

        Timer flashT = new Timer();
        flashT.OneShot = true;
        flashT.Autostart = false;
        AddChild(flashT);
        flashT.Start(flashDuration);

        await ToSignal(flashT, Timer.SignalName.Timeout);

        shader.SetShaderParameter("flash", false);
        flashT.QueueFree();
    }

    public override void _ExitTree()
    {
        eventbus.applyDamage -= onApplyDmg;
    }

}
