using System;
using UnityEngine;

// Token: 0x02000071 RID: 113
public class MOV_Observer : MonoBehaviour
{
	// Token: 0x060004E9 RID: 1257 RVA: 0x000AB7C8 File Offset: 0x000A99C8
	private void Start()
	{
		this.infoNeutreTattica = GameObject.FindGameObjectWithTag("InfoNeutreTattica");
		this.terzaCamera = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaCamere[2];
		this.limiteVelocità = base.GetComponent<NavMeshAgent>().speed;
	}

	// Token: 0x060004EA RID: 1258 RVA: 0x000AB814 File Offset: 0x000A9A14
	private void Update()
	{
		this.ConfermaControllo();
	}

	// Token: 0x060004EB RID: 1259 RVA: 0x000AB81C File Offset: 0x000A9A1C
	private void FixedUpdate()
	{
		if (this.èInPrimaPersona && !base.GetComponent<PresenzaAlleato>().èParà)
		{
			this.MovimentoTraslazioni();
		}
	}

	// Token: 0x060004EC RID: 1260 RVA: 0x000AB840 File Offset: 0x000A9A40
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

	// Token: 0x060004ED RID: 1261 RVA: 0x000AB8B8 File Offset: 0x000A9AB8
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

	// Token: 0x060004EE RID: 1262 RVA: 0x000ABA44 File Offset: 0x000A9C44
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
		this.terzaCamera.transform.Rotate(-num * this.velocitàRotazVert * Time.deltaTime, 0f, 0f);
	}

	// Token: 0x040012C8 RID: 4808
	public float velocitàRotazOriz;

	// Token: 0x040012C9 RID: 4809
	public float velocitàRotazVert;

	// Token: 0x040012CA RID: 4810
	public float angVertMaxPP;

	// Token: 0x040012CB RID: 4811
	public float angVertMinPP;

	// Token: 0x040012CC RID: 4812
	private GameObject infoNeutreTattica;

	// Token: 0x040012CD RID: 4813
	private GameObject terzaCamera;

	// Token: 0x040012CE RID: 4814
	private Transform terzaCameraTransform;

	// Token: 0x040012CF RID: 4815
	private bool èInPrimaPersona;

	// Token: 0x040012D0 RID: 4816
	private float limiteVelocità;

	// Token: 0x040012D1 RID: 4817
	private float timerRinculo;

	// Token: 0x040012D2 RID: 4818
	private Rigidbody corpoRigido;
}
