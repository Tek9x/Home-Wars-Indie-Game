using System;
using UnityEngine;

// Token: 0x0200006C RID: 108
public class MOV_LightRifleman : MonoBehaviour
{
	// Token: 0x060004C8 RID: 1224 RVA: 0x000AA33C File Offset: 0x000A853C
	private void Start()
	{
		this.infoNeutreTattica = GameObject.FindGameObjectWithTag("InfoNeutreTattica");
		this.terzaCamera = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaCamere[2];
		this.limiteVelocità = base.GetComponent<NavMeshAgent>().speed;
		this.ossoArmaTransform = this.ossoArma.transform;
	}

	// Token: 0x060004C9 RID: 1225 RVA: 0x000AA398 File Offset: 0x000A8598
	private void Update()
	{
		this.ConfermaControllo();
	}

	// Token: 0x060004CA RID: 1226 RVA: 0x000AA3A0 File Offset: 0x000A85A0
	private void FixedUpdate()
	{
		if (this.èInPrimaPersona && !base.GetComponent<PresenzaAlleato>().èParà)
		{
			this.MovimentoTraslazioni();
		}
	}

	// Token: 0x060004CB RID: 1227 RVA: 0x000AA3C4 File Offset: 0x000A85C4
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

	// Token: 0x060004CC RID: 1228 RVA: 0x000AA43C File Offset: 0x000A863C
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

	// Token: 0x060004CD RID: 1229 RVA: 0x000AA5C8 File Offset: 0x000A87C8
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
		if (base.GetComponent<ATT_LightRifleman>().avviaRinculo)
		{
			this.ossoArma.transform.Rotate(-Vector3.right * base.GetComponent<PresenzaAlleato>().ListaValoriArma1[8] * base.GetComponent<ATT_LightRifleman>().timerRinculo * Time.deltaTime);
		}
	}

	// Token: 0x04001284 RID: 4740
	public float velocitàRotazOriz;

	// Token: 0x04001285 RID: 4741
	public float velocitàRotazVert;

	// Token: 0x04001286 RID: 4742
	public float angVertMaxPP;

	// Token: 0x04001287 RID: 4743
	public float angVertMinPP;

	// Token: 0x04001288 RID: 4744
	public GameObject ossoArma;

	// Token: 0x04001289 RID: 4745
	private Transform ossoArmaTransform;

	// Token: 0x0400128A RID: 4746
	private GameObject infoNeutreTattica;

	// Token: 0x0400128B RID: 4747
	private GameObject terzaCamera;

	// Token: 0x0400128C RID: 4748
	private Transform terzaCameraTransform;

	// Token: 0x0400128D RID: 4749
	private bool èInPrimaPersona;

	// Token: 0x0400128E RID: 4750
	private float limiteVelocità;

	// Token: 0x0400128F RID: 4751
	private float timerRinculo;

	// Token: 0x04001290 RID: 4752
	private Rigidbody corpoRigido;
}
