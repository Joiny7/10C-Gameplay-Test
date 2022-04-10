using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private Transform[] CameraPoints;

	[SerializeField]
	private Camera Cam;

	private int CurrentIndex = 0;

    void Update()
    {
		CheckCameraInput();
	}


	private void CheckCameraInput()
	{
		if (Input.GetKeyDown(KeyCode.LeftArrow))
		{
			MoveCameraLeft();
		}
		else if (Input.GetKeyDown(KeyCode.RightArrow))
		{
			MoveCameraRight();
		}
	}

	private void MoveCameraRight()
    {
		System.GC.Collect();
		CurrentIndex++;

		if (CurrentIndex >= 4)
			CurrentIndex = 0;

		Cam.transform.position = CameraPoints[CurrentIndex].position;
		Cam.transform.rotation = CameraPoints[CurrentIndex].rotation;
	}

	private void MoveCameraLeft()
    {
		System.GC.Collect();
		CurrentIndex--;

		if (CurrentIndex <= -1)
			CurrentIndex = 3;

		Cam.transform.position = CameraPoints[CurrentIndex].position;
		Cam.transform.rotation = CameraPoints[CurrentIndex].rotation;
	}
}