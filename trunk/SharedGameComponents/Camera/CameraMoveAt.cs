using UnityEngine;
using System.Collections;

namespace SGC.Cameras {
    /// <summary>
    /// Make the camera follow some gameobject.
    /// </summary>
    public class CameraMoveAt : MonoBehaviour {
        
        //-----------------------------------------------------------------------------------------------------------------------------//				

        [System.Serializable]
        public class CameraConstrainsts {
            //-----------------------------------------------------------------------------------------------------------------------------//			

            public bool freezeX;
            public bool freezeY;

            //-----------------------------------------------------------------------------------------------------------------------------//			
        }

        //-----------------------------------------------------------------------------------------------------------------------------//				
        
        public float offsetX;
        public float offsetY;
        public float smoothTime;
        public Transform gameTransform;
        public UnityEngine.Camera unityCamera;
        public CameraConstrainsts constrainsts;

        //-----------------------------------------------------------------------------------------------------------------------------//					
        
        private float h;
        private float v;
        private float smoothDampX;
        private float smoothDampY;
        private Vector3 velocity = Vector3.zero;
        
        //-----------------------------------------------------------------------------------------------------------------------------//					
   
        void Start() {            
            if (this.gameTransform == null) Debug.LogWarning("gameTransform not set");
            if (this.unityCamera == null) {
                Debug.LogWarning("unityCamera not set, setting to MainCamera");
                this.unityCamera = UnityEngine.Camera.main;
            }
        }
     
        //-----------------------------------------------------------------------------------------------------------------------------//			
      
        void LateUpdate() {
            CameraSettings();            
        }
     
        //-----------------------------------------------------------------------------------------------------------------------------//			
       
        private void CameraSettings() {
            if (this.gameTransform == null) return;
            
            this.smoothDampX = Mathf.SmoothDamp(this.unityCamera.transform.position.x, this.gameTransform.transform.position.x + this.offsetX, ref this.velocity.x, this.smoothTime);            
            this.smoothDampY = Mathf.SmoothDamp(this.unityCamera.transform.position.y, this.gameTransform.transform.position.y + this.offsetY, ref this.velocity.y, this.smoothTime);

            if (this.constrainsts.freezeX && this.constrainsts.freezeY) { // Frezze
                this.smoothDampY = this.unityCamera.transform.position.y;
                this.smoothDampX = this.unityCamera.transform.position.x;
            } else if (!this.constrainsts.freezeX && this.constrainsts.freezeY) { // Only X            
                this.smoothDampY = this.unityCamera.transform.position.y;
            } else if (constrainsts.freezeX && !constrainsts.freezeY) { // Only Y
                this.smoothDampX = this.unityCamera.transform.position.x;
            } //if

            this.unityCamera.transform.position = new Vector3(this.smoothDampX, this.smoothDampY, this.unityCamera.transform.position.z);
        }
     
        //-----------------------------------------------------------------------------------------------------------------------------//				
        
        void UpdateConstraints(CameraConstrainsts constraints) {
            this.constrainsts = constraints;
        }
        
        //-----------------------------------------------------------------------------------------------------------------------------//			
    }
}
