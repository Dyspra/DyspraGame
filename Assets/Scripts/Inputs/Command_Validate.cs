public class Command_Validate : Dyspra.AbstractCommand
{
    public override void Execute(UnityEngine.GameObject actor)
    {
        Player player = actor.GetComponent<Player>();
        if (player != null)
            player.ValidateInMenu();
    }
}
