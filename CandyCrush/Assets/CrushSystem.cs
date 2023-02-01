using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CrushSystem : MonoBehaviour
{
    public static int Score = 0;

    [SerializeField] TextMeshProUGUI m_Object;

    [SerializeField] Vector2 startPos;
    int gridSize = 7;
    [SerializeField] GameObject[] beans;

    List<GameObject> beansList = new List<GameObject>();

    int selectedBeans;
    GameObject selectedOne;
    GameObject selectedTwo;

    void Start()
    {
        SetupBoard();
    }

    private void Update()
    {
        m_Object.text = "CURRENT SCORE: " + Score;
    }

    public void SetupBoard()
    {   
        foreach(GameObject bean in beansList)
            Destroy(bean);

        for (int horizontalOffet = 0; horizontalOffet < gridSize; horizontalOffet++)
        {
            for (int verticalOffset = 0; verticalOffset < gridSize; verticalOffset++)
            {
                var newBean = Instantiate(beans[Random.Range(0, beans.Length)], new Vector2(-50 + (horizontalOffet * 75), 200 - (verticalOffset * 75)), new Quaternion(0, 0, 0, 0));
                Debug.Log("i make bean");
                newBean.transform.SetParent(gameObject.transform, false);
                beansList.Add(newBean);
            }
        }
    }

    public void LockBeans(GameObject obj)
    {

        selectedBeans++;

        if(selectedBeans == 1)
        {
            selectedOne = obj;
            return;
        }
        else
        {
            selectedTwo = obj;
        }

        if(selectedOne == selectedTwo)
        {
            selectedOne= null;
            selectedTwo= null;
        }

        if(Vector2.Distance(selectedOne.transform.position, selectedTwo.transform.position) < 80)
            Switcheroo(selectedOne, selectedTwo);
    }

    void Switcheroo(GameObject first, GameObject second)
    {
        Vector2 tempos1 = first.transform.position;
        Vector2 tempos2 = second.transform.position;

        first.transform.position = tempos2;
        second.transform.position = tempos1;

        first.GetComponent<Bean>().GoToDelay();
        second.GetComponent<Bean>().GoToDelay();

        selectedBeans = 0;
    }
}
