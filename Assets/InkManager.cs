using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InkManager : MonoBehaviour
{
	public Material InkColor;

	private void Start()
	{
		InkColor = GetComponent<Renderer>().material;
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("TattooGun"))
		{
			Debug.Log("ink chosen " + gameObject.name);

			other.GetComponent<TattooGunManager>().ChangeInkColor(InkColor);
		}
	}
}