using System;
using UnityEngine;

// Token: 0x02000050 RID: 80
public class MOV_AUTOM_StrikeAircraft : MonoBehaviour
{
	// Token: 0x06000409 RID: 1033 RVA: 0x0009E18C File Offset: 0x0009C38C
	private void Start()
	{
		this.infoNeutreTattica = GameObject.FindGameObjectWithTag("InfoNeutreTattica");
		this.primaCamera = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaCamere[0];
		this.terzaCamera = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaCamere[2];
		this.velocitàTraslazione = this.velocitàTraslazioneIniziale;
		this.velocitàSlittamento = this.velocitàSlittamentoIniziale;
		this.layerNavigazione = 256;
		this.muoviti = true;
		this.destinazione = new Vector3(0f, 100f, 0f);
		this.puntoDiEntrata = base.transform.position;
	}

	// Token: 0x0600040A RID: 1034 RVA: 0x0009E238 File Offset: 0x0009C438
	private void Update()
	{
		this.origineSensori = base.transform.position;
		this.SensoriAnteriori();
		this.SensoriPosteriori();
		if (base.gameObject != this.terzaCamera.GetComponent<TerzaCamera>().ospiteCamera)
		{
			this.target = base.GetComponent<ATT_StrikeAircraft>().unitàBersaglio;
			if (!base.GetComponent<PresenzaAlleato>().tornaAllaBase)
			{
				if (this.ripetitoreDiAttaccoOrdinato)
				{
					this.ritornoSuBersaglio = true;
					this.ripetitoreDiAttaccoOrdinato = false;
				}
				if (this.target != null && !this.target.GetComponent<PresenzaNemico>().insettoVolante && this.ritornoSuBersaglio)
				{
					this.destinazione = this.target.transform.position;
					Quaternion to = Quaternion.LookRotation(this.destinazione - base.transform.position);
					base.transform.rotation = Quaternion.RotateTowards(base.transform.rotation, to, this.velocitàAutoRotazione * Time.deltaTime);
				}
				if (this.target == null)
				{
					if (base.GetComponent<PresenzaAlleato>().destinazioneOrdinata)
					{
						if (base.GetComponent<PresenzaAlleato>().destinazioneSenzaNavMesh.y < 200f)
						{
							this.destinazione = base.GetComponent<PresenzaAlleato>().destinazioneSenzaNavMesh + Vector3.up * 100f;
						}
						else if (base.GetComponent<PresenzaAlleato>().destinazioneSenzaNavMesh.y >= 200f && base.GetComponent<PresenzaAlleato>().destinazioneSenzaNavMesh.y < 300f)
						{
							this.destinazione = base.GetComponent<PresenzaAlleato>().destinazioneSenzaNavMesh + Vector3.up * 50f;
						}
						else
						{
							this.destinazione = base.GetComponent<PresenzaAlleato>().destinazioneSenzaNavMesh + Vector3.up * 30f;
						}
						Quaternion to2 = Quaternion.LookRotation(this.destinazione - base.transform.position);
						base.transform.rotation = Quaternion.RotateTowards(base.transform.rotation, to2, this.velocitàAutoRotazione * Time.deltaTime);
					}
					if (this.muoviti)
					{
						this.NavigazioneSenzaTarget();
					}
				}
				else if (this.muoviti)
				{
					this.NavigazioneConTarget();
				}
			}
			else
			{
				this.Rientro();
				this.destinazione = this.puntoDiEntrata;
				this.NavigazioneSenzaTarget();
			}
		}
	}

	// Token: 0x0600040B RID: 1035 RVA: 0x0009E4B8 File Offset: 0x0009C6B8
	private void SensoriAnteriori()
	{
		if (Physics.Linecast(base.transform.position, this.destinazione, this.layerNavigazione))
		{
			this.destinazioneInVista = false;
		}
		else
		{
			this.destinazioneInVista = true;
		}
		float maxDistance = 200f;
		Quaternion rotation = Quaternion.identity;
		this.numeroRaggiTrue = 8;
		bool flag = false;
		if (Physics.Raycast(base.transform.position, base.transform.forward, out this.hitSensoreCentrale, (float)this.layerNavigazione))
		{
			if (this.destinazioneInVista)
			{
				maxDistance = 30f;
			}
			else
			{
				float num = Vector3.Distance(this.hitSensoreCentrale.point, base.transform.position);
				if (num < 200f)
				{
					maxDistance = num + 30f;
				}
			}
		}
		int num2 = 5;
		while (num2 <= 90 && this.numeroRaggiTrue == 8)
		{
			rotation = Quaternion.AngleAxis((float)num2, base.transform.right);
			this.direzioneRaggioLibero = Vector3.zero;
			float num3 = 99999f;
			this.numeroRaggiTrue = 0;
			for (int i = 0; i < 360; i += 45)
			{
				Quaternion rotation2 = Quaternion.AngleAxis((float)i, base.transform.forward);
				Ray ray = new Ray(this.origineSensori, rotation2 * (rotation * base.transform.forward));
				if (!Physics.Raycast(ray, out this.hitSensoreCircolareAnteriore, maxDistance, this.layerNavigazione))
				{
					float num4 = Vector3.Distance(this.destinazione, ray.GetPoint(50f));
					if (num4 < num3)
					{
						num3 = num4;
						this.direzioneRaggioLibero = rotation2 * (rotation * base.transform.forward);
					}
				}
				else
				{
					this.numeroRaggiTrue++;
				}
				if (!flag && num2 == 45 && i == 0 && Physics.Raycast(ray, out this.hitSensoreCircolareAnteriore, 50f, this.layerNavigazione))
				{
					this.VirataDiPericolo();
					flag = true;
				}
			}
			num2 += 40;
		}
	}

	// Token: 0x0600040C RID: 1036 RVA: 0x0009E6D4 File Offset: 0x0009C8D4
	private void SensoriPosteriori()
	{
		this.slittamentoVerticale1 = 0f;
		this.slittamentoOrizzontale1 = 0f;
		this.slittamentoVerticale2 = 0f;
		this.slittamentoOrizzontale2 = 0f;
		for (int i = 35; i < 95; i += 50)
		{
			Quaternion rotation = Quaternion.AngleAxis((float)i, base.transform.right);
			for (int j = 0; j < 360; j += 90)
			{
				Quaternion rotation2 = Quaternion.AngleAxis((float)j, base.transform.forward);
				Ray ray = new Ray(this.origineSensori, rotation2 * (rotation * base.transform.forward));
				if (Physics.Raycast(ray, out this.hitSensoreCircolarePosteriore, 20f, this.layerNavigazione))
				{
					if (i == 40)
					{
						if (j == 0)
						{
							this.slittamentoVerticale1 = this.velocitàSlittamento;
						}
						else if (j == 90)
						{
							this.slittamentoOrizzontale1 = -this.velocitàSlittamento;
						}
						else if (j == 180)
						{
							this.slittamentoVerticale1 = -this.velocitàSlittamento;
						}
						else if (j == 270)
						{
							this.slittamentoOrizzontale1 = this.velocitàSlittamento;
						}
					}
					if (i == 85)
					{
						if (j == 0)
						{
							this.slittamentoVerticale2 = this.velocitàSlittamento;
						}
						else if (j == 90)
						{
							this.slittamentoOrizzontale2 = -this.velocitàSlittamento;
						}
						else if (j == 180)
						{
							this.slittamentoVerticale2 = -this.velocitàSlittamento;
						}
						else if (j == 270)
						{
							this.slittamentoOrizzontale2 = this.velocitàSlittamento;
						}
					}
				}
			}
		}
	}

	// Token: 0x0600040D RID: 1037 RVA: 0x0009E87C File Offset: 0x0009CA7C
	private void NavigazioneSenzaTarget()
	{
		Vector3 normalized = (this.destinazione - base.transform.position).normalized;
		float num = Vector3.Distance(base.transform.position, this.destinazione);
		if (this.target == null && num < 8f)
		{
			this.inAttesaDiOrdini = true;
			base.GetComponent<PresenzaAlleato>().destinazioneOrdinata = false;
		}
		if (base.GetComponent<PresenzaAlleato>().destinazioneOrdinata)
		{
			this.inAttesaDiOrdini = false;
		}
		if (!this.inAttesaDiOrdini || base.GetComponent<PresenzaAlleato>().tornaAllaBase)
		{
			if (this.destinazioneInVista)
			{
				Quaternion to = Quaternion.LookRotation(this.destinazione - base.transform.position);
				base.transform.rotation = Quaternion.RotateTowards(base.transform.rotation, to, this.velocitàAutoRotazione * Time.deltaTime);
			}
			else
			{
				Quaternion to2 = Quaternion.LookRotation(this.direzioneRaggioLibero);
				base.transform.rotation = Quaternion.RotateTowards(base.transform.rotation, to2, this.velocitàAutoRotazione * Time.deltaTime);
			}
		}
		else
		{
			base.transform.Rotate(base.transform.up * 50f * Time.deltaTime);
		}
		base.transform.position += base.transform.up * (this.slittamentoVerticale1 + this.slittamentoVerticale2) * this.velocitàSlittamento * Time.deltaTime;
		base.transform.position += base.transform.right * (this.slittamentoOrizzontale1 + this.slittamentoOrizzontale2) * this.velocitàSlittamento * Time.deltaTime;
		if (Physics.Raycast(this.origineSensori, base.transform.forward, 10f, this.layerNavigazione))
		{
			base.transform.Rotate(base.transform.up * 1000f * Time.deltaTime);
		}
		base.transform.position += base.transform.forward * this.velocitàTraslazione * Time.deltaTime;
	}

	// Token: 0x0600040E RID: 1038 RVA: 0x0009EAF0 File Offset: 0x0009CCF0
	private void NavigazioneConTarget()
	{
		Vector3 normalized = (this.destinazione - base.transform.position).normalized;
		float num = Vector3.Distance(base.transform.position, this.destinazione);
		if (num < 150f)
		{
			this.ritornoSuBersaglio = false;
		}
		if (num > 300f)
		{
			this.ritornoSuBersaglio = true;
		}
		if (this.ritornoSuBersaglio)
		{
			if (this.destinazioneInVista)
			{
				Quaternion to = Quaternion.LookRotation(this.destinazione - base.transform.position);
				base.transform.rotation = Quaternion.RotateTowards(base.transform.rotation, to, this.velocitàAutoRotazione * Time.deltaTime);
			}
			else
			{
				Quaternion to2 = Quaternion.LookRotation(this.direzioneRaggioLibero);
				base.transform.rotation = Quaternion.RotateTowards(base.transform.rotation, to2, this.velocitàAutoRotazione * Time.deltaTime);
			}
		}
		else
		{
			Vector3 normalized2 = Vector3.ProjectOnPlane(base.transform.forward, Vector3.up).normalized;
			float num2 = Vector3.Dot(normalized2, -base.transform.up);
			if ((double)num2 < 0.4)
			{
				base.transform.Rotate(new Vector3(-20f * Time.deltaTime, 0f, 0f));
			}
		}
		base.transform.position += base.transform.up * (this.slittamentoVerticale1 + this.slittamentoVerticale2) * this.velocitàSlittamento * Time.deltaTime;
		base.transform.position += base.transform.right * (this.slittamentoOrizzontale1 + this.slittamentoOrizzontale2) * this.velocitàSlittamento * Time.deltaTime;
		if (Physics.Raycast(this.origineSensori, base.transform.forward, 10f, this.layerNavigazione))
		{
			base.transform.Rotate(base.transform.up * 1000f * Time.deltaTime);
		}
		base.transform.position += base.transform.forward * this.velocitàTraslazione * Time.deltaTime;
	}

	// Token: 0x0600040F RID: 1039 RVA: 0x0009ED78 File Offset: 0x0009CF78
	private void VirataDiPericolo()
	{
		base.transform.Rotate(base.transform.up * 1000f * Time.deltaTime);
	}

	// Token: 0x06000410 RID: 1040 RVA: 0x0009EDB0 File Offset: 0x0009CFB0
	private void Rientro()
	{
		float num = Vector3.Distance(base.transform.position, this.puntoDiEntrata);
		if (base.GetComponent<PresenzaAlleato>().tornaAllaBase && num < 50f)
		{
			base.GetComponent<PresenzaAlleato>().ritornoEffettuato = true;
		}
	}

	// Token: 0x04001077 RID: 4215
	public float velocitàTraslazioneIniziale;

	// Token: 0x04001078 RID: 4216
	private float velocitàTraslazione;

	// Token: 0x04001079 RID: 4217
	public float velocitàSlittamentoIniziale;

	// Token: 0x0400107A RID: 4218
	private float velocitàSlittamento;

	// Token: 0x0400107B RID: 4219
	public float velocitàAutoRotazione;

	// Token: 0x0400107C RID: 4220
	private Vector3 origineSensori;

	// Token: 0x0400107D RID: 4221
	public GameObject target;

	// Token: 0x0400107E RID: 4222
	private int layerNavigazione;

	// Token: 0x0400107F RID: 4223
	private GameObject infoNeutreTattica;

	// Token: 0x04001080 RID: 4224
	private GameObject terzaCamera;

	// Token: 0x04001081 RID: 4225
	private GameObject primaCamera;

	// Token: 0x04001082 RID: 4226
	private RaycastHit hitSensoreCentrale;

	// Token: 0x04001083 RID: 4227
	private RaycastHit hitSensoreCircolareAnteriore;

	// Token: 0x04001084 RID: 4228
	private RaycastHit hitSensoreCircolarePosteriore;

	// Token: 0x04001085 RID: 4229
	private Vector3 destinazione;

	// Token: 0x04001086 RID: 4230
	private bool destinazioneInVista;

	// Token: 0x04001087 RID: 4231
	private int ampiezzaSensoreCircolare;

	// Token: 0x04001088 RID: 4232
	private int numeroRaggiTrue;

	// Token: 0x04001089 RID: 4233
	private float slittamentoVerticale1;

	// Token: 0x0400108A RID: 4234
	private float slittamentoOrizzontale1;

	// Token: 0x0400108B RID: 4235
	private float slittamentoVerticale2;

	// Token: 0x0400108C RID: 4236
	private float slittamentoOrizzontale2;

	// Token: 0x0400108D RID: 4237
	private float timerRotazione;

	// Token: 0x0400108E RID: 4238
	private Vector3 direzioneRaggioLibero;

	// Token: 0x0400108F RID: 4239
	public bool ripetitoreDiAttaccoOrdinato;

	// Token: 0x04001090 RID: 4240
	public bool muoviti;

	// Token: 0x04001091 RID: 4241
	private bool inAttesaDiOrdini;

	// Token: 0x04001092 RID: 4242
	public bool ritornoSuBersaglio;

	// Token: 0x04001093 RID: 4243
	private Vector3 puntoDiEntrata;
}
