using System;
using UnityEngine;

// Token: 0x02000060 RID: 96
public class MOV_ArmoredAirship : MonoBehaviour
{
	// Token: 0x06000479 RID: 1145 RVA: 0x000A5F70 File Offset: 0x000A4170
	private void Start()
	{
		this.infoNeutreTattica = GameObject.FindGameObjectWithTag("InfoNeutreTattica");
		this.terzaCamera = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaCamere[2];
		this.suonoTorretta1 = this.torrettaArma1.GetComponent<AudioSource>();
		this.suonoTorretta2 = this.torrettaArma2.GetComponent<AudioSource>();
		this.suonoTorretta3 = this.torrettaArma3.GetComponent<AudioSource>();
	}

	// Token: 0x0600047A RID: 1146 RVA: 0x000A5FDC File Offset: 0x000A41DC
	private void Update()
	{
		this.ConfermaControllo();
	}

	// Token: 0x0600047B RID: 1147 RVA: 0x000A5FE4 File Offset: 0x000A41E4
	private void FixedUpdate()
	{
		if (this.èInPrimaPersona)
		{
			this.MovimentoInPrimaPersonaFisico();
		}
	}

	// Token: 0x0600047C RID: 1148 RVA: 0x000A5FF8 File Offset: 0x000A41F8
	private void ConfermaControllo()
	{
		if (base.gameObject == this.terzaCamera.GetComponent<TerzaCamera>().ospiteCamera)
		{
			this.èInPrimaPersona = true;
		}
		else
		{
			this.èInPrimaPersona = false;
			if (base.GetComponent<Rigidbody>())
			{
				UnityEngine.Object.Destroy(base.GetComponent<Rigidbody>());
			}
		}
	}

	// Token: 0x0600047D RID: 1149 RVA: 0x000A6054 File Offset: 0x000A4254
	private void MovimentoInPrimaPersonaFisico()
	{
		if (!base.GetComponent<Rigidbody>())
		{
			base.GetComponent<CapsuleCollider>().isTrigger = false;
			base.GetComponent<CapsuleCollider>().material = this.materialeFisico;
			base.gameObject.AddComponent<Rigidbody>();
			this.corpoRigido = base.GetComponent<Rigidbody>();
			this.corpoRigido.constraints = RigidbodyConstraints.FreezeRotationZ;
			this.corpoRigido.useGravity = false;
			this.corpoRigido.mass = 1f;
			this.corpoRigido.drag = 1f;
			this.corpoRigido.angularDrag = 0.3f;
		}
		if (this.velocitàTraslDavDietroEffettiva < this.velocitàMax)
		{
			if (Input.GetKey(KeyCode.W))
			{
				this.velocitàTraslDavDietroEffettiva += this.accTraslAvantiDietro;
			}
		}
		else if (Input.GetKey(KeyCode.W))
		{
			this.velocitàTraslDavDietroEffettiva = this.velocitàMax;
		}
		if (!Input.GetKey(KeyCode.W) && this.velocitàTraslDavDietroEffettiva > 0f)
		{
			this.velocitàTraslDavDietroEffettiva -= 1f;
		}
		if (this.velocitàTraslDavDietroEffettiva > -this.velocitàMax)
		{
			if (Input.GetKey(KeyCode.S))
			{
				this.velocitàTraslDavDietroEffettiva -= this.accTraslAvantiDietro;
			}
		}
		else if (Input.GetKey(KeyCode.S))
		{
			this.velocitàTraslDavDietroEffettiva = -this.velocitàMax;
		}
		if (!Input.GetKey(KeyCode.S) && this.velocitàTraslDavDietroEffettiva < 0f)
		{
			this.velocitàTraslDavDietroEffettiva += 1f;
		}
		if (this.velocitàTraslLatEffettiva < this.velocitàMax / 1.5f)
		{
			if (Input.GetKey(KeyCode.A))
			{
				this.velocitàTraslLatEffettiva += this.accTraslLaterale;
			}
		}
		else if (Input.GetKey(KeyCode.A))
		{
			this.velocitàTraslLatEffettiva = this.velocitàMax / 1.5f;
		}
		if (this.velocitàTraslLatEffettiva > -this.velocitàMax / 1.5f)
		{
			if (Input.GetKey(KeyCode.D))
			{
				this.velocitàTraslLatEffettiva -= this.accTraslLaterale;
			}
		}
		else if (Input.GetKey(KeyCode.D))
		{
			this.velocitàTraslLatEffettiva = -this.velocitàMax / 1.5f;
		}
		if (!Input.GetKey(KeyCode.A) && this.velocitàTraslLatEffettiva > 0f)
		{
			this.velocitàTraslLatEffettiva -= 1f;
		}
		if (!Input.GetKey(KeyCode.D) && this.velocitàTraslLatEffettiva < 0f)
		{
			this.velocitàTraslLatEffettiva += 1f;
		}
		if (this.velocitàTraslSalitaEffettiva < this.velocitàMax / 1.5f)
		{
			if (Input.GetKey(KeyCode.Space))
			{
				this.velocitàTraslSalitaEffettiva += this.accTraslSalita;
			}
		}
		else if (Input.GetKey(KeyCode.Space))
		{
			this.velocitàTraslSalitaEffettiva = this.velocitàMax / 1.5f;
		}
		if (this.velocitàTraslSalitaEffettiva > -this.velocitàMax / 1.5f)
		{
			if (Input.GetKey(KeyCode.LeftShift))
			{
				this.velocitàTraslSalitaEffettiva -= this.accTraslSalita;
			}
		}
		else if (Input.GetKey(KeyCode.LeftShift))
		{
			this.velocitàTraslSalitaEffettiva = -this.velocitàMax / 1.5f;
		}
		if (!Input.GetKey(KeyCode.Space) && this.velocitàTraslSalitaEffettiva > 0f)
		{
			this.velocitàTraslSalitaEffettiva -= 1f;
		}
		if (!Input.GetKey(KeyCode.LeftShift) && this.velocitàTraslSalitaEffettiva < 0f)
		{
			this.velocitàTraslSalitaEffettiva += 1f;
		}
		if (this.terzaCamera.GetComponent<TerzaCamera>().èTPS)
		{
			this.forzaImbardata = Input.GetAxis("Mouse X");
			this.velocitàImbarEffettiva += this.forzaImbardata;
			if (this.velocitàImbarEffettiva > this.imbardataVelocitàMax)
			{
				this.velocitàImbarEffettiva = this.imbardataVelocitàMax;
			}
			if (this.velocitàImbarEffettiva < -this.imbardataVelocitàMax)
			{
				this.velocitàImbarEffettiva = -this.imbardataVelocitàMax;
			}
			if (this.forzaImbardata == 0f)
			{
				if (this.velocitàImbarEffettiva > 0f)
				{
					this.velocitàImbarEffettiva -= 0.8f;
				}
				else if (this.velocitàImbarEffettiva < 0f)
				{
					this.velocitàImbarEffettiva += 0.8f;
				}
			}
			this.forzaBeccheggio = Input.GetAxis("Mouse Y");
			if (this.forzaBeccheggio > 0f && (base.transform.eulerAngles.x < this.angBeccheggioMin || base.transform.eulerAngles.x > 90f))
			{
				this.velocitàBecchEffettiva += this.forzaBeccheggio;
			}
			if (this.forzaBeccheggio < 0f && (base.transform.eulerAngles.x > this.angBeccheggioMax || base.transform.eulerAngles.x < 90f))
			{
				this.velocitàBecchEffettiva += this.forzaBeccheggio;
			}
			if (this.velocitàBecchEffettiva > this.beccheggioVelocitàMax)
			{
				this.velocitàBecchEffettiva = this.beccheggioVelocitàMax;
			}
			if (this.velocitàBecchEffettiva < -this.beccheggioVelocitàMax)
			{
				this.velocitàBecchEffettiva = -this.beccheggioVelocitàMax;
			}
			if (this.forzaBeccheggio == 0f)
			{
				if (this.velocitàBecchEffettiva > 0f)
				{
					this.velocitàBecchEffettiva -= 0.8f;
				}
				else if (this.velocitàBecchEffettiva < 0f)
				{
					this.velocitàBecchEffettiva += 0.8f;
				}
			}
		}
		else
		{
			float num = Vector3.Dot(this.baseArma1.transform.forward, base.transform.up);
			float num2 = Input.GetAxis("Mouse X") * this.velocitàRotazioneTorretta;
			this.torrettaArma1.transform.Rotate(0f, num2, 0f);
			this.torrettaArma2.transform.Rotate(0f, num2, 0f);
			this.torrettaArma3.transform.Rotate(0f, num2, 0f);
			float num3 = -Input.GetAxis("Mouse Y") * this.velocitàRotazioneCannoni;
			if (num > this.angCannoniVertMin && num3 > 0f)
			{
				this.baseArma1.transform.Rotate(num3, 0f, 0f);
				this.baseArma2.transform.Rotate(num3, 0f, 0f);
				this.baseArma3.transform.Rotate(num3, 0f, 0f);
			}
			if (num < this.angCannoniVertMax && num3 < 0f)
			{
				this.baseArma1.transform.Rotate(num3, 0f, 0f);
				this.baseArma2.transform.Rotate(num3, 0f, 0f);
				this.baseArma3.transform.Rotate(num3, 0f, 0f);
			}
			if (num2 == 0f && num3 == 0f)
			{
				this.suonoTorretta1.Stop();
				this.suonoTorretta2.Stop();
				this.suonoTorretta3.Stop();
				this.suonoTorrettaPartito = false;
			}
			else if (!this.suonoTorrettaPartito)
			{
				this.suonoTorrettaPartito = true;
				if (base.GetComponent<PresenzaAlleato>().armaAttivaInFPS == 0)
				{
					this.suonoTorretta1.Play();
				}
				else if (base.GetComponent<PresenzaAlleato>().armaAttivaInFPS == 1)
				{
					this.suonoTorretta2.Play();
				}
				else if (base.GetComponent<PresenzaAlleato>().armaAttivaInFPS == 2)
				{
					this.suonoTorretta3.Play();
				}
			}
		}
		this.corpoRigido.AddForce(base.transform.forward * this.velocitàTraslDavDietroEffettiva + -base.transform.right * this.velocitàTraslLatEffettiva + base.transform.up * this.velocitàTraslSalitaEffettiva, ForceMode.Force);
		this.corpoRigido.AddTorque(base.transform.right * this.velocitàBecchEffettiva + Vector3.up * this.velocitàImbarEffettiva, ForceMode.Force);
		base.transform.eulerAngles = new Vector3(base.transform.eulerAngles.x, base.transform.eulerAngles.y, 0f);
	}

	// Token: 0x040011B5 RID: 4533
	public float velocitàMax;

	// Token: 0x040011B6 RID: 4534
	public float accTraslAvantiDietro;

	// Token: 0x040011B7 RID: 4535
	public float accTraslLaterale;

	// Token: 0x040011B8 RID: 4536
	public float accTraslSalita;

	// Token: 0x040011B9 RID: 4537
	public float beccheggioVelocitàMax;

	// Token: 0x040011BA RID: 4538
	public float forzaBeccheggio;

	// Token: 0x040011BB RID: 4539
	public float imbardataVelocitàMax;

	// Token: 0x040011BC RID: 4540
	public float forzaImbardata;

	// Token: 0x040011BD RID: 4541
	public float angBeccheggioMax;

	// Token: 0x040011BE RID: 4542
	public float angBeccheggioMin;

	// Token: 0x040011BF RID: 4543
	public float velocitàRotazioneTorretta;

	// Token: 0x040011C0 RID: 4544
	public float velocitàRotazioneCannoni;

	// Token: 0x040011C1 RID: 4545
	public float angCannoniVertMin;

	// Token: 0x040011C2 RID: 4546
	public float angCannoniVertMax;

	// Token: 0x040011C3 RID: 4547
	public GameObject torrettaArma1;

	// Token: 0x040011C4 RID: 4548
	public GameObject torrettaArma2;

	// Token: 0x040011C5 RID: 4549
	public GameObject torrettaArma3;

	// Token: 0x040011C6 RID: 4550
	public GameObject baseArma1;

	// Token: 0x040011C7 RID: 4551
	public GameObject baseArma2;

	// Token: 0x040011C8 RID: 4552
	public GameObject baseArma3;

	// Token: 0x040011C9 RID: 4553
	public PhysicMaterial materialeFisico;

	// Token: 0x040011CA RID: 4554
	public float velocitàTraslLatEffettiva;

	// Token: 0x040011CB RID: 4555
	public float velocitàTraslDavDietroEffettiva;

	// Token: 0x040011CC RID: 4556
	public float velocitàTraslSalitaEffettiva;

	// Token: 0x040011CD RID: 4557
	private float velocitàBecchEffettiva;

	// Token: 0x040011CE RID: 4558
	private float velocitàImbarEffettiva;

	// Token: 0x040011CF RID: 4559
	private GameObject infoNeutreTattica;

	// Token: 0x040011D0 RID: 4560
	private GameObject terzaCamera;

	// Token: 0x040011D1 RID: 4561
	private bool èInPrimaPersona;

	// Token: 0x040011D2 RID: 4562
	private Rigidbody corpoRigido;

	// Token: 0x040011D3 RID: 4563
	private AudioSource suonoTorretta1;

	// Token: 0x040011D4 RID: 4564
	private AudioSource suonoTorretta2;

	// Token: 0x040011D5 RID: 4565
	private AudioSource suonoTorretta3;

	// Token: 0x040011D6 RID: 4566
	public bool suonoTorrettaPartito;
}
