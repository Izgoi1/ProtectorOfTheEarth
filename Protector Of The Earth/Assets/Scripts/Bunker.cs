using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bunker : MonoBehaviour
{
    private int bunkerHp = 3;

    private void Update()
    {
        if (bunkerHp < 1) 
        {
            this.gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Missile"))
        {
            bunkerHp--;
        }
        else if(other.gameObject.layer == LayerMask.NameToLayer("Invader"))
        {
            bunkerHp = 0;
        }

    }
}
