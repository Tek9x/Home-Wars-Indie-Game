using System;
using UnityEngine;

// Token: 0x02000070 RID: 112
public class MOV_Mortar : MonoBehaviour
{
	// Token: 0x060004E3 RID: 1251 RVA: 0x000AB498 File Offset: 0x000A9698
	private void Start()
	{
		this.infoNeutreTattica = GameObject.FindGameObjectWithTag("InfoNeutreTattica");
		this.terzaCamera = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaCamere[2];
		this.limiteVelocità = base.GetComponent<NavMeshAgent>().speed;
	}

	// Token: 0x060004E4 RID: 1252 RVA: 0x000AB4E4 File Offset: 0x000A96E4
	private void Update()
	{
		this.ConfermaControllo();
	}

	// Token: 0x060004E5 RID: 1253 RVA: 0x000AB4EC File Offset: 0x000A96EC
	private void FixedUpdate()
	{
		if (this.èInPrimaPersona && !base.GetComponent<PresenzaAlleato>().èParà)
		{
			this.MovimentoInPrimaPersona();
		}
	}

	// Token: 0x060004E6 RID: 1254 RVA: 0x000AB510 File Offset: 0x000A9710
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
				UnityEngine.Object.Destroy(base.gameObject.GetComponent<Rigidbody>());
			}
		}
	}

	// Token: 0x060004E7 RID: 1255 RVA: 0x000AB570 File Offset: 0x000A9770
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
		if (Input.GetKey(KeyCode.A) && magnitude < this.limiteVelocità)
		{
			this.corpoRigido.AddForce(-base.transform.right * 500f, ForceMode.Force);
		}
		if (Input.GetKey(KeyCode.D) && magnitude < this.limiteVelocità)
		{
			this.corpoRigido.AddForce(base.transform.right * 500f, ForceMode.Force);
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
		this.rotazioneSuGiù += Input.GetAxis("Mouse Y") * 1.2f;
		this.rotazioneSuGiù = Mathf.Clamp(this.rotazioneSuGiù, -30f, 40f);
		this.terzaCamera.transform.localEulerAngles = new Vector3(-this.rotazioneSuGiù, 0f, 0f);
	}

	// Token: 0x040012BF RID: 4799
	public float velocitàRotazione;

	// Token: 0x040012C0 RID: 4800
	public float angVertMin;

	// Token: 0x040012C1 RID: 4801
	public float angVertMax;

	// Token: 0x040012C2 RID: 4802
	private GameObject infoNeutreTattica;

	// Token: 0x040012C3 RID: 4803
	private GameObject terzaCamera;

	// Token: 0x040012C4 RID: 4804
	private bool èInPrimaPersona;

	// Token: 0x040012C5 RID: 4805
	private float rotazioneSuGiù;

	// Token: 0x040012C6 RID: 4806
	private float limiteVelocità;

	// Token: 0x040012C7 RID: 4807
	private Rigidbody corpoRigido;
}
