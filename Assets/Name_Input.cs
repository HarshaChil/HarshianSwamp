using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Name_Input : MonoBehaviour
{
    private string username;
    public void show()
    {
        gameObject.SetActive(true);
    }
    public void hide()
    {
        gameObject.SetActive(false);
    }
    private void Awake()
    {
        hide();
    }
    public void ReadStringInput(string s)
    {
        username = s;
    }
    public string getName()
    {
        return username;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
