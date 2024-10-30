using Meta.WitAi.Utilities;
using UnityEngine;
using UnityEngine.UI;

public class InputManager : MonoBehaviour
{
	public float leftTriggerValue, rightTriggerValue;
	public bool isPressingGripL, isPressingGripR = false;
	public bool isVibratingL, isVibratingR = false;

	public TattooGunManager TattooGunManager;

	private void Update()
	{
		UpdateVoltage();
		VibrateControllers();
		UpdateGripStatus();
	}

	private void UpdateGripStatus()
	{
		//leftTriggerValue = OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, OVRInput.Controller.LTouch);
		rightTriggerValue = OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, OVRInput.Controller.RTouch);
		//isPressingGripL = leftTriggerValue > 0.0;
		isPressingGripR = rightTriggerValue > 0.0;
	}

	private void VibrateControllers()
	{
		// Get trigger values (0.0 to 1.0)
		//leftTriggerValue = OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, OVRInput.Controller.LTouch);
		rightTriggerValue = OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, OVRInput.Controller.RTouch);

		//isVibratingL = leftTriggerValue > 0.0;
		isVibratingR = rightTriggerValue > 0.0;

		//OVRInput.SetControllerVibration(0.5f, leftTriggerValue, OVRInput.Controller.LTouch); // Left controller
		OVRInput.SetControllerVibration(0.5f, rightTriggerValue, OVRInput.Controller.RTouch); // Right controller
	}

	private void UpdateVoltage()
	{
		// Get trigger values, multiply by 10 to get 0-10 value
		//leftTriggerValue = OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, OVRInput.Controller.LTouch) * 10f;
		rightTriggerValue = OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, OVRInput.Controller.RTouch) * 10f;

		TattooGunManager.gunVoltage = rightTriggerValue;
	}
}