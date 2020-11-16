using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    // Script inspired by:
    // http://gamedesigntheory.blogspot.com/2010/09/controlling-aspect-ratio-in-unity.html
    public class Blackbars : MonoBehaviour
    {
        public float targetAspect = 1f;

        private void Start()
        {
            float windowAspect = (float) Screen.width / Screen.height;
            float scaleHeight = windowAspect / targetAspect;
            var camera = this.gameObject.GetComponent<Camera>();
            var rect = camera.rect;

            if (scaleHeight < 1.0f)
            {
                rect.width = 1.0f;
                rect.height = scaleHeight;
                rect.x = 0;
                rect.y = (1.0f - scaleHeight) / 2.0f;
            }
            else
            {
                float scaleWidth = 1.0f / scaleHeight;
                rect.width = scaleWidth;
                rect.height = 1.0f;
                rect.x = (1.0f - scaleWidth) / 2.0f;
                rect.y = 0;
            }

            camera.rect = rect;
        }
    }
}
