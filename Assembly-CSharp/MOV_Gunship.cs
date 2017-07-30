using System;
using UnityEngine;

// Token: 0x02000054 RID: 84
public class MOV_Gunship : MonoBehaviour
{
	// Token: 0x06000424 RID: 1060 RVA: 0x0009FE88 File Offset: 0x0009E088
	private void Start()
	{
		this.infoNeutreTattica = GameObject.FindGameObjectWithTag("InfoNeutreTattica");
		this.terzaCamera = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaCamere[2];
		this.corpoAereo = base.transform.GetChild(2).gameObject;
	}

	// Token: 0x06000425 RID: 1061 RVA: 0x0009FED8 File Offset: 0x0009E0D8
	private void Update()
	{
		if (base.gameObject == this.terzaCamera.GetComponent<TerzaCamera>().ospiteCamera)
		{
			this.MovimentoInPrimaPersona();
		}
	}

	// Token: 0x06000426 RID: 1062 RVA: 0x0009FF0C File Offset: 0x0009E10C
	private void MovimentoInPrimaPersona()
	{
		float deltaTime = Time.deltaTime;
		float num = Vector3.Dot(this.baseArma2.transform.forward, this.corpoAereo.transform.up);
		Vector3 normalized = Vector3.ProjectOnPlane(this.baseArma2.transform.forward, this.corpoAereo.transform.up).normalized;
		float num2 = Vector3.Dot(normalized, this.corpoAereo.transform.forward);
		float num3 = -Input.GetAxis("Mouse Y");
		float axis = Input.GetAxis("Mouse X");
		if (this.terzaCamera.GetComponent<TerzaCamera>().èTPS)
		{
			this.terzaCamera.transform.Rotate(num3 * deltaTime * this.velocitàRotazionArmi, 0f, 0f);
			this.terzaCamera.transform.Rotate(0f, axis * deltaTime * this.velocitàRotazionArmi, 0f);
			this.terzaCamera.transform.eulerAngles = new Vector3(this.terzaCamera.transform.eulerAngles.x, this.terzaCamera.transform.eulerAngles.y, 0f);
		}
		else
		{
			if (num > this.angCannoniVertMin && num3 > 0f)
			{
				this.baseArma1.transform.Rotate(num3 * deltaTime * this.velocitàRotazionArmi, 0f, 0f);
				this.baseArma2.transform.Rotate(num3 * deltaTime * this.velocitàRotazionArmi, 0f, 0f);
				this.baseArma3.transform.Rotate(num3 * deltaTime * this.velocitàRotazionArmi, 0f, 0f);
				this.terzaCamera.transform.Rotate(num3 * deltaTime * this.velocitàRotazionArmi, 0f, 0f);
			}
			if (num < this.angCannoniVertMax && num3 < 0f)
			{
				this.baseArma1.transform.Rotate(num3 * deltaTime * this.velocitàRotazionArmi, 0f, 0f);
				this.baseArma2.transform.Rotate(num3 * deltaTime * this.velocitàRotazionArmi, 0f, 0f);
				this.baseArma3.transform.Rotate(num3 * deltaTime * this.velocitàRotazionArmi, 0f, 0f);
				this.terzaCamera.transform.Rotate(num3 * deltaTime * this.velocitàRotazionArmi, 0f, 0f);
			}
			if (num2 < this.angOrizArmi && axis > 0f)
			{
				this.baseArma1.transform.Rotate(0f, axis * deltaTime * this.velocitàRotazionArmi, 0f);
				this.baseArma2.transform.Rotate(0f, axis * deltaTime * this.velocitàRotazionArmi, 0f);
				this.baseArma3.transform.Rotate(0f, axis * deltaTime * this.velocitàRotazionArmi, 0f);
				this.terzaCamera.transform.Rotate(0f, axis * deltaTime * this.velocitàRotazionArmi, 0f);
			}
			if (num2 > -this.angOrizArmi && axis < 0f)
			{
				this.baseArma1.transform.Rotate(0f, axis * deltaTime * this.velocitàRotazionArmi, 0f);
				this.baseArma2.transform.Rotate(0f, axis * deltaTime * this.velocitàRotazionArmi, 0f);
				this.baseArma3.transform.Rotate(0f, axis * deltaTime * this.velocitàRotazionArmi, 0f);
				this.terzaCamera.transform.Rotate(0f, axis * deltaTime * this.velocitàRotazionArmi, 0f);
			}
			this.terzaCamera.transform.localEulerAngles = new Vector3(this.terzaCamera.transform.localEulerAngles.x, this.terzaCamera.transform.localEulerAngles.y, 0f);
		}
	}

	// Token: 0x040010D0 RID: 4304
	public float velocitàRotazionArmi;

	// Token: 0x040010D1 RID: 4305
	public float angCannoniVertMin;

	// Token: 0x040010D2 RID: 4306
	public float angCannoniVertMax;

	// Token: 0x040010D3 RID: 4307
	public float angOrizArmi;

	// Token: 0x040010D4 RID: 4308
	public GameObject baseArma1;

	// Token: 0x040010D5 RID: 4309
	public GameObject baseArma2;

	// Token: 0x040010D6 RID: 4310
	public GameObject baseArma3;

	// Token: 0x040010D7 RID: 4311
	private GameObject infoNeutreTattica;

	// Token: 0x040010D8 RID: 4312
	private GameObject terzaCamera;

	// Token: 0x040010D9 RID: 4313
	private GameObject corpoAereo;
}
