using UnityEngine;

namespace Assets.Cadriciel.Scripts
{
    public class EnnemyTracking : MonoBehaviour
    {
        public int TrackingForce;

        private GameObject _ennemy;

        // Use this for initialization
        void Start ()
        {
            _ennemy = null;
        }
	
        // Update is called once per frame
        void FixedUpdate ()
        {
            if (_ennemy != null)
            {
                var dist = _ennemy.transform.position - this.transform.parent.position;
                dist.Set(dist.x, 0, dist.z);

                var rigidBody = this.GetComponentInParent<Rigidbody>();
                rigidBody.AddForce(dist.normalized * TrackingForce);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.attachedRigidbody != null && _ennemy == null && other.attachedRigidbody.gameObject.GetComponent<CarController>() != null)
            {
                _ennemy = other.attachedRigidbody.gameObject;
            }
        }
    }
}
