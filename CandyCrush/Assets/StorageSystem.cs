using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StorageSystem : MonoBehaviour
{
    public int proteinCount { get; protected set; }
    public List<Character> ownedCharacters { get; protected set; } = new List<Character>();

    void Awake()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("StorageSystem");

        if (objs.Length > 1)
        {
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(this.gameObject);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            foreach (Character character in ownedCharacters)
            {
                Debug.Log("character: " + character.name);
            }
        }
    }

    public void ChangeProteinCount(int alteration)
    {
        proteinCount += alteration;
    }

    public void AddCharacter(Character character)
    {
        ownedCharacters.Add(character);
        Debug.Log("YOU GOT: " + ownedCharacters[ownedCharacters.Count - 1].name);
    }
}
