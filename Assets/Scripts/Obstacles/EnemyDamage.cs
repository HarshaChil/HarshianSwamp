using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    [SerializeField] Dialogue textBox;
    bool frogHit;
    bool maskHit;

    private void Start()
    {
        textBox.GetComponent<Dialogue>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Frog")
        {
            if (frogHit) return;
            textBox.changeFrogScore(-3);
            frogHit = true;
        }

        if (collision.tag == "Mask")
        {
            if (maskHit) return;
            textBox.changeMaskScore(-3);
            maskHit = true;
        }
    }

    private void Update()
    {
        frogHit = false;
        maskHit = false;
    }
}
