using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cherry : MonoBehaviour
{
    [SerializeField] Dialogue textBox;
    BoxCollider2D boxCollider;
    Rigidbody2D bdoy;

    public bool shouldHide;



    Renderer canSeeObject;

    private void Awake()
    {
        canSeeObject = GetComponent<Renderer>();
        boxCollider = GetComponent<BoxCollider2D>();
        bdoy = GetComponent<Rigidbody2D>();
        shouldHide = false;
    }

    void Start()
    {
        textBox.GetComponent<Dialogue>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Frog") //Who ever has the cherry gets their score to increase
        {
            textBox.startGame = true;
            textBox.frog = false;
            shouldHide = true; //Deactivate it when someone gets it
        }
        if (collision.tag == "Mask")
        {
            textBox.startGame = true;
            textBox.frog = true;
            shouldHide = true;
        }
    }

    public void Activate() //So other objects can activate it
    {
        gameObject.SetActive(true);
    }

    private void Update()
    {
        Hide(shouldHide);
    }
    public void Hide(bool hide)
    {
        if (hide)
        {
            transform.position = new Vector2(-.71f, 6.61f);
            bdoy.constraints = RigidbodyConstraints2D.FreezePosition;

        }
        else
        {
            bdoy.constraints = RigidbodyConstraints2D.None;
            textBox.startGame = false;
        }


    }
}
