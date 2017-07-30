using System;
using UnityEngine;

// Token: 0x02000051 RID: 81
public class MOV_Bomber : MonoBehaviour
{
	// Token: 0x06000412 RID: 1042 RVA: 0x0009EE04 File Offset: 0x0009D004
	private void Start()
	{
		this.infoNeutreTattica = GameObject.FindGameObjectWithTag("InfoNeutreTattica");
		this.terzaCamera = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaCamere[2];
	}

	// Token: 0x06000413 RID: 1043 RVA: 0x0009EE40 File Offset: 0x0009D040
	private void Update()
	{
		this.ConfermaControllo();
	}

	// Token: 0x06000414 RID: 1044 RVA: 0x0009EE48 File Offset: 0x0009D048
	private void FixedUpdate()
	{
		if (this.èInPrimaPersona)
		{
			if (!base.GetComponent<PresenzaAlleato>().tornaAllaBase)
			{
				this.sensibilitàComandi = base.GetComponent<PresenzaAlleato>().sensAerei;
				this.voloInverso = base.GetComponent<PresenzaAlleato>().voloInvertito;
				this.MovimentoInPrimaPersona();
			}
			else
			{
				base.transform.position += base.transform.forward * 30f * Time.deltaTime;
			}
		}
	}

	// Token: 0x06000415 RID: 1045 RVA: 0x0009EED4 File Offset: 0x0009D0D4
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

	// Token: 0x06000416 RID: 1046 RVA: 0x0009EF30 File Offset: 0x0009D130
	private void MovimentoInPrimaPersona()
	{
		if (!base.GetComponent<Rigidbody>())
		{
			base.GetComponent<CapsuleCollider>().isTrigger = false;
			base.GetComponent<CapsuleCollider>().material = this.materialeFisico;
			base.gameObject.AddComponent<Rigidbody>();
			this.corpoRigido = base.GetComponent<Rigidbody>();
			this.corpoRigido.useGravity = false;
			this.corpoRigido.mass = 1f;
			this.corpoRigido.drag = 1f;
			this.corpoRigido.angularDrag = 0.3f;
			this.corpoRigido.interpolation = RigidbodyInterpolation.Extrapolate;
		}
		if (this.velocitàFrontaleEffettiva < this.frontaleVelocitàMax)
		{
			if (Input.GetKey(KeyCode.W))
			{
				this.velocitàFrontaleEffettiva += this.accFrontale;
			}
		}
		else if (Input.GetKey(KeyCode.W))
		{
			this.velocitàFrontaleEffettiva = this.frontaleVelocitàMax;
		}
		if (this.velocitàFrontaleEffettiva > this.frontaleVelocitàMin)
		{
			if (Input.GetKey(KeyCode.S))
			{
				this.velocitàFrontaleEffettiva -= this.accFrontale;
			}
		}
		else if (Input.GetKey(KeyCode.S))
		{
			this.velocitàFrontaleEffettiva = this.frontaleVelocitàMin;
		}
		if (this.velocitàImbarEffettiva < this.imbardataVelocitàMax)
		{
			if (Input.GetKey(KeyCode.D))
			{
				this.velocitàImbarEffettiva += this.accImbardata;
			}
		}
		else if (Input.GetKey(KeyCode.D))
		{
			this.velocitàImbarEffettiva = this.imbardataVelocitàMax;
		}
		if (this.velocitàImbarEffettiva > -this.imbardataVelocitàMax)
		{
			if (Input.GetKey(KeyCode.A))
			{
				this.velocitàImbarEffettiva -= this.accImbardata;
			}
		}
		else if (Input.GetKey(KeyCode.A))
		{
			this.velocitàImbarEffettiva = -this.imbardataVelocitàMax;
		}
		if (!Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.A))
		{
			this.velocitàImbarEffettiva = 0f;
		}
		if (!base.GetComponent<ATT_Bomber>().visualeBombAttiva)
		{
			this.accRollio = Input.GetAxis("Mouse X");
			this.velocitàRollioEffettiva += this.accRollio;
			if (this.velocitàRollioEffettiva > this.rollioVelocitàMax)
			{
				this.velocitàRollioEffettiva = this.rollioVelocitàMax;
			}
			if (this.velocitàRollioEffettiva < -this.rollioVelocitàMax)
			{
				this.velocitàRollioEffettiva = -this.rollioVelocitàMax;
			}
			if (this.accRollio == 0f)
			{
				this.velocitàRollioEffettiva = 0f;
			}
			this.accBeccheggio = Input.GetAxis("Mouse Y");
			if (this.accBeccheggio > 0f)
			{
				this.velocitàBecchEffettiva += this.accBeccheggio;
			}
			if (this.accBeccheggio < 0f)
			{
				this.velocitàBecchEffettiva += this.accBeccheggio;
			}
			if (this.velocitàBecchEffettiva > this.beccheggioVelocitàMax)
			{
				this.velocitàBecchEffettiva = this.beccheggioVelocitàMax;
			}
			if (this.velocitàBecchEffettiva < -this.beccheggioVelocitàMax)
			{
				this.velocitàBecchEffettiva = -this.beccheggioVelocitàMax;
			}
			if (this.accBeccheggio == 0f)
			{
				this.velocitàBecchEffettiva = 0f;
			}
		}
		this.corpoRigido.AddForce(base.transform.forward * this.velocitàFrontaleEffettiva, ForceMode.Force);
		if (this.voloInverso == 0)
		{
			this.corpoRigido.AddTorque(base.transform.right * this.velocitàBecchEffettiva * this.sensibilitàComandi + base.transform.up * this.velocitàImbarEffettiva * this.sensibilitàComandi + base.transform.forward * -this.velocitàRollioEffettiva * this.sensibilitàComandi, ForceMode.Force);
		}
		else
		{
			this.corpoRigido.AddTorque(-base.transform.right * this.velocitàBecchEffettiva * this.sensibilitàComandi + base.transform.up * this.velocitàImbarEffettiva * this.sensibilitàComandi + base.transform.forward * -this.velocitàRollioEffettiva * this.sensibilitàComandi, ForceMode.Force);
		}
	}

	// Token: 0x04001094 RID: 4244
	public float accFrontale;

	// Token: 0x04001095 RID: 4245
	public float frontaleVelocitàMax;

	// Token: 0x04001096 RID: 4246
	public float frontaleVelocitàMin;

	// Token: 0x04001097 RID: 4247
	public float accBeccheggio;

	// Token: 0x04001098 RID: 4248
	public float beccheggioVelocitàMax;

	// Token: 0x04001099 RID: 4249
	public float accImbardata;

	// Token: 0x0400109A RID: 4250
	public float imbardataVelocitàMax;

	// Token: 0x0400109B RID: 4251
	public float accRollio;

	// Token: 0x0400109C RID: 4252
	public float rollioVelocitàMax;

	// Token: 0x0400109D RID: 4253
	public PhysicMaterial materialeFisico;

	// Token: 0x0400109E RID: 4254
	public float velocitàFrontaleEffettiva;

	// Token: 0x0400109F RID: 4255
	private float velocitàRollioEffettiva;

	// Token: 0x040010A0 RID: 4256
	private float velocitàBecchEffettiva;

	// Token: 0x040010A1 RID: 4257
	private float velocitàImbarEffettiva;

	// Token: 0x040010A2 RID: 4258
	private GameObject infoNeutreTattica;

	// Token: 0x040010A3 RID: 4259
	private GameObject terzaCamera;

	// Token: 0x040010A4 RID: 4260
	private bool èInPrimaPersona;

	// Token: 0x040010A5 RID: 4261
	private Rigidbody corpoRigido;

	// Token: 0x040010A6 RID: 4262
	private float sensibilitàComandi;

	// Token: 0x040010A7 RID: 4263
	private int voloInverso;
}
