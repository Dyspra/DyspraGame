using UnityEngine;

namespace Dyspra
{

    /// <summary>
    /// A command is an action. It can be bind to an input handler
    /// </summary>
    public abstract class AbstractCommand
    {
        abstract public void Execute(GameObject actor);
    }
}
