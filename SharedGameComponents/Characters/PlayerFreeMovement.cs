using UnityEngine;
using System.Collections;

namespace SGC.Characters {
    [RequireComponent(typeof(CharacterController))]
	public class PlayerFreeMovement : Character {
		
		//-----------------------------------------------------------------------------------------------------------------------------//		

		public ControllerMovement movement = new ControllerMovement();
		public bool canControl             = true; //If false, you can't control the player

		//-----------------------------------------------------------------------------------------------------------------------------//	

		private CharacterController controller;
        private float countSlideMovement;
	   
		//-----------------------------------------------------------------------------------------------------------------------------//	
			   
		public override void Awake() {
			this.movement.direction = this.transform.TransformDirection(Vector3.right);
			this.controller         = this.GetComponent<CharacterController>();
			this.movement.transform = this.gameObject.transform;            
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
            CharacterMovement.UpdateSmoothedMovementFreeDirection(ref movement, controller, canControl);
   
            var lastPosition          = transform.position; // Save lastPosition for velocity calculation.
            var currentMovementOffset = (movement.direction * movement.speed);  // Calculate actual motion
            currentMovementOffset    *= Time.smoothDeltaTime; // We always want the movement to be framerate independent.  Multiplying by Time.smoothDeltaTime does this.
            movement.collisionFlags   = controller.Move(currentMovementOffset);
            movement.velocity         = (transform.position - lastPosition) / Time.smoothDeltaTime;
        }

		//-----------------------------------------------------------------------------------------------------------------------------//		

        public virtual void DefaultSettings() {
            if (!movement.canSlideMovement) {
                movement.direction = Vector3.zero;
            } else {
                if (canControl) {
                    float h, v;
                    if (CharacterMovement.IsMoving(out h, out v)) {
                        movement.direction.x = (h != movement.direction.x) ? h : movement.direction.x;
                        movement.direction.y = (v != movement.direction.y) ? v : movement.direction.y;
                    } //if
                    countSlideMovement += Time.deltaTime;
                    if (countSlideMovement >= movement.slideMovementTimer) {
                        movement.direction = Vector3.zero;
                        countSlideMovement = 0;
                    }
                } else {
                    movement.direction = Vector3.zero;

                }
            } //if            
        }

        //-----------------------------------------------------------------------------------------------------------------------------//
    }
}