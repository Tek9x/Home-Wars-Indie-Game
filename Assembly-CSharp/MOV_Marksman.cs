using System;
using UnityEngine;

// Token: 0x0200006E RID: 110
public class MOV_Marksman : MonoBehaviour
{
	// Token: 0x060004D6 RID: 1238 RVA: 0x000AAAB0 File Offset: 0x000A8CB0
	private void Start()
	{
		this.infoNeutreTattica = GameObject.FindGameObjectWithTag("InfoNeutreTattica");
		this.terzaCamera = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaCamere[2];
		this.limiteVelocità = base.GetComponent<NavMeshAgent>().speed;
		this.ossoArmaTransform = this.ossoArma.transform;
	}

	// Token: 0x060004D7 RID: 1239 RVA: 0x000AAB0C File Offset: 0x000A8D0C
	private void Update()
	{
		this.ConfermaControllo();
	}

	// Token: 0x060004D8 RID: 1240 RVA: 0x000AAB14 File Offset: 0x000A8D14
	private void FixedUpdate()
	{
		if (this.èInPrimaPersona && !base.GetComponent<PresenzaAlleato>().èParà)
		{
			this.MovimentoTraslazioni();
		}
	}

	// Token: 0x060004D9 RID: 1241 RVA: 0x000AAB38 File Offset: 0x000A8D38
	private void ConfermaControllo()
	{
		if (base.gameObject == this.terzaCamera.GetComponent<TerzaCamera>().ospiteCamera)
		{
			this.èInPrimaPersona = true;
			this.terzaCameraTransform = this.terzaCamera.transform;
			this.MovimentoRotazioni();
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

	// Token: 0x060004DA RID: 1242 RVA: 0x000AABB0 File Offset: 0x000A8DB0
	private void MovimentoTraslazioni()
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
	}

	// Token: 0x060004DB RID: 1243 RVA: 0x000AAD3C File Offset: 0x000A8F3C
	private void MovimentoRotazioni()
	{
		float axis = Input.GetAxis("Mouse X");
		if (this.terzaCamera.GetComponent<TerzaCamera>().èFPS)
		{
			base.transform.Rotate(0f, axis * this.velocitàRotazOriz / 1.5f * Time.deltaTime, 0f);
		}
		else
		{
			base.transform.Rotate(0f, axis * this.velocitàRotazOriz * Time.deltaTime, 0f);
		}
		float num = 0f;
		float axis2 = Input.GetAxis("Mouse Y");
		float num2 = Vector3.Dot(this.terzaCameraTransform.forward, Vector3.up);
		if (axis2 > 0f && num2 < this.angVertMaxPP)
		{
			num = axis2;
		}
		if (axis2 < 0f && num2 > this.angVertMinPP)
		{
			num = axis2;
		}
		this.ossoArma.transform.Rotate(-num * this.velocitàRotazVert * Time.deltaTime, 0f, 0f);
		if (base.GetComponent<ATT_Marksman>().avviaRinculo)
		{
			this.ossoArma.transform.Rotate(-Vector3.right * base.GetComponent<PresenzaAlleato>().ListaValoriArma1[8] * base.GetComponent<ATT_Marksman>().timerRinculo * Time.deltaTime);
		}
	}

	// Token: 0x0400129C RID: 4764
	public float velocitàRotazOriz;

	// Token: 0x0400129D RID: 4765
	public float velocitàRotazVert;

	// Token: 0x0400129E RID: 4766
	public float angVertMaxPP;

	// Token: 0x0400129F RID: 4767
	public float angVertMinPP;

	// Token: 0x040012A0 RID: 4768
	public GameObject ossoArma;

	// Token: 0x040012A1 RID: 4769
	private Transform ossoArmaTransform;

	// Token: 0x040012A2 RID: 4770
	private GameObject infoNeutreTattica;

	// Token: 0x040012A3 RID: 4771
	private GameObject terzaCamera;

	// Token: 0x040012A4 RID: 4772
	private Transform terzaCameraTransform;

	// Token: 0x040012A5 RID: 4773
	private bool èInPrimaPersona;

	// Token: 0x040012A6 RID: 4774
	private float limiteVelocità;

	// Token: 0x040012A7 RID: 4775
	private float timerRinculo;

	// Token: 0x040012A8 RID: 4776
	private Rigidbody corpoRigido;
}
