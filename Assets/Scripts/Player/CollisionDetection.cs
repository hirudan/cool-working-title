using System;
using UnityEngine;

/*
 * TODO This can probably be removed and added to some sort of player object when we have that.
 */
namespace Player
{
    public class CollisionDetection : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.layer == 10)
            {
                Debug.Log("I'm hit!");
            }
        }
    }
}
