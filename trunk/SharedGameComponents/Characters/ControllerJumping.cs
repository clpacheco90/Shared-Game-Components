using UnityEngine;
using System.Collections;

namespace SGC.Characters {
    [System.Serializable]
    public class ControllerJumping {
        //-----------------------------------------------------------------------------------------------------------------------------//			
        public bool enabled = true; // Can the character jump?
        public float height = 0.5f;// How high do we jump when pressing jump and letting go immediately	
        public float extraHeight = 1.6f;// We add extraHeight units (meters) on top when holding the button down longer while jumping
        public float speedSmoothing = 3.0f;// How fast does the character change speeds?  Higher is faster.
        public float jumpSpeed = 9.5f;// How fast does the character move horizontally when in the air.			
        public float repeatTime = -0.05f;// This prevents inordinarily too quick jumping //! [Edited]
        public float timeout = 0.15f;
        [System.NonSerializedAttribute]
        public bool justBecameUngrounded = false;// If the player holding the btn jump when 
        [System.NonSerializedAttribute]
        public bool jumping = false;// Are we jumping? (Initiated with jump button and not grounded yet)	
        [System.NonSerializedAttribute]
        public bool reachedApex = false;
        [System.NonSerializedAttribute]
        public float lastButtonTime = 0.0f;// Last time the jump button was clicked down	//! [Edited] -10
        [System.NonSerializedAttribute]
        public float lastGroundedTime = -10.0f;// Last time we were grounded
        [System.NonSerializedAttribute]
        public float groundingTimeout = 0.1f;
        [System.NonSerializedAttribute]
        public float lastTime = -1.0f;// Last time we performed a jump	
        [System.NonSerializedAttribute]
        public float lastStartHeight = 0.0f;// the height we jumped from (Used to determine for how long to apply extra jump power after jumping.)
        [System.NonSerializedAttribute]
        public bool touchedCeiling = false;
        [System.NonSerializedAttribute]
        public bool buttonReleased = true;
        //-----------------------------------------------------------------------------------------------------------------------------//			
    }
}
