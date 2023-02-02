using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class CrushSystem : MonoBehaviour
{
    public static int Score = 0;

    [SerializeField] TextMeshProUGUI m_Object;

    [SerializeField] Vector2 startPos;
    int gridSize = 7;
    [SerializeField] GameObject[] beans;

    List<GameObject> beansList = new List<GameObject>();

    int selectedBeans;
    public GameObject selectedOne { get; private set; }
    public GameObject selectedTwo { get; private set; }

    [SerializeField] GameObject circleObject;
    GameObject selectCircleOne;
    GameObject selectCircleTwo;

    bool moveDelay;

    Coroutine coro;

    void Start()
    {
        SetupBoard();
        InvokeRepeating("RefillBoard", 0f, 1f);
    }

    private void Update()
    {
        m_Object.text = "CURRENT SCORE: " + Score;
    }

    public void SetupBoard()
    {
        Score = 0;
        foreach(GameObject bean in beansList)
            Destroy(bean);

        for (int horizontalOffet = 0; horizontalOffet < gridSize; horizontalOffet++)
        {
            for (int verticalOffset = 0; verticalOffset < gridSize; verticalOffset++)
            {
                var newBean = Instantiate(beans[Random.Range(0, beans.Length)], new Vector2(-50 + (horizontalOffet * 75), 200 - (verticalOffset * 75)), new Quaternion(0, 0, 0, 0));
                newBean.transform.SetParent(gameObject.transform, false);
                beansList.Add(newBean);
            }
        }
    }

    public void LockBeans(GameObject obj)
    {
        if (moveDelay)
            return;

        selectedBeans++;

        if(selectedBeans == 1)
        {
            selectedOne = obj;
            selectCircleOne = Instantiate(circleObject, obj.transform.position, new Quaternion(0, 0, 0, 0));
            selectCircleOne.transform.SetParent(gameObject.transform);
            return;
        }
        else
        {
            selectedTwo = obj;
            selectCircleTwo = Instantiate(circleObject, obj.transform.position, new Quaternion(0, 0, 0, 0));
            selectCircleTwo.transform.SetParent(gameObject.transform);
        }

        if(selectedOne == selectedTwo)
        {
            ClearSelection();
            return;
        }

        if (selectCircleOne != null && selectCircleTwo != null)
            if(Vector2.Distance(selectedOne.transform.position, selectedTwo.transform.position) < 80)
            Switcheroo(selectedOne, selectedTwo);

        ClearSelection();
    }

    void ClearSelection()
    {
        selectedOne = null;
        selectedTwo = null;
        Destroy(selectCircleOne);
        Destroy(selectCircleTwo);
        selectedBeans = 0;
    }

    void Switcheroo(GameObject first, GameObject second)
    {
        moveDelay = true;
        Vector2 tempos1 = first.transform.position;
        Vector2 tempos2 = second.transform.position;

        first.transform.position = tempos2;
        second.transform.position = tempos1;

        first.GetComponent<Bean>().CheckSides();
        second.GetComponent<Bean>().CheckSides();

        coro = StartCoroutine(LegalityCheck(first, second));
    }

    IEnumerator LegalityCheck(GameObject first, GameObject second)
    {
        bool noMore = false;

        Vector2 tempos1 = first.transform.position;
        Vector2 tempos2 = second.transform.position;

        yield return new WaitForSeconds(0.5f);

        if(first == null || second == null)
            noMore = true;

        if(!noMore)
        {
            if (first.GetComponent<Bean>().illegalMoveFound == false || second.GetComponent<Bean>().illegalMoveFound == false)
            {
                Debug.Log("i can make a move!");
                first.GetComponent<Bean>().GoToDelay();
                second.GetComponent<Bean>().GoToDelay();
            }
            else
            {
                first.transform.position = tempos2;
                second.transform.position = tempos1;
            }
        }

        moveDelay = false;
    }

    public void RefillBoard()
    {
        for(int i = 0; i < 7; i++)
        {

            GameObject temp = Instantiate(new GameObject(), new Vector2(-50 + 75 * i, 275), new Quaternion(0, 0, 0, 0));
            temp.transform.SetParent(gameObject.transform, false);

            Vector2 checkPos = temp.transform.position;
            RaycastHit2D hitDown = Physics2D.Raycast(checkPos, Vector2.down, 75);

            Debug.DrawLine(temp.transform.position, new Vector2(temp.transform.position.x, temp.transform.position.y - 75), Color.blue, 1);
            GameObject obj = hitDown.collider.GameObject();
            if(obj == null)
            {
                var newBean = Instantiate(beans[Random.Range(0, beans.Length)], new Vector2(-50 + (i * 75), 200), new Quaternion(0, 0, 0, 0));
                newBean.transform.SetParent(gameObject.transform, false);
            }

            Destroy(temp);
        }
    }
}
