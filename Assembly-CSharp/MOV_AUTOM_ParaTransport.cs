using System;
using UnityEngine;

// Token: 0x0200004D RID: 77
public class MOV_AUTOM_ParaTransport : MonoBehaviour
{
	// Token: 0x060003EE RID: 1006 RVA: 0x0009C450 File Offset: 0x0009A650
	private void Start()
	{
		this.infoNeutreTattica = GameObject.FindGameObjectWithTag("InfoNeutreTattica");
		this.primaCamera = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaCamere[0];
		this.terzaCamera = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaCamere[2];
		this.infoAlleati = GameObject.FindGameObjectWithTag("Info Alleati");
		this.velocitàTraslazione = this.velocitàTraslazioneIniziale;
		this.velocitàSlittamento = this.velocitàSlittamentoIniziale;
		this.layerNavigazione = 256;
		this.destinazione = this.infoAlleati.GetComponent<InfoGenericheAlleati>().puntoDiLancioParà;
		base.transform.forward = (this.destinazione - base.transform.position).normalized;
		this.puntoDiEntrata = base.transform.position;
	}

	// Token: 0x060003EF RID: 1007 RVA: 0x0009C528 File Offset: 0x0009A728
	private void Update()
	{
		this.origineSensori = base.transform.position;
		if (this.rientroAttivo)
		{
			this.destinazione = this.puntoDiEntrata;
		}
		this.MovimentoEliche();
		this.SensoriAnteriori();
		this.SensoriPosteriori();
		this.Navigazione();
		this.Rientro();
	}

	// Token: 0x060003F0 RID: 1008 RVA: 0x0009C57C File Offset: 0x0009A77C
	private void MovimentoEliche()
	{
		this.elica1.transform.Rotate(Vector3.up * 1000f * Time.deltaTime);
		this.elica2.transform.Rotate(Vector3.up * 1000f * Time.deltaTime);
		this.elica3.transform.Rotate(Vector3.up * 1000f * Time.deltaTime);
		this.elica4.transform.Rotate(Vector3.up * 1000f * Time.deltaTime);
	}

	// Token: 0x060003F1 RID: 1009 RVA: 0x0009C630 File Offset: 0x0009A830
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

	// Token: 0x060003F2 RID: 1010 RVA: 0x0009C84C File Offset: 0x0009AA4C
	private void SensoriPosteriori()
	{
		this.slittamentoVerticale1 = 0f;
		this.slittamentoOrizzontale1 = 0f;
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

	// Token: 0x060003F3 RID: 1011 RVA: 0x0009C9DC File Offset: 0x0009ABDC
	private void Navigazione()
	{
		Vector3 a = new Vector3(base.transform.position.x, this.destinazione.y, base.transform.position.z);
		float num = Vector3.Distance(a, this.destinazione);
		if (num < 30f)
		{
			this.lancioAvviato = true;
		}
		if (!this.lancioAvviato || this.rientroAttivo)
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
		base.transform.position += base.transform.up * (this.slittamentoVerticale1 + this.slittamentoVerticale2) * this.velocitàSlittamento * Time.deltaTime;
		base.transform.position += base.transform.right * (this.slittamentoOrizzontale1 + this.slittamentoOrizzontale2) * this.velocitàSlittamento * Time.deltaTime;
		if (Physics.Raycast(this.origineSensori, base.transform.forward, 10f, this.layerNavigazione))
		{
			base.transform.Rotate(base.transform.up * 1000f * Time.deltaTime);
		}
		base.transform.position += base.transform.forward * this.velocitàTraslazione * Time.deltaTime;
	}

	// Token: 0x060003F4 RID: 1012 RVA: 0x0009CBF8 File Offset: 0x0009ADF8
	private void VirataDiPericolo()
	{
		base.transform.Rotate(base.transform.up * 1000f * Time.deltaTime);
	}

	// Token: 0x060003F5 RID: 1013 RVA: 0x0009CC30 File Offset: 0x0009AE30
	private void Rientro()
	{
		float num = Vector3.Distance(base.transform.position, this.puntoDiEntrata);
		if (this.rientroAttivo && num < 100f)
		{
			base.GetComponent<PresenzaAlleato>().ritornoEffettuato = true;
		}
	}

	// Token: 0x0400102E RID: 4142
	public float velocitàTraslazioneIniziale;

	// Token: 0x0400102F RID: 4143
	private float velocitàTraslazione;

	// Token: 0x04001030 RID: 4144
	public float velocitàSlittamentoIniziale;

	// Token: 0x04001031 RID: 4145
	private float velocitàSlittamento;

	// Token: 0x04001032 RID: 4146
	public float velocitàAutoRotazione;

	// Token: 0x04001033 RID: 4147
	private Vector3 origineSensori;

	// Token: 0x04001034 RID: 4148
	private int layerNavigazione;

	// Token: 0x04001035 RID: 4149
	private GameObject infoNeutreTattica;

	// Token: 0x04001036 RID: 4150
	private GameObject terzaCamera;

	// Token: 0x04001037 RID: 4151
	private GameObject primaCamera;

	// Token: 0x04001038 RID: 4152
	private GameObject infoAlleati;

	// Token: 0x04001039 RID: 4153
	private RaycastHit hitSensoreCentrale;

	// Token: 0x0400103A RID: 4154
	private RaycastHit hitSensoreCircolareAnteriore;

	// Token: 0x0400103B RID: 4155
	private RaycastHit hitSensoreCircolarePosteriore;

	// Token: 0x0400103C RID: 4156
	private Vector3 destinazione;

	// Token: 0x0400103D RID: 4157
	private bool destinazioneInVista;

	// Token: 0x0400103E RID: 4158
	private int ampiezzaSensoreCircolare;

	// Token: 0x0400103F RID: 4159
	private int numeroRaggiTrue;

	// Token: 0x04001040 RID: 4160
	private float slittamentoVerticale1;

	// Token: 0x04001041 RID: 4161
	private float slittamentoOrizzontale1;

	// Token: 0x04001042 RID: 4162
	private float slittamentoVerticale2;

	// Token: 0x04001043 RID: 4163
	private float slittamentoOrizzontale2;

	// Token: 0x04001044 RID: 4164
	private Vector3 direzioneRaggioLibero;

	// Token: 0x04001045 RID: 4165
	public GameObject elica1;

	// Token: 0x04001046 RID: 4166
	public GameObject elica2;

	// Token: 0x04001047 RID: 4167
	public GameObject elica3;

	// Token: 0x04001048 RID: 4168
	public GameObject elica4;

	// Token: 0x04001049 RID: 4169
	public bool lancioAvviato;

	// Token: 0x0400104A RID: 4170
	public bool rientroAttivo;

	// Token: 0x0400104B RID: 4171
	private Vector3 puntoDiEntrata;
}
