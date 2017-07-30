using System;
using UnityEngine;

// Token: 0x0200006A RID: 106
public class MOV_HeavyRifleman : MonoBehaviour
{
	// Token: 0x060004BA RID: 1210 RVA: 0x000A9B5C File Offset: 0x000A7D5C
	private void Start()
	{
		this.infoNeutreTattica = GameObject.FindGameObjectWithTag("InfoNeutreTattica");
		this.terzaCamera = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaCamere[2];
		this.limiteVelocità = base.GetComponent<NavMeshAgent>().speed;
		this.ossoArmaTransform = this.ossoArma.transform;
	}

	// Token: 0x060004BB RID: 1211 RVA: 0x000A9BB8 File Offset: 0x000A7DB8
	private void Update()
	{
		this.ConfermaControllo();
	}

	// Token: 0x060004BC RID: 1212 RVA: 0x000A9BC0 File Offset: 0x000A7DC0
	private void FixedUpdate()
	{
		if (this.èInPrimaPersona && !base.GetComponent<PresenzaAlleato>().èParà)
		{
			this.MovimentoTraslazioni();
		}
	}

	// Token: 0x060004BD RID: 1213 RVA: 0x000A9BE4 File Offset: 0x000A7DE4
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

	// Token: 0x060004BE RID: 1214 RVA: 0x000A9C5C File Offset: 0x000A7E5C
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

	// Token: 0x060004BF RID: 1215 RVA: 0x000A9DE8 File Offset: 0x000A7FE8
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
		if (base.GetComponent<ATT_HeavyRifleman>().avviaRinculo)
		{
			this.ossoArma.transform.Rotate(-Vector3.right * base.GetComponent<PresenzaAlleato>().ListaValoriArma1[8] * base.GetComponent<ATT_HeavyRifleman>().timerRinculo * Time.deltaTime);
		}
	}

	// Token: 0x0400126A RID: 4714
	public float velocitàRotazOriz;

	// Token: 0x0400126B RID: 4715
	public float velocitàRotazVert;

	// Token: 0x0400126C RID: 4716
	public float angVertMaxPP;

	// Token: 0x0400126D RID: 4717
	public float angVertMinPP;

	// Token: 0x0400126E RID: 4718
	public GameObject ossoArma;

	// Token: 0x0400126F RID: 4719
	private Transform ossoArmaTransform;

	// Token: 0x04001270 RID: 4720
	private GameObject infoNeutreTattica;

	// Token: 0x04001271 RID: 4721
	private GameObject terzaCamera;

	// Token: 0x04001272 RID: 4722
	private Transform terzaCameraTransform;

	// Token: 0x04001273 RID: 4723
	private bool èInPrimaPersona;

	// Token: 0x04001274 RID: 4724
	private float limiteVelocità;

	// Token: 0x04001275 RID: 4725
	private float timerRinculo;

	// Token: 0x04001276 RID: 4726
	private Rigidbody corpoRigido;
}
