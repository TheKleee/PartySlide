using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MEC;

public class EditorManager : MonoBehaviour
{
    #region Singleton:
    public static EditorManager instance;

    private void Awake()
    {
        instance = this;
    }
    #endregion

    [Header("Data:")]
    public PlayerController pCont;
    public PlayerMovement pMove;

    [Header("On Ground:")]
    public float maxSpeed; //Range From 40 to 55
    public float speed; //Range from 5 to 15

    [Header("In Air:")]
    public float redBalloonSpeed;
    public float greenBalloonSpeed;
    public float blueBalloonSpeed;

    [Header("Sliders")]
    public Slider[] sliders = new Slider[5];
    public Text[] numbers = new Text[5];

    private void Start()
    {
        Timing.RunCoroutine(_SetStats().CancelWith(gameObject));
    }
    IEnumerator<float> _SetStats()
    {
        yield return 0;

        //On Ground:
        maxSpeed = sliders[0].value = pCont.maxSpeed;
    }

    public void SetEditorStats()
    {
        pCont.maxSpeed = maxSpeed;
        pMove.speed = speed;


        pMove.redBalloonSpeed = redBalloonSpeed;
        pCont.greenBalloonSpeed = greenBalloonSpeed;
        pCont.blueBalloonSpeed = blueBalloonSpeed;
    }

    #region Editor Manager:
    [Header("Editor Manager:")]
    public GameObject editorManager;

    public void ActiveEM()
    {
        editorManager.SetActive(!editorManager.activeSelf);

        if (editorManager.activeSelf)
        {
            //Set the sliders:
            maxSpeed = sliders[0].value;
            numbers[0].text = maxSpeed.ToString();

            speed = sliders[1].value;
            numbers[1].text = speed.ToString();

            redBalloonSpeed = sliders[2].value;
            numbers[2].text = (Mathf.Round(redBalloonSpeed * 10) / 10).ToString();

            greenBalloonSpeed = sliders[3].value;
            numbers[3].text = (Mathf.Round(greenBalloonSpeed * 10) / 10).ToString();

            blueBalloonSpeed = sliders[4].value;
            numbers[4].text = (Mathf.Round(blueBalloonSpeed * 10) / 10).ToString();
        }
    }
    public void ExitEM()
    {
        editorManager.SetActive(false);
    }
    #endregion

    #region Sliders:
    //Phase 1:
    public void MaxSlider()
    {
        maxSpeed = sliders[0].value;
        numbers[0].text = maxSpeed.ToString();
    }

    public void SpeedSlider()
    {
        speed = sliders[1].value;
        numbers[1].text = speed.ToString();

    }
    //Phase 2:
    public void RedSlider()
    {
        redBalloonSpeed = sliders[2].value;
        numbers[2].text = (Mathf.Round(redBalloonSpeed * 10) / 10).ToString();

    }
    public void GreenSlider()
    {
        greenBalloonSpeed = sliders[3].value;
        numbers[3].text = (Mathf.Round(greenBalloonSpeed * 10) / 10).ToString();

    }
    public void BlueSlider()
    {
        blueBalloonSpeed = sliders[4].value;
        numbers[4].text = (Mathf.Round(blueBalloonSpeed * 10) / 10).ToString();
    }
    #endregion
}
