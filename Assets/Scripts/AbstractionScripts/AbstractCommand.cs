using UnityEngine;

namespace Dyspra
{
    public abstract class AbstractCommand
    {
        abstract public void Execute(GameObject actor);
    }
}
