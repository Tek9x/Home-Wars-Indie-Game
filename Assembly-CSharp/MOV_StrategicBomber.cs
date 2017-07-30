using System;
using UnityEngine;

// Token: 0x02000059 RID: 89
public class MOV_StrategicBomber : MonoBehaviour
{
	// Token: 0x0600043C RID: 1084 RVA: 0x000A1750 File Offset: 0x0009F950
	private void Start()
	{
		this.infoNeutreTattica = GameObject.FindGameObjectWithTag("InfoNeutreTattica");
		this.terzaCamera = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaCamere[2];
	}

	// Token: 0x0600043D RID: 1085 RVA: 0x000A178C File Offset: 0x0009F98C
	private void Update()
	{
		this.ConfermaControllo();
	}

	// Token: 0x0600043E RID: 1086 RVA: 0x000A1794 File Offset: 0x0009F994
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
				base.transform.position += base.transform.forward * 15f * Time.deltaTime;
			}
		}
	}

	// Token: 0x0600043F RID: 1087 RVA: 0x000A1820 File Offset: 0x0009FA20
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

	// Token: 0x06000440 RID: 1088 RVA: 0x000A187C File Offset: 0x0009FA7C
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
		if (!base.GetComponent<ATT_StrategicBomber>().visualeBombAttiva)
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

	// Token: 0x0400110E RID: 4366
	public float accFrontale;

	// Token: 0x0400110F RID: 4367
	public float frontaleVelocitàMax;

	// Token: 0x04001110 RID: 4368
	public float frontaleVelocitàMin;

	// Token: 0x04001111 RID: 4369
	public float accBeccheggio;

	// Token: 0x04001112 RID: 4370
	public float beccheggioVelocitàMax;

	// Token: 0x04001113 RID: 4371
	public float accImbardata;

	// Token: 0x04001114 RID: 4372
	public float imbardataVelocitàMax;

	// Token: 0x04001115 RID: 4373
	public float accRollio;

	// Token: 0x04001116 RID: 4374
	public float rollioVelocitàMax;

	// Token: 0x04001117 RID: 4375
	public PhysicMaterial materialeFisico;

	// Token: 0x04001118 RID: 4376
	public float velocitàFrontaleEffettiva;

	// Token: 0x04001119 RID: 4377
	private float velocitàRollioEffettiva;

	// Token: 0x0400111A RID: 4378
	private float velocitàBecchEffettiva;

	// Token: 0x0400111B RID: 4379
	private float velocitàImbarEffettiva;

	// Token: 0x0400111C RID: 4380
	private GameObject infoNeutreTattica;

	// Token: 0x0400111D RID: 4381
	private GameObject terzaCamera;

	// Token: 0x0400111E RID: 4382
	private bool èInPrimaPersona;

	// Token: 0x0400111F RID: 4383
	private Rigidbody corpoRigido;

	// Token: 0x04001120 RID: 4384
	private float sensibilitàComandi;

	// Token: 0x04001121 RID: 4385
	private int voloInverso;
}
