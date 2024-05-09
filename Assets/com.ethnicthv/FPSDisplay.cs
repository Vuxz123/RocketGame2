using UnityEngine;
using TMPro;
using UnityEngine.Serialization;

namespace com.ethnicthv
{
    public class FPSDisplay: MonoBehaviour
    {
        public TextMeshProUGUI display;
        
        public float deltaTime = 0.2f;
        
        private void Update()
        {
            deltaTime -= Time.deltaTime;
            if (deltaTime <= 0f)
            {
                var fps = 1.0f / Time.unscaledDeltaTime;
                display.text = $"FPS: {fps:0.}";
                deltaTime = 0.2f;
            }
        }
        
        
        
    }
}