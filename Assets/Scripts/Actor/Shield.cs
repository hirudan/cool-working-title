using System;
using UnityEngine;

namespace Actor
{
    public class Shield : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.layer == 10)
            {
                Debug.Log("Shields holding!");
            }
        }
    }
}
