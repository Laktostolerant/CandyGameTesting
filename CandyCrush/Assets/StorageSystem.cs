using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StorageSystem : MonoBehaviour
{
    [Header("All available characters in the game")]
    [SerializeField] public Character[] characters;

    public int proteinCount { get; protected set; }

    [Header("The slots where owned characters are displayed")]
    [SerializeField] Image[] displaySlots;

    void Awake()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("StorageSystem");

        if (objs.Length > 1)
        {
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(this.gameObject);
    }

    private void Start()
    {
        foreach(Character character in characters)
        {
            character.ResetLevel();
        }
    }

    public void ChangeProteinCount(int alteration)
    {
        proteinCount += alteration;
    }

    public void AddCharacter(int characterIndex)
    {
        characters[characterIndex].AcquireCharacter();
    }

    public void InventoryDisplay()
    {
        for(int index = 0; index < characters.Length; index++) 
        {
            Debug.Log("setting display!");
            displaySlots[index].GetComponent<Image>().sprite = characters[index].currentIcon;
        }
    }
}
