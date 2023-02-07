using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class Character : ScriptableObject
{
    [SerializeField] string name;
    [TextArea(3, 5)]
    [SerializeField] string description;
    [SerializeField] Texture2D icon;
}
