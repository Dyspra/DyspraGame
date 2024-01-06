using System.Collections;
using UnityEngine;

public class LaunchMeduseTutorial : MonoBehaviour
{
    [SerializeField] private GameObject _tutoToEnableGO;

	// Camera traveling
    [SerializeField] private Transform startCameraPos;
	[SerializeField] private float _introCutsceneTime;
	[SerializeField] private GameObject _camToMove;
    [SerializeField] private GameObject _bandeauGO;
    private Vector3 finalCameraPos = new Vector3(0.0f, 0.3f, -0.55f);
    public float speed;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(MoveCameraBeforeTuto());
    }

	private IEnumerator MoveCameraBeforeTuto()
	{
		_camToMove.transform.position = startCameraPos.position;
		_camToMove.transform.rotation = startCameraPos.rotation;
        Vector3 posToGo = new Vector3(0f, 0.3f, -0.55f);
		float timer = _introCutsceneTime;
		while (timer > 0.0f)
		{
			timer -= Time.deltaTime;
			_camToMove.transform.localPosition = Vector3.Lerp(_camToMove.transform.localPosition, posToGo, Time.deltaTime * speed);
			_camToMove.transform.rotation = Quaternion.Lerp(_camToMove.transform.rotation, Quaternion.identity, Time.deltaTime * speed);
			yield return null;
		}
		Destroy(_bandeauGO);

		_tutoToEnableGO.SetActive(true);
		_camToMove.transform.localPosition = finalCameraPos;
		_camToMove.transform.localRotation = Quaternion.identity;
	}
}
