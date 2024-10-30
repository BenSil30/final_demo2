using Meta.WitAi.Utilities;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;

public class InputManager : MonoBehaviour
{
	public float rightTriggerValue, rightHandValue;
	public bool isPressingGripR, isHoldingHandR = false;
	public bool isVibratingR = false;

	public TattooGunManager TattooGunManager;
	public OVRHand rightHand;

	private void Update()
	{
		UpdateVoltage();
		VibrateControllers();
		UpdateGripStatus();
	}

	private void UpdateGripStatus()
	{
		InputDevice controller = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
		if (controller.isValid &&
			controller.characteristics.HasFlag(InputDeviceCharacteristics.HeldInHand) &&
			controller.characteristics.HasFlag(InputDeviceCharacteristics.Controller))
		{
			rightTriggerValue = OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, OVRInput.Controller.RTouch);
			isPressingGripR = rightTriggerValue > 0.0;
		}
		else
		{
			rightHandValue = rightHand.GetFingerPinchStrength(OVRHand.HandFinger.Middle);

			isHoldingHandR = rightHandValue > 0.0;
		}
	}

	private void VibrateControllers()
	{
		// dont vibrate if tattoo gun isn't picked up
		if (!TattooGunManager.TattooGunSelected) return;

		InputDevice controller = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
		if (!controller.isValid ||
			!controller.characteristics.HasFlag(InputDeviceCharacteristics.HeldInHand) ||
			!controller.characteristics.HasFlag(InputDeviceCharacteristics.Controller))
		{
			return;
		}
		rightTriggerValue = OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, OVRInput.Controller.RTouch);

		isVibratingR = rightTriggerValue > 0.0;
		rightTriggerValue = Mathf.Clamp(rightTriggerValue, 0.0f, 3.5f);

		OVRInput.SetControllerVibration(0.3f, rightTriggerValue, OVRInput.Controller.RTouch); // Right controller
	}

	private void UpdateVoltage()
	{
		InputDevice controller = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
		if (controller.isValid &&
			controller.characteristics.HasFlag(InputDeviceCharacteristics.HeldInHand) &&
			controller.characteristics.HasFlag(InputDeviceCharacteristics.Controller))
		{
			// Get trigger values, multiply by 10 to get 0-10 value
			rightTriggerValue = OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, OVRInput.Controller.RTouch) * 10f;

			TattooGunManager.gunVoltage = rightTriggerValue;
		}
		else
		{
			rightHandValue = rightHand.GetFingerPinchStrength(OVRHand.HandFinger.Index) * 10f;
			TattooGunManager.gunVoltage = rightHandValue;
		}
	}
}