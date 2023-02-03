using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GachaSystem : MonoBehaviour
{
    [SerializeField] List<Character> characterList = new List<Character>();
    List<Character> ownedCharacters = new List<Character>();

    void Start()
    {
        
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.E)) 
        {
            ObtainNewCharacter();
        }
    }

    void ObtainNewCharacter()
    {
        ownedCharacters.Add(Instantiate(characterList[Random.Range(0, characterList.Count)]));
    }
}
