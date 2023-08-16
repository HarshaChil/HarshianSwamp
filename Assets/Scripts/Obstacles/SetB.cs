using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetB : MonoBehaviour
{
    public bool isSetB;
    [SerializeField] GameObject[] walls;

    private void Awake()
    {
        isSetB = false;
    }
    private void Start()
    {
        // wall.GetComponent<GameObject>();
    }

    private void Update()
    {
        // changeSet();

    }
    void changeSet()
    {
        if (isSetB)
        {
            for (int i = 0; i < walls.Length; i++)
            {
                walls[i].gameObject.SetActive(true);
            }
        }
        else
        {
            for (int i = 0; i < walls.Length; i++)
            {
                walls[i].gameObject.SetActive(false);
            }
        }
    }
}
