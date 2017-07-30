using System;
using UnityEngine;

// Token: 0x0200007B RID: 123
public class MOV_BattleTank : MonoBehaviour
{
	// Token: 0x06000527 RID: 1319 RVA: 0x000ADECC File Offset: 0x000AC0CC
	private void Start()
	{
		this.infoNeutreTattica = GameObject.FindGameObjectWithTag("InfoNeutreTattica");
		this.terzaCamera = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaCamere[2];
		this.limiteVelocità = base.GetComponent<NavMeshAgent>().speed;
		this.suonoTorretta = this.torretta.GetComponent<AudioSource>();
		this.suonoMotore = base.GetComponent<AudioSource>();
		this.primaPartenza = true;
	}

	// Token: 0x06000528 RID: 1320 RVA: 0x000ADF3C File Offset: 0x000AC13C
	private void Update()
	{
		if (base.gameObject != this.terzaCamera.GetComponent<TerzaCamera>().ospiteCamera && base.GetComponent<Rigidbody>())
		{
			UnityEngine.Object.Destroy(base.gameObject.GetComponent<Rigidbody>());
		}
		this.GestioneSuoni();
	}

	// Token: 0x06000529 RID: 1321 RVA: 0x000ADF90 File Offset: 0x000AC190
	private void FixedUpdate()
	{
		if (base.gameObject == this.terzaCamera.GetComponent<TerzaCamera>().ospiteCamera)
		{
			this.MovimentoInPrimaPersona();
		}
	}

	// Token: 0x0600052A RID: 1322 RVA: 0x000ADFC4 File Offset: 0x000AC1C4
	private void GestioneSuoni()
	{
		if (base.gameObject == this.terzaCamera.GetComponent<TerzaCamera>().ospiteCamera)
		{
			this.suonoMotore.volume = base.GetComponent<ATT_BattleTank>().volumeMotoreIniziale;
			if (Input.GetKeyUp(KeyCode.Q))
			{
				this.suonoMotore.clip = this.motoreFermo;
				this.suonoMotore.Play();
				base.GetComponent<ATT_BattleTank>().stopFinito = false;
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

	// Token: 0x0600052B RID: 1323 RVA: 0x000AE1CC File Offset: 0x000AC3CC
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
		float num = Vector3.Dot(this.cannoni1e2.transform.forward, base.transform.up);
		float axis = Input.GetAxis("Mouse X");
		this.torretta.transform.Rotate(0f, axis, 0f);
		float num2 = -Input.GetAxis("Mouse Y");
		if (num > this.angCannoniVertMin && num2 > 0f)
		{
			this.cannoni1e2.transform.Rotate(num2, 0f, 0f);
			this.cannoni3e4.transform.Rotate(num2, 0f, 0f);
		}
		if (num < this.angCannoniVertMax && num2 < 0f)
		{
			this.cannoni1e2.transform.Rotate(num2, 0f, 0f);
			this.cannoni3e4.transform.Rotate(num2, 0f, 0f);
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

	// Token: 0x0400135E RID: 4958
	public float velocitàRotazioneMezzo;

	// Token: 0x0400135F RID: 4959
	public float velocitàRotazioneTorretta;

	// Token: 0x04001360 RID: 4960
	public float velocitàRotazioneCannoni;

	// Token: 0x04001361 RID: 4961
	public float angCannoniVertMin;

	// Token: 0x04001362 RID: 4962
	public float angCannoniVertMax;

	// Token: 0x04001363 RID: 4963
	public GameObject torretta;

	// Token: 0x04001364 RID: 4964
	public GameObject cannoni1e2;

	// Token: 0x04001365 RID: 4965
	public GameObject cannoni3e4;

	// Token: 0x04001366 RID: 4966
	private GameObject infoNeutreTattica;

	// Token: 0x04001367 RID: 4967
	private GameObject terzaCamera;

	// Token: 0x04001368 RID: 4968
	private float limiteVelocità;

	// Token: 0x04001369 RID: 4969
	private AudioSource suonoTorretta;

	// Token: 0x0400136A RID: 4970
	private AudioSource suonoMotore;

	// Token: 0x0400136B RID: 4971
	public AudioClip motoreFermo;

	// Token: 0x0400136C RID: 4972
	public AudioClip motorePartenza;

	// Token: 0x0400136D RID: 4973
	public AudioClip motoreViaggio;

	// Token: 0x0400136E RID: 4974
	public AudioClip motoreStop;

	// Token: 0x0400136F RID: 4975
	private float timerPartenza;

	// Token: 0x04001370 RID: 4976
	private float timerStop;

	// Token: 0x04001371 RID: 4977
	private bool primaPartenza;

	// Token: 0x04001372 RID: 4978
	private bool inPartenza;

	// Token: 0x04001373 RID: 4979
	private bool partenzaFinita;

	// Token: 0x04001374 RID: 4980
	private bool inStop;

	// Token: 0x04001375 RID: 4981
	private bool stopFinito;

	// Token: 0x04001376 RID: 4982
	public bool suonoTorrettaPartito;

	// Token: 0x04001377 RID: 4983
	private Rigidbody corpoRigido;
}
