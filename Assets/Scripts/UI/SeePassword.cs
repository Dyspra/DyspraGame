using UnityEngine;
using UnityEngine.UI;

public class SeePassword : MonoBehaviour
{
    [SerializeField] InputField password;

    public void SwitchPasswordSeenStatus()
    {
        password.contentType = (password.contentType != InputField.ContentType.Password) ?
            InputField.ContentType.Password :
            InputField.ContentType.Standard;
        password.Select();
    }
}
