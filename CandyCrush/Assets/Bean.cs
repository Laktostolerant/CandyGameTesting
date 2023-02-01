using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static System.Net.WebRequestMethods;

public class Bean : MonoBehaviour
{
    [SerializeField] public int beanIndex;

    ContactFilter2D filter;
    RaycastHit2D hit;
    Collider2D collider;
    List<GameObject> verticalConnected = new List<GameObject>();
    List<GameObject> horizontalConnected = new List<GameObject>();

    private void Start()
    {
        collider = GetComponent<Collider2D>();
        InvokeRepeating("UpdateBoard", 0f, 1f);
    }

    public void CheckSides()
    {
        collider.enabled = false;

        List<GameObject> verticalConnected = new List<GameObject>();
        List<GameObject> horizontalConnected = new List<GameObject>();

        RaycastHit2D hitUp = Physics2D.Raycast(transform.position, Vector2.up, 100);
        RaycastHit2D hitDown = Physics2D.Raycast(transform.position, Vector2.down, 100);
        RaycastHit2D hitLeft = Physics2D.Raycast(transform.position, Vector2.left, 100);
        RaycastHit2D hitRight = Physics2D.Raycast(transform.position, Vector2.right, 100);       

        if (hitUp.collider != null)
        {
            if (hitUp.collider.gameObject.GetComponent<Bean>().beanIndex == beanIndex)
            {
                verticalConnected.Add(hitUp.collider.gameObject);
                Debug.DrawLine(transform.position, hitUp.collider.transform.position, Color.green, 5);

                GameObject tempObject = new GameObject();
                while (tempObject != null)
                {
                    tempObject = hitUp.collider.gameObject.GetComponent<Bean>().CheckFurther(Vector2.up);
                    if (tempObject != null)
                    {
                        verticalConnected.Add(tempObject);
                        Debug.DrawLine(hitUp.transform.position, tempObject.transform.position, Color.black, 5);
                    }
                    continue;
                }
            }
        }

        if (hitDown.collider != null)
        {
            if (hitDown.collider.gameObject.GetComponent<Bean>().beanIndex == beanIndex)
            {
                verticalConnected.Add(hitDown.collider.gameObject);
                Debug.DrawLine(transform.position, hitDown.collider.transform.position, Color.green, 5);

                GameObject tempObject = new GameObject();
                while (tempObject != null)
                {
                    tempObject = hitDown.collider.gameObject.GetComponent<Bean>().CheckFurther(Vector2.down);
                    if (tempObject != null)
                    {
                        verticalConnected.Add(tempObject);
                        Debug.DrawLine(hitDown.transform.position, tempObject.transform.position, Color.black, 5);
                    }
                    continue;
                }
            }
        }

        if (hitLeft.collider != null)
        {
            if (hitLeft.collider.gameObject.GetComponent<Bean>().beanIndex == beanIndex)
            {
                horizontalConnected.Add(hitLeft.collider.gameObject);
                Debug.DrawLine(transform.position, hitLeft.collider.transform.position, Color.green, 5);

                GameObject tempObject = new GameObject();
                while (tempObject != null)
                {
                    tempObject = hitLeft.collider.gameObject.GetComponent<Bean>().CheckFurther(Vector2.left);
                    if (tempObject != null)
                    {
                        verticalConnected.Add(tempObject);
                        Debug.DrawLine(hitLeft.transform.position, tempObject.transform.position, Color.black, 5);
                    }
                    continue;
                }
            }
        }

        if (hitRight.collider != null)
        {
            if (hitRight.collider.gameObject.GetComponent<Bean>().beanIndex == beanIndex)
            {
                horizontalConnected.Add(hitRight.collider.gameObject);
                Debug.DrawLine(transform.position, hitRight.collider.transform.position, Color.green, 5);

                GameObject tempObject = new GameObject();
                while (tempObject != null)
                {
                    tempObject = hitRight.collider.gameObject.GetComponent<Bean>().CheckFurther(Vector2.right);
                    if (tempObject != null)
                    {
                        verticalConnected.Add(tempObject);
                        Debug.DrawLine(hitRight.transform.position, tempObject.transform.position, Color.black, 5);
                    }
                    continue;
                }
            }
        }

        collider.enabled = true;

        verticalConnected.Add(gameObject);
        horizontalConnected.Add(gameObject);
        StartCoroutine(SmallDelay(verticalConnected, horizontalConnected));
    }

    IEnumerator SmallDelay(List<GameObject> vL, List<GameObject> hL)
    {
        yield return new WaitForSeconds(0.5f);
        CheckIfLinedUp(vL, hL);
    }

    void CheckIfLinedUp(List<GameObject> vL, List<GameObject> hL)
    {
        Debug.Log("vertical count: " + vL.Count + " and horizontal count: " + hL.Count);
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
        RaycastHit2D hitDown = Physics2D.Raycast(transform.position, Vector2.down, 100);
        Debug.Log("uwu");

        if (hitDown.collider == null)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y - 75);
        }
    }

    public GameObject CheckFurther(Vector2 direction)
    {
        collider.enabled = false;
        RaycastHit2D tempHit = Physics2D.Raycast(transform.position, direction, 100);

        if (tempHit.collider != null)
        {
            if (tempHit.collider.gameObject.GetComponent<Bean>().beanIndex == beanIndex)
            {
                collider.enabled = true;
                return tempHit.transform.gameObject;
            }
        }
        collider.enabled = true;
        return null;
    }

    public void SelfDestruct()
    {
        CrushSystem.Score += 10;
        Destroy(gameObject);
    }
}
