using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MEC;

[RequireComponent (typeof(PlayerController))]
public class SkinsManager : MonoBehaviour
{
    #region Private stuff >:)
    [Header("Player Movement:")]
    [SerializeField] private PlayerMovement pMove; //This is where we'll store the skins data...
    [Header("UI Controller:")]
    [SerializeField] private UiController uCont;
    [Space]
    [SerializeField] private List<int> skinNum = new List<int>() { 0, 1, 2, 3, 4, 5 };
    private int price;
    [HideInInspector] public RemoveAds rAds;
    #endregion

    [Header("Skins Button:")]
    public Button skinsBtn;
    public Sprite[] skinsBtnImg = new Sprite[6];

    [Header("Skins Layout:")]
    public GameObject skinsLayout;  //Set active to true/false to view/hide skins...

    [Header("Buy Price:")]
    public Text buyTxt;

    [Header("Selected And Unlocked:")]
    public GameObject[] Selected = new GameObject[6];
    public GameObject[] Unlocked = new GameObject[6];
    public Button[] Skins = new Button[6];

    private void Start()
    {
        skinsBtn.GetComponent<Image>().sprite = skinsBtnImg[SaveData.instance.bodyId];
    }

    public void ShowSkinsLayout()
    {
        skinsLayout.SetActive(!skinsLayout.activeSelf);

        if (rAds.removeAds.activeSelf)
            rAds.removeAds.SetActive(false);

        EditorManager.instance.ExitEM();


        //Set the skins:
        for (int i = 0; i < pMove.bodyUnlocked.Count; i++)
        {
             skinNum.Remove(pMove.bodyUnlocked[i]);
        }

        CheckSkins();
        SetPrice();
    }
    /// <summary>
    /// Call this when you need to see available skins...
    /// </summary>
    public void CheckSkins()
    {
        //Activate the selected marker
        foreach(GameObject s in Selected)
            s.SetActive(false);
        Selected[pMove.bodyId].SetActive(true);

        //Turn On The Unlocked Skin Buttons...
        for (int i = 0; i < Skins.Length; i++)
        {
            if (pMove.bodyUnlocked.Contains(i))
            {
                Skins[i].GetComponent<Image>().color = Color.white;
                Skins[i].GetComponent<Image>().raycastTarget = true;
            }
        }
    }
    /// <summary>
    /// Call this whenever you need to change the price...
    /// </summary>
    public void SetPrice()
    {
        //Disable the buy button if there are no more skins to buy!!!
        if (skinNum.Count == 0)
            buyTxt.transform.parent.gameObject.SetActive(false);

        //Set the price:
        price = 300 * pMove.bodyUnlocked.Count;
        buyTxt.text = price.ToString();
    }
    /// <summary>
    /// Call this when you purchase a skin...
    /// </summary>
    public void SaveChanges()
    {
        //Save Price:
        SaveData.instance.bodyUnlocked = pMove.bodyUnlocked;
        SaveData.instance.SaveGame();
    }

    //Call this from another script >:O
    public void SelectSkin(int skinId)
    {
        foreach (GameObject s in Selected)
            s.SetActive(false);

        Selected[skinId].SetActive(true);

        //Change the skin from the pMove!!! >:)
        pMove.bodyId = skinId;
        pMove.CheckSkins();

        skinsBtn.GetComponent<Image>().sprite = skinsBtnImg[skinId];

        //Save the selected skin id... :|
        SaveData.instance.bodyId = skinId;
        SaveData.instance.SaveGame();
    }

    #region Buying:
    public void BuySkin()
    {
        //Get a random skin...
        if(SaveData.instance.points > price)
            Timing.RunCoroutine(_SkinShuffle().CancelWith(gameObject));
    }

    IEnumerator<float> _SkinShuffle()
    {
        float dur = 2f;
        do
        {
            dur -= .2f;
            //Shuffle (Special xD)
            int shuffleId = Random.Range(0, skinNum.Count);
            int shuffle = skinNum[shuffleId];
            //Turn one on...
            Unlocked[shuffle].SetActive(true);
            yield return Timing.WaitForSeconds(.2f);
            //Turn it off...
            Unlocked[shuffle].SetActive(false);
            yield return Timing.WaitForSeconds(.2f);
            //Delay... do nothing :|
        } while (dur > 0);

        //Unlock the selected skin...
        int unlockedId = Random.Range(0, skinNum.Count);
        int unlockedSkin = skinNum[unlockedId];
        Unlocked[unlockedSkin].SetActive(true);
        yield return Timing.WaitForSeconds(.2f);
        Unlocked[unlockedSkin].SetActive(false);
        //Other Stuff xD
        skinNum.Remove(unlockedSkin);
        pMove.bodyUnlocked.Add(unlockedSkin);   //TestO ... like restO! >:\

        //Check if you have any more skins to unlock:
        if (skinNum.Count == 0)
            buyTxt.transform.parent.gameObject.SetActive(false);

        //Save:
        uCont.BuySkin(price);
        SaveChanges();

        //Set a new Price...
        SetPrice();
        CheckSkins();
    }
    #endregion
}
