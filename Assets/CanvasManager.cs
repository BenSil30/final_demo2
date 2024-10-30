using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class CanvasManager : MonoBehaviour
{
	public GameObject TattooGun;
	public GameObject TattooGunNeedleTip;
	public GameObject TattooGunNeedleStart;

	public TextMeshPro GunDepthText, NumScarsText;
	public Vector3 GunDepth;
	public float numScars = 0f;

	public bool isPenetrated;

	// Start is called before the first frame update
	private void Start()
	{
		isPenetrated = false;
		GunDepth = Vector3.zero;
	}

	// Update is called once per frame
	private void Update()
	{
		if (isPenetrated)
		{
			GunDepth = CalculatePenetrationDepth();
			GunDepthText.text = "Depth: " + GunDepth.z;
			if (GunDepth.z > 0.25f)
			{
				numScars++;
			}
			NumScarsText.text = "Num Scars: " + numScars;
			if (numScars > 15)
			{
				NumScarsText.text = "You have injured your canvas: " + numScars;
				TattooGun.SetActive(false);
			}
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("TattooGun"))
		{
			isPenetrated = true;
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.CompareTag("TattooGun"))
		{
			isPenetrated = false;
		}
	}

	public Vector3 CalculatePenetrationDepth()
	{
		Collider canvasCollider = GetComponent<BoxCollider>();
		Collider gunCollider = TattooGun.GetComponent<BoxCollider>();

		// Calculate the bounds of both colliders
		Bounds boundsA = canvasCollider.bounds;
		Bounds boundsB = gunCollider.bounds;

		// Calculate the half extents of both boxes
		Vector3 halfExtentsA = boundsA.extents;
		Vector3 halfExtentsB = boundsB.extents;

		// Calculate the center of both boxes
		Vector3 centerA = boundsA.center;
		Vector3 centerB = boundsB.center;

		// Calculate the distance between the centers
		Vector3 distance = centerB - centerA;

		// Calculate the penetration depth along each axis
		float depthX = halfExtentsA.x + halfExtentsB.x - Mathf.Abs(distance.x);
		float depthY = halfExtentsA.y + halfExtentsB.y - Mathf.Abs(distance.y);
		float depthZ = halfExtentsA.z + halfExtentsB.z - Mathf.Abs(distance.z);

		// Determine the penetration depth (only positive values)
		return new Vector3(
			Mathf.Max(0, depthX),
			Mathf.Max(0, depthY),
			Mathf.Max(0, depthZ)
		);
	}
}