using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MEC;

public enum Phase
{
    start,
    p1,
    mid,
    p2,
    end
}
public class PlayerController : MonoBehaviour
{
    #region 18+ >:D
    //Balloons:
    [HideInInspector] public int redBalloonCount;
    [HideInInspector] public int blueBalloonCount;
    [HideInInspector] public int greenBalloonCount;
    [HideInInspector] public PlayerMovement pMove;
    private Camera cam;
    private bool mapStart;

    //Start Dur:
    private float startRedDur;
    private float startGreenDur;
    private float startBlueDur;

    //Scale:
    private Vector3 redScale;
    private Vector3 greenScale;
    private Vector3 blueScale;

    //Can Move or not:
    [HideInInspector] public bool phaseMove; //Set to false when you win/lose the level!!!
    #endregion

    [Header("Max Map Range:")]
    public float maxMapRange = 485f;        //Max z range...

    [Header("Current Phase:")]
    public Phase phase;   //The Current phase of the game... : /

    [Header("Speed:")]
    [Range(0, 50)] public float minSpeed;  //The Speed that is affected by the balloon change of speed
    [Range(0, 100)]public float maxSpeed;  //The Speed change over time => the terminal velocity
    [Space]
    public float startSpeed;        //This speed is lower than the minSpeed;
    public float curSpeed;          //This is the speed the player will use in order to move forward in both stages :|

    [Header("Balloons:")]
    [Range(0, 10)] public float redBalDur;      //The duration of a red balloon => during the Phase 1
    [Range(0, 10)] public float blueBalDur;
    [Range(0, 10)] public float greenBalDur;
    [HideInInspector] public int balloonCount;  //Total number of balloons... used for checking the win state : \
    [Space]
    [HideInInspector] public GameObject[] balloons = new GameObject[3];
    public GameObject Vfx;  //This will just be a confetti explosion! >: D

    [Header("Balloon Special Speed:")]
    public float greenBalloonSpeed = 3f; //From 2 to 4
    public float blueBalloonSpeed = 1.25f; //From 1 to 2
    private void Start()
    {
        //Setting the speed up:
        minSpeed = Random.Range(25, 40);
        startSpeed = minSpeed - Random.Range(0, 11);
        curSpeed = startSpeed;
        maxSpeed = Random.Range(40, 55);

        startRedDur = redBalDur;
        startGreenDur = greenBalDur;
        startBlueDur = blueBalDur;
    }

    private void FixedUpdate()
    {
        if (phaseMove)
        {
            //Scale Balloons:
            if (balloons[0].activeSelf)
                balloons[0].transform.localScale =
                    Vector3.Lerp(
                        balloons[0].transform.localScale,
                        redScale,
                        .1f
                    );  //Red...

            if (balloons[1].activeSelf)
                balloons[1].transform.localScale =
                    Vector3.Lerp(
                        balloons[1].transform.localScale,
                        greenScale,
                        .1f
                    );  //Green...

            if (balloons[2].activeSelf)
                balloons[2].transform.localScale =
                    Vector3.Lerp(
                        balloons[2].transform.localScale,
                        blueScale,
                        .1f
                    );  //Blue...

            //Phase 1:
            if (phase == Phase.p1)
            {
                //Run forward...
                //Pick up the coins and balloons...
                transform.Translate(transform.forward * curSpeed * Time.deltaTime);
                if (curSpeed < maxSpeed)
                {
                    curSpeed += .25f;
                    if (curSpeed > maxSpeed)
                        curSpeed = maxSpeed;
                }
            }

            //Mid Phase:
            else if (phase == Phase.mid)
            {
                transform.Translate((transform.forward * 1.5f *  (2 * blueBalloonCount + maxSpeed / 1.15f) * Time.deltaTime) / 2);
                //Kinda makes it like there's a max of 10 balloons here :\
                float descent = 25 - 3 * greenBalloonCount;
                if (descent <= 0)
                    descent = 1;
                transform.Translate(-transform.up * descent * Time.deltaTime);
            }

            //Phase 2:
            else if (phase == Phase.p2)
            {
                if (transform.position.z < maxMapRange)
                {
                    transform.Translate(transform.forward * blueBalloonSpeed * (2 * blueBalloonCount + maxSpeed / 1.15f) * Time.deltaTime);
                    //Kinda makes it like there's a max of 10 balloons here :\
                    float descent = 20 - greenBalloonSpeed * greenBalloonCount;
                    if (descent <= 0)
                        descent = 1;
                    transform.Translate(-transform.up * descent * Time.deltaTime);
                } else {
                    //pop the balloons!
                    if (greenBalloonCount > 0 || blueBalloonCount > 0)
                        RemoveBalloons();

                    transform.Translate(-transform.up * 20 * Time.deltaTime);
                }
            }
        }
    }
    public void RemoveBalloons()
    {

        if (balloons[1].activeSelf)
        {
            balloons[1].SetActive(false);
            GameObject conf = Instantiate(Vfx, balloons[1].transform.position, Vfx.transform.rotation);
            conf.transform.SetParent(transform);
        }

        if (balloons[2].activeSelf)
        {
            balloons[2].SetActive(false);
            GameObject conf2 = Instantiate(Vfx, balloons[2].transform.position, Vfx.transform.rotation);
            conf2.transform.SetParent(transform);
        }

        greenBalloonCount = blueBalloonCount = 0;
    }
    public void LevelStart()
    {
        //Change the UI, Start the level, rotate the player towards a position using a coroutine, drop the hight down to (y = 0)
    }

    #region Call these from PlayerMovement.cs
    public void Lose()
    {
        phaseMove = false;
        pMove.EndPhase(false);
        //Do not update the ui... and don't save the progress :S
    }

    public void Win()
    {
        phaseMove = false;
        pMove.EndPhase(true);
        //update the ui... + save prog : |
    }
    #endregion

    #region Balloon Effects:
    //Phase 1:
    public void SlowDown()
    {
        //Pick up a balloon of any color to slow down a bit...
        if (curSpeed > minSpeed)
            curSpeed -= 20f;
        else
            curSpeed = minSpeed;
    }
    //Red:
    public void Red()
    {
        if (mapStart)
        {
            GameObject conf = Instantiate(Vfx, balloons[0].transform.position, Vfx.transform.rotation);
            conf.transform.SetParent(transform);
        }
        mapStart = true;

        redBalloonCount++;
        if(redBalloonCount == 1)
        {
            //Call vfx
            balloons[0].SetActive(true);
        }

        redBalDur += startRedDur;
        Timing.RunCoroutine(_RedDur().CancelWith(gameObject));
    }
    IEnumerator<float> _RedDur()
    {
        //LeanTween.scale(balloons[0], redScale, redBalDur);
        yield return Timing.WaitForSeconds(redBalDur);
        redBalloonCount--;

        if (redBalloonCount == 0)
        {
            //Call vfx
            balloons[0].SetActive(false);

            GameObject conf = Instantiate(Vfx, balloons[0].transform.position, Vfx.transform.rotation);
            conf.transform.SetParent(transform);
        }
    }
    //Green:
    public void Green()
    {
        GameObject conf = Instantiate(Vfx, balloons[1].transform.position, Vfx.transform.rotation);
        conf.transform.SetParent(transform);

        greenBalloonCount++;
        if (greenBalloonCount == 1)
        {
            //Call vfx
            balloons[1].SetActive(true);
        }

        greenBalDur += startGreenDur;
        Timing.RunCoroutine(_GreenDur().CancelWith(gameObject));
    }
    IEnumerator<float> _GreenDur()
    {
        //LeanTween.scale(balloons[0], redScale, redBalDur);
        yield return Timing.WaitForSeconds(greenBalDur);
        greenBalloonCount--;

        if (greenBalloonCount == 0)
        {
            //Call vfx

            if (balloons[1].activeSelf)
            {
                GameObject conf = Instantiate(Vfx, balloons[1].transform.position, Vfx.transform.rotation);
                conf.transform.SetParent(transform);
            }

            balloons[1].SetActive(false);
        }
    }
    //Blue:
    public void Blue()
    {
        GameObject conf = Instantiate(Vfx, balloons[2].transform.position, Vfx.transform.rotation);
        conf.transform.SetParent(transform);

        blueBalloonCount++;
        if (blueBalloonCount == 1)
        {
            //Call vfx
            balloons[2].SetActive(true);
        }

        blueBalDur += startBlueDur;
        Timing.RunCoroutine(_BlueDur().CancelWith(gameObject));
    }
    IEnumerator<float> _BlueDur()
    {
        //LeanTween.scale(balloons[0], redScale, redBalDur);
        yield return Timing.WaitForSeconds(blueBalDur);
        blueBalloonCount--;

        if (blueBalloonCount == 0)
        {
            //Call vfx


            if (balloons[2].activeSelf)
            {
                GameObject conf = Instantiate(Vfx, balloons[2].transform.position, Vfx.transform.rotation);
                conf.transform.SetParent(transform);
            }

            balloons[2].SetActive(false);
        }
    }


    //Phase 2:
    public void RedBalloon()
    {
        //Rise up!
        //Deflate the red balloon!
        redBalDur = 1f;
        redBalloonCount--;

        if (redBalloonCount == 0)
        {
            balloons[0].SetActive(false);
            GameObject conf = Instantiate(Vfx, balloons[0].transform.position, Vfx.transform.rotation);
            conf.transform.SetParent(transform);
        }
    }
    public void GreenBalloon()
    {
        if (greenBalloonCount >= 0)
        {
            //Forward Speed!
            greenBalloonCount--;

            if (greenBalloonCount == 0)
            {
                balloons[1].SetActive(false);
                GameObject conf = Instantiate(Vfx, balloons[1].transform.position, Vfx.transform.rotation);
                conf.transform.SetParent(transform);
            }
        }
    }
    public void BlueBalloon()
    {
        if (blueBalloonCount >= 0)
        {
            //Descent Speed decrease... so these are filled with helium or something >:\
            blueBalloonCount--;

            if (blueBalloonCount == 0)
            {
                balloons[2].SetActive(false);
                GameObject conf = Instantiate(Vfx, balloons[2].transform.position, Vfx.transform.rotation);
                conf.transform.SetParent(transform);
            }
        }
    }

    //Phase 1 and 2:
    public void Deflate()
    {
        //Run this all the time...
        //Make sure the camera FOV is connected with this function in a way! xD
        cam = pMove.cam;
        balloonCount = redBalloonCount + greenBalloonCount + blueBalloonCount;
        Timing.RunCoroutine(_Deflating().CancelWith(gameObject));
    }

    IEnumerator<float> _Deflating()
    {
        while (balloonCount > 0)
        {
            balloonCount = redBalloonCount + greenBalloonCount + blueBalloonCount;

            if (redBalloonCount > 0)
                redScale = new Vector3(redBalloonCount, redBalloonCount, redBalloonCount) / 3f;

            if (greenBalloonCount > 0)
                greenScale = new Vector3(greenBalloonCount, greenBalloonCount, greenBalloonCount) / 3f;

            if (blueBalloonCount > 0)
                blueScale = new Vector3(blueBalloonCount, blueBalloonCount, blueBalloonCount) / 3f;

            #region Camera Controll:
            //Position:
            float desiredPos = cam.transform.localPosition.y + balloonCount / 7;
            if (cam.transform.localPosition.y < desiredPos)
                cam.transform.localPosition += new Vector3(0, .01f, -.005f);
            else if(cam.transform.localPosition.y > desiredPos)
                cam.transform.localPosition -= new Vector3(0, .01f, -.005f);
            //FOV:
            float desiredFOV = 70 + balloonCount * 1.5f;
            if (cam.fieldOfView < desiredFOV)
                cam.fieldOfView += .05f;
            else if (cam.fieldOfView > desiredFOV)
                cam.fieldOfView -= .05f;
            #endregion

            yield return 0;
        }

        if (phase == Phase.p1)
            Lose();
    }
    #endregion
}
