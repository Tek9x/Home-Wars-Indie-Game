using System;
using UnityEngine;

// Token: 0x02000066 RID: 102
public class MOV_AssaultTroop : MonoBehaviour
{
	// Token: 0x0600049E RID: 1182 RVA: 0x000A8C64 File Offset: 0x000A6E64
	private void Start()
	{
		this.infoNeutreTattica = GameObject.FindGameObjectWithTag("InfoNeutreTattica");
		this.terzaCamera = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaCamere[2];
		this.limiteVelocità = base.GetComponent<NavMeshAgent>().speed;
		this.ossoArmaTransform = this.ossoArma.transform;
	}

	// Token: 0x0600049F RID: 1183 RVA: 0x000A8CC0 File Offset: 0x000A6EC0
	private void Update()
	{
		this.ConfermaControllo();
	}

	// Token: 0x060004A0 RID: 1184 RVA: 0x000A8CC8 File Offset: 0x000A6EC8
	private void FixedUpdate()
	{
		if (this.èInPrimaPersona && !base.GetComponent<PresenzaAlleato>().èParà)
		{
			this.MovimentoTraslazioni();
		}
	}

	// Token: 0x060004A1 RID: 1185 RVA: 0x000A8CEC File Offset: 0x000A6EEC
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

	// Token: 0x060004A2 RID: 1186 RVA: 0x000A8D64 File Offset: 0x000A6F64
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

	// Token: 0x060004A3 RID: 1187 RVA: 0x000A8EF0 File Offset: 0x000A70F0
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
		if (base.GetComponent<ATT_AssaultTroop>().avviaRinculo)
		{
			this.ossoArma.transform.Rotate(-Vector3.right * base.GetComponent<PresenzaAlleato>().ListaValoriArma1[8] * base.GetComponent<ATT_AssaultTroop>().timerRinculo * Time.deltaTime);
		}
	}

	// Token: 0x04001238 RID: 4664
	public float velocitàRotazOriz;

	// Token: 0x04001239 RID: 4665
	public float velocitàRotazVert;

	// Token: 0x0400123A RID: 4666
	public float angVertMaxPP;

	// Token: 0x0400123B RID: 4667
	public float angVertMinPP;

	// Token: 0x0400123C RID: 4668
	public GameObject ossoArma;

	// Token: 0x0400123D RID: 4669
	private Transform ossoArmaTransform;

	// Token: 0x0400123E RID: 4670
	private GameObject infoNeutreTattica;

	// Token: 0x0400123F RID: 4671
	private GameObject terzaCamera;

	// Token: 0x04001240 RID: 4672
	private Transform terzaCameraTransform;

	// Token: 0x04001241 RID: 4673
	private bool èInPrimaPersona;

	// Token: 0x04001242 RID: 4674
	private float limiteVelocità;

	// Token: 0x04001243 RID: 4675
	private float timerRinculo;

	// Token: 0x04001244 RID: 4676
	private Rigidbody corpoRigido;
}
