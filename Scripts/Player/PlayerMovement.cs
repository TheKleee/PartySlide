using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MEC;

public enum Lane
{
    red,
    green,
    blue
}
public class PlayerMovement : MonoBehaviour
{
    #region FBI open up! >:O
    private bool end;
    private Rigidbody rb;
    private bool startedMoving;
    [HideInInspector] public PlayerController pCont;     //Player controller is right here sir! :|
    [HideInInspector] public Camera cam;
    private UiController uiCont;

    [SerializeField] private Lane lane;
    private bool toBlueLane;            //Just changed the lane to the blue one :/
    private bool toGreenLane;           //Same as above but green :|
    private bool inLane = true;         //Am IIIIIIIII in a LAAAAANEEEE OR NOOOT?? >:O
    private Vector3 saveLastPos = Vector3.zero;

    private GameObject oldBody;         //Destroy this when creating a new one!!!
    #endregion

    [Header("--- Exposed Data: ---")]
    public Vector3 mapBlueRotation = new Vector3(0, 0, 20f);
    public Vector3 mapGreenRotation = new Vector3(0, 0, 340f);
    [Space]
    public Vector3 bodyBlueRotation = new Vector3(0, 0, 335f);
    public Vector3 bodyGreenRotation = new Vector3(0, 0, 25f);
    [Space]
    public Vector3 positionBlue = new Vector3(-3.86f, 0.16f, 0);    //Might need to delete this later...
    public Vector3 positionGreen = new Vector3(3.86f, 0.16f, 0);    //Idk... xD

    [Header("Green and Blue lane detection:")]
    public float xRange = 2.3f;    //Detect when you are leaving the red field...

    [Header("Speed:")]
    public float speed;

    [Header("Joystic:")]
    public Joystick joystic;

    [Header("The Visual Mesh:")]
    //Set Body From SkinsManager.cs
    public GameObject[] body = new GameObject[6];       //This will be rotated in various ways to create many cool effects!!! >:D
    [HideInInspector] public int bodyId;                //Stored in bodyUnlocked
    public List<int> bodyUnlocked = new List<int>();    //From 0 to 5
    private PlayerInfo info;

    [Header("Map:")]
    public GameObject map;          //Map will also rotate... who'd guess C:<
    private Rigidbody board;        //Drop it in Phase Two!!! >:D

    [Header("Trail:")]
    public FollowPlayer trail;
    private Animator anim;

    [Header("Red Balloon Special:")]
    public float redBalloonSpeed = 2;   //From 1 to 3

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        pCont = GetComponentInParent<PlayerController>();
        if (SaveData.instance != null)
        {
            bodyUnlocked = SaveData.instance.bodyUnlocked;
            bodyId = SaveData.instance.bodyId;
            CheckSkins();   //Call this whenever you are changing the skin -.-
        }
        uiCont = pCont.GetComponent<UiController>();
        pCont.pMove = this;
        cam = FindObjectOfType<Camera>();
    }

    public void CheckSkins()
    {
        if (oldBody != null)
            Destroy(oldBody);   //We !NO! need !!OLD!! and !!!FRAIL!!! >:C

        GameObject skin = Instantiate(body[bodyId], transform, false);  //This needs to be tested... I've never set parent from Instante() before >:O
        skin.transform.localPosition = new Vector3(0, skin.transform.localPosition.y, 0);
        oldBody = skin;
        info = skin.GetComponent<PlayerInfo>();
        anim = skin.GetComponent<Animator>();

        board = info.board;
        trail.target = board.transform.GetChild(1).transform.GetChild(0).transform;
        pCont.balloons = info.balloons;
    }

    private void Update()
    {
        if (!end)
        {
            if (!startedMoving)
            {
                //Check for joystic input!!! => startedMoving = false;
                if (joystic.Horizontal != 0)
                {
                    startedMoving = true;
                    pCont.phase = Phase.p1;
                    pCont.phaseMove = true;

                    pCont.Red();
                    pCont.Deflate();
                    uiCont.Progress();
                    EditorManager.instance.SetEditorStats();
                }
            }
            else
            {
                if (pCont.phase == Phase.p1 || pCont.phase == Phase.mid)
                {
                    switch (lane)
                    {
                        case Lane.red:
                            //Move left/right until specific x position...
                            if (inLane)
                            {
                                if (joystic.Horizontal > 0)
                                {
                                    //Move right :]
                                    transform.Translate(transform.right * Time.deltaTime * speed);

                                    //Off to the green you go >:D
                                    if (transform.localPosition.x >= xRange)
                                    {
                                        if (pCont.phase == Phase.p1)
                                        {
                                            lane = Lane.green;
                                            toGreenLane = true;
                                            inLane = false; 
                                        } else
                                            transform.Translate(transform.right * Time.deltaTime * -speed);
                                    }
                                }
                                else if (joystic.Horizontal < 0)
                                {
                                    //Move left C:
                                    transform.Translate(-transform.right * Time.deltaTime * speed);

                                    //Get to the blue lane! >:O
                                    if (transform.localPosition.x <= -xRange)
                                    {
                                        if (pCont.phase == Phase.p1)
                                        {
                                            lane = Lane.blue;
                                            toBlueLane = true;
                                            inLane = false;
                                        } else
                                            transform.Translate(transform.right * Time.deltaTime * speed);
                                    }
                                }
                                //Visual Mesh Rotation:
                                oldBody.transform.localEulerAngles = new Vector3(0, 0, -joystic.Horizontal * 20f); //Works like a charm!
                            }
                            break;

                        case Lane.green:
                            //You can only move left to get back to the red lane
                            if (toGreenLane)
                            {
                                toGreenLane = false;
                                ChangeToGreen();
                            }
                            if (inLane)
                            {
                                //Allow only to move back to the red lane >:)
                                if (joystic.Horizontal < 0)
                                {
                                    //Move back to the red lane...
                                    ChangeToRed();
                                }
                            }
                            break;

                        case Lane.blue:
                            //Same as green but if you move right this time -.-
                            if (toBlueLane)
                            {
                                toBlueLane = false;
                                ChangeToBlue();
                            }
                            if (inLane)
                            {
                                //Allow only to move back to the red lane C:<
                                if (joystic.Horizontal > 0)
                                {
                                    //Move back to the red lane... Again! >:O
                                    ChangeToRed();
                                }
                            }
                            break;
                    }
                }

                else if (pCont.phase == Phase.p2)
                {
                    #region PC:
                    if (Input.GetMouseButtonDown(0))
                    {
                        pCont.GreenBalloon();
                        pCont.BlueBalloon();
                    }
                    #endregion

                    #region Mobile:
                    if (Input.touches.Length > 0 && Input.touches[0].phase == TouchPhase.Began)
                    {
                        pCont.GreenBalloon();
                        pCont.BlueBalloon();
                    }
                    #endregion
                }
            }
        }
    }

    #region Change Lanes:
    private void ChangeToBlue()
    {
        //Smooth transition to the blue lane :]
        Timing.RunCoroutine(_Blue().CancelWith(gameObject));
    }

    IEnumerator<float> _Blue()
    {
        saveLastPos = transform.localPosition;
        saveLastPos.x -= .1f;

        //Set Map First:
        LeanTween.rotate(map, mapBlueRotation, .15f);
        oldBody.transform.localEulerAngles = bodyBlueRotation;

        float swapSpeed = 10f;
        do
        {
            //Player:
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, positionBlue, 20 * Time.deltaTime);

            swapSpeed -= 1.5f;
            yield return Timing.WaitForSeconds(.001f);
        } while (swapSpeed > 0);


        //map.transform.eulerAngles = mapBlueRotation;
        transform.localPosition = positionBlue;

        inLane = true;
    }

    private void ChangeToGreen()
    {
        //Smooth transition to the green lane :
        Timing.RunCoroutine(_Green().CancelWith(gameObject));
    }

    IEnumerator<float> _Green()
    {
        saveLastPos = transform.localPosition;
        saveLastPos.x -= .1f;

        //Set Map First:
        LeanTween.rotate(map, mapGreenRotation, .15f);
        oldBody.transform.localEulerAngles = bodyGreenRotation;

        float swapSpeed = 10f;
        do
        {
            //Player:
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, positionGreen, 20 * Time.deltaTime);

            swapSpeed -= 1.5f;
            yield return Timing.WaitForSeconds(.001f);
        } while (swapSpeed > 0);


        //map.transform.eulerAngles = mapGreenRotation;
        transform.localPosition = positionGreen;
        inLane = true;
    }

    private void ChangeToRed()
    {
        //Smooth transition to the red lane >: ]
        oldBody.transform.localEulerAngles = Vector3.zero;
        //Map:
        LeanTween.rotate(map, Vector3.zero, .15f);

        transform.localPosition = saveLastPos;

        lane = Lane.red;
        inLane = true;
    }
    #endregion

    #region Phases:
    /// <summary>
    /// This is called on trigger enter!!!
    /// </summary>
    public void PhaseTwo()
    {
        inLane = false;
        //Parent Height!!!
        LeanTween.moveY(pCont.gameObject, 20 + redBalloonSpeed * pCont.redBalloonCount, 1f);  //Test...
        Timing.RunCoroutine(_CallRed().CancelWith(gameObject));

        //Move to center!!! >:O
        LeanTween.moveX(gameObject, 0, .25f);
        LeanTween.rotate(oldBody, Vector3.zero, .5f).setEaseOutBack();
        //Move the camera to phase 2 camera position! Everything else should remain the same!
        //Rotate player to the Vector3.zero rotation => LeanTween!
        //Use the balloon abilities
        Timing.RunCoroutine(_SetPhaseTwo().CancelWith(board.gameObject));
        
    }
    IEnumerator<float> _CallRed()
    {
        int countRed = pCont.redBalloonCount;  //Save the initial red balloon count value
        do
        {
            pCont.RedBalloon();
            yield return Timing.WaitForSeconds(1f / countRed);
        } while (pCont.redBalloonCount > 0);
    }
    IEnumerator<float> _SetPhaseTwo()
    {
        PhaseTwoCamera();

        pCont.phase = Phase.p2;

        anim.SetBool("Float", true);

        //Testing board stuff : D
        yield return Timing.WaitForSeconds(.25f);
        rb.isKinematic = false;

        board.transform.parent = null;
        board.useGravity = true;
        board.constraints = RigidbodyConstraints.None;
        board.AddTorque((-transform.up + transform.forward) * 5, ForceMode.Impulse);


        yield return Timing.WaitForSeconds(.75f);
        Destroy(board.gameObject);
    }

    public void PhaseMid()
    {
        rb.isKinematic = false;
        if(lane != Lane.red)
            ChangeToRed();

        //Parent Height!!!
        LeanTween.moveY(pCont.gameObject, 15 + 1.5f * pCont.redBalloonCount, 1f);  //Test...

        //Move to center!!! >:O
        LeanTween.moveX(gameObject, 0, .25f);
        LeanTween.rotate(oldBody, Vector3.zero, .5f).setEaseOutBack();
        //Move the camera to phase 2 camera position! Everything else should remain the same!
        PhaseTwoCamera();
        //Rotate player to the Vector3.zero rotation => LeanTween!
        //Use the balloon abilities
        pCont.phase = Phase.mid;
    }
    public void EndPhase(bool won)
    {
        //Get out of your update functions!!! >:O
        end = true;
        pCont.phaseMove = false;
        pCont.phase = Phase.end;

        PhaseEndCamera();
        //Call an animation based on if you won or lost...
        if (won)
        {
            anim.SetBool("Won", true);
        } else {
            uiCont.Lost();
            anim.SetBool("Failed", true);
        }
    }
    #endregion

    #region test... so delete later! >:O 
    private void OnCollisionEnter(Collision test)
    {
        if (pCont.phase == Phase.p2)
        {
            rb.angularVelocity = Vector3.zero;
            rb.velocity = Vector3.zero;
            rb.isKinematic = true;

            pCont.phase = Phase.end;    //Ye ye ye! C:<
            anim.SetBool("Won", true);  //Test.. >:O

            pCont.Win();
        }
        else if(pCont.phase == Phase.mid)
        {
            //This needs to be tested!!! :|
            pCont.phase = Phase.p1;
            //anim.SetBool("Float", false);

            rb.angularVelocity = Vector3.zero;
            rb.velocity = Vector3.zero;
            rb.isKinematic = true;

            //Test... :\
            //pCont.transform.position = new Vector3(0, Mathf.Ceil(pCont.transform.position.y - .2f), pCont.transform.position.z);
            transform.localPosition = new Vector3(transform.localPosition.x, 0.5f, 0);

            PhaseOneCamera();
        }
    }

    #endregion
    [Header("Player Confetti:")]
    public GameObject confetti;
    public void CallConfetti()
    {
        confetti.SetActive(true);
    }

    #region Camera Controller:
    private void PhaseTwoCamera()
    {
        LeanTween.moveLocalY(cam.gameObject, 15, 1f);
        LeanTween.rotateX(cam.gameObject, 30, 1f);
    }

    private void PhaseOneCamera()
    {
        LeanTween.moveLocalY(cam.gameObject, 7, 1f);
        LeanTween.rotateX(cam.gameObject, 12, 1f);
    }

    private void PhaseEndCamera()
    {
        LeanTween.moveLocalY(cam.gameObject, 7, 1f);
        LeanTween.rotateX(cam.gameObject, 12, 1f);

        GameWon(transform);
    }

    public void GameWon(Transform target)
    {
        Timing.RunCoroutine(_SetTarget(target).CancelWith(gameObject));
    }

    IEnumerator<float> _SetTarget(Transform t)
    {
        yield return Timing.WaitForSeconds(.25f);
        while (t != null)
        {
            if (cam.fieldOfView < 90)
                cam.fieldOfView += .05f;

            cam.transform.RotateAround(t.position, Vector3.up, -35 * Time.deltaTime);
            yield return Timing.WaitForSeconds(0);
        }
    }
    #endregion
}
