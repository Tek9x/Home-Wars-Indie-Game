using System;
using UnityEngine;

// Token: 0x0200007A RID: 122
public class MOV_AssaultVehicle : MonoBehaviour
{
	// Token: 0x06000520 RID: 1312 RVA: 0x000AD710 File Offset: 0x000AB910
	private void Start()
	{
		this.infoNeutreTattica = GameObject.FindGameObjectWithTag("InfoNeutreTattica");
		this.terzaCamera = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaCamere[2];
		this.limiteVelocità = base.GetComponent<NavMeshAgent>().speed;
		this.suonoTorretta = this.torretta.GetComponent<AudioSource>();
		this.suonoMotore = base.GetComponent<AudioSource>();
		this.primaPartenza = true;
		this.ruote1 = base.transform.GetChild(1).transform.GetChild(1).gameObject;
		this.ruote2 = base.transform.GetChild(1).transform.GetChild(2).gameObject;
		this.ruote3 = base.transform.GetChild(1).transform.GetChild(3).gameObject;
		this.ruote4 = base.transform.GetChild(1).transform.GetChild(4).gameObject;
	}

	// Token: 0x06000521 RID: 1313 RVA: 0x000AD808 File Offset: 0x000ABA08
	private void Update()
	{
		if (base.gameObject != this.terzaCamera.GetComponent<TerzaCamera>().ospiteCamera)
		{
			if (base.GetComponent<Rigidbody>())
			{
				UnityEngine.Object.Destroy(base.gameObject.GetComponent<Rigidbody>());
			}
		}
		else
		{
			this.GestioneSuoni();
			this.GestioneRuote();
		}
	}

	// Token: 0x06000522 RID: 1314 RVA: 0x000AD868 File Offset: 0x000ABA68
	private void FixedUpdate()
	{
		if (base.gameObject == this.terzaCamera.GetComponent<TerzaCamera>().ospiteCamera)
		{
			this.MovimentoInPrimaPersona();
		}
	}

	// Token: 0x06000523 RID: 1315 RVA: 0x000AD89C File Offset: 0x000ABA9C
	private void GestioneSuoni()
	{
		if (base.gameObject == this.terzaCamera.GetComponent<TerzaCamera>().ospiteCamera)
		{
			this.suonoMotore.volume = base.GetComponent<ATT_AssaultVehicle>().volumeMotoreIniziale;
			if (Input.GetKeyUp(KeyCode.Q))
			{
				this.suonoMotore.clip = this.motoreFermo;
				this.suonoMotore.Play();
				base.GetComponent<ATT_AssaultVehicle>().stopFinito = false;
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

	// Token: 0x06000524 RID: 1316 RVA: 0x000ADAA4 File Offset: 0x000ABCA4
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
			if (Input.GetKey(KeyCode.A))
			{
				this.corpoRigido.angularVelocity = new Vector3(0f, -this.velocitàRotazioneMezzo, 0f);
			}
			if (Input.GetKey(KeyCode.D))
			{
				this.corpoRigido.angularVelocity = new Vector3(0f, this.velocitàRotazioneMezzo, 0f);
			}
		}
		if (Input.GetKey(KeyCode.S) && magnitude < this.limiteVelocità)
		{
			this.corpoRigido.AddForce(-base.transform.forward * 500f, ForceMode.Force);
			if (Input.GetKey(KeyCode.A))
			{
				this.corpoRigido.angularVelocity = new Vector3(0f, -this.velocitàRotazioneMezzo, 0f);
			}
			if (Input.GetKey(KeyCode.D))
			{
				this.corpoRigido.angularVelocity = new Vector3(0f, this.velocitàRotazioneMezzo, 0f);
			}
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

	// Token: 0x06000525 RID: 1317 RVA: 0x000ADD88 File Offset: 0x000ABF88
	private void GestioneRuote()
	{
		if (Input.GetKey(KeyCode.W))
		{
			this.ruote1.transform.Rotate(Vector3.right * 3f);
			this.ruote2.transform.Rotate(Vector3.right * 3f);
			this.ruote3.transform.Rotate(-Vector3.right * 3f);
			this.ruote4.transform.Rotate(-Vector3.right * 3f);
		}
		if (Input.GetKey(KeyCode.S))
		{
			this.ruote1.transform.Rotate(-Vector3.right * 3f);
			this.ruote2.transform.Rotate(-Vector3.right * 3f);
			this.ruote3.transform.Rotate(-Vector3.right * 3f);
			this.ruote4.transform.Rotate(-Vector3.right * 3f);
		}
	}

	// Token: 0x04001341 RID: 4929
	public float velocitàRotazioneMezzo;

	// Token: 0x04001342 RID: 4930
	public float velocitàRotazioneTorretta;

	// Token: 0x04001343 RID: 4931
	public float velocitàRotazioneCannoni;

	// Token: 0x04001344 RID: 4932
	public float angCannoniVertMin;

	// Token: 0x04001345 RID: 4933
	public float angCannoniVertMax;

	// Token: 0x04001346 RID: 4934
	public GameObject torretta;

	// Token: 0x04001347 RID: 4935
	public GameObject cannoni;

	// Token: 0x04001348 RID: 4936
	private GameObject infoNeutreTattica;

	// Token: 0x04001349 RID: 4937
	private GameObject terzaCamera;

	// Token: 0x0400134A RID: 4938
	private float limiteVelocità;

	// Token: 0x0400134B RID: 4939
	private AudioSource suonoTorretta;

	// Token: 0x0400134C RID: 4940
	private AudioSource suonoMotore;

	// Token: 0x0400134D RID: 4941
	public AudioClip motoreFermo;

	// Token: 0x0400134E RID: 4942
	public AudioClip motorePartenza;

	// Token: 0x0400134F RID: 4943
	public AudioClip motoreViaggio;

	// Token: 0x04001350 RID: 4944
	public AudioClip motoreStop;

	// Token: 0x04001351 RID: 4945
	private float timerPartenza;

	// Token: 0x04001352 RID: 4946
	private float timerStop;

	// Token: 0x04001353 RID: 4947
	private bool primaPartenza;

	// Token: 0x04001354 RID: 4948
	private bool inPartenza;

	// Token: 0x04001355 RID: 4949
	private bool partenzaFinita;

	// Token: 0x04001356 RID: 4950
	private bool inStop;

	// Token: 0x04001357 RID: 4951
	private bool stopFinito;

	// Token: 0x04001358 RID: 4952
	public bool suonoTorrettaPartito;

	// Token: 0x04001359 RID: 4953
	private GameObject ruote1;

	// Token: 0x0400135A RID: 4954
	private GameObject ruote2;

	// Token: 0x0400135B RID: 4955
	private GameObject ruote3;

	// Token: 0x0400135C RID: 4956
	private GameObject ruote4;

	// Token: 0x0400135D RID: 4957
	private Rigidbody corpoRigido;
}
