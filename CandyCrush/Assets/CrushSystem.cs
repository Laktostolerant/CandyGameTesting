using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrushSystem : MonoBehaviour
{
    public static int Score = 0;
    [SerializeField] Vector2 startPos;
    int gridSize = 7;
    [SerializeField] GameObject[] beans;

    List<GameObject> beansList = new List<GameObject>();

    void Start()
    {
        SetupBoard();
    }

    public void SetupBoard()
    {   
        foreach(GameObject bean in beansList)
        {
            Destroy(bean);
        }

        int i = 0;
        int j = 0;

        for (i = 0; i < gridSize; i++)
        {
            for (j = 0; j < gridSize; j++)
            {
                var newBean = Instantiate(beans[Random.Range(0, beans.Length)], new Vector2(-50 + (i * 75), 200 - (j * 75)), new Quaternion(0, 0, 0, 0));
                newBean.transform.SetParent(gameObject.transform, false);
                beansList.Add(newBean);
            }
        }
    }
}
