using Player;
using UnityEngine;

namespace PowerUps
{
    public class ScoreDrop : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer == 8)
            {
                PlayerStats.Score += 100;
            }
        }
    }
}
