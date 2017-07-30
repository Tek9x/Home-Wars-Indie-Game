using System;
using UnityEngine;

// Token: 0x0200004E RID: 78
public class MOV_AUTOM_Spitlead : MonoBehaviour
{
	// Token: 0x060003F7 RID: 1015 RVA: 0x0009CC80 File Offset: 0x0009AE80
	private void Start()
	{
		this.infoAlleati = GameObject.FindGameObjectWithTag("Info Alleati");
		this.VarieMappaLocale = GameObject.FindWithTag("VarieMappaLocale");
		this.centroTraiettoria = new Vector3(this.VarieMappaLocale.GetComponent<VarieMappaLocale>().centroPerGunship.x, base.GetComponent<PresenzaAlleato>().quotaDiPartenza, this.VarieMappaLocale.GetComponent<VarieMappaLocale>().centroPerGunship.z);
		this.raggioTraiettoria = this.VarieMappaLocale.GetComponent<VarieMappaLocale>().raggioInserimentoPerGunship;
		if (this.infoAlleati.GetComponent<InfoGenericheAlleati>().tipoBattaglia == 5)
		{
			this.centroTraiettoria = new Vector3(this.VarieMappaLocale.GetComponent<VarieMappaLocale>().centroPerGunship.x, 200f, this.VarieMappaLocale.GetComponent<VarieMappaLocale>().centroPerGunship.z);
		}
		this.corpoAereo = base.transform.GetChild(2).gameObject;
		this.puntoDiEntrata = base.transform.position;
	}

	// Token: 0x060003F8 RID: 1016 RVA: 0x0009CD7C File Offset: 0x0009AF7C
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
			this.Rientro();
		}
	}

	// Token: 0x060003F9 RID: 1017 RVA: 0x0009CE58 File Offset: 0x0009B058
	private void MovimentoEliche()
	{
		this.elica1.transform.Rotate(Vector3.up * 2000f * Time.deltaTime);
		this.elica2.transform.Rotate(Vector3.up * 2000f * Time.deltaTime);
		this.elica3.transform.Rotate(Vector3.up * 2000f * Time.deltaTime);
		this.elica4.transform.Rotate(Vector3.up * 2000f * Time.deltaTime);
	}

	// Token: 0x060003FA RID: 1018 RVA: 0x0009CF0C File Offset: 0x0009B10C
	private void AvvicinamentoATraiettoria()
	{
		base.transform.LookAt(this.centroTraiettoria);
		base.transform.position += base.transform.forward * this.velocitàTraslazione * Time.deltaTime;
		if (Vector3.Distance(base.transform.position, this.centroTraiettoria) < 10f)
		{
			this.toccoCentro = true;
		}
	}

	// Token: 0x060003FB RID: 1019 RVA: 0x0009CF88 File Offset: 0x0009B188
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

	// Token: 0x060003FC RID: 1020 RVA: 0x0009D0B0 File Offset: 0x0009B2B0
	private void MantenimentoTraiettoria()
	{
		base.transform.RotateAround(this.centroTraiettoria, -Vector3.up, this.velocitàRotazione * Time.deltaTime);
		Vector3 normalized = Vector3.Cross(base.transform.position - this.centroTraiettoria, Vector3.up).normalized;
		Quaternion to = Quaternion.LookRotation(normalized);
		base.transform.rotation = Quaternion.RotateTowards(base.transform.rotation, to, 20f * Time.deltaTime);
	}

	// Token: 0x060003FD RID: 1021 RVA: 0x0009D13C File Offset: 0x0009B33C
	private void Rientro()
	{
		float num = Vector3.Distance(base.transform.position, this.puntoDiEntrata);
		if (base.GetComponent<PresenzaAlleato>().tornaAllaBase && num < 50f)
		{
			base.GetComponent<PresenzaAlleato>().ritornoEffettuato = true;
		}
	}

	// Token: 0x0400104C RID: 4172
	private GameObject infoAlleati;

	// Token: 0x0400104D RID: 4173
	private Vector3 centroTraiettoria;

	// Token: 0x0400104E RID: 4174
	private float raggioTraiettoria;

	// Token: 0x0400104F RID: 4175
	private GameObject VarieMappaLocale;

	// Token: 0x04001050 RID: 4176
	private bool toccoCentro;

	// Token: 0x04001051 RID: 4177
	private bool inTraiettoria;

	// Token: 0x04001052 RID: 4178
	public float velocitàTraslazione;

	// Token: 0x04001053 RID: 4179
	public float velocitàRotazione;

	// Token: 0x04001054 RID: 4180
	public float inclinazioneTraiettoria;

	// Token: 0x04001055 RID: 4181
	private GameObject corpoAereo;

	// Token: 0x04001056 RID: 4182
	public GameObject elica1;

	// Token: 0x04001057 RID: 4183
	public GameObject elica2;

	// Token: 0x04001058 RID: 4184
	public GameObject elica3;

	// Token: 0x04001059 RID: 4185
	public GameObject elica4;

	// Token: 0x0400105A RID: 4186
	private Vector3 puntoDiEntrata;
}
