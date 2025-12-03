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
        GD.Print($"{parent.Name} took {dmg} damage, from {dmgDealer.Name},remaining health: {targetData.Health}");

        if (targetData.Health <= 0 && IsInstanceValid(parent))
        {
            if (Owner is Enemy enemy) { enemy.Die(); }
            else if(Owner is Player player) { player.Die(); }
        }
    }

    public async void flash()
    {
        float flashDuration = 0.15f;
        AnimatedSprite2D sprite = parent.GetNodeOrNull<AnimatedSprite2D>("AnimatedSprite2D");
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
