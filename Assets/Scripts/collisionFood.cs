using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class collisionFood : MonoBehaviour
{
    private GameObject foodToDestroy = null;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        foodToDestroy = collision.gameObject;
    }
    public GameObject hasCollided()
    {
        return foodToDestroy;
    }
}
