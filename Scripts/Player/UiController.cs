using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using MEC;
using TMPro;

[RequireComponent(typeof(PlayerController))]
public class UiController : MonoBehaviour
{
    //Display Points
    //Progress bar
    //Save data at the end
    //Display and hide ui elements based on current phase...
    #region Hidde..n >: D
    [HideInInspector] public int points;    //This might be called from other .cs ... if not just make it private -.-
    [HideInInspector] public int balloons;
    private Camera cam;
    #endregion
    [Header("Is Bonus Level:")]
    public bool bonus;

    [Header("Level:")]
    public int lvl;     //The current active level

    [Header("Phase Ui:")]
    public GameObject phaseStart;
    public GameObject phaseBonusStart;
    public GameObject phaseOne;
    public GameObject phaseTwo;
    public GameObject phaseEnd;
    public GameObject phaseLose;

    [Header("Progress Bar:")]
    public Image progressFill;      //The progress fill amount! :D
    public RectTransform pointer;   //Follow the progress... :/

    [Header("Player and Goal:")]
    public PlayerController player;
    public Transform goal;         //This cannot be empty mate : \

    [Header("Party Parent:")]
    public UiAnimation party;
    public TextMeshProUGUI totalPts;

    [Header("Phase Start UI:")]
    public Image Level;
    public Image LevelNext;
    public Image LevelLast;
    public Image LevelBonus;
    [Space]
    public TextMeshProUGUI number;
    public TextMeshProUGUI number2;
    public TextMeshProUGUI number3;

    [Header("Phase 1:")]
    public TextMeshProUGUI curLvl;
    public UiAnimation bPts;
    public TextMeshProUGUI balloonPts;

    [Header("Phase End:")]
    public TextMeshProUGUI ptsFromlvl;
    public TextMeshProUGUI multiplayer;
    public TextMeshProUGUI sumPts;
    [HideInInspector] public int xNum;

    [Header("Points Icon:")]
    public GameObject pts;
    public GameObject plusOne;
    private void Awake()
    {
        if (player == null) player = GetComponent<PlayerController>();
    }
    private void Start()
    {
        #region Loading Saved Data:
        if(SaveData.instance != null)
        {
            lvl = SaveData.instance.lvl;

            //Points:
            totalPts.text = SaveData.instance.points.ToString();

            //Start UI:
            if (lvl == SaveData.instance.nextTreeLvls[1])
                LevelNext.color = Color.white;

            if (lvl == SaveData.instance.nextTreeLvls[2])
            {
                LevelNext.color = Color.white;
                LevelLast.color = Color.white;
            }

            if (SaveData.instance.changeNumbers)
            {
                SaveData.instance.nextTreeLvls[0] = lvl;
                SaveData.instance.nextTreeLvls[1] = lvl + 1;
                SaveData.instance.nextTreeLvls[2] = lvl + 2;
            }

            number.text = SaveData.instance.nextTreeLvls[0].ToString();
            number2.text = SaveData.instance.nextTreeLvls[1].ToString();
            number3.text = SaveData.instance.nextTreeLvls[2].ToString();

            if (lvl == SaveData.instance.nextTreeLvls[0] &&
                SaveData.instance.changeNumbers &&
                !SaveData.instance.bonusLvl)
                SaveData.instance.changeNumbers = false;

            if (!SaveData.instance.bonusLvl)
            {
                curLvl.text = lvl.ToString();
                phaseStart.SetActive(true);
            }
            else
            {
                curLvl.text = "B";
                phaseBonusStart.SetActive(true);
            }

            SaveData.instance.SaveGame();
        }
        #endregion

        cam = Camera.main;
    }

    #region Points

    #region Main Pts:
    public void AddPts()
    {
        //Party Parent:
        points++;
        party.CallAction();

        totalPts.text = (SaveData.instance.points + points).ToString();
        
        Timing.RunCoroutine(_PtsIcon().CancelWith(gameObject));
    }

    IEnumerator<float> _PtsIcon()
    {
        //Create the object:
        GameObject ptsIcon = Instantiate(pts);
        ptsIcon.GetComponent<RectTransform>().SetParent(party.GetComponent<RectTransform>(), false);
        //Set the size and pos:
        ptsIcon.transform.position = cam.WorldToScreenPoint(player.pMove.transform.position);
        ptsIcon.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
        //Call the actions:
        LeanTween.moveLocal(ptsIcon, new Vector2(50, -2), .5f);
        LeanTween.scale(ptsIcon, new Vector3(1, 1, 1), .5f);

        yield return Timing.WaitForSeconds(1f);
        Destroy(ptsIcon);
    }
    #endregion

    public void BuySkin(int price)
    {
        SaveData.instance.points -= price;

        totalPts.text = (SaveData.instance.points + points).ToString();
        SaveData.instance.SaveGame();
    }

    public void RewardAdPts(int reward)
    {
        SaveData.instance.points += reward;
        totalPts.text = SaveData.instance.points.ToString();
        SaveData.instance.SaveGame();
    }

    #region Balloons:
    public void BonusPts(Transform onePos)
    {
        balloons++;
        bPts.CallAction();

        balloonPts.text = balloons.ToString();

        Timing.RunCoroutine(_PlusOne(onePos).CancelWith(gameObject));
    }

    IEnumerator<float> _PlusOne(Transform plusPos)
    {
        //Create the +1 Icon :D
        GameObject bln = Instantiate(plusOne);
        bln.GetComponent<RectTransform>().SetParent(party.GetComponent<RectTransform>(), false);
        //Set The size and pos : |
        bln.transform.position = cam.WorldToScreenPoint(plusPos.position);
        bln.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);

        yield return Timing.WaitForSeconds(.5f);
        Destroy(bln);
    }
    #endregion

    public void EndLvlBonus()
    {
        ptsFromlvl.text = balloons.ToString();

        sumPts.text = (xNum * balloons).ToString();

        SaveData.instance.points += (xNum * balloons) + points;

        party.CallAction();
        totalPts.text = SaveData.instance.points.ToString();


        SaveData.instance.SaveGame();


    }
    #endregion

    #region Progress Bar:
    public void Progress()
    {
        if (!SaveData.instance.bonusLvl)
            phaseStart.SetActive(false);
        else
            phaseBonusStart.SetActive(false);

        phaseOne.SetActive(true);
        Timing.RunCoroutine(_Prog().CancelWith(gameObject));
    }
    IEnumerator<float> _Prog()
    {
        float startDist = Vector3.Distance(player.transform.position, goal.position);
        float dist = Vector3.Distance(player.transform.position, goal.position);
        while (dist > 0)
        {
            dist = Vector3.Distance(player.transform.position, goal.position);

            if (player.phase == Phase.p2)
                break;


            if (dist != 0)
            {
                pointer.anchoredPosition = new Vector2(
                       progressFill.fillAmount * 230,
                       pointer.anchoredPosition.y);
                progressFill.fillAmount = 1 - (dist / startDist);
                
            }
            else
            {
                pointer.anchoredPosition = new Vector2(230, pointer.anchoredPosition.y);
                progressFill.fillAmount = 1;
            }

            yield return Timing.WaitForSeconds(0.01f);
        }
        phaseOne.SetActive(false);
        phaseTwo.SetActive(true);
    }
    #endregion

    #region Ending:
    public void EndLevel()
    {
        phaseTwo.SetActive(false);
        phaseEnd.SetActive(true);

        if (SaveData.instance.bonusLvl)
        {
            SaveData.instance.changeNumbers = true;
            SaveData.instance.bonusLvl = false;
        }
        else
            lvl++;

        if (lvl > SaveData.instance.nextTreeLvls[2]) SaveData.instance.bonusLvl = true;

        if(SaveData.instance.changeNumbers) SaveData.instance.bonusLvl = false;


        SaveData.instance.lvl = lvl;

        EndLvlBonus();
    }

    public void Lost()
    {
        phaseOne.SetActive(false);
        phaseLose.SetActive(true);
    }

    public void NextLevel()
    {
        //Load it from SaveData.cs ofc xD
        SceneManager.LoadSceneAsync(0);     //Test... We'll be loading the Loading Scene later on :|
    }
    #endregion
}
