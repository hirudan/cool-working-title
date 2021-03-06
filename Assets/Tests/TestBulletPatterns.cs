﻿using System.Collections;
using System.Collections.Generic;
using BulletManagement;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    [TestFixture]
    public class TestBulletPatterns
    {
        private static Vector3[] getBulletPositions(IReadOnlyList<Bullet> bullets)
        {
            Vector3[] positions = new Vector3[bullets.Count];
            for(int x = 0; x < bullets.Count; ++x)
            {
                positions[x] = bullets[x].transform.position;
            }
            return positions;
        }

        // A Test behaves as an ordinary method
        [Test]
        public void TestBulletBasic()
        {
            // Use the Assert class to test conditions
            Assert.True(true);
        }

        // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
        // `yield return null;` to skip a frame.
        [UnityTest]
        public IEnumerator TestOctoPattern()
        {
            int waitNUnits = 2;
            // Randomly picked tolerance after some average testing
            float deviationTolerance = 0.008f;

            GameObject patternGameObject =
                MonoBehaviour.Instantiate(Resources.Load<GameObject>("Emitters/OctoEmitter"));
            SlipTimeEmitter octoEmitter = patternGameObject.GetComponent<SlipTimeEmitter>();

            // Wait for game loop to update, wait 1f seconds to shoot first bullet
            yield return new WaitForSeconds(1f);

            Bullet[] bullets = Object.FindObjectsOfType<Bullet>();

            // Once emitter has been instantiated, bullets should appear.
            // Check that there are exactly 8 bullets
            Debug.Log("total bullets: " + bullets.Length.ToString());
            Assert.True(bullets.Length == 8);

            // Track each bullet's differential speed to see if they are in the correct position
            Vector3[] priorLocations = getBulletPositions(bullets);
            // Wait exactly n second to get n unit of translation (because of deltaTime multiplication in Bullet.cs)
            yield return new WaitForSeconds(waitNUnits);
            Vector3[] newLocations = getBulletPositions(bullets);

            // Subtract each position and check if they are equal to roughly our derivative
            Debug.Log("Bullet position deviations after " + waitNUnits.ToString() + " seconds:");
            for(int x = 0; x < bullets.Length; ++x) {
                Vector3 difference = newLocations[x] - priorLocations[x];
                int bulletId = bullets[x].BulletId;
                float c = (Mathf.PI / 4) * (bulletId % 8);
                float dx = Mathf.Cos(c) * waitNUnits;
                float dy = Mathf.Sin(c) * waitNUnits;
                float dz = 0f;

                Vector3 derivative = new Vector3(dx, dy, dz);
                Vector3 comparison = (derivative - difference);

                // Print magnitude of deviation
                Debug.Log("Bullet ID: " + bulletId.ToString());
                Debug.Log(newLocations[x]);
                Debug.Log(priorLocations[x]);
                Debug.Log(comparison.magnitude);

                // Randomly check tolerance of 
                Assert.True(comparison.magnitude <= deviationTolerance);
            }
        }
    }
}
