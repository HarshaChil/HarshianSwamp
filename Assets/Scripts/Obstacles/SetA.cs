using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetA : MonoBehaviour
{
    public bool isSetA;
    [SerializeField] GameObject[] platforms;

    private void Awake()
    {
        isSetA = true;
    }
    private void Start()
    {
        // wall.GetComponent<GameObject>();
    }

    private void Update()
    {
        //changeSet();

    }
    void changeSet()
    {
        if (isSetA)
        {
            for (int i = 0; i < platforms.Length; i++)
            {
                platforms[i].gameObject.SetActive(true);
            }
        }
        else
        {
            for (int i = 0; i < platforms.Length; i++)
            {
                platforms[i].gameObject.SetActive(false);
            }
        }
    }
}
