public class Command_Teleport : Dyspra.AbstractCommand
{
    public override void Execute(UnityEngine.GameObject actor)
    {
        Player player = actor.GetComponent<Player>();
        if (player != null)
            player.Teleport();
    }
}
