using System;
using UnityEngine;

// Token: 0x020000C1 RID: 193
public class VarieMappaStrategicaCasa : MonoBehaviour
{
	// Token: 0x060006BD RID: 1725 RVA: 0x000ED95C File Offset: 0x000EBB5C
	private void Start()
	{
		this.cameraCasa = GameObject.FindGameObjectWithTag("MainCamera");
		this.cameraCasa.transform.position = this.posCameraCasa;
		this.cameraCasa.transform.eulerAngles = this.rotCameraCasa;
	}

	// Token: 0x060006BE RID: 1726 RVA: 0x000ED9A8 File Offset: 0x000EBBA8
	private void Update()
	{
	}

	// Token: 0x04001909 RID: 6409
	public Vector3 posCameraCasa;

	// Token: 0x0400190A RID: 6410
	public Vector3 rotCameraCasa;

	// Token: 0x0400190B RID: 6411
	private GameObject cameraCasa;

	// Token: 0x0400190C RID: 6412
	public Vector3 posCameraInGestHeadquarters;

	// Token: 0x0400190D RID: 6413
	public Vector3 rotCameraInGestHeadquarters;
}
