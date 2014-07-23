using UnityEngine;
using System.Collections;

namespace Levels {
    public class LoadLevel : MonoBehaviour {
        
        //-----------------------------------------------------------------------------------------------------------------------------//			
        
        [System.Serializable]
        public class AutoFadeConfig {
            
            //-----------------------------------------------------------------------------------------------------------------------------//			

            public bool clickEvent;
            public string nextScene;
            public Color fadeColor;
            public float fadeOutTime;
            public float fadeInTime;
            public bool loadLeveAsync; //! Not Implemented
            
            //-----------------------------------------------------------------------------------------------------------------------------//			
        }

        [System.Serializable]
        public class WaitToLoad{
            
            //-----------------------------------------------------------------------------------------------------------------------------//			

            public bool wait;
            public float waitForSeconds = 3f;
            
            //-----------------------------------------------------------------------------------------------------------------------------//			
        }

        //-----------------------------------------------------------------------------------------------------------------------------//			

        public AutoFadeConfig autoFadeConfig;
        public WaitToLoad waitToLoad;

        //-----------------------------------------------------------------------------------------------------------------------------//			

        private bool load = false;

        //-----------------------------------------------------------------------------------------------------------------------------//				

        void Start() {
            if (!waitToLoad.wait) return;
            StartCoroutine(EnableLoadLevel());
        }

        //-----------------------------------------------------------------------------------------------------------------------------//			

        IEnumerator EnableLoadLevel() {            
            yield return new WaitForSeconds(waitToLoad.waitForSeconds);
            waitToLoad.wait = false;
        }

        //-----------------------------------------------------------------------------------------------------------------------------//		
        
        void Update() {
            
            if (waitToLoad.wait) return;
            if (autoFadeConfig.clickEvent) {
                if ((Input.touchCount > 0) || (Input.anyKey)) load = true;
                if (!load) return;
            }
            AutoFade.LoadLevel(autoFadeConfig);

        }
        
        //-----------------------------------------------------------------------------------------------------------------------------//			
    }
}