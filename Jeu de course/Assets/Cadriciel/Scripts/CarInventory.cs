using System;
using UnityEngine;

namespace Assets.Cadriciel.Scripts
{
    public class CarInventory : MonoBehaviour
    {
        public GUITexture ItemDisplay;

        public PickUpItem.PowerUps AvailablePowerUp
        {
            get { return _availablePowerUp; }
            set
            {
                _availablePowerUp = value;

                if (ItemDisplay == null)
                {
                    return;
                }

                switch (value)
                {
                    case PickUpItem.PowerUps.Nothing:
                        ItemDisplay.texture = Resources.Load("Textures/NothingItem") as Texture;
                        break;
                    case PickUpItem.PowerUps.GreenBubble:
                        ItemDisplay.texture = Resources.Load("Textures/GreenBubble") as Texture;
                        break;
                    case PickUpItem.PowerUps.RedBubble:
                        ItemDisplay.texture = Resources.Load("Textures/RedBubble") as Texture;
                        break;
                    case PickUpItem.PowerUps.BlueBubble:
                        ItemDisplay.texture = Resources.Load("Textures/BlueBubble") as Texture;
                        break;
                }
            }
        }

        private GameObject _projectilesHolder;
        private PickUpItem.PowerUps _availablePowerUp;

        // Use this for initialization
        void Start () {
            _projectilesHolder = GameObject.Find("Projectiles");	
        }
	
        // Update is called once per frame
        void Update () {
#if CROSS_PLATFORM_INPUT
            var fire = CrossPlatformInput.GetAxis("Fire1");
#else
		var fire = Input.GetAxis("Fire");
#endif
            if (Math.Abs(fire) > 0.01 && AvailablePowerUp != PickUpItem.PowerUps.Nothing)
            {
                switch (AvailablePowerUp)
                {
                    case PickUpItem.PowerUps.GreenBubble:
                    case PickUpItem.PowerUps.RedBubble:
                    case PickUpItem.PowerUps.BlueBubble:
                        createBubble(AvailablePowerUp);
                        break;
                    case PickUpItem.PowerUps.Nothing:
                    default:
                        break;
                }

                AvailablePowerUp = PickUpItem.PowerUps.Nothing;
            }
        }

        private void createBubble(PickUpItem.PowerUps mode)
        {
            GameObject bubble = null;
            switch (mode)
            {
                case PickUpItem.PowerUps.GreenBubble:
                    bubble = Instantiate(Resources.Load("GreenBubble")) as GameObject;
                    break;
                case PickUpItem.PowerUps.RedBubble:
                    bubble = Instantiate(Resources.Load("RedBubble")) as GameObject;
                    bubble.GetComponentInChildren<EnnemyTracking>().Owner = this.gameObject;
                    break;
                case PickUpItem.PowerUps.BlueBubble:
                    bubble = Instantiate(Resources.Load("BlueBubble")) as GameObject;
                    bubble.GetComponent<WaypointProgressTracker>().circuit = GameObject.Find("Path B").GetComponent<WaypointCircuit>();
                    break;
                default:
                    throw new Exception("This is not a bubble");
            }

            bubble.transform.parent = _projectilesHolder.transform;
            bubble.transform.position = this.transform.FindChild("SpecialObjectStartPoint").position;

            var direction = bubble.transform.position - this.transform.position;
            bubble.rigidbody.velocity = direction.normalized;
        }
    }
}
