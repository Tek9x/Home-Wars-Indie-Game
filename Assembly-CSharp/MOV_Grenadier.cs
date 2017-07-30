using System;
using UnityEngine;

// Token: 0x02000069 RID: 105
public class MOV_Grenadier : MonoBehaviour
{
	// Token: 0x060004B3 RID: 1203 RVA: 0x000A976C File Offset: 0x000A796C
	private void Start()
	{
		this.infoNeutreTattica = GameObject.FindGameObjectWithTag("InfoNeutreTattica");
		this.terzaCamera = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaCamere[2];
		this.limiteVelocità = base.GetComponent<NavMeshAgent>().speed;
		this.ossoArmaTransform = this.ossoArma.transform;
	}

	// Token: 0x060004B4 RID: 1204 RVA: 0x000A97C8 File Offset: 0x000A79C8
	private void Update()
	{
		this.ConfermaControllo();
	}

	// Token: 0x060004B5 RID: 1205 RVA: 0x000A97D0 File Offset: 0x000A79D0
	private void FixedUpdate()
	{
		if (this.èInPrimaPersona && !base.GetComponent<PresenzaAlleato>().èParà)
		{
			this.MovimentoTraslazioni();
		}
	}

	// Token: 0x060004B6 RID: 1206 RVA: 0x000A97F4 File Offset: 0x000A79F4
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

	// Token: 0x060004B7 RID: 1207 RVA: 0x000A986C File Offset: 0x000A7A6C
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

	// Token: 0x060004B8 RID: 1208 RVA: 0x000A99F8 File Offset: 0x000A7BF8
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
		if (base.GetComponent<ATT_Grenadier>().avviaRinculo)
		{
			this.ossoArma.transform.Rotate(-Vector3.right * base.GetComponent<PresenzaAlleato>().ListaValoriArma1[8] * base.GetComponent<ATT_Grenadier>().timerRinculo * Time.deltaTime);
		}
	}

	// Token: 0x0400125D RID: 4701
	public float velocitàRotazOriz;

	// Token: 0x0400125E RID: 4702
	public float velocitàRotazVert;

	// Token: 0x0400125F RID: 4703
	public float angVertMaxPP;

	// Token: 0x04001260 RID: 4704
	public float angVertMinPP;

	// Token: 0x04001261 RID: 4705
	public GameObject ossoArma;

	// Token: 0x04001262 RID: 4706
	private Transform ossoArmaTransform;

	// Token: 0x04001263 RID: 4707
	private GameObject infoNeutreTattica;

	// Token: 0x04001264 RID: 4708
	private GameObject terzaCamera;

	// Token: 0x04001265 RID: 4709
	private Transform terzaCameraTransform;

	// Token: 0x04001266 RID: 4710
	private bool èInPrimaPersona;

	// Token: 0x04001267 RID: 4711
	private float limiteVelocità;

	// Token: 0x04001268 RID: 4712
	private float timerRinculo;

	// Token: 0x04001269 RID: 4713
	private Rigidbody corpoRigido;
}
