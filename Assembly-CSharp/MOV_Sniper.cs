using System;
using UnityEngine;

// Token: 0x02000074 RID: 116
public class MOV_Sniper : MonoBehaviour
{
	// Token: 0x060004FE RID: 1278 RVA: 0x000AC2D0 File Offset: 0x000AA4D0
	private void Start()
	{
		this.infoNeutreTattica = GameObject.FindGameObjectWithTag("InfoNeutreTattica");
		this.terzaCamera = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaCamere[2];
		this.limiteVelocità = base.GetComponent<NavMeshAgent>().speed;
		this.ossoArmaTransform = this.ossoArma.transform;
	}

	// Token: 0x060004FF RID: 1279 RVA: 0x000AC32C File Offset: 0x000AA52C
	private void Update()
	{
		this.ConfermaControllo();
	}

	// Token: 0x06000500 RID: 1280 RVA: 0x000AC334 File Offset: 0x000AA534
	private void FixedUpdate()
	{
		if (this.èInPrimaPersona && !base.GetComponent<PresenzaAlleato>().èParà)
		{
			this.MovimentoTraslazioni();
		}
	}

	// Token: 0x06000501 RID: 1281 RVA: 0x000AC358 File Offset: 0x000AA558
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

	// Token: 0x06000502 RID: 1282 RVA: 0x000AC3D0 File Offset: 0x000AA5D0
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

	// Token: 0x06000503 RID: 1283 RVA: 0x000AC55C File Offset: 0x000AA75C
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
		if (base.GetComponent<ATT_Sniper>().avviaRinculo)
		{
			this.ossoArma.transform.Rotate(-Vector3.right * base.GetComponent<PresenzaAlleato>().ListaValoriArma1[8] * base.GetComponent<ATT_Sniper>().timerRinculo * Time.deltaTime);
		}
	}

	// Token: 0x040012ED RID: 4845
	public float velocitàRotazOriz;

	// Token: 0x040012EE RID: 4846
	public float velocitàRotazVert;

	// Token: 0x040012EF RID: 4847
	public float angVertMaxPP;

	// Token: 0x040012F0 RID: 4848
	public float angVertMinPP;

	// Token: 0x040012F1 RID: 4849
	public GameObject ossoArma;

	// Token: 0x040012F2 RID: 4850
	private Transform ossoArmaTransform;

	// Token: 0x040012F3 RID: 4851
	private GameObject infoNeutreTattica;

	// Token: 0x040012F4 RID: 4852
	private GameObject terzaCamera;

	// Token: 0x040012F5 RID: 4853
	private Transform terzaCameraTransform;

	// Token: 0x040012F6 RID: 4854
	private bool èInPrimaPersona;

	// Token: 0x040012F7 RID: 4855
	private float limiteVelocità;

	// Token: 0x040012F8 RID: 4856
	private float timerRinculo;

	// Token: 0x040012F9 RID: 4857
	private Rigidbody corpoRigido;
}
