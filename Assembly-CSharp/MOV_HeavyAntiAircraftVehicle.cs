using System;
using UnityEngine;

// Token: 0x0200007C RID: 124
public class MOV_HeavyAntiAircraftVehicle : MonoBehaviour
{
	// Token: 0x0600052D RID: 1325 RVA: 0x000AE494 File Offset: 0x000AC694
	private void Start()
	{
		this.infoNeutreTattica = GameObject.FindGameObjectWithTag("InfoNeutreTattica");
		this.terzaCamera = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaCamere[2];
		this.limiteVelocità = base.GetComponent<NavMeshAgent>().speed;
		this.suonoTorretta = this.torretta.GetComponent<AudioSource>();
		this.suonoMotore = base.GetComponent<AudioSource>();
		this.primaPartenza = true;
	}

	// Token: 0x0600052E RID: 1326 RVA: 0x000AE504 File Offset: 0x000AC704
	private void Update()
	{
		if (base.gameObject != this.terzaCamera.GetComponent<TerzaCamera>().ospiteCamera && base.GetComponent<Rigidbody>())
		{
			UnityEngine.Object.Destroy(base.gameObject.GetComponent<Rigidbody>());
		}
		this.GestioneSuoni();
	}

	// Token: 0x0600052F RID: 1327 RVA: 0x000AE558 File Offset: 0x000AC758
	private void FixedUpdate()
	{
		if (base.gameObject == this.terzaCamera.GetComponent<TerzaCamera>().ospiteCamera)
		{
			this.MovimentoInPrimaPersona();
		}
	}

	// Token: 0x06000530 RID: 1328 RVA: 0x000AE58C File Offset: 0x000AC78C
	private void GestioneSuoni()
	{
		if (base.gameObject == this.terzaCamera.GetComponent<TerzaCamera>().ospiteCamera)
		{
			this.suonoMotore.volume = base.GetComponent<ATT_HeavyAntiAircraftVehicle>().volumeMotoreIniziale;
			if (Input.GetKeyUp(KeyCode.Q))
			{
				this.suonoMotore.clip = this.motoreFermo;
				this.suonoMotore.Play();
				base.GetComponent<ATT_HeavyAntiAircraftVehicle>().stopFinito = false;
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

	// Token: 0x06000531 RID: 1329 RVA: 0x000AE794 File Offset: 0x000AC994
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

	// Token: 0x04001378 RID: 4984
	public float velocitàRotazioneMezzo;

	// Token: 0x04001379 RID: 4985
	public float velocitàRotazioneTorretta;

	// Token: 0x0400137A RID: 4986
	public float velocitàRotazioneCannoni;

	// Token: 0x0400137B RID: 4987
	public float angCannoniVertMin;

	// Token: 0x0400137C RID: 4988
	public float angCannoniVertMax;

	// Token: 0x0400137D RID: 4989
	public GameObject torretta;

	// Token: 0x0400137E RID: 4990
	public GameObject cannoni;

	// Token: 0x0400137F RID: 4991
	private GameObject infoNeutreTattica;

	// Token: 0x04001380 RID: 4992
	private GameObject terzaCamera;

	// Token: 0x04001381 RID: 4993
	private float limiteVelocità;

	// Token: 0x04001382 RID: 4994
	private AudioSource suonoTorretta;

	// Token: 0x04001383 RID: 4995
	private AudioSource suonoMotore;

	// Token: 0x04001384 RID: 4996
	public AudioClip motoreFermo;

	// Token: 0x04001385 RID: 4997
	public AudioClip motorePartenza;

	// Token: 0x04001386 RID: 4998
	public AudioClip motoreViaggio;

	// Token: 0x04001387 RID: 4999
	public AudioClip motoreStop;

	// Token: 0x04001388 RID: 5000
	private float timerPartenza;

	// Token: 0x04001389 RID: 5001
	private float timerStop;

	// Token: 0x0400138A RID: 5002
	private bool primaPartenza;

	// Token: 0x0400138B RID: 5003
	private bool inPartenza;

	// Token: 0x0400138C RID: 5004
	private bool partenzaFinita;

	// Token: 0x0400138D RID: 5005
	private bool inStop;

	// Token: 0x0400138E RID: 5006
	private bool stopFinito;

	// Token: 0x0400138F RID: 5007
	public bool suonoTorrettaPartito;

	// Token: 0x04001390 RID: 5008
	private Rigidbody corpoRigido;
}
