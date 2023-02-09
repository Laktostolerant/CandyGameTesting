using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu()]
public class Character : ScriptableObject
{
    bool owned;
    [SerializeField] string characterIndex;
    [SerializeField] public Sprite icon;

    public void Start()
    {
        owned = false;
    }

    public void AcquireCharacter()
    {
        owned = true;
    }

    public bool IsOwned()
    {
        return owned;
    }
}
