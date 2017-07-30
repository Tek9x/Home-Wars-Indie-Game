using System;
using UnityEngine;

// Token: 0x02000076 RID: 118
public class MOV_150mmHowitzer : MonoBehaviour
{
	// Token: 0x06000508 RID: 1288 RVA: 0x000AC798 File Offset: 0x000AA998
	private void Start()
	{
		this.infoNeutreTattica = GameObject.FindGameObjectWithTag("InfoNeutreTattica");
		this.terzaCamera = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaCamere[2];
		this.limiteVelocità = base.GetComponent<NavMeshAgent>().speed;
		this.ruote1 = base.transform.GetChild(1).transform.GetChild(1).gameObject;
		this.suonoMovimento = base.GetComponent<AudioSource>();
	}

	// Token: 0x06000509 RID: 1289 RVA: 0x000AC810 File Offset: 0x000AAA10
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

	// Token: 0x0600050A RID: 1290 RVA: 0x000AC870 File Offset: 0x000AAA70
	private void FixedUpdate()
	{
		if (base.gameObject == this.terzaCamera.GetComponent<TerzaCamera>().ospiteCamera)
		{
			this.MovimentoInPrimaPersona();
		}
	}

	// Token: 0x0600050B RID: 1291 RVA: 0x000AC8A4 File Offset: 0x000AAAA4
	private void GestioneSuoni()
	{
		if ((Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S)) && !this.inPartenza)
		{
			this.suonoMovimento.Play();
			this.inPartenza = true;
		}
		if (!Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S))
		{
			this.suonoMovimento.Stop();
			this.inPartenza = false;
		}
	}

	// Token: 0x0600050C RID: 1292 RVA: 0x000AC910 File Offset: 0x000AAB10
	private void MovimentoInPrimaPersona()
	{
		if (!base.GetComponent<Rigidbody>())
		{
			base.GetComponent<NavMeshAgent>().enabled = false;
			base.GetComponent<BoxCollider>().isTrigger = false;
			base.gameObject.AddComponent<Rigidbody>();
			this.corpoRigido = base.GetComponent<Rigidbody>();
			this.corpoRigido.constraints = (RigidbodyConstraints)80;
			this.corpoRigido.mass = 10f;
			this.corpoRigido.drag = 0.1f;
			this.corpoRigido.angularDrag = 0.1f;
		}
		float magnitude = this.corpoRigido.velocity.magnitude;
		if (Input.GetKey(KeyCode.W) && magnitude < this.limiteVelocità)
		{
			this.corpoRigido.AddForce(base.transform.forward * 500f, ForceMode.Force);
		}
		if (Input.GetKey(KeyCode.S) && magnitude < this.limiteVelocità)
		{
			this.corpoRigido.AddForce(-base.transform.forward * 500f, ForceMode.Force);
		}
		float axis = Input.GetAxis("Mouse X");
		if (this.terzaCamera.GetComponent<TerzaCamera>().èFPS)
		{
			base.transform.Rotate(0f, axis * this.velocitàRotazione / 1.5f, 0f);
		}
		else
		{
			base.transform.Rotate(0f, axis * this.velocitàRotazione, 0f);
		}
		this.rotazioneSuGiù += Input.GetAxis("Mouse Y") * 0.7f;
		this.rotazioneSuGiù = Mathf.Clamp(this.rotazioneSuGiù, -50f, 40f);
		this.terzaCamera.transform.localEulerAngles = new Vector3(-this.rotazioneSuGiù, 0f, 0f);
	}

	// Token: 0x0600050D RID: 1293 RVA: 0x000ACAE8 File Offset: 0x000AACE8
	private void GestioneRuote()
	{
		if (Input.GetKey(KeyCode.W))
		{
			this.ruote1.transform.Rotate(Vector3.right * 2.5f);
		}
		if (Input.GetKey(KeyCode.S))
		{
			this.ruote1.transform.Rotate(-Vector3.right * 2.5f);
		}
	}

	// Token: 0x040012FB RID: 4859
	public float velocitàRotazione;

	// Token: 0x040012FC RID: 4860
	public float angVertMin;

	// Token: 0x040012FD RID: 4861
	public float angVertMax;

	// Token: 0x040012FE RID: 4862
	private GameObject infoNeutreTattica;

	// Token: 0x040012FF RID: 4863
	private GameObject terzaCamera;

	// Token: 0x04001300 RID: 4864
	private bool èInPrimaPersona;

	// Token: 0x04001301 RID: 4865
	private float rotazioneSuGiù;

	// Token: 0x04001302 RID: 4866
	private float limiteVelocità;

	// Token: 0x04001303 RID: 4867
	private Rigidbody corpoRigido;

	// Token: 0x04001304 RID: 4868
	private GameObject ruote1;

	// Token: 0x04001305 RID: 4869
	private AudioSource suonoMovimento;

	// Token: 0x04001306 RID: 4870
	private bool inPartenza;
}
