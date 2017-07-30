using System;
using UnityEngine;

// Token: 0x02000052 RID: 82
public class MOV_FighterAircraft : MonoBehaviour
{
	// Token: 0x06000418 RID: 1048 RVA: 0x0009F390 File Offset: 0x0009D590
	private void Start()
	{
		this.infoNeutreTattica = GameObject.FindGameObjectWithTag("InfoNeutreTattica");
		this.terzaCamera = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaCamere[2];
	}

	// Token: 0x06000419 RID: 1049 RVA: 0x0009F3CC File Offset: 0x0009D5CC
	private void Update()
	{
		this.ConfermaControllo();
	}

	// Token: 0x0600041A RID: 1050 RVA: 0x0009F3D4 File Offset: 0x0009D5D4
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

	// Token: 0x0600041B RID: 1051 RVA: 0x0009F460 File Offset: 0x0009D660
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

	// Token: 0x0600041C RID: 1052 RVA: 0x0009F4BC File Offset: 0x0009D6BC
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

	// Token: 0x040010A8 RID: 4264
	public float accFrontale;

	// Token: 0x040010A9 RID: 4265
	public float frontaleVelocitàMax;

	// Token: 0x040010AA RID: 4266
	public float frontaleVelocitàMin;

	// Token: 0x040010AB RID: 4267
	public float accBeccheggio;

	// Token: 0x040010AC RID: 4268
	public float beccheggioVelocitàMax;

	// Token: 0x040010AD RID: 4269
	public float accImbardata;

	// Token: 0x040010AE RID: 4270
	public float imbardataVelocitàMax;

	// Token: 0x040010AF RID: 4271
	public float accRollio;

	// Token: 0x040010B0 RID: 4272
	public float rollioVelocitàMax;

	// Token: 0x040010B1 RID: 4273
	public PhysicMaterial materialeFisico;

	// Token: 0x040010B2 RID: 4274
	public float velocitàFrontaleEffettiva;

	// Token: 0x040010B3 RID: 4275
	private float velocitàRollioEffettiva;

	// Token: 0x040010B4 RID: 4276
	private float velocitàBecchEffettiva;

	// Token: 0x040010B5 RID: 4277
	private float velocitàImbarEffettiva;

	// Token: 0x040010B6 RID: 4278
	private GameObject infoNeutreTattica;

	// Token: 0x040010B7 RID: 4279
	private GameObject terzaCamera;

	// Token: 0x040010B8 RID: 4280
	private bool èInPrimaPersona;

	// Token: 0x040010B9 RID: 4281
	private Rigidbody corpoRigido;

	// Token: 0x040010BA RID: 4282
	private float sensibilitàComandi;

	// Token: 0x040010BB RID: 4283
	private int voloInverso;
}
