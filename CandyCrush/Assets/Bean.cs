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

    public bool illegalMoveFound { get; private set; }

    LayerMask rayMask = 1 << 5;

    private void Start()
    {
        crushSystem = GameObject.FindWithTag("GameManager").GetComponent<CrushSystem>();
        StartCoroutine(SetupDelay());
        collider = GetComponent<Collider2D>();
        StartCoroutine(Delay());
    }

    IEnumerator SetupDelay()
    {
        yield return new WaitForSeconds(0.01f);
        canMove = true;
    }

    public void CheckSides()
    {
        collider.enabled = false;

        RaycastHit2D hitUp = Physics2D.Raycast(transform.position, Vector2.up, 100, rayMask);
        RaycastHit2D hitDown = Physics2D.Raycast(transform.position, Vector2.down, 100, rayMask);
        RaycastHit2D hitLeft = Physics2D.Raycast(transform.position, Vector2.left, 100, rayMask);
        RaycastHit2D hitRight = Physics2D.Raycast(transform.position, Vector2.right, 100, rayMask);

        Debug.DrawLine(transform.position, new Vector2(transform.position.x + 10, transform.position.y + 10), Color.black, 10);

        verticalConnected.AddRange(CheckNeighbouring(hitUp, true, Vector2.up));
        verticalConnected.AddRange(CheckNeighbouring(hitDown, true, Vector2.down));
        horizontalConnected.AddRange(CheckNeighbouring(hitLeft, false, Vector2.left));
        horizontalConnected.AddRange(CheckNeighbouring(hitRight, false, Vector2.right));

        collider.enabled = true;

        verticalConnected.Add(gameObject);
        horizontalConnected.Add(gameObject);

        CheckIfLinedUp();
    }

    List<GameObject> CheckNeighbouring(RaycastHit2D hitter, bool vertical, Vector2 dir)
    {
        collider.enabled = false;
        List<GameObject> tempList = new List<GameObject>();
        if (hitter.collider != null)
        {
            if (hitter.collider.gameObject.GetComponent<Bean>().beanIndex == beanIndex)
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
        List<GameObject> tempListToAddToMain = new List<GameObject>();
        RaycastHit2D tempHit = Physics2D.Raycast(transform.position, dir, 100, rayMask);
        GameObject temObject = tempHit.collider.GameObject();

        if (temObject != null && temObject.GetComponent<Collider2D>().gameObject.GetComponent<Bean>().beanIndex == beanIndex)
        {
            tempListToAddToMain.Add(temObject);
            Debug.DrawLine(originPos.transform.position, temObject.transform.position, Color.red, 5);
            tempListToAddToMain.AddRange(temObject.GetComponent<Bean>().BouncerVertical(temObject.gameObject, dir));
        }

        collider.enabled = true;
        if (tempListToAddToMain == null)
            return null;
        else
            return tempListToAddToMain;
    }

    public List<GameObject> BouncerHorizontal(GameObject originPos, Vector2 dir)
    {
        collider.enabled = false;
        List<GameObject> tempListToAddToMain = new List<GameObject>();
        RaycastHit2D tempHit = Physics2D.Raycast(transform.position, dir, 100, rayMask);
        GameObject tempObject = tempHit.collider.GameObject();

        if (tempObject != null && tempObject.GetComponent<Collider2D>().gameObject.GetComponent<Bean>().beanIndex == beanIndex)
        {
            tempListToAddToMain.Add(tempObject);
            Debug.DrawLine(originPos.transform.position, tempObject.transform.position, Color.red, 5);
            tempListToAddToMain.AddRange(tempObject.GetComponent<Bean>().BouncerHorizontal(tempObject.gameObject, dir));
        }

        collider.enabled = true;
        if (tempListToAddToMain == null)
            return null;
        else
            return tempListToAddToMain;
    }

    void CheckIfLinedUp()
    {
        illegalMoveFound = false;

        if (verticalConnected.Count > 2 && horizontalConnected.Count > 2)
        {
            foreach (GameObject obj in verticalConnected)
            {
                obj.GetComponent<Bean>().SelfDestruct();
            }

            foreach (GameObject obj in horizontalConnected)
            {
                obj.GetComponent<Bean>().SelfDestruct();
            }
            return;
        }

        if (verticalConnected.Count > 2 && horizontalConnected.Count < 3)
        {
            foreach (GameObject obj in verticalConnected)
            {
                obj.GetComponent<Bean>().SelfDestruct();
            }
            return;
        }

        if (horizontalConnected.Count > 2 && verticalConnected.Count < 3)
        {
            foreach (GameObject obj in horizontalConnected)
            {
                obj.GetComponent<Bean>().SelfDestruct();
            }
            return;
        }
        verticalConnected.Clear();
        horizontalConnected.Clear();
        illegalMoveFound = true;
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

            RaycastHit2D landCheck = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y - 26), Vector2.down, 74);
            GameObject obj = landCheck.collider.GameObject();
            Debug.DrawRay(new Vector2(transform.position.x, transform.position.y - 26), Vector2.down * 74, Color.blue, 75);
            if (obj)
                StartCoroutine(Delay());
        }
    }

    public void SelfDestruct()
    {
        StorageSystem storageSystem = GameObject.FindWithTag("StorageSystem").GetComponent<StorageSystem>();
        if (storageSystem)
            storageSystem.ChangeProteinCount(10);
        Destroy(gameObject);
    }

    public void CallForMove() { crushSystem.LockBeans(gameObject); }

    public void GoToDelay() { StartCoroutine(Delay()); }

    public IEnumerator Delay()
    {
        yield return new WaitForSeconds(0.1f);
        CheckSides();
    }
}
