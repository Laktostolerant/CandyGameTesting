using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StorageSystem : MonoBehaviour
{
    [Header("All available characters in the game")]
    [SerializeField] public Character[] characters;

    public int proteinCount { get; protected set; }

    [Header("The slots where owned characters are displayed")]
    [SerializeField] GameObject[] displaySlots;
    [SerializeField] Sprite defaultCard;

    void Awake()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("StorageSystem");

        if (objs.Length > 1)
        {
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(this.gameObject);
    }

    public void ChangeProteinCount(int alteration)
    {
        proteinCount += alteration;
    }

    public void AddCharacter(int characterIndex)
    {
        characters[characterIndex].AcquireCharacter();
        Debug.Log("You got " + characters[characterIndex].name);
    }

    public void OpenInventory()
    {
        StartCoroutine(InventoryDisplay());
    }

    IEnumerator InventoryDisplay()
    {
        yield return new WaitForSeconds(0.1f);

        for(int index = 0; index < characters.Length; index++) 
        {
            if(characters[index].IsOwned())
            {
                displaySlots[index].GetComponent<Image>().sprite = characters[index].icon;
            }
            else
            {
                displaySlots[index].GetComponent<Image>().sprite = defaultCard;
            }
        }
    }

    public void OnLevelWasLoaded(int level)
    {
        Debug.Log("I RAN");
        if (level != 1)
            return;

        GachaSystem gachaSystem = GameObject.FindWithTag("GachaSystem").GetComponent<GachaSystem>();
        displaySlots = gachaSystem.displaySlots;
    }
}
