using UnityEngine;
using System.Collections;

namespace SGC.Characters {
    [RequireComponent(typeof(CharacterController))]
	public class PlayerSideScrolling : Character {
		
		//-----------------------------------------------------------------------------------------------------------------------------//		

		public ControllerMovement movement = new ControllerMovement();
		public ControllerJumping jump      = new ControllerJumping();
		public bool canControl             = true; //If false, you can't control the player

		//-----------------------------------------------------------------------------------------------------------------------------//	

		private CharacterController controller;
        private float extraHeightAux;
	   
		//-----------------------------------------------------------------------------------------------------------------------------//	
			   
		public override void Awake() {
			this.movement.direction = this.transform.TransformDirection(Vector3.right);
			this.controller         = this.GetComponent<CharacterController>();
			this.movement.transform = this.gameObject.transform;
            extraHeightAux = jump.extraHeight;

            //! Change for methods in CharController
		   /* if (canControl) {
				Camera.main.gameObject.GetComponent<CameraMoveAt>()._gameObj = this.transform; 
			}*/
			base.Awake();
		}
		
		//-----------------------------------------------------------------------------------------------------------------------------//			
	 
		public override void Update() {
            UpdateMovement();
            DefaultSettings();
			base.Update();
		}
        
        //-----------------------------------------------------------------------------------------------------------------------------//		

        private void UpdateMovement() {
            movement.transform.position = new Vector3(movement.transform.position.x, movement.transform.position.y, movement.transform.position.z);
            CharacterMovement.UpdateSmoothedMovementDirection(ref movement, jump, controller, canControl);
            CharacterMovement.ApplyGravity(ref movement, ref jump, controller, canControl);
            CharacterMovement.ApplyJumping(ref movement, ref jump, controller, canControl);

            var lastPosition          = transform.position; // Save lastPosition for velocity calculation.
            var currentMovementOffset = (movement.direction * movement.speed) + (new Vector3(0.0f, movement.verticalSpeed, 0.0f));  // Calculate actual motion
            currentMovementOffset    *= Time.smoothDeltaTime; // We always want the movement to be framerate independent.  Multiplying by Time.smoothDeltaTime does this.
            movement.collisionFlags   = controller.Move(currentMovementOffset);
            movement.velocity         = (transform.position - lastPosition) / Time.smoothDeltaTime;
        }

		//-----------------------------------------------------------------------------------------------------------------------------//		

        public virtual void DefaultSettings() {
            if (!controller.isGrounded) return;
            jump.extraHeight = extraHeightAux;
            //movement.gravity = gravityAux;
            movement.direction = Vector3.zero;

        }

        //-----------------------------------------------------------------------------------------------------------------------------//
    }
}