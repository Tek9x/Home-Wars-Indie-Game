using System;
using UnityEngine;

// Token: 0x02000053 RID: 83
public class MOV_GroundAttack : MonoBehaviour
{
	// Token: 0x0600041E RID: 1054 RVA: 0x0009F90C File Offset: 0x0009DB0C
	private void Start()
	{
		this.infoNeutreTattica = GameObject.FindGameObjectWithTag("InfoNeutreTattica");
		this.terzaCamera = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaCamere[2];
	}

	// Token: 0x0600041F RID: 1055 RVA: 0x0009F948 File Offset: 0x0009DB48
	private void Update()
	{
		this.ConfermaControllo();
	}

	// Token: 0x06000420 RID: 1056 RVA: 0x0009F950 File Offset: 0x0009DB50
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

	// Token: 0x06000421 RID: 1057 RVA: 0x0009F9DC File Offset: 0x0009DBDC
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

	// Token: 0x06000422 RID: 1058 RVA: 0x0009FA38 File Offset: 0x0009DC38
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

	// Token: 0x040010BC RID: 4284
	public float accFrontale;

	// Token: 0x040010BD RID: 4285
	public float frontaleVelocitàMax;

	// Token: 0x040010BE RID: 4286
	public float frontaleVelocitàMin;

	// Token: 0x040010BF RID: 4287
	public float accBeccheggio;

	// Token: 0x040010C0 RID: 4288
	public float beccheggioVelocitàMax;

	// Token: 0x040010C1 RID: 4289
	public float accImbardata;

	// Token: 0x040010C2 RID: 4290
	public float imbardataVelocitàMax;

	// Token: 0x040010C3 RID: 4291
	public float accRollio;

	// Token: 0x040010C4 RID: 4292
	public float rollioVelocitàMax;

	// Token: 0x040010C5 RID: 4293
	public PhysicMaterial materialeFisico;

	// Token: 0x040010C6 RID: 4294
	public float velocitàFrontaleEffettiva;

	// Token: 0x040010C7 RID: 4295
	private float velocitàRollioEffettiva;

	// Token: 0x040010C8 RID: 4296
	private float velocitàBecchEffettiva;

	// Token: 0x040010C9 RID: 4297
	private float velocitàImbarEffettiva;

	// Token: 0x040010CA RID: 4298
	private GameObject infoNeutreTattica;

	// Token: 0x040010CB RID: 4299
	private GameObject terzaCamera;

	// Token: 0x040010CC RID: 4300
	private bool èInPrimaPersona;

	// Token: 0x040010CD RID: 4301
	private Rigidbody corpoRigido;

	// Token: 0x040010CE RID: 4302
	private float sensibilitàComandi;

	// Token: 0x040010CF RID: 4303
	private int voloInverso;
}
