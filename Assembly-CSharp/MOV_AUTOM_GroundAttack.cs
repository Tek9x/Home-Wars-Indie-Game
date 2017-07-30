using System;
using UnityEngine;

// Token: 0x02000049 RID: 73
public class MOV_AUTOM_GroundAttack : MonoBehaviour
{
	// Token: 0x060003CA RID: 970 RVA: 0x000996E8 File Offset: 0x000978E8
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

	// Token: 0x060003CB RID: 971 RVA: 0x00099794 File Offset: 0x00097994
	private void Update()
	{
		this.origineSensori = base.transform.position;
		this.SensoriAnteriori();
		this.SensoriPosteriori();
		if (base.gameObject != this.terzaCamera.GetComponent<TerzaCamera>().ospiteCamera)
		{
			this.target = base.GetComponent<ATT_GroundAttack>().unitàBersaglio;
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

	// Token: 0x060003CC RID: 972 RVA: 0x00099A14 File Offset: 0x00097C14
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

	// Token: 0x060003CD RID: 973 RVA: 0x00099C30 File Offset: 0x00097E30
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

	// Token: 0x060003CE RID: 974 RVA: 0x00099DD8 File Offset: 0x00097FD8
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

	// Token: 0x060003CF RID: 975 RVA: 0x0009A04C File Offset: 0x0009824C
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

	// Token: 0x060003D0 RID: 976 RVA: 0x0009A2D4 File Offset: 0x000984D4
	private void VirataDiPericolo()
	{
		base.transform.Rotate(base.transform.up * 1000f * Time.deltaTime);
	}

	// Token: 0x060003D1 RID: 977 RVA: 0x0009A30C File Offset: 0x0009850C
	private void Rientro()
	{
		float num = Vector3.Distance(base.transform.position, this.puntoDiEntrata);
		if (base.GetComponent<PresenzaAlleato>().tornaAllaBase && num < 50f)
		{
			base.GetComponent<PresenzaAlleato>().ritornoEffettuato = true;
		}
	}

	// Token: 0x04000FCA RID: 4042
	public float velocitàTraslazioneIniziale;

	// Token: 0x04000FCB RID: 4043
	private float velocitàTraslazione;

	// Token: 0x04000FCC RID: 4044
	public float velocitàSlittamentoIniziale;

	// Token: 0x04000FCD RID: 4045
	private float velocitàSlittamento;

	// Token: 0x04000FCE RID: 4046
	public float velocitàAutoRotazione;

	// Token: 0x04000FCF RID: 4047
	private Vector3 origineSensori;

	// Token: 0x04000FD0 RID: 4048
	public GameObject target;

	// Token: 0x04000FD1 RID: 4049
	private int layerNavigazione;

	// Token: 0x04000FD2 RID: 4050
	private GameObject infoNeutreTattica;

	// Token: 0x04000FD3 RID: 4051
	private GameObject terzaCamera;

	// Token: 0x04000FD4 RID: 4052
	private GameObject primaCamera;

	// Token: 0x04000FD5 RID: 4053
	private RaycastHit hitSensoreCentrale;

	// Token: 0x04000FD6 RID: 4054
	private RaycastHit hitSensoreCircolareAnteriore;

	// Token: 0x04000FD7 RID: 4055
	private RaycastHit hitSensoreCircolarePosteriore;

	// Token: 0x04000FD8 RID: 4056
	private Vector3 destinazione;

	// Token: 0x04000FD9 RID: 4057
	private bool destinazioneInVista;

	// Token: 0x04000FDA RID: 4058
	private int ampiezzaSensoreCircolare;

	// Token: 0x04000FDB RID: 4059
	private int numeroRaggiTrue;

	// Token: 0x04000FDC RID: 4060
	private float slittamentoVerticale1;

	// Token: 0x04000FDD RID: 4061
	private float slittamentoOrizzontale1;

	// Token: 0x04000FDE RID: 4062
	private float slittamentoVerticale2;

	// Token: 0x04000FDF RID: 4063
	private float slittamentoOrizzontale2;

	// Token: 0x04000FE0 RID: 4064
	private float timerRotazione;

	// Token: 0x04000FE1 RID: 4065
	private Vector3 direzioneRaggioLibero;

	// Token: 0x04000FE2 RID: 4066
	public bool ripetitoreDiAttaccoOrdinato;

	// Token: 0x04000FE3 RID: 4067
	public bool muoviti;

	// Token: 0x04000FE4 RID: 4068
	private bool inAttesaDiOrdini;

	// Token: 0x04000FE5 RID: 4069
	public bool ritornoSuBersaglio;

	// Token: 0x04000FE6 RID: 4070
	private Vector3 puntoDiEntrata;
}
