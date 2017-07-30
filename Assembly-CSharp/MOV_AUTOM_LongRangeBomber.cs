using System;
using UnityEngine;

// Token: 0x0200004C RID: 76
public class MOV_AUTOM_LongRangeBomber : MonoBehaviour
{
	// Token: 0x060003E4 RID: 996 RVA: 0x0009B454 File Offset: 0x00099654
	private void Start()
	{
		this.infoNeutreTattica = GameObject.FindGameObjectWithTag("InfoNeutreTattica");
		this.primaCamera = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaCamere[0];
		this.terzaCamera = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaCamere[2];
		this.velocitàTraslazione = this.velocitàTraslazioneIniziale;
		this.velocitàSlittamento = this.velocitàSlittamentoIniziale;
		this.layerNavigazione = 256;
		this.destinazione = new Vector3(0f, 200f, 0f);
		this.puntoDiEntrata = base.transform.position;
	}

	// Token: 0x060003E5 RID: 997 RVA: 0x0009B4F8 File Offset: 0x000996F8
	private void Update()
	{
		this.origineSensori = base.transform.position;
		this.SensoriAnteriori();
		this.SensoriPosteriori();
		if (base.gameObject != this.terzaCamera.GetComponent<TerzaCamera>().ospiteCamera)
		{
			this.target = base.GetComponent<ATT_LongRangeBomber>().unitàBersaglio;
			if (!base.GetComponent<PresenzaAlleato>().tornaAllaBase)
			{
				if (this.ripetitoreDiAttaccoOrdinato)
				{
					this.ritornoSuBersaglio = true;
					this.ripetitoreDiAttaccoOrdinato = false;
				}
				if (this.target != null && !this.target.GetComponent<PresenzaNemico>().insettoVolante && this.ritornoSuBersaglio)
				{
					if (base.GetComponent<PresenzaAlleato>().destinazioneSenzaNavMesh.y < 150f)
					{
						this.destinazione = this.target.transform.position + Vector3.up * 220f;
					}
					else if (base.GetComponent<PresenzaAlleato>().destinazioneSenzaNavMesh.y >= 150f && base.GetComponent<PresenzaAlleato>().destinazioneSenzaNavMesh.y < 250f)
					{
						this.destinazione = this.target.transform.position + Vector3.up * 60f;
					}
					else
					{
						this.destinazione = this.target.transform.position + Vector3.up * 30f;
					}
					Quaternion to = Quaternion.LookRotation(this.destinazione - base.transform.position);
					base.transform.rotation = Quaternion.RotateTowards(base.transform.rotation, to, this.velocitàAutoRotazione * Time.deltaTime);
				}
				if (!base.GetComponent<PresenzaAlleato>().attaccoZonaOrdinato)
				{
					if (this.target == null)
					{
						if (base.GetComponent<PresenzaAlleato>().destinazioneOrdinata)
						{
							if (base.GetComponent<PresenzaAlleato>().destinazioneSenzaNavMesh.y < 150f)
							{
								this.destinazione = base.GetComponent<PresenzaAlleato>().destinazioneSenzaNavMesh + Vector3.up * 220f;
							}
							else if (base.GetComponent<PresenzaAlleato>().destinazioneSenzaNavMesh.y >= 150f && base.GetComponent<PresenzaAlleato>().destinazioneSenzaNavMesh.y < 250f)
							{
								this.destinazione = base.GetComponent<PresenzaAlleato>().destinazioneSenzaNavMesh + Vector3.up * 60f;
							}
							else
							{
								this.destinazione = base.GetComponent<PresenzaAlleato>().destinazioneSenzaNavMesh + Vector3.up * 30f;
							}
							Quaternion to2 = Quaternion.LookRotation(this.destinazione - base.transform.position);
							base.transform.rotation = Quaternion.RotateTowards(base.transform.rotation, to2, this.velocitàAutoRotazione * Time.deltaTime);
						}
						this.NavigazioneSenzaTarget();
					}
					else
					{
						this.NavigazioneConTarget();
					}
				}
				else
				{
					if (base.GetComponent<PresenzaAlleato>().attaccoOrdinato || base.GetComponent<PresenzaAlleato>().destinazioneOrdinata)
					{
						base.GetComponent<PresenzaAlleato>().attaccoZonaOrdinato = false;
					}
					Vector3 luogoAttZonaBomb = base.GetComponent<PresenzaAlleato>().luogoAttZonaBomb;
					if (luogoAttZonaBomb.y < 150f)
					{
						this.destinazione = luogoAttZonaBomb + Vector3.up * 220f;
					}
					else if (luogoAttZonaBomb.y >= 150f && luogoAttZonaBomb.y < 250f)
					{
						this.destinazione = luogoAttZonaBomb + Vector3.up * 60f;
					}
					else
					{
						this.destinazione = luogoAttZonaBomb + Vector3.up * 30f;
					}
					Quaternion to3 = Quaternion.LookRotation(this.destinazione - base.transform.position);
					base.transform.rotation = Quaternion.RotateTowards(base.transform.rotation, to3, this.velocitàAutoRotazione * Time.deltaTime);
					this.NavigazioneAttaccoZona();
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

	// Token: 0x060003E6 RID: 998 RVA: 0x0009B948 File Offset: 0x00099B48
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

	// Token: 0x060003E7 RID: 999 RVA: 0x0009BB64 File Offset: 0x00099D64
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

	// Token: 0x060003E8 RID: 1000 RVA: 0x0009BD0C File Offset: 0x00099F0C
	private void NavigazioneSenzaTarget()
	{
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
			base.transform.Rotate(base.transform.up * 20f * Time.deltaTime);
		}
		base.transform.position += base.transform.up * (this.slittamentoVerticale1 + this.slittamentoVerticale2) * this.velocitàSlittamento * Time.deltaTime;
		base.transform.position += base.transform.right * (this.slittamentoOrizzontale1 + this.slittamentoOrizzontale2) * this.velocitàSlittamento * Time.deltaTime;
		if (Physics.Raycast(this.origineSensori, base.transform.forward, 10f, this.layerNavigazione))
		{
			base.transform.Rotate(base.transform.up * 1000f * Time.deltaTime);
		}
		base.transform.position += base.transform.forward * this.velocitàTraslazione * Time.deltaTime;
	}

	// Token: 0x060003E9 RID: 1001 RVA: 0x0009BF60 File Offset: 0x0009A160
	private void NavigazioneConTarget()
	{
		Vector3 a = new Vector3(base.transform.position.x, this.target.transform.position.y, base.transform.position.z);
		float num = Vector3.Distance(a, this.target.transform.position);
		if (num < 20f)
		{
			this.ritornoSuBersaglio = false;
		}
		if (num > 250f)
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
		base.transform.position += base.transform.up * (this.slittamentoVerticale1 + this.slittamentoVerticale2) * this.velocitàSlittamento * Time.deltaTime;
		base.transform.position += base.transform.right * (this.slittamentoOrizzontale1 + this.slittamentoOrizzontale2) * this.velocitàSlittamento * Time.deltaTime;
		if (Physics.Raycast(this.origineSensori, base.transform.forward, 10f, this.layerNavigazione))
		{
			base.transform.Rotate(base.transform.up * 1000f * Time.deltaTime);
		}
		base.transform.position += base.transform.forward * this.velocitàTraslazione * Time.deltaTime;
	}

	// Token: 0x060003EA RID: 1002 RVA: 0x0009C19C File Offset: 0x0009A39C
	private void NavigazioneAttaccoZona()
	{
		Vector3 luogoAttZonaBomb = base.GetComponent<PresenzaAlleato>().luogoAttZonaBomb;
		Vector3 a = new Vector3(base.transform.position.x, luogoAttZonaBomb.y, base.transform.position.z);
		float num = Vector3.Distance(a, luogoAttZonaBomb);
		if (num < 20f)
		{
			this.ritornoSuBersaglio = false;
		}
		if (num > 250f)
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
		base.transform.position += base.transform.up * (this.slittamentoVerticale1 + this.slittamentoVerticale2) * this.velocitàSlittamento * Time.deltaTime;
		base.transform.position += base.transform.right * (this.slittamentoOrizzontale1 + this.slittamentoOrizzontale2) * this.velocitàSlittamento * Time.deltaTime;
		if (Physics.Raycast(this.origineSensori, base.transform.forward, 10f, this.layerNavigazione))
		{
			base.transform.Rotate(base.transform.up * 1000f * Time.deltaTime);
		}
		base.transform.position += base.transform.forward * this.velocitàTraslazione * Time.deltaTime;
	}

	// Token: 0x060003EB RID: 1003 RVA: 0x0009C3C4 File Offset: 0x0009A5C4
	private void VirataDiPericolo()
	{
		base.transform.Rotate(base.transform.up * 1000f * Time.deltaTime);
	}

	// Token: 0x060003EC RID: 1004 RVA: 0x0009C3FC File Offset: 0x0009A5FC
	private void Rientro()
	{
		float num = Vector3.Distance(base.transform.position, this.puntoDiEntrata);
		if (base.GetComponent<PresenzaAlleato>().tornaAllaBase && num < 50f)
		{
			base.GetComponent<PresenzaAlleato>().ritornoEffettuato = true;
		}
	}

	// Token: 0x04001012 RID: 4114
	public float velocitàTraslazioneIniziale;

	// Token: 0x04001013 RID: 4115
	private float velocitàTraslazione;

	// Token: 0x04001014 RID: 4116
	public float velocitàSlittamentoIniziale;

	// Token: 0x04001015 RID: 4117
	private float velocitàSlittamento;

	// Token: 0x04001016 RID: 4118
	public float velocitàAutoRotazione;

	// Token: 0x04001017 RID: 4119
	private Vector3 origineSensori;

	// Token: 0x04001018 RID: 4120
	public GameObject target;

	// Token: 0x04001019 RID: 4121
	private int layerNavigazione;

	// Token: 0x0400101A RID: 4122
	private GameObject infoNeutreTattica;

	// Token: 0x0400101B RID: 4123
	private GameObject terzaCamera;

	// Token: 0x0400101C RID: 4124
	private GameObject primaCamera;

	// Token: 0x0400101D RID: 4125
	private RaycastHit hitSensoreCentrale;

	// Token: 0x0400101E RID: 4126
	private RaycastHit hitSensoreCircolareAnteriore;

	// Token: 0x0400101F RID: 4127
	private RaycastHit hitSensoreCircolarePosteriore;

	// Token: 0x04001020 RID: 4128
	private Vector3 destinazione;

	// Token: 0x04001021 RID: 4129
	private bool destinazioneInVista;

	// Token: 0x04001022 RID: 4130
	private int ampiezzaSensoreCircolare;

	// Token: 0x04001023 RID: 4131
	private int numeroRaggiTrue;

	// Token: 0x04001024 RID: 4132
	private float slittamentoVerticale1;

	// Token: 0x04001025 RID: 4133
	private float slittamentoOrizzontale1;

	// Token: 0x04001026 RID: 4134
	private float slittamentoVerticale2;

	// Token: 0x04001027 RID: 4135
	private float slittamentoOrizzontale2;

	// Token: 0x04001028 RID: 4136
	private float timerRotazione;

	// Token: 0x04001029 RID: 4137
	private Vector3 direzioneRaggioLibero;

	// Token: 0x0400102A RID: 4138
	public bool ripetitoreDiAttaccoOrdinato;

	// Token: 0x0400102B RID: 4139
	private bool inAttesaDiOrdini;

	// Token: 0x0400102C RID: 4140
	public bool ritornoSuBersaglio;

	// Token: 0x0400102D RID: 4141
	private Vector3 puntoDiEntrata;
}
