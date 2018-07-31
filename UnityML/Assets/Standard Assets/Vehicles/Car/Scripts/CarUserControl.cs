using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

namespace UnityStandardAssets.Vehicles.Car
{
    [RequireComponent(typeof (CarController))]
    public class CarUserControl : MonoBehaviour
    {
        private CarController m_Car; // the car controller we want to use


        private void Awake()
        {
            // get the car controller
            m_Car = GetComponent<CarController>();
        }

        public float Steering
        {
            get;set;
        }

        public float GoalSpeed
        {
            get;set;
        }

        public float Throttle
        {
            get; set;
        }

        private float scaleDiffFac = 10f;

        private void FixedUpdate()
        {
            // pass the input to the car!
            //float h = CrossPlatformInputManager.GetAxis("Horizontal");
            //float v = CrossPlatformInputManager.GetAxis("Vertical");
#if !MOBILE_INPUT
            float handbrake = CrossPlatformInputManager.GetAxis("Jump");


            //Throttle = Mathf.Min(1f, Mathf.Max(-1f, (GoalSpeed - GetComponent<Rigidbody>().velocity.magnitude) / scaleDiffFac));

            float clampedThrottle = Throttle;
            if (GetComponent<Rigidbody>().velocity.magnitude < 1f && clampedThrottle < 0)
            {
                clampedThrottle = 0f;
            }
            if (Vector3.Angle(GetComponent<Rigidbody>().velocity, transform.forward) > 90f)
            {
                clampedThrottle = 1f;
            }

            m_Car.Move(Steering, clampedThrottle, clampedThrottle, 0f);
#else
            m_Car.Move(h, v, v, 0f);
#endif
        }
    }
}
