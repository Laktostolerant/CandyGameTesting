using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bean : MonoBehaviour
{
    [SerializeField] public int beanIndex;

    ContactFilter2D filter;
    RaycastHit2D hit;
    Collider2D collider;
    List<GameObject> verticalConnected = new List<GameObject>();
    List<GameObject> horizontalConnected = new List<GameObject>();
    bool canMove = false;

    CrushSystem crushSystem;

    LayerMask rayMask = 1 << 5;

    private void Start()
    {
        crushSystem = GameObject.FindWithTag("GameManager").GetComponent<CrushSystem>();
        StartCoroutine(SetupDelay());
        collider = GetComponent<Collider2D>();
        InvokeRepeating("UpdateBoard", 0f, 1f);
    }

    IEnumerator SetupDelay()
    {
        yield return new WaitForSeconds(0.1f);
        canMove = true;
    }

    public void CheckSides()
    {
        collider.enabled = false;

        List<GameObject> verticalConnected = new List<GameObject>();
        List<GameObject> horizontalConnected = new List<GameObject>();

        RaycastHit2D hitUp = Physics2D.Raycast(transform.position, Vector2.up, 100, rayMask);
        RaycastHit2D hitDown = Physics2D.Raycast(transform.position, Vector2.down, 100, rayMask);
        RaycastHit2D hitLeft = Physics2D.Raycast(transform.position, Vector2.left, 100, rayMask);
        RaycastHit2D hitRight = Physics2D.Raycast(transform.position, Vector2.right, 100, rayMask);

        verticalConnected.AddRange(CheckNeighbouring(hitUp, true, Vector2.up));
        verticalConnected.AddRange(CheckNeighbouring(hitDown, true, Vector2.down));
        horizontalConnected.AddRange(CheckNeighbouring(hitLeft, false, Vector2.left));
        horizontalConnected.AddRange(CheckNeighbouring(hitRight, false, Vector2.right));

        collider.enabled = true;

        verticalConnected.Add(gameObject);
        horizontalConnected.Add(gameObject);
        StartCoroutine(SmallDelay(verticalConnected, horizontalConnected));
    }

    List<GameObject> CheckNeighbouring(RaycastHit2D hitter, bool vertical, Vector2 dir)
    {
        collider.enabled = false;
        List<GameObject> tempList = new List<GameObject>();
        if (hitter.collider != null)
        {
            if(hitter.collider.gameObject.GetComponent<Bean>().beanIndex == beanIndex)
            {
                if (vertical)
                {
                    tempList.Add(hitter.collider.gameObject);
                    Debug.DrawLine(transform.position, hitter.collider.gameObject.transform.position, Color.green, 5);
                    tempList.AddRange(hitter.collider.gameObject.GetComponent<Bean>().BouncerVertical(hitter.collider.gameObject, dir));
                }
                else
                {
                    tempList.Add(hitter.collider.gameObject);
                    Debug.DrawLine(transform.position, hitter.collider.gameObject.transform.position, Color.green, 5);
                    tempList.AddRange(hitter.collider.gameObject.GetComponent<Bean>().BouncerHorizontal(hitter.collider.gameObject, dir));
                }
            }
        }

        collider.enabled = true;
        return tempList;
    }

    public List<GameObject> BouncerVertical(GameObject originPos, Vector2 dir)
    {
        collider.enabled = false;
        List<GameObject> temper = new List<GameObject>();
        RaycastHit2D tempHit = Physics2D.Raycast(transform.position, dir, 100, rayMask);
        GameObject tempy = tempHit.collider.GameObject();

        if (tempy != null && tempy.GetComponent<Collider2D>().gameObject.GetComponent<Bean>().beanIndex == beanIndex)
        {
            temper.Add(tempy);
            Debug.DrawLine(originPos.transform.position, tempy.transform.position, Color.red, 5);
            tempy.GetComponent<Bean>().BouncerVertical(tempy.gameObject, dir);
        }

        collider.enabled = true;
        if (temper == null)
            return null;
        else
            return temper;
    }

    public List<GameObject> BouncerHorizontal(GameObject originPos, Vector2 dir)
    {
        collider.enabled = false;
        List<GameObject> temper = new List<GameObject>();
        RaycastHit2D tempHit = Physics2D.Raycast(transform.position, dir, 100, rayMask);
        GameObject tempy = tempHit.collider.GameObject();

        if (tempy != null && tempy.GetComponent<Collider2D>().gameObject.GetComponent<Bean>().beanIndex == beanIndex)
        {
            temper.Add(tempy);
            Debug.DrawLine(originPos.transform.position, tempy.transform.position, Color.red, 5);
            tempy.GetComponent<Bean>().BouncerHorizontal(tempy.gameObject, dir);
        }

        collider.enabled = true;
        if (temper == null)
            return null;
        else
            return temper;
    }

    IEnumerator SmallDelay(List<GameObject> vL, List<GameObject> hL)
    {
        yield return new WaitForSeconds(0.5f);
        CheckIfLinedUp(vL, hL);
    }

    void CheckIfLinedUp(List<GameObject> vL, List<GameObject> hL)
    {
        if(vL.Count > 2 && hL.Count > 2)
        {
            foreach(GameObject obj in vL)
            {
                obj.GetComponent<Bean>().SelfDestruct();
            }

            foreach(GameObject obj in hL)
            {
                obj.GetComponent<Bean>().SelfDestruct();
            }
        }
        
        if(vL.Count > 2 && hL.Count < 3)
        {
            foreach (GameObject obj in vL)
            {
                obj.GetComponent<Bean>().SelfDestruct();
            }
        }

        if(hL.Count > 2 && vL.Count < 3)
        {
            foreach (GameObject obj in hL)
            {
                obj.GetComponent<Bean>().SelfDestruct();
            }
        }
    }

    public void UpdateBoard()
    {
        if (!canMove)
            return;

        RaycastHit2D hitDown = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y - 26), Vector2.down, 75);
        GameObject tempy = hitDown.collider.GameObject();

        if (tempy == null)
        {
            transform.position = new Vector2(transform.position.x, transform.position.y - 75);

            RaycastHit2D landCheck = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y - 26), Vector2.down, 75);
            GameObject obj = hitDown.collider.GameObject();
            if(obj != null)
                CheckSides();
        }
    }

    public void SelfDestruct()
    {
        CrushSystem.Score += 10;
        Destroy(gameObject);
    }

    public void CallForMove()
    {
        crushSystem.LockBeans(gameObject);
    }

    public void GoToDelay() { StartCoroutine(Delay()); }

    public IEnumerator Delay()
    {
        Debug.Log("hello");
        yield return new WaitForSeconds(0.1f);
        CheckSides();
    }
}
