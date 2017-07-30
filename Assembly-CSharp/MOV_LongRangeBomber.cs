using System;
using UnityEngine;

// Token: 0x02000056 RID: 86
public class MOV_LongRangeBomber : MonoBehaviour
{
	// Token: 0x0600042E RID: 1070 RVA: 0x000A092C File Offset: 0x0009EB2C
	private void Start()
	{
		this.infoNeutreTattica = GameObject.FindGameObjectWithTag("InfoNeutreTattica");
		this.terzaCamera = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaCamere[2];
	}

	// Token: 0x0600042F RID: 1071 RVA: 0x000A0968 File Offset: 0x0009EB68
	private void Update()
	{
		this.ConfermaControllo();
	}

	// Token: 0x06000430 RID: 1072 RVA: 0x000A0970 File Offset: 0x0009EB70
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
				base.transform.position += base.transform.forward * 20f * Time.deltaTime;
			}
		}
	}

	// Token: 0x06000431 RID: 1073 RVA: 0x000A09FC File Offset: 0x0009EBFC
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

	// Token: 0x06000432 RID: 1074 RVA: 0x000A0A58 File Offset: 0x0009EC58
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
		if (!base.GetComponent<ATT_LongRangeBomber>().visualeBombAttiva)
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

	// Token: 0x040010EE RID: 4334
	public float accFrontale;

	// Token: 0x040010EF RID: 4335
	public float frontaleVelocitàMax;

	// Token: 0x040010F0 RID: 4336
	public float frontaleVelocitàMin;

	// Token: 0x040010F1 RID: 4337
	public float accBeccheggio;

	// Token: 0x040010F2 RID: 4338
	public float beccheggioVelocitàMax;

	// Token: 0x040010F3 RID: 4339
	public float accImbardata;

	// Token: 0x040010F4 RID: 4340
	public float imbardataVelocitàMax;

	// Token: 0x040010F5 RID: 4341
	public float accRollio;

	// Token: 0x040010F6 RID: 4342
	public float rollioVelocitàMax;

	// Token: 0x040010F7 RID: 4343
	public PhysicMaterial materialeFisico;

	// Token: 0x040010F8 RID: 4344
	public float velocitàFrontaleEffettiva;

	// Token: 0x040010F9 RID: 4345
	private float velocitàRollioEffettiva;

	// Token: 0x040010FA RID: 4346
	private float velocitàBecchEffettiva;

	// Token: 0x040010FB RID: 4347
	private float velocitàImbarEffettiva;

	// Token: 0x040010FC RID: 4348
	private GameObject infoNeutreTattica;

	// Token: 0x040010FD RID: 4349
	private GameObject terzaCamera;

	// Token: 0x040010FE RID: 4350
	private bool èInPrimaPersona;

	// Token: 0x040010FF RID: 4351
	private Rigidbody corpoRigido;

	// Token: 0x04001100 RID: 4352
	private float sensibilitàComandi;

	// Token: 0x04001101 RID: 4353
	private int voloInverso;
}
