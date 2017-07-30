using System;
using UnityEngine;

// Token: 0x0200007D RID: 125
public class MOV_HeavyFightingVehicle : MonoBehaviour
{
	// Token: 0x06000533 RID: 1331 RVA: 0x000AEA24 File Offset: 0x000ACC24
	private void Start()
	{
		this.infoNeutreTattica = GameObject.FindGameObjectWithTag("InfoNeutreTattica");
		this.terzaCamera = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaCamere[2];
		this.limiteVelocità = base.GetComponent<NavMeshAgent>().speed;
		this.suonoTorretta = this.torretta.GetComponent<AudioSource>();
		this.suonoMotore = base.GetComponent<AudioSource>();
		this.primaPartenza = true;
	}

	// Token: 0x06000534 RID: 1332 RVA: 0x000AEA94 File Offset: 0x000ACC94
	private void Update()
	{
		if (base.gameObject != this.terzaCamera.GetComponent<TerzaCamera>().ospiteCamera && base.GetComponent<Rigidbody>())
		{
			UnityEngine.Object.Destroy(base.gameObject.GetComponent<Rigidbody>());
		}
		this.GestioneSuoni();
	}

	// Token: 0x06000535 RID: 1333 RVA: 0x000AEAE8 File Offset: 0x000ACCE8
	private void FixedUpdate()
	{
		if (base.gameObject == this.terzaCamera.GetComponent<TerzaCamera>().ospiteCamera)
		{
			this.MovimentoInPrimaPersona();
		}
	}

	// Token: 0x06000536 RID: 1334 RVA: 0x000AEB1C File Offset: 0x000ACD1C
	private void GestioneSuoni()
	{
		if (base.gameObject == this.terzaCamera.GetComponent<TerzaCamera>().ospiteCamera)
		{
			this.suonoMotore.volume = base.GetComponent<ATT_HeavyFightingVehicle>().volumeMotoreIniziale;
			if (Input.GetKeyUp(KeyCode.Q))
			{
				this.suonoMotore.clip = this.motoreFermo;
				this.suonoMotore.Play();
				base.GetComponent<ATT_HeavyFightingVehicle>().stopFinito = false;
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

	// Token: 0x06000537 RID: 1335 RVA: 0x000AED24 File Offset: 0x000ACF24
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

	// Token: 0x04001391 RID: 5009
	public float velocitàRotazioneMezzo;

	// Token: 0x04001392 RID: 5010
	public float velocitàRotazioneTorretta;

	// Token: 0x04001393 RID: 5011
	public float velocitàRotazioneCannoni;

	// Token: 0x04001394 RID: 5012
	public float angCannoniVertMin;

	// Token: 0x04001395 RID: 5013
	public float angCannoniVertMax;

	// Token: 0x04001396 RID: 5014
	public GameObject torretta;

	// Token: 0x04001397 RID: 5015
	public GameObject cannoni;

	// Token: 0x04001398 RID: 5016
	private GameObject infoNeutreTattica;

	// Token: 0x04001399 RID: 5017
	private GameObject terzaCamera;

	// Token: 0x0400139A RID: 5018
	private float limiteVelocità;

	// Token: 0x0400139B RID: 5019
	private AudioSource suonoTorretta;

	// Token: 0x0400139C RID: 5020
	private AudioSource suonoMotore;

	// Token: 0x0400139D RID: 5021
	public AudioClip motoreFermo;

	// Token: 0x0400139E RID: 5022
	public AudioClip motorePartenza;

	// Token: 0x0400139F RID: 5023
	public AudioClip motoreViaggio;

	// Token: 0x040013A0 RID: 5024
	public AudioClip motoreStop;

	// Token: 0x040013A1 RID: 5025
	private float timerPartenza;

	// Token: 0x040013A2 RID: 5026
	private float timerStop;

	// Token: 0x040013A3 RID: 5027
	private bool primaPartenza;

	// Token: 0x040013A4 RID: 5028
	private bool inPartenza;

	// Token: 0x040013A5 RID: 5029
	private bool partenzaFinita;

	// Token: 0x040013A6 RID: 5030
	private bool inStop;

	// Token: 0x040013A7 RID: 5031
	private bool stopFinito;

	// Token: 0x040013A8 RID: 5032
	public bool suonoTorrettaPartito;

	// Token: 0x040013A9 RID: 5033
	private Rigidbody corpoRigido;
}
