using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class NpcQuestMarker : MonoBehaviour
{
    [SerializeField] private Image[] exclamationImg;
    [SerializeField] private Image[] questionImg;

    private Camera _cam;
    // Start is called before the first frame update
    void Start()
    {
        _cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.LookRotation(transform.position - _cam.transform.position);
    }

    public void ShowExclamation()
    {
        foreach (var img in exclamationImg)
        {
            img.gameObject.SetActive(true);
        }

        foreach (var img in questionImg)
        {
            img.gameObject.SetActive(false);
        }
    }

    public void ShowQuestionMark()
    {
        foreach (var img in exclamationImg)
        {
            img.gameObject.SetActive(false);
        }

        foreach (var img in questionImg)
        {
            img.gameObject.SetActive(true);
        }
    }

    public void HideAllImages()
    {
        foreach (var img in exclamationImg)
        {
            img.gameObject.SetActive(false);
        }

        foreach (var img in questionImg)
        {
            img.gameObject.SetActive(false);
        }

    }
}
