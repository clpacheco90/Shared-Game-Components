using UnityEngine;
using System.Collections;

namespace SGC.Characters {
    [System.Serializable]
    public class ControllerMovement {
        //-----------------------------------------------------------------------------------------------------------------------------//			
        
        public bool enabled      = true;// Can the character jump?
        public float runSpeed    = 7.0f;// The speed when running 	
        //public float slideFactor = 0.05f;// The speed when sliding up and around corners 
        [System.NonSerializedAttribute]
        public float slideX         = 0.0f;
        public float gravity        = 60.0f;// The gravity for the character
        public float maxFallSpeed   = 20.0f;
        public float speedSmoothing = 20.0f; // How fast does the character change speeds?  Higher is faster.
        public bool canSlideMovement; // This will keep the movement for some time after release the button
        public float slideMovementTimer = 0.0f; // timer after the button is released
        public bool canRotation;// This enables the rotation 
        public float rotationSmoothing = 10.0f;// This controls how fast the graphics of the character "turn around" when the player turns around using the controls.		
        [System.NonSerializedAttribute]
        public Vector3 direction = Vector3.zero;// The current move direction in x-y.  This will always been (1,0,0) or (-1,0,0)	
        [System.NonSerializedAttribute]
        public float verticalSpeed = 0.0f;// The current vertical speed
        [System.NonSerializedAttribute]
        public float speed = 0.0f;// The current movement speed.  This gets smoothed by speedSmoothing.
        [System.NonSerializedAttribute]
        public bool isMoving = false;// Is the user pressing the left or right movement keys?
        [System.NonSerializedAttribute]
        public bool isClimbing = false;// Is the user pressing the up or down movement keys?
        [System.NonSerializedAttribute]
        public CollisionFlags collisionFlags;// The last collision flags returned from controller.Move
        [System.NonSerializedAttribute]
        public Vector3 velocity;// We will keep track of an approximation of the character's current velocity, so that we return it from GetVelocity () for our camera to use for prediction.
        [System.NonSerializedAttribute]
        public float hangTime = 0.0f;// This will keep track of how long we have we been in the air (not grounded)	
        [System.NonSerializedAttribute]
        public Transform transform = null;
        [System.NonSerializedAttribute]
        public Transform activePlatform = null;
       
        //-----------------------------------------------------------------------------------------------------------------------------//
        
        public ControllerMovement(Transform transform) {
            this.transform = transform;
        }
        
        //-----------------------------------------------------------------------------------------------------------------------------//

        public ControllerMovement() {
        }
        
        //-----------------------------------------------------------------------------------------------------------------------------//
    }
}
