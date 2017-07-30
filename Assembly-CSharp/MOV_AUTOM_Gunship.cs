using System;
using UnityEngine;

// Token: 0x0200004A RID: 74
public class MOV_AUTOM_Gunship : MonoBehaviour
{
	// Token: 0x060003D3 RID: 979 RVA: 0x0009A360 File Offset: 0x00098560
	private void Start()
	{
		this.VarieMappaLocale = GameObject.FindWithTag("VarieMappaLocale");
		this.centroTraiettoria = this.VarieMappaLocale.GetComponent<VarieMappaLocale>().centroPerGunship;
		this.raggioTraiettoria = this.VarieMappaLocale.GetComponent<VarieMappaLocale>().raggioInserimentoPerGunship;
		this.corpoAereo = base.transform.GetChild(2).gameObject;
		this.puntoDiEntrata = base.transform.position;
	}

	// Token: 0x060003D4 RID: 980 RVA: 0x0009A3D4 File Offset: 0x000985D4
	private void Update()
	{
		this.MovimentoEliche();
		if (!base.GetComponent<PresenzaAlleato>().tornaAllaBase)
		{
			if (!this.toccoCentro)
			{
				this.AvvicinamentoATraiettoria();
			}
			else if (!this.inTraiettoria)
			{
				this.FaseAssestamento();
			}
			else
			{
				this.MantenimentoTraiettoria();
			}
		}
		else
		{
			Quaternion to = Quaternion.LookRotation(this.puntoDiEntrata - base.transform.position);
			base.transform.rotation = Quaternion.RotateTowards(base.transform.rotation, to, 30f * Time.deltaTime);
			base.transform.position += base.transform.forward * this.velocitàTraslazione * Time.deltaTime;
			if (this.corpoAereo.transform.localEulerAngles.z > 0f && this.corpoAereo.transform.localEulerAngles.z < 40f)
			{
				this.corpoAereo.transform.Rotate(0f, 0f, -10f * Time.deltaTime);
			}
			this.Rientro();
		}
	}

	// Token: 0x060003D5 RID: 981 RVA: 0x0009A518 File Offset: 0x00098718
	private void MovimentoEliche()
	{
		this.elica1.transform.Rotate(Vector3.up * 1000f * Time.deltaTime);
		this.elica2.transform.Rotate(Vector3.up * 1000f * Time.deltaTime);
		this.elica3.transform.Rotate(Vector3.up * 1000f * Time.deltaTime);
		this.elica4.transform.Rotate(Vector3.up * 1000f * Time.deltaTime);
	}

	// Token: 0x060003D6 RID: 982 RVA: 0x0009A5CC File Offset: 0x000987CC
	private void AvvicinamentoATraiettoria()
	{
		base.transform.LookAt(this.centroTraiettoria);
		base.transform.position += base.transform.forward * this.velocitàTraslazione * Time.deltaTime;
		if (Vector3.Distance(base.transform.position, this.centroTraiettoria) < 10f)
		{
			this.toccoCentro = true;
		}
	}

	// Token: 0x060003D7 RID: 983 RVA: 0x0009A648 File Offset: 0x00098848
	private void FaseAssestamento()
	{
		base.transform.position += base.transform.forward * this.velocitàTraslazione * Time.deltaTime;
		Vector3 normalized = Vector3.ProjectOnPlane(base.transform.forward, Vector3.up).normalized;
		Quaternion to = Quaternion.LookRotation(normalized);
		base.transform.rotation = Quaternion.RotateTowards(base.transform.rotation, to, 10f * Time.deltaTime);
		if (this.corpoAereo.transform.localEulerAngles.z < this.inclinazioneTraiettoria)
		{
			this.corpoAereo.transform.Rotate(0f, 0f, 10f * Time.deltaTime);
		}
		base.transform.Rotate(-Vector3.up * 10f * Time.deltaTime);
		if (Vector3.Distance(base.transform.position, this.centroTraiettoria) > this.raggioTraiettoria)
		{
			this.inTraiettoria = true;
		}
	}

	// Token: 0x060003D8 RID: 984 RVA: 0x0009A770 File Offset: 0x00098970
	private void MantenimentoTraiettoria()
	{
		base.transform.RotateAround(this.centroTraiettoria, -Vector3.up, this.velocitàRotazione * Time.deltaTime);
		Vector3 normalized = Vector3.Cross(base.transform.position - this.centroTraiettoria, Vector3.up).normalized;
		Quaternion to = Quaternion.LookRotation(normalized);
		base.transform.rotation = Quaternion.RotateTowards(base.transform.rotation, to, 20f * Time.deltaTime);
	}

	// Token: 0x060003D9 RID: 985 RVA: 0x0009A7FC File Offset: 0x000989FC
	private void Rientro()
	{
		float num = Vector3.Distance(base.transform.position, this.puntoDiEntrata);
		if (base.GetComponent<PresenzaAlleato>().tornaAllaBase && num < 50f)
		{
			base.GetComponent<PresenzaAlleato>().ritornoEffettuato = true;
		}
	}

	// Token: 0x04000FE7 RID: 4071
	private Vector3 centroTraiettoria;

	// Token: 0x04000FE8 RID: 4072
	private float raggioTraiettoria;

	// Token: 0x04000FE9 RID: 4073
	private GameObject VarieMappaLocale;

	// Token: 0x04000FEA RID: 4074
	private bool toccoCentro;

	// Token: 0x04000FEB RID: 4075
	private bool inTraiettoria;

	// Token: 0x04000FEC RID: 4076
	public float velocitàTraslazione;

	// Token: 0x04000FED RID: 4077
	public float velocitàRotazione;

	// Token: 0x04000FEE RID: 4078
	public float inclinazioneTraiettoria;

	// Token: 0x04000FEF RID: 4079
	private GameObject corpoAereo;

	// Token: 0x04000FF0 RID: 4080
	public GameObject elica1;

	// Token: 0x04000FF1 RID: 4081
	public GameObject elica2;

	// Token: 0x04000FF2 RID: 4082
	public GameObject elica3;

	// Token: 0x04000FF3 RID: 4083
	public GameObject elica4;

	// Token: 0x04000FF4 RID: 4084
	private Vector3 puntoDiEntrata;
}
