using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class NpcQuestMarker : MonoBehaviour
{
    [SerializeField] private Image exclamationImg;
    [SerializeField] private Image questionImg;

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
        exclamationImg.gameObject.SetActive(true);
        questionImg.gameObject.SetActive(false);
    }

    public void ShowQuestionMark()
    {
        exclamationImg.gameObject.SetActive(false);
        questionImg.gameObject.SetActive(true);
    }

    public void HideAllImages()
    {
        exclamationImg.gameObject.SetActive(false);
        questionImg.gameObject.SetActive(false);

    }
}
