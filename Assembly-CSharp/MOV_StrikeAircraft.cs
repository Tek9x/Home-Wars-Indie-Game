using System;
using UnityEngine;

// Token: 0x0200005A RID: 90
public class MOV_StrikeAircraft : MonoBehaviour
{
	// Token: 0x06000442 RID: 1090 RVA: 0x000A1CDC File Offset: 0x0009FEDC
	private void Start()
	{
		this.infoNeutreTattica = GameObject.FindGameObjectWithTag("InfoNeutreTattica");
		this.terzaCamera = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaCamere[2];
	}

	// Token: 0x06000443 RID: 1091 RVA: 0x000A1D18 File Offset: 0x0009FF18
	private void Update()
	{
		this.ConfermaControllo();
	}

	// Token: 0x06000444 RID: 1092 RVA: 0x000A1D20 File Offset: 0x0009FF20
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

	// Token: 0x06000445 RID: 1093 RVA: 0x000A1DAC File Offset: 0x0009FFAC
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

	// Token: 0x06000446 RID: 1094 RVA: 0x000A1E08 File Offset: 0x000A0008
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

	// Token: 0x04001122 RID: 4386
	public float accFrontale;

	// Token: 0x04001123 RID: 4387
	public float frontaleVelocitàMax;

	// Token: 0x04001124 RID: 4388
	public float frontaleVelocitàMin;

	// Token: 0x04001125 RID: 4389
	public float accBeccheggio;

	// Token: 0x04001126 RID: 4390
	public float beccheggioVelocitàMax;

	// Token: 0x04001127 RID: 4391
	public float accImbardata;

	// Token: 0x04001128 RID: 4392
	public float imbardataVelocitàMax;

	// Token: 0x04001129 RID: 4393
	public float accRollio;

	// Token: 0x0400112A RID: 4394
	public float rollioVelocitàMax;

	// Token: 0x0400112B RID: 4395
	public PhysicMaterial materialeFisico;

	// Token: 0x0400112C RID: 4396
	public float velocitàFrontaleEffettiva;

	// Token: 0x0400112D RID: 4397
	private float velocitàRollioEffettiva;

	// Token: 0x0400112E RID: 4398
	private float velocitàBecchEffettiva;

	// Token: 0x0400112F RID: 4399
	private float velocitàImbarEffettiva;

	// Token: 0x04001130 RID: 4400
	private GameObject infoNeutreTattica;

	// Token: 0x04001131 RID: 4401
	private GameObject terzaCamera;

	// Token: 0x04001132 RID: 4402
	private bool èInPrimaPersona;

	// Token: 0x04001133 RID: 4403
	private Rigidbody corpoRigido;

	// Token: 0x04001134 RID: 4404
	private float sensibilitàComandi;

	// Token: 0x04001135 RID: 4405
	private int voloInverso;
}
