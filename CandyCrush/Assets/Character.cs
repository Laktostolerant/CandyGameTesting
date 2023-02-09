using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu()]
public class Character : ScriptableObject
{
    public bool owned { get; private set; }

    [SerializeField] string characterIndex;
    int level;
    [SerializeField] Sprite[] iconTiers;
    public Sprite currentIcon { get; private set; } 


    public void AcquireCharacter()
    {
        level++;
        Debug.Log("character " + characterIndex + " is now level " + level);
        if (iconTiers[level])
            currentIcon = iconTiers[level];
        else
        {
            currentIcon = iconTiers[iconTiers.Length - 1];
        }
    }

    public void ResetLevel()
    {
        level = 0;
    }
}
