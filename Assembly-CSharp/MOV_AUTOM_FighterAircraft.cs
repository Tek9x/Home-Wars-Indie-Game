using System;
using UnityEngine;

// Token: 0x02000048 RID: 72
public class MOV_AUTOM_FighterAircraft : MonoBehaviour
{
	// Token: 0x060003C1 RID: 961 RVA: 0x00098AE4 File Offset: 0x00096CE4
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

	// Token: 0x060003C2 RID: 962 RVA: 0x00098B90 File Offset: 0x00096D90
	private void Update()
	{
		this.origineSensori = base.transform.position;
		this.SensoriAnteriori();
		this.SensoriPosteriori();
		if (base.gameObject != this.terzaCamera.GetComponent<TerzaCamera>().ospiteCamera)
		{
			this.target = base.GetComponent<ATT_FighterAircraft>().unitàBersaglio;
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

	// Token: 0x060003C3 RID: 963 RVA: 0x00098E10 File Offset: 0x00097010
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

	// Token: 0x060003C4 RID: 964 RVA: 0x0009902C File Offset: 0x0009722C
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

	// Token: 0x060003C5 RID: 965 RVA: 0x000991D4 File Offset: 0x000973D4
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

	// Token: 0x060003C6 RID: 966 RVA: 0x00099448 File Offset: 0x00097648
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

	// Token: 0x060003C7 RID: 967 RVA: 0x0009965C File Offset: 0x0009785C
	private void VirataDiPericolo()
	{
		base.transform.Rotate(base.transform.up * 1000f * Time.deltaTime);
	}

	// Token: 0x060003C8 RID: 968 RVA: 0x00099694 File Offset: 0x00097894
	private void Rientro()
	{
		float num = Vector3.Distance(base.transform.position, this.puntoDiEntrata);
		if (base.GetComponent<PresenzaAlleato>().tornaAllaBase && num < 50f)
		{
			base.GetComponent<PresenzaAlleato>().ritornoEffettuato = true;
		}
	}

	// Token: 0x04000FAD RID: 4013
	public float velocitàTraslazioneIniziale;

	// Token: 0x04000FAE RID: 4014
	private float velocitàTraslazione;

	// Token: 0x04000FAF RID: 4015
	public float velocitàSlittamentoIniziale;

	// Token: 0x04000FB0 RID: 4016
	private float velocitàSlittamento;

	// Token: 0x04000FB1 RID: 4017
	public float velocitàAutoRotazione;

	// Token: 0x04000FB2 RID: 4018
	private Vector3 origineSensori;

	// Token: 0x04000FB3 RID: 4019
	public GameObject target;

	// Token: 0x04000FB4 RID: 4020
	private int layerNavigazione;

	// Token: 0x04000FB5 RID: 4021
	private GameObject infoNeutreTattica;

	// Token: 0x04000FB6 RID: 4022
	private GameObject terzaCamera;

	// Token: 0x04000FB7 RID: 4023
	private GameObject primaCamera;

	// Token: 0x04000FB8 RID: 4024
	private RaycastHit hitSensoreCentrale;

	// Token: 0x04000FB9 RID: 4025
	private RaycastHit hitSensoreCircolareAnteriore;

	// Token: 0x04000FBA RID: 4026
	private RaycastHit hitSensoreCircolarePosteriore;

	// Token: 0x04000FBB RID: 4027
	private Vector3 destinazione;

	// Token: 0x04000FBC RID: 4028
	private bool destinazioneInVista;

	// Token: 0x04000FBD RID: 4029
	private int ampiezzaSensoreCircolare;

	// Token: 0x04000FBE RID: 4030
	private int numeroRaggiTrue;

	// Token: 0x04000FBF RID: 4031
	private float slittamentoVerticale1;

	// Token: 0x04000FC0 RID: 4032
	private float slittamentoOrizzontale1;

	// Token: 0x04000FC1 RID: 4033
	private float slittamentoVerticale2;

	// Token: 0x04000FC2 RID: 4034
	private float slittamentoOrizzontale2;

	// Token: 0x04000FC3 RID: 4035
	private float timerRotazione;

	// Token: 0x04000FC4 RID: 4036
	private Vector3 direzioneRaggioLibero;

	// Token: 0x04000FC5 RID: 4037
	public bool ripetitoreDiAttaccoOrdinato;

	// Token: 0x04000FC6 RID: 4038
	public bool muoviti;

	// Token: 0x04000FC7 RID: 4039
	private bool inAttesaDiOrdini;

	// Token: 0x04000FC8 RID: 4040
	public bool ritornoSuBersaglio;

	// Token: 0x04000FC9 RID: 4041
	private Vector3 puntoDiEntrata;
}
