public class Command_Back : Dyspra.AbstractCommand
{
    public override void Execute(UnityEngine.GameObject actor)
    {
        Player player = actor.GetComponent<Player>();
        if (player != null)
            player.BackInMenu();
    }
}
