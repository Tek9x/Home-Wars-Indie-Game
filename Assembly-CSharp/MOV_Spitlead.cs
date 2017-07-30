using System;
using UnityEngine;

// Token: 0x02000058 RID: 88
public class MOV_Spitlead : MonoBehaviour
{
	// Token: 0x06000438 RID: 1080 RVA: 0x000A1024 File Offset: 0x0009F224
	private void Start()
	{
		this.infoNeutreTattica = GameObject.FindGameObjectWithTag("InfoNeutreTattica");
		this.terzaCamera = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaCamere[2];
		this.corpoAereo = base.transform.GetChild(2).gameObject;
	}

	// Token: 0x06000439 RID: 1081 RVA: 0x000A1074 File Offset: 0x0009F274
	private void Update()
	{
		if (base.gameObject == this.terzaCamera.GetComponent<TerzaCamera>().ospiteCamera)
		{
			this.MovimentoInPrimaPersona();
		}
	}

	// Token: 0x0600043A RID: 1082 RVA: 0x000A10A8 File Offset: 0x0009F2A8
	private void MovimentoInPrimaPersona()
	{
		float deltaTime = Time.deltaTime;
		float num = -Input.GetAxis("Mouse Y");
		float axis = Input.GetAxis("Mouse X");
		if (this.terzaCamera.GetComponent<TerzaCamera>().èTPS)
		{
			this.terzaCamera.transform.Rotate(num * deltaTime * this.velocitàRotazionArmi, 0f, 0f);
			this.terzaCamera.transform.Rotate(0f, axis * deltaTime * this.velocitàRotazionArmi, 0f);
			this.terzaCamera.transform.eulerAngles = new Vector3(this.terzaCamera.transform.eulerAngles.x, this.terzaCamera.transform.eulerAngles.y, 0f);
			this.terzaCamera.transform.localEulerAngles = new Vector3(this.terzaCamera.transform.localEulerAngles.x, this.terzaCamera.transform.localEulerAngles.y, 0f);
		}
		else if (base.GetComponent<PresenzaAlleato>().armaAttivaInFPS == 0)
		{
			float num2 = Vector3.Dot(base.transform.forward, this.torrettaFPS1.transform.right);
			if (num2 >= -0.85f || axis <= 0f)
			{
				if (num2 <= 0.92f || axis >= 0f)
				{
					this.torrettaFPS1.transform.Rotate(new Vector3(0f, axis * this.velocitàRotazionArmi, 0f) * Time.deltaTime);
				}
			}
			float num3 = Vector3.Dot(base.transform.forward, this.torrettaFPS1.transform.up);
			if (num3 >= -0.85f || num >= 0f)
			{
				if (num3 <= 0.92f || num <= 0f)
				{
					this.torrettaFPS1.transform.Rotate(new Vector3(num * this.velocitàRotazionArmi, 0f, 0f) * Time.deltaTime);
				}
			}
			this.torrettaFPS1.transform.localEulerAngles = new Vector3(this.torrettaFPS1.transform.localEulerAngles.x, this.torrettaFPS1.transform.localEulerAngles.y, 0f);
		}
		else if (base.GetComponent<PresenzaAlleato>().armaAttivaInFPS == 1)
		{
			this.torrettaFPS2.transform.Rotate(new Vector3(0f, axis * this.velocitàRotazionArmi, 0f) * Time.deltaTime);
			float num4 = Vector3.Dot(base.transform.up, this.torrettaFPS2.transform.forward);
			if (num4 >= -0.1f || num <= 0f)
			{
				if (num4 <= 0.92f || num >= 0f)
				{
					this.torrettaFPS2.transform.Rotate(new Vector3(num * this.velocitàRotazionArmi, 0f, 0f) * Time.deltaTime);
				}
			}
			this.torrettaFPS2.transform.localEulerAngles = new Vector3(this.torrettaFPS2.transform.localEulerAngles.x, this.torrettaFPS2.transform.localEulerAngles.y, 0f);
		}
		else if (base.GetComponent<PresenzaAlleato>().armaAttivaInFPS == 2)
		{
			this.torrettaFPS3.transform.Rotate(new Vector3(0f, axis * this.velocitàRotazionArmi, 0f) * Time.deltaTime);
			float num5 = Vector3.Dot(base.transform.up, this.torrettaFPS3.transform.forward);
			if (num5 <= 0f || num >= 0f)
			{
				if (num5 >= -0.92f || num <= 0f)
				{
					this.torrettaFPS3.transform.Rotate(new Vector3(num * this.velocitàRotazionArmi, 0f, 0f) * Time.deltaTime);
				}
			}
			this.torrettaFPS3.transform.localEulerAngles = new Vector3(this.torrettaFPS3.transform.localEulerAngles.x, this.torrettaFPS3.transform.localEulerAngles.y, 0f);
		}
		else if (base.GetComponent<PresenzaAlleato>().armaAttivaInFPS == 3)
		{
			float num6 = Vector3.Dot(-base.transform.forward, this.torrettaFPS4.transform.right);
			if (num6 >= -0.92f || axis <= 0f)
			{
				if (num6 <= 0.92f || axis >= 0f)
				{
					this.torrettaFPS4.transform.Rotate(new Vector3(0f, axis * this.velocitàRotazionArmi, 0f) * Time.deltaTime);
				}
			}
			float num7 = Vector3.Dot(base.transform.forward, this.torrettaFPS4.transform.up);
			if (num7 >= -0.85f || num >= 0f)
			{
				if (num7 <= 0.85f || num <= 0f)
				{
					this.torrettaFPS4.transform.Rotate(new Vector3(num * this.velocitàRotazionArmi, 0f, 0f) * Time.deltaTime);
				}
			}
			this.torrettaFPS4.transform.localEulerAngles = new Vector3(this.torrettaFPS4.transform.localEulerAngles.x, this.torrettaFPS4.transform.localEulerAngles.y, 0f);
		}
	}

	// Token: 0x04001106 RID: 4358
	public float velocitàRotazionArmi;

	// Token: 0x04001107 RID: 4359
	public GameObject torrettaFPS1;

	// Token: 0x04001108 RID: 4360
	public GameObject torrettaFPS2;

	// Token: 0x04001109 RID: 4361
	public GameObject torrettaFPS3;

	// Token: 0x0400110A RID: 4362
	public GameObject torrettaFPS4;

	// Token: 0x0400110B RID: 4363
	private GameObject infoNeutreTattica;

	// Token: 0x0400110C RID: 4364
	private GameObject terzaCamera;

	// Token: 0x0400110D RID: 4365
	private GameObject corpoAereo;
}
