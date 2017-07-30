using System;
using UnityEngine;

// Token: 0x0200006F RID: 111
public class MOV_Mech : MonoBehaviour
{
	// Token: 0x060004DD RID: 1245 RVA: 0x000AAEB0 File Offset: 0x000A90B0
	private void Start()
	{
		this.infoNeutreTattica = GameObject.FindGameObjectWithTag("InfoNeutreTattica");
		this.terzaCamera = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaCamere[2];
		this.limiteVelocità = base.GetComponent<NavMeshAgent>().speed;
		this.suonoTorretta = this.torretta.GetComponent<AudioSource>();
		this.suonoMotore = base.GetComponent<AudioSource>();
		this.mechAnim = base.GetComponent<Animator>();
	}

	// Token: 0x060004DE RID: 1246 RVA: 0x000AAF24 File Offset: 0x000A9124
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
		}
	}

	// Token: 0x060004DF RID: 1247 RVA: 0x000AAF7C File Offset: 0x000A917C
	private void FixedUpdate()
	{
		if (base.gameObject == this.terzaCamera.GetComponent<TerzaCamera>().ospiteCamera)
		{
			this.MovimentoInPrimaPersona();
		}
	}

	// Token: 0x060004E0 RID: 1248 RVA: 0x000AAFB0 File Offset: 0x000A91B0
	private void GestioneSuoni()
	{
		if (base.gameObject == this.terzaCamera.GetComponent<TerzaCamera>().ospiteCamera)
		{
			this.suonoMotore.volume = base.GetComponent<ATT_Mech>().volumeMotoreIniziale;
			if ((Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S)) && !this.inMovimento)
			{
				this.suonoMotore.clip = this.motoreViaggio;
				this.suonoMotore.Play();
				this.inMovimento = true;
				this.fermo = false;
			}
			if (!Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S) && !this.fermo)
			{
				this.suonoMotore.clip = this.motoreFermo;
				this.suonoMotore.Play();
				this.inMovimento = false;
				this.fermo = true;
			}
		}
	}

	// Token: 0x060004E1 RID: 1249 RVA: 0x000AB090 File Offset: 0x000A9290
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
		if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S))
		{
			this.mechAnim.SetBool(this.camminataHash, true);
		}
		if (!Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S))
		{
			this.mechAnim.SetBool(this.camminataHash, false);
		}
		float num = Vector3.Dot(this.baseArma1e2.transform.forward, base.transform.up);
		float axis = Input.GetAxis("Mouse X");
		this.torretta.transform.Rotate(0f, axis, 0f);
		float num2 = -Input.GetAxis("Mouse Y");
		if (!this.terzaCamera.GetComponent<TerzaCamera>().èFPS)
		{
			if (num > this.angCannoniVertMin && num2 > 0f)
			{
				this.baseArma1e2.transform.Rotate(num2, 0f, 0f);
				this.baseArma3.transform.Rotate(num2, 0f, 0f);
			}
			if (num < this.angCannoniVertMax && num2 < 0f)
			{
				this.baseArma1e2.transform.Rotate(num2, 0f, 0f);
				this.baseArma3.transform.Rotate(num2, 0f, 0f);
			}
		}
		else
		{
			if (num > this.angCannoniVertMin && num2 > 0f)
			{
				this.baseArma1e2.transform.Rotate(num2, 0f, 0f);
				this.baseArma3.transform.Rotate(num2, 0f, 0f);
				this.terzaCamera.transform.Rotate(num2, 0f, 0f);
			}
			if (num < this.angCannoniVertMax && num2 < 0f)
			{
				this.baseArma1e2.transform.Rotate(num2, 0f, 0f);
				this.baseArma3.transform.Rotate(num2, 0f, 0f);
				this.terzaCamera.transform.Rotate(num2, 0f, 0f);
			}
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

	// Token: 0x040012A9 RID: 4777
	public float velocitàRotazioneMezzo;

	// Token: 0x040012AA RID: 4778
	public float velocitàRotazioneTorretta;

	// Token: 0x040012AB RID: 4779
	public float velocitàRotazioneCannoni;

	// Token: 0x040012AC RID: 4780
	public float angCannoniVertMin;

	// Token: 0x040012AD RID: 4781
	public float angCannoniVertMax;

	// Token: 0x040012AE RID: 4782
	public GameObject torretta;

	// Token: 0x040012AF RID: 4783
	public GameObject baseArma1e2;

	// Token: 0x040012B0 RID: 4784
	public GameObject baseArma3;

	// Token: 0x040012B1 RID: 4785
	private GameObject infoNeutreTattica;

	// Token: 0x040012B2 RID: 4786
	private GameObject terzaCamera;

	// Token: 0x040012B3 RID: 4787
	private bool èInPrimaPersona;

	// Token: 0x040012B4 RID: 4788
	private float limiteVelocità;

	// Token: 0x040012B5 RID: 4789
	private AudioSource suonoTorretta;

	// Token: 0x040012B6 RID: 4790
	private AudioSource suonoMotore;

	// Token: 0x040012B7 RID: 4791
	public AudioClip motoreFermo;

	// Token: 0x040012B8 RID: 4792
	public AudioClip motoreViaggio;

	// Token: 0x040012B9 RID: 4793
	private bool inMovimento;

	// Token: 0x040012BA RID: 4794
	private bool fermo;

	// Token: 0x040012BB RID: 4795
	public bool suonoTorrettaPartito;

	// Token: 0x040012BC RID: 4796
	private Rigidbody corpoRigido;

	// Token: 0x040012BD RID: 4797
	private Animator mechAnim;

	// Token: 0x040012BE RID: 4798
	private int camminataHash = Animator.StringToHash("camminata-attivata");
}
