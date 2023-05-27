using UnityEngine;

public class DeathZone : MonoBehaviour
{
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.GetComponent<IBall>() != null && other.gameObject.GetComponent<RespawnTuto>() == null)
            Destroy(other.gameObject);
    }
}
