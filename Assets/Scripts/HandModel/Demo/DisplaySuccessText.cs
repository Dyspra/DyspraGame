using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplaySuccessText : MonoBehaviour
{
    [SerializeField]
    private GameObject SuccessText;

    [SerializeField]
    private FirstExercice firstExercice;

    [SerializeField]
    private float textSpeed = 1000f;

    private bool textStarted = false;
    private bool textEnd = false;

    private Vector3 textStartPos = new Vector3(-500, 0);

    void Update()
    {
        CheckTextActiveStatus();
    }

    void FixedUpdate()
    {
        if (SuccessText.activeSelf)
        {
            MoveText();
        }
    }

    private void CheckTextActiveStatus()
    {
        if (!SuccessText.activeSelf && firstExercice.isSuccess)
        {
            SuccessText.SetActive(true);
        }
        else if (SuccessText.activeSelf && !firstExercice.isSuccess && !textEnd)
        {
            textEnd = true;
        }
        else if (SuccessText.activeSelf && !firstExercice.isSuccess && SuccessText.transform.localPosition.x >= 500)
        {
            SuccessText.SetActive(false);
            textStarted = false;
            textEnd = false;
        }
    }

    private void MoveText()
    {
        if (textStarted == false)
        {
            SuccessText.transform.localPosition = textStartPos;
            textStarted = true;
        }
        else if (SuccessText.transform.localPosition.x < 0 && !textEnd)
        {
            SuccessText.transform.localPosition += new Vector3(textSpeed * Time.deltaTime, 0);
        }
        else if (SuccessText.transform.localPosition.x < 500 && textEnd)
        {
            SuccessText.transform.localPosition += new Vector3(textSpeed * Time.deltaTime, 0);
        }
    }
}
