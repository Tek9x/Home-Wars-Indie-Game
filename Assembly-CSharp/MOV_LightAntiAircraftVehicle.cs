using System;
using UnityEngine;

// Token: 0x0200007F RID: 127
public class MOV_LightAntiAircraftVehicle : MonoBehaviour
{
	// Token: 0x0600053F RID: 1343 RVA: 0x000AF544 File Offset: 0x000AD744
	private void Start()
	{
		this.infoNeutreTattica = GameObject.FindGameObjectWithTag("InfoNeutreTattica");
		this.terzaCamera = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaCamere[2];
		this.limiteVelocità = base.GetComponent<NavMeshAgent>().speed;
		this.suonoTorretta = this.torretta.GetComponent<AudioSource>();
		this.suonoMotore = base.GetComponent<AudioSource>();
		this.primaPartenza = true;
	}

	// Token: 0x06000540 RID: 1344 RVA: 0x000AF5B4 File Offset: 0x000AD7B4
	private void Update()
	{
		if (base.gameObject != this.terzaCamera.GetComponent<TerzaCamera>().ospiteCamera && base.GetComponent<Rigidbody>())
		{
			UnityEngine.Object.Destroy(base.gameObject.GetComponent<Rigidbody>());
		}
		this.GestioneSuoni();
	}

	// Token: 0x06000541 RID: 1345 RVA: 0x000AF608 File Offset: 0x000AD808
	private void FixedUpdate()
	{
		if (base.gameObject == this.terzaCamera.GetComponent<TerzaCamera>().ospiteCamera)
		{
			this.MovimentoInPrimaPersona();
		}
	}

	// Token: 0x06000542 RID: 1346 RVA: 0x000AF63C File Offset: 0x000AD83C
	private void GestioneSuoni()
	{
		if (base.gameObject == this.terzaCamera.GetComponent<TerzaCamera>().ospiteCamera)
		{
			this.suonoMotore.volume = base.GetComponent<ATT_LightAntiAircraftVehicle>().volumeMotoreIniziale;
			if (Input.GetKeyUp(KeyCode.Q))
			{
				this.suonoMotore.clip = this.motoreFermo;
				this.suonoMotore.Play();
				base.GetComponent<ATT_LightAntiAircraftVehicle>().stopFinito = false;
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

	// Token: 0x06000543 RID: 1347 RVA: 0x000AF844 File Offset: 0x000ADA44
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

	// Token: 0x040013C3 RID: 5059
	public float velocitàRotazioneMezzo;

	// Token: 0x040013C4 RID: 5060
	public float velocitàRotazioneTorretta;

	// Token: 0x040013C5 RID: 5061
	public float velocitàRotazioneCannoni;

	// Token: 0x040013C6 RID: 5062
	public float angCannoniVertMin;

	// Token: 0x040013C7 RID: 5063
	public float angCannoniVertMax;

	// Token: 0x040013C8 RID: 5064
	public GameObject torretta;

	// Token: 0x040013C9 RID: 5065
	public GameObject cannoni;

	// Token: 0x040013CA RID: 5066
	private GameObject infoNeutreTattica;

	// Token: 0x040013CB RID: 5067
	private GameObject terzaCamera;

	// Token: 0x040013CC RID: 5068
	private float limiteVelocità;

	// Token: 0x040013CD RID: 5069
	private AudioSource suonoTorretta;

	// Token: 0x040013CE RID: 5070
	private AudioSource suonoMotore;

	// Token: 0x040013CF RID: 5071
	public AudioClip motoreFermo;

	// Token: 0x040013D0 RID: 5072
	public AudioClip motorePartenza;

	// Token: 0x040013D1 RID: 5073
	public AudioClip motoreViaggio;

	// Token: 0x040013D2 RID: 5074
	public AudioClip motoreStop;

	// Token: 0x040013D3 RID: 5075
	private float timerPartenza;

	// Token: 0x040013D4 RID: 5076
	private float timerStop;

	// Token: 0x040013D5 RID: 5077
	private bool primaPartenza;

	// Token: 0x040013D6 RID: 5078
	private bool inPartenza;

	// Token: 0x040013D7 RID: 5079
	private bool partenzaFinita;

	// Token: 0x040013D8 RID: 5080
	private bool inStop;

	// Token: 0x040013D9 RID: 5081
	private bool stopFinito;

	// Token: 0x040013DA RID: 5082
	public bool suonoTorrettaPartito;

	// Token: 0x040013DB RID: 5083
	private Rigidbody corpoRigido;
}
