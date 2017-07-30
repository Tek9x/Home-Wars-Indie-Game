using System;
using UnityEngine;

// Token: 0x0200004B RID: 75
public class MOV_AUTOM_InterceptorAircraft : MonoBehaviour
{
	// Token: 0x060003DB RID: 987 RVA: 0x0009A850 File Offset: 0x00098A50
	private void Start()
	{
		this.infoNeutreTattica = GameObject.FindGameObjectWithTag("InfoNeutreTattica");
		this.primaCamera = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaCamere[0];
		this.terzaCamera = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaCamere[2];
		this.velocitàTraslazione = this.velocitàTraslazioneIniziale;
		this.velocitàSlittamento = this.velocitàSlittamentoIniziale;
		this.layerNavigazione = 256;
		this.muoviti = true;
		this.destinazione = new Vector3(0f, 130f, 0f);
		this.puntoDiEntrata = base.transform.position;
	}

	// Token: 0x060003DC RID: 988 RVA: 0x0009A8FC File Offset: 0x00098AFC
	private void Update()
	{
		this.origineSensori = base.transform.position;
		this.SensoriAnteriori();
		this.SensoriPosteriori();
		if (base.gameObject != this.terzaCamera.GetComponent<TerzaCamera>().ospiteCamera)
		{
			this.target = base.GetComponent<ATT_InterceptorAircraft>().unitàBersaglio;
			if (!base.GetComponent<PresenzaAlleato>().tornaAllaBase)
			{
				if (this.ripetitoreDiAttaccoOrdinato)
				{
					this.ritornoSuBersaglio = true;
					this.ripetitoreDiAttaccoOrdinato = false;
				}
				if (this.target != null && this.target.GetComponent<PresenzaNemico>().insettoVolante && this.ritornoSuBersaglio)
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
							this.destinazione = base.GetComponent<PresenzaAlleato>().destinazioneSenzaNavMesh + Vector3.up * 130f;
						}
						else if (base.GetComponent<PresenzaAlleato>().destinazioneSenzaNavMesh.y >= 200f && base.GetComponent<PresenzaAlleato>().destinazioneSenzaNavMesh.y < 300f)
						{
							this.destinazione = base.GetComponent<PresenzaAlleato>().destinazioneSenzaNavMesh + Vector3.up * 55f;
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

	// Token: 0x060003DD RID: 989 RVA: 0x0009AB7C File Offset: 0x00098D7C
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

	// Token: 0x060003DE RID: 990 RVA: 0x0009AD98 File Offset: 0x00098F98
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

	// Token: 0x060003DF RID: 991 RVA: 0x0009AF40 File Offset: 0x00099140
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

	// Token: 0x060003E0 RID: 992 RVA: 0x0009B1B4 File Offset: 0x000993B4
	private void NavigazioneConTarget()
	{
		Vector3 normalized = (this.destinazione - base.transform.position).normalized;
		float num = Vector3.Distance(base.transform.position, this.destinazione);
		if (num < 80f)
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
		base.transform.position += base.transform.up * (this.slittamentoVerticale1 + this.slittamentoVerticale2) * this.velocitàSlittamento * Time.deltaTime;
		base.transform.position += base.transform.right * (this.slittamentoOrizzontale1 + this.slittamentoOrizzontale2) * this.velocitàSlittamento * Time.deltaTime;
		if (Physics.Raycast(this.origineSensori, base.transform.forward, 10f, this.layerNavigazione))
		{
			base.transform.Rotate(base.transform.up * 1000f * Time.deltaTime);
		}
		base.transform.position += base.transform.forward * this.velocitàTraslazione * Time.deltaTime;
	}

	// Token: 0x060003E1 RID: 993 RVA: 0x0009B3C8 File Offset: 0x000995C8
	private void VirataDiPericolo()
	{
		base.transform.Rotate(base.transform.up * 1000f * Time.deltaTime);
	}

	// Token: 0x060003E2 RID: 994 RVA: 0x0009B400 File Offset: 0x00099600
	private void Rientro()
	{
		float num = Vector3.Distance(base.transform.position, this.puntoDiEntrata);
		if (base.GetComponent<PresenzaAlleato>().tornaAllaBase && num < 50f)
		{
			base.GetComponent<PresenzaAlleato>().ritornoEffettuato = true;
		}
	}

	// Token: 0x04000FF5 RID: 4085
	public float velocitàTraslazioneIniziale;

	// Token: 0x04000FF6 RID: 4086
	private float velocitàTraslazione;

	// Token: 0x04000FF7 RID: 4087
	public float velocitàSlittamentoIniziale;

	// Token: 0x04000FF8 RID: 4088
	private float velocitàSlittamento;

	// Token: 0x04000FF9 RID: 4089
	public float velocitàAutoRotazione;

	// Token: 0x04000FFA RID: 4090
	private Vector3 origineSensori;

	// Token: 0x04000FFB RID: 4091
	public GameObject target;

	// Token: 0x04000FFC RID: 4092
	private int layerNavigazione;

	// Token: 0x04000FFD RID: 4093
	private GameObject infoNeutreTattica;

	// Token: 0x04000FFE RID: 4094
	private GameObject terzaCamera;

	// Token: 0x04000FFF RID: 4095
	private GameObject primaCamera;

	// Token: 0x04001000 RID: 4096
	private RaycastHit hitSensoreCentrale;

	// Token: 0x04001001 RID: 4097
	private RaycastHit hitSensoreCircolareAnteriore;

	// Token: 0x04001002 RID: 4098
	private RaycastHit hitSensoreCircolarePosteriore;

	// Token: 0x04001003 RID: 4099
	private Vector3 destinazione;

	// Token: 0x04001004 RID: 4100
	private bool destinazioneInVista;

	// Token: 0x04001005 RID: 4101
	private int ampiezzaSensoreCircolare;

	// Token: 0x04001006 RID: 4102
	private int numeroRaggiTrue;

	// Token: 0x04001007 RID: 4103
	private float slittamentoVerticale1;

	// Token: 0x04001008 RID: 4104
	private float slittamentoOrizzontale1;

	// Token: 0x04001009 RID: 4105
	private float slittamentoVerticale2;

	// Token: 0x0400100A RID: 4106
	private float slittamentoOrizzontale2;

	// Token: 0x0400100B RID: 4107
	private float timerRotazione;

	// Token: 0x0400100C RID: 4108
	private Vector3 direzioneRaggioLibero;

	// Token: 0x0400100D RID: 4109
	public bool ripetitoreDiAttaccoOrdinato;

	// Token: 0x0400100E RID: 4110
	public bool muoviti;

	// Token: 0x0400100F RID: 4111
	private bool inAttesaDiOrdini;

	// Token: 0x04001010 RID: 4112
	public bool ritornoSuBersaglio;

	// Token: 0x04001011 RID: 4113
	private Vector3 puntoDiEntrata;
}
