using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthDisplay : MonoBehaviour
{
    [SerializeField] private List<GameObject> hearts;
    [SerializeField] private List<GameObject> greyHearts;
    [SerializeField] private List<GameObject> halfHearts;

    public void UpdateHp(int value)
    {
        for (int i = 0; i < hearts.Count; i++)
        {
            if (GameManager.Instance.GetDifficulty() > 0)
            {
                if (i < value)
                {
                    hearts[i].SetActive(true);
                    greyHearts[i].SetActive(false);
                    halfHearts[i].SetActive(false);
                }
                else
                {
                    hearts[i].SetActive(false);
                    greyHearts[i].SetActive(true);
                    halfHearts[i].SetActive(false);
                }
            }
            else
            {
                if (2*i+1 == value)
                {
                    hearts[i].SetActive(false);
                    greyHearts[i].SetActive(false);
                    halfHearts[i].SetActive(true);
                }
                else if (2*i+1 < value)
                {
                    hearts[i].SetActive(true);
                    greyHearts[i].SetActive(false);
                    halfHearts[i].SetActive(false);
                }
                else
                {
                    hearts[i].SetActive(false);
                    greyHearts[i].SetActive(true);
                    halfHearts[i].SetActive(false);
                }
            }
        }
    }
}
