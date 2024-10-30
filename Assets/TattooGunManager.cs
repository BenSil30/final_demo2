using Meta.WitAi.Utilities;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TattooGunManager : MonoBehaviour
{
	public GameObject drawingSphere;
	public GameObject TattooGunNeedleStart;

	public bool TattooGunSelected;
	public GameObject Canvas;
	public InputManager inputManager;

	public Material DefaultMat;

	public float inkAmount, gunVoltage;
	private float timeElapsed = 0f;

	public TextMeshPro inkAmountText, voltageText;

	// Start is called before the first frame update
	private void Start()
	{
		inkAmount = 0f;

		// get black default mat
		DefaultMat = GetComponent<Renderer>().materials[0];
	}

	private void Update()
	{
		// if a second has passed, decrement ink amount - this should always run because if there is no trigger being pressed, it will not decrement
		timeElapsed += Time.deltaTime;
		if (timeElapsed >= 1f)
		{
			DecrementInkAmount();
			timeElapsed = 0f;
		}

		// clear color from ink gun if empty
		if (inkAmount <= 0f)
		{
			ClearInkFromGun();
		}

		voltageText.text = "Voltage: " + gunVoltage;
		inkAmountText.text = "Ink Amount: " + inkAmount;
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("HandLocation"))
		{
			TattooGunSelected = true;
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.CompareTag("HandLocation"))
		{
			TattooGunSelected = false;
		}
	}

	private void OnTriggerStay(Collider other)
	{
		Debug.Log("now drawing" + other.gameObject.name);

		if (other.CompareTag("Canvas") & gunVoltage > 0f & inkAmount > 0f)
		{
			CanvasManager tempManager = Canvas.GetComponent<CanvasManager>();
			Vector3 spawnPos = other.ClosestPoint(TattooGunNeedleStart.transform.position);

			GameObject temp = Instantiate(drawingSphere, spawnPos, Quaternion.identity);
			temp.GetComponent<Renderer>().material = GetComponent<Renderer>().materials[0];

			float depthZ = (1 - tempManager.GunDepth.z) * .01f;
			depthZ *= (gunVoltage / 10f) * 1.02f;
			temp.transform.localScale = new Vector3(depthZ, depthZ, depthZ);
		}
	}

	#region Ink management

	public void ChangeInkColor(Material newColor)
	{
		ClearInkFromGun();
		//if (SelectedInk == null) return;
		Debug.Log("Changing ink " + newColor.name);
		inkAmount = 100f;

		// change mat to new color
		Material[] temp = GetComponent<Renderer>().materials;
		temp[0] = newColor;
		GetComponent<Renderer>().materials = temp;
	}

	public void ClearInkFromGun()
	{
		Debug.Log("ink cleared");
		inkAmount = 0f;

		// change mat to black
		Material[] temp = GetComponent<Renderer>().materials;
		temp[0] = DefaultMat;
		GetComponent<Renderer>().materials = temp;
	}

	public void DecrementInkAmount()
	{
		if (!TattooGunSelected) return;
		inkAmount -= gunVoltage;
		inkAmount = Mathf.Clamp(inkAmount, 0f, 100f);
	}

	#endregion Ink management
}