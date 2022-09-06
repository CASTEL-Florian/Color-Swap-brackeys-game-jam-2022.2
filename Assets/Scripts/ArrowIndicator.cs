using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowIndicator : MonoBehaviour
{
    [SerializeField] private Vector2 rectangleUpLeftCorner;
    [SerializeField] private Vector2 rectangleWidthHeight;
    [SerializeField] private GameObject arrowPrefab;
    private GameObject arrow;
    private void Start()
    {
        arrow = Instantiate(arrowPrefab);
        arrow.SetActive(false);
    }

    private void Update()
    {
        if (transform.position.x < rectangleUpLeftCorner.x || transform.position.x > rectangleUpLeftCorner.x + rectangleWidthHeight.x || transform.position.y < rectangleUpLeftCorner.y || transform.position.y > rectangleUpLeftCorner.y + rectangleWidthHeight.y)
        {
            arrow.SetActive(true);
            if (Mathf.Abs(transform.position.x) > Mathf.Abs(transform.position.y))
            {
                arrow.transform.position = new Vector3(Mathf.Sign(transform.position.x) * rectangleWidthHeight.x / 2, transform.position.y * Mathf.Abs(rectangleWidthHeight.x / (2 * transform.position.x)), 0);
            }
            else
            {
                arrow.transform.position = new Vector3(transform.position.x * Mathf.Abs(rectangleWidthHeight.y / (2 * transform.position.y)), Mathf.Sign(transform.position.y) * rectangleWidthHeight.y / 2, 0);
            }
            Vector3 dir = (transform.position - arrow.transform.position);
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            arrow.transform.rotation = Quaternion.Euler(0, 0, angle);
        }
        else
            arrow.SetActive(false);
    }

    public void EnemyDead()
    {
        Destroy(arrow);
    }
}
