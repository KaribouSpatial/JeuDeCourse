using UnityEngine;

namespace Assets.Cadriciel.Scripts
{
    public class FirstEnnemyTracker : MonoBehaviour
    {
        public float DistanceCutOff;
        public int TrackingForce;
        public Transform TrackTarget;

        private Transform _ennemyTarget;

        // Use this for initialization
        private void Start()
        {
            _ennemyTarget = GameObject.Find("Game Manager").GetComponent<RaceManager>().CarsPositions[0].transform;
            //_ennemyTarget = RaceManager.CarsPositions[0].transform;
        }

        // Update is called once per frame
        private void FixedUpdate()
        {
            var ennemyDist = _ennemyTarget.position - this.transform.position;
            if (ennemyDist.magnitude > DistanceCutOff)
            {
                this.rigidbody.AddForce((TrackTarget.position - this.transform.position).normalized*TrackingForce);
            }
            else
            {
                this.rigidbody.AddForce(ennemyDist.normalized*TrackingForce);
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            foreach (ContactPoint contact in collision.contacts)
            {
                // Collision with the wanted ennemy
                if (contact.otherCollider.attachedRigidbody != null && contact.otherCollider.attachedRigidbody.transform == _ennemyTarget)
                {
                    Destroy(this.gameObject);
                    return;
                }
            }
        }
    }
}
