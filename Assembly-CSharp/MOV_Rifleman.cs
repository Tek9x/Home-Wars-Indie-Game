using System;
using UnityEngine;

// Token: 0x02000072 RID: 114
public class MOV_Rifleman : MonoBehaviour
{
	// Token: 0x060004F0 RID: 1264 RVA: 0x000ABB4C File Offset: 0x000A9D4C
	private void Start()
	{
		this.infoNeutreTattica = GameObject.FindGameObjectWithTag("InfoNeutreTattica");
		this.terzaCamera = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaCamere[2];
		this.limiteVelocità = base.GetComponent<NavMeshAgent>().speed;
		this.ossoArmaTransform = this.ossoArma.transform;
	}

	// Token: 0x060004F1 RID: 1265 RVA: 0x000ABBA8 File Offset: 0x000A9DA8
	private void Update()
	{
		this.ConfermaControllo();
	}

	// Token: 0x060004F2 RID: 1266 RVA: 0x000ABBB0 File Offset: 0x000A9DB0
	private void FixedUpdate()
	{
		if (this.èInPrimaPersona && !base.GetComponent<PresenzaAlleato>().èParà)
		{
			this.MovimentoTraslazioni();
		}
	}

	// Token: 0x060004F3 RID: 1267 RVA: 0x000ABBD4 File Offset: 0x000A9DD4
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

	// Token: 0x060004F4 RID: 1268 RVA: 0x000ABC4C File Offset: 0x000A9E4C
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

	// Token: 0x060004F5 RID: 1269 RVA: 0x000ABDD8 File Offset: 0x000A9FD8
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
		if (base.GetComponent<ATT_Rifleman>().avviaRinculo)
		{
			this.ossoArma.transform.Rotate(-Vector3.right * base.GetComponent<PresenzaAlleato>().ListaValoriArma1[8] * base.GetComponent<ATT_Rifleman>().timerRinculo * Time.deltaTime);
		}
	}

	// Token: 0x040012D3 RID: 4819
	public float velocitàRotazOriz;

	// Token: 0x040012D4 RID: 4820
	public float velocitàRotazVert;

	// Token: 0x040012D5 RID: 4821
	public float angVertMaxPP;

	// Token: 0x040012D6 RID: 4822
	public float angVertMinPP;

	// Token: 0x040012D7 RID: 4823
	public GameObject ossoArma;

	// Token: 0x040012D8 RID: 4824
	private Transform ossoArmaTransform;

	// Token: 0x040012D9 RID: 4825
	private GameObject infoNeutreTattica;

	// Token: 0x040012DA RID: 4826
	private GameObject terzaCamera;

	// Token: 0x040012DB RID: 4827
	private Transform terzaCameraTransform;

	// Token: 0x040012DC RID: 4828
	private bool èInPrimaPersona;

	// Token: 0x040012DD RID: 4829
	private float limiteVelocità;

	// Token: 0x040012DE RID: 4830
	private float timerRinculo;

	// Token: 0x040012DF RID: 4831
	private Rigidbody corpoRigido;
}
