using System;
using UnityEngine;

// Token: 0x02000080 RID: 128
public class MOV_LightFightingVehicle : MonoBehaviour
{
	// Token: 0x06000545 RID: 1349 RVA: 0x000AFAD4 File Offset: 0x000ADCD4
	private void Start()
	{
		this.infoNeutreTattica = GameObject.FindGameObjectWithTag("InfoNeutreTattica");
		this.terzaCamera = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaCamere[2];
		this.limiteVelocità = base.GetComponent<NavMeshAgent>().speed;
		this.suonoTorretta = this.torretta.GetComponent<AudioSource>();
		this.suonoMotore = base.GetComponent<AudioSource>();
		this.primaPartenza = true;
	}

	// Token: 0x06000546 RID: 1350 RVA: 0x000AFB44 File Offset: 0x000ADD44
	private void Update()
	{
		if (base.gameObject != this.terzaCamera.GetComponent<TerzaCamera>().ospiteCamera && base.GetComponent<Rigidbody>())
		{
			UnityEngine.Object.Destroy(base.gameObject.GetComponent<Rigidbody>());
		}
		this.GestioneSuoni();
	}

	// Token: 0x06000547 RID: 1351 RVA: 0x000AFB98 File Offset: 0x000ADD98
	private void FixedUpdate()
	{
		if (base.gameObject == this.terzaCamera.GetComponent<TerzaCamera>().ospiteCamera)
		{
			this.MovimentoInPrimaPersona();
		}
	}

	// Token: 0x06000548 RID: 1352 RVA: 0x000AFBCC File Offset: 0x000ADDCC
	private void GestioneSuoni()
	{
		if (base.gameObject == this.terzaCamera.GetComponent<TerzaCamera>().ospiteCamera)
		{
			this.suonoMotore.volume = base.GetComponent<ATT_LightFightingVehicle>().volumeMotoreIniziale;
			if (Input.GetKeyUp(KeyCode.Q))
			{
				this.suonoMotore.clip = this.motoreFermo;
				this.suonoMotore.Play();
				base.GetComponent<ATT_LightFightingVehicle>().stopFinito = false;
			}
			if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S))
			{
				this.timerPartenza += Time.deltaTime;
				this.timerStop = 0f;
				this.primaPartenza = false;
				this.inStop = false;
				this.stopFinito = false;
			}
			if (!this.inPartenza && this.timerPartenza > 0f)
			{
				this.suonoMotore.clip = this.motorePartenza;
				this.suonoMotore.Play();
				this.inPartenza = true;
			}
			if (!this.partenzaFinita && this.timerPartenza > this.motorePartenza.length)
			{
				this.suonoMotore.clip = this.motoreViaggio;
				this.suonoMotore.Play();
				this.partenzaFinita = true;
			}
			if (!Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S))
			{
				this.timerStop += Time.deltaTime;
				this.timerPartenza = 0f;
				this.inPartenza = false;
				this.partenzaFinita = false;
			}
			if (!this.inStop && this.timerStop > 0f)
			{
				this.suonoMotore.clip = this.motoreStop;
				this.suonoMotore.Play();
				this.inStop = true;
			}
			if (!this.stopFinito && this.timerStop > this.motoreStop.length)
			{
				this.suonoMotore.clip = this.motoreFermo;
				this.suonoMotore.Play();
				this.stopFinito = true;
			}
		}
	}

	// Token: 0x06000549 RID: 1353 RVA: 0x000AFDD4 File Offset: 0x000ADFD4
	private void MovimentoInPrimaPersona()
	{
		if (!base.GetComponent<Rigidbody>())
		{
			base.GetComponent<NavMeshAgent>().enabled = false;
			base.gameObject.AddComponent<Rigidbody>();
			this.corpoRigido = base.GetComponent<Rigidbody>();
			this.corpoRigido.constraints = (RigidbodyConstraints)80;
			this.corpoRigido.mass = 10f;
			this.corpoRigido.drag = 0.1f;
			this.corpoRigido.angularDrag = 0.1f;
		}
		float magnitude = this.corpoRigido.velocity.magnitude;
		float magnitude2 = this.corpoRigido.angularVelocity.magnitude;
		if (Input.GetKey(KeyCode.W) && magnitude < this.limiteVelocità)
		{
			this.corpoRigido.AddForce(base.transform.forward * 500f, ForceMode.Force);
		}
		if (Input.GetKey(KeyCode.S) && magnitude < this.limiteVelocità)
		{
			this.corpoRigido.AddForce(-base.transform.forward * 500f, ForceMode.Force);
		}
		if (Input.GetKey(KeyCode.A))
		{
			this.corpoRigido.angularVelocity = new Vector3(0f, -this.velocitàRotazioneMezzo, 0f);
		}
		if (Input.GetKey(KeyCode.D))
		{
			this.corpoRigido.angularVelocity = new Vector3(0f, this.velocitàRotazioneMezzo, 0f);
		}
		float num = Vector3.Dot(this.cannoni.transform.forward, base.transform.up);
		float axis = Input.GetAxis("Mouse X");
		this.torretta.transform.Rotate(0f, axis, 0f);
		float num2 = -Input.GetAxis("Mouse Y");
		if (num > this.angCannoniVertMin && num2 > 0f)
		{
			this.cannoni.transform.Rotate(num2, 0f, 0f);
		}
		if (num < this.angCannoniVertMax && num2 < 0f)
		{
			this.cannoni.transform.Rotate(num2, 0f, 0f);
		}
		if (axis == 0f && num2 == 0f)
		{
			this.suonoTorretta.Stop();
			this.suonoTorrettaPartito = false;
		}
		else if (!this.suonoTorrettaPartito)
		{
			this.suonoTorretta.Play();
			this.suonoTorrettaPartito = true;
		}
	}

	// Token: 0x040013DC RID: 5084
	public float velocitàRotazioneMezzo;

	// Token: 0x040013DD RID: 5085
	public float velocitàRotazioneTorretta;

	// Token: 0x040013DE RID: 5086
	public float velocitàRotazioneCannoni;

	// Token: 0x040013DF RID: 5087
	public float angCannoniVertMin;

	// Token: 0x040013E0 RID: 5088
	public float angCannoniVertMax;

	// Token: 0x040013E1 RID: 5089
	public GameObject torretta;

	// Token: 0x040013E2 RID: 5090
	public GameObject cannoni;

	// Token: 0x040013E3 RID: 5091
	private GameObject infoNeutreTattica;

	// Token: 0x040013E4 RID: 5092
	private GameObject terzaCamera;

	// Token: 0x040013E5 RID: 5093
	private float limiteVelocità;

	// Token: 0x040013E6 RID: 5094
	private AudioSource suonoTorretta;

	// Token: 0x040013E7 RID: 5095
	private AudioSource suonoMotore;

	// Token: 0x040013E8 RID: 5096
	public AudioClip motoreFermo;

	// Token: 0x040013E9 RID: 5097
	public AudioClip motorePartenza;

	// Token: 0x040013EA RID: 5098
	public AudioClip motoreViaggio;

	// Token: 0x040013EB RID: 5099
	public AudioClip motoreStop;

	// Token: 0x040013EC RID: 5100
	private float timerPartenza;

	// Token: 0x040013ED RID: 5101
	private float timerStop;

	// Token: 0x040013EE RID: 5102
	private bool primaPartenza;

	// Token: 0x040013EF RID: 5103
	private bool inPartenza;

	// Token: 0x040013F0 RID: 5104
	private bool partenzaFinita;

	// Token: 0x040013F1 RID: 5105
	private bool inStop;

	// Token: 0x040013F2 RID: 5106
	private bool stopFinito;

	// Token: 0x040013F3 RID: 5107
	public bool suonoTorrettaPartito;

	// Token: 0x040013F4 RID: 5108
	private Rigidbody corpoRigido;
}
