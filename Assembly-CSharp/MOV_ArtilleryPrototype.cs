using System;
using UnityEngine;

// Token: 0x02000079 RID: 121
public class MOV_ArtilleryPrototype : MonoBehaviour
{
	// Token: 0x0600051C RID: 1308 RVA: 0x000AD588 File Offset: 0x000AB788
	private void Start()
	{
		this.infoNeutreTattica = GameObject.FindGameObjectWithTag("InfoNeutreTattica");
		this.terzaCamera = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaCamere[2];
		this.limiteVelocità = base.GetComponent<NavMeshAgent>().speed;
		this.suonoIngranaggi = base.GetComponent<AudioSource>();
		this.suonoIngranaggiPartito = true;
	}

	// Token: 0x0600051D RID: 1309 RVA: 0x000AD5E8 File Offset: 0x000AB7E8
	private void FixedUpdate()
	{
		if (base.gameObject == this.terzaCamera.GetComponent<TerzaCamera>().ospiteCamera)
		{
			this.MovimentoInPrimaPersona();
		}
	}

	// Token: 0x0600051E RID: 1310 RVA: 0x000AD61C File Offset: 0x000AB81C
	private void MovimentoInPrimaPersona()
	{
		float axis = Input.GetAxis("Mouse X");
		this.torretta.transform.Rotate(0f, axis * this.velocitàRotazioneTorretta * Time.deltaTime, 0f);
		this.rotazioneSuGiù += Input.GetAxis("Mouse Y") * 0.3f;
		this.rotazioneSuGiù = Mathf.Clamp(this.rotazioneSuGiù, -50f, 40f);
		this.terzaCamera.transform.localEulerAngles = new Vector3(-this.rotazioneSuGiù, 0f, 0f);
		if (axis != 0f && !this.suonoIngranaggiPartito)
		{
			this.suonoIngranaggi.Play();
			this.suonoIngranaggiPartito = true;
		}
		if (axis == 0f)
		{
			this.suonoIngranaggi.Stop();
			this.suonoIngranaggiPartito = false;
		}
	}

	// Token: 0x04001337 RID: 4919
	public float velocitàRotazioneTorretta;

	// Token: 0x04001338 RID: 4920
	public float angCannoniVertMin;

	// Token: 0x04001339 RID: 4921
	public float angCannoniVertMax;

	// Token: 0x0400133A RID: 4922
	public GameObject torretta;

	// Token: 0x0400133B RID: 4923
	private GameObject infoNeutreTattica;

	// Token: 0x0400133C RID: 4924
	private GameObject terzaCamera;

	// Token: 0x0400133D RID: 4925
	private float limiteVelocità;

	// Token: 0x0400133E RID: 4926
	private AudioSource suonoIngranaggi;

	// Token: 0x0400133F RID: 4927
	private bool suonoIngranaggiPartito;

	// Token: 0x04001340 RID: 4928
	private float rotazioneSuGiù;
}
