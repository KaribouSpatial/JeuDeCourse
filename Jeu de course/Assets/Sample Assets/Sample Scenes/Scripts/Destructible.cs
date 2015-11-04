using UnityEngine;
using System.Collections;

public class Destructible : MonoBehaviour 
{
	[SerializeField]
	private int _shotsToDestroy = 1;

	private int _hp;

	void Start()
	{
		_hp = _shotsToDestroy;
	}

	public void Damage()
	{
		if (--_hp <= 0) 
		{
			Destroy(this.gameObject);
		}
	}
}
