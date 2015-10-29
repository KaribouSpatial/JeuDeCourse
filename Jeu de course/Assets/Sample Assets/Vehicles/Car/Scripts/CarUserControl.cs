using UnityEngine;

[RequireComponent(typeof(CarController))]
public class CarUserControl : MonoBehaviour
{
    private CarController car;  // the car controller we want to use
 

    void Awake ()
    {
        // get the car controller
        car = GetComponent<CarController>();
    }


    void FixedUpdate()
    {
        // pass the input to the car!
#if CROSS_PLATFORM_INPUT
		float h = CrossPlatformInput.GetAxis("Horizontal");
		float v = CrossPlatformInput.GetAxis("Vertical");
		float jump = CrossPlatformInput.GetAxis("Jump");

#else
		float h = Input.GetAxis("Horizontal");
		float v = Input.GetAxis("Vertical");
		float jump = Input.GetAxis("Jump");
#endif
		
        car.Move(h,v,jump);
    }
}
