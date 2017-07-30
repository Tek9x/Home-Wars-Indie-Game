using System;
using UnityEngine;

// Token: 0x02000057 RID: 87
public class MOV_ParaTransport : MonoBehaviour
{
	// Token: 0x06000434 RID: 1076 RVA: 0x000A0EB8 File Offset: 0x0009F0B8
	private void Start()
	{
		this.infoNeutreTattica = GameObject.FindGameObjectWithTag("InfoNeutreTattica");
		this.terzaCamera = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaCamere[2];
		this.corpoAereo = base.transform.GetChild(2).gameObject;
	}

	// Token: 0x06000435 RID: 1077 RVA: 0x000A0F08 File Offset: 0x0009F108
	private void Update()
	{
		if (base.gameObject == this.terzaCamera.GetComponent<TerzaCamera>().ospiteCamera)
		{
			this.MovimentoInPrimaPersona();
		}
	}

	// Token: 0x06000436 RID: 1078 RVA: 0x000A0F3C File Offset: 0x0009F13C
	private void MovimentoInPrimaPersona()
	{
		float deltaTime = Time.deltaTime;
		float num = -Input.GetAxis("Mouse Y");
		float axis = Input.GetAxis("Mouse X");
		if (this.terzaCamera.GetComponent<TerzaCamera>().èTPS)
		{
			this.terzaCamera.transform.Rotate(num * deltaTime * this.velocitàRotCamera, 0f, 0f);
			this.terzaCamera.transform.Rotate(0f, axis * deltaTime * this.velocitàRotCamera, 0f);
			this.terzaCamera.transform.eulerAngles = new Vector3(this.terzaCamera.transform.eulerAngles.x, this.terzaCamera.transform.eulerAngles.y, 0f);
		}
	}

	// Token: 0x04001102 RID: 4354
	public float velocitàRotCamera;

	// Token: 0x04001103 RID: 4355
	private GameObject infoNeutreTattica;

	// Token: 0x04001104 RID: 4356
	private GameObject terzaCamera;

	// Token: 0x04001105 RID: 4357
	private GameObject corpoAereo;
}
