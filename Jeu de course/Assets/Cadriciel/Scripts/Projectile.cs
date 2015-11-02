using UnityEngine;

namespace Assets.Cadriciel.Scripts
{
    public class Projectile : MonoBehaviour
    {
        public int GravityMultiplication = 1;

        public float MaxTime = 0;
        public int MaxRebounds = 0;
        public bool ExplodesWithCars = true;

        public int ExplosionForce;
        public int TorqueForce;
        public float SpeedReduction = 1;

        public float Speed;

        private float _elapsedTimeSinceSpawn;
        private int _nbRebounds;
        private Collider _lastCollider;

        // Use this for initialization
        void Start ()
        {
            _elapsedTimeSinceSpawn = 0;
            _nbRebounds = 0;
        }
	
        // Update is called once per frame
        void FixedUpdate ()
        {
            _elapsedTimeSinceSpawn += Time.fixedDeltaTime;
            if (MaxTime > 0 && _elapsedTimeSinceSpawn > MaxTime)
            {
                Destroy(this.gameObject);
                return;
            }

            this.rigidbody.velocity = this.rigidbody.velocity.normalized*Speed;
        
            // Increment gravity
            this.rigidbody.AddForce((GravityMultiplication - 1) * Physics.gravity);
        }

        void OnCollisionEnter(Collision collision)
        {
            foreach (ContactPoint contact in collision.contacts)
            {
                // Collision with a car
                if (contact.otherCollider.attachedRigidbody != null && contact.otherCollider.attachedRigidbody.gameObject.GetComponent<CarController>() != null)
                {
                    contact.otherCollider.attachedRigidbody.AddForce(0, ExplosionForce, 0);
                    contact.otherCollider.attachedRigidbody.velocity *= SpeedReduction;
                    contact.otherCollider.attachedRigidbody.AddTorque(new Vector3(TorqueForce, TorqueForce, TorqueForce));

                    if (ExplodesWithCars)
                    {
                        Destroy(this.gameObject);
                    }
                    return;
                }

                if (contact.otherCollider.gameObject.name != "Track" && contact.otherCollider.gameObject.GetComponent<PickUpItem>() == null && contact.thisCollider is CapsuleCollider)
                {
                    // Never collide two times with the same object
                    if (_lastCollider != null && _lastCollider == contact.otherCollider)
                    {
                        continue;
                    }
                    _lastCollider = contact.otherCollider;

                    // We will do ourselve the collision
                    this.rigidbody.velocity = this.rigidbody.velocity - 2 * Vector3.Dot(this.rigidbody.velocity, contact.normal.normalized) * contact.normal.normalized;
                    // Move the object outside of the collision
                    this.rigidbody.position += this.rigidbody.velocity * Time.fixedDeltaTime * 2;
                    // Reduce the speed so we can avoid a double collision
                    this.rigidbody.velocity *= 0.001f;

                    _nbRebounds++;
                    if (MaxRebounds > 0 && _nbRebounds >= MaxRebounds)
                    {
                        Destroy(this.gameObject);
                        return;
                    }
                }
            }
        }
    }
}
