using System;
using UnityEngine;

// Token: 0x02000055 RID: 85
public class MOV_InterceptorAircraft : MonoBehaviour
{
	// Token: 0x06000428 RID: 1064 RVA: 0x000A0354 File Offset: 0x0009E554
	private void Start()
	{
		this.infoNeutreTattica = GameObject.FindGameObjectWithTag("InfoNeutreTattica");
		this.terzaCamera = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaCamere[2];
	}

	// Token: 0x06000429 RID: 1065 RVA: 0x000A0390 File Offset: 0x0009E590
	private void Update()
	{
		this.ConfermaControllo();
	}

	// Token: 0x0600042A RID: 1066 RVA: 0x000A0398 File Offset: 0x0009E598
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

	// Token: 0x0600042B RID: 1067 RVA: 0x000A0424 File Offset: 0x0009E624
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

	// Token: 0x0600042C RID: 1068 RVA: 0x000A0480 File Offset: 0x0009E680
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
		if (Input.GetKey(KeyCode.LeftShift))
		{
			this.velocitàFrontaleEffettiva = this.frontaleVelocitàMax * 3f;
			base.GetComponent<PresenzaAlleato>().carburante -= Time.deltaTime * 5f;
		}
		else if (Input.GetKeyUp(KeyCode.LeftShift))
		{
			this.velocitàFrontaleEffettiva = this.frontaleVelocitàMax;
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

	// Token: 0x040010DA RID: 4314
	public float accFrontale;

	// Token: 0x040010DB RID: 4315
	public float frontaleVelocitàMax;

	// Token: 0x040010DC RID: 4316
	public float frontaleVelocitàMin;

	// Token: 0x040010DD RID: 4317
	public float accBeccheggio;

	// Token: 0x040010DE RID: 4318
	public float beccheggioVelocitàMax;

	// Token: 0x040010DF RID: 4319
	public float accImbardata;

	// Token: 0x040010E0 RID: 4320
	public float imbardataVelocitàMax;

	// Token: 0x040010E1 RID: 4321
	public float accRollio;

	// Token: 0x040010E2 RID: 4322
	public float rollioVelocitàMax;

	// Token: 0x040010E3 RID: 4323
	public PhysicMaterial materialeFisico;

	// Token: 0x040010E4 RID: 4324
	public float velocitàFrontaleEffettiva;

	// Token: 0x040010E5 RID: 4325
	private float velocitàRollioEffettiva;

	// Token: 0x040010E6 RID: 4326
	private float velocitàBecchEffettiva;

	// Token: 0x040010E7 RID: 4327
	private float velocitàImbarEffettiva;

	// Token: 0x040010E8 RID: 4328
	private GameObject infoNeutreTattica;

	// Token: 0x040010E9 RID: 4329
	private GameObject terzaCamera;

	// Token: 0x040010EA RID: 4330
	private bool èInPrimaPersona;

	// Token: 0x040010EB RID: 4331
	private Rigidbody corpoRigido;

	// Token: 0x040010EC RID: 4332
	private float sensibilitàComandi;

	// Token: 0x040010ED RID: 4333
	private int voloInverso;
}
