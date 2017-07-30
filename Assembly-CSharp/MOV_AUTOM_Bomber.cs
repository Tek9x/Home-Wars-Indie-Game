using System;
using UnityEngine;

// Token: 0x02000047 RID: 71
public class MOV_AUTOM_Bomber : MonoBehaviour
{
	// Token: 0x060003B7 RID: 951 RVA: 0x00097BD8 File Offset: 0x00095DD8
	private void Start()
	{
		this.infoNeutreTattica = GameObject.FindGameObjectWithTag("InfoNeutreTattica");
		this.primaCamera = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaCamere[0];
		this.terzaCamera = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaCamere[2];
		this.velocitàTraslazione = this.velocitàTraslazioneIniziale;
		this.velocitàSlittamento = this.velocitàSlittamentoIniziale;
		this.layerNavigazione = 256;
		this.destinazione = new Vector3(0f, 150f, 0f);
		this.puntoDiEntrata = base.transform.position;
	}

	// Token: 0x060003B8 RID: 952 RVA: 0x00097C7C File Offset: 0x00095E7C
	private void Update()
	{
		this.origineSensori = base.transform.position;
		this.SensoriAnteriori();
		this.SensoriPosteriori();
		if (base.gameObject != this.terzaCamera.GetComponent<TerzaCamera>().ospiteCamera)
		{
			this.target = base.GetComponent<ATT_Bomber>().unitàBersaglio;
			if (!base.GetComponent<PresenzaAlleato>().tornaAllaBase)
			{
				if (this.ripetitoreDiAttaccoOrdinato)
				{
					this.ritornoSuBersaglio = true;
					this.ripetitoreDiAttaccoOrdinato = false;
				}
				if (this.target != null && !this.target.GetComponent<PresenzaNemico>().insettoVolante && this.ritornoSuBersaglio)
				{
					if (base.GetComponent<PresenzaAlleato>().destinazioneSenzaNavMesh.y < 200f)
					{
						this.destinazione = this.target.transform.position + Vector3.up * 150f;
					}
					else if (base.GetComponent<PresenzaAlleato>().destinazioneSenzaNavMesh.y >= 200f && base.GetComponent<PresenzaAlleato>().destinazioneSenzaNavMesh.y < 300f)
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
							if (base.GetComponent<PresenzaAlleato>().destinazioneSenzaNavMesh.y < 200f)
							{
								this.destinazione = base.GetComponent<PresenzaAlleato>().destinazioneSenzaNavMesh + Vector3.up * 150f;
							}
							else if (base.GetComponent<PresenzaAlleato>().destinazioneSenzaNavMesh.y >= 200f && base.GetComponent<PresenzaAlleato>().destinazioneSenzaNavMesh.y < 300f)
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
					if (luogoAttZonaBomb.y < 200f)
					{
						this.destinazione = luogoAttZonaBomb + Vector3.up * 150f;
					}
					else if (luogoAttZonaBomb.y >= 200f && luogoAttZonaBomb.y < 300f)
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

	// Token: 0x060003B9 RID: 953 RVA: 0x000980CC File Offset: 0x000962CC
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

	// Token: 0x060003BA RID: 954 RVA: 0x000982E8 File Offset: 0x000964E8
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

	// Token: 0x060003BB RID: 955 RVA: 0x00098490 File Offset: 0x00096690
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
			base.transform.Rotate(base.transform.up * 50f * Time.deltaTime);
		}
		base.transform.position += base.transform.up * (this.slittamentoVerticale1 + this.slittamentoVerticale2) * this.velocitàSlittamento * Time.deltaTime;
		base.transform.position += base.transform.right * (this.slittamentoOrizzontale1 + this.slittamentoOrizzontale2) * this.velocitàSlittamento * Time.deltaTime;
		base.transform.position += base.transform.forward * this.velocitàTraslazione * Time.deltaTime;
	}

	// Token: 0x060003BC RID: 956 RVA: 0x00098694 File Offset: 0x00096894
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
		base.transform.position += base.transform.forward * this.velocitàTraslazione * Time.deltaTime;
	}

	// Token: 0x060003BD RID: 957 RVA: 0x00098880 File Offset: 0x00096A80
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
		base.transform.position += base.transform.forward * this.velocitàTraslazione * Time.deltaTime;
	}

	// Token: 0x060003BE RID: 958 RVA: 0x00098A58 File Offset: 0x00096C58
	private void VirataDiPericolo()
	{
		base.transform.Rotate(base.transform.up * 1000f * Time.deltaTime);
	}

	// Token: 0x060003BF RID: 959 RVA: 0x00098A90 File Offset: 0x00096C90
	private void Rientro()
	{
		float num = Vector3.Distance(base.transform.position, this.puntoDiEntrata);
		if (base.GetComponent<PresenzaAlleato>().tornaAllaBase && num < 50f)
		{
			base.GetComponent<PresenzaAlleato>().ritornoEffettuato = true;
		}
	}

	// Token: 0x04000F91 RID: 3985
	public float velocitàTraslazioneIniziale;

	// Token: 0x04000F92 RID: 3986
	private float velocitàTraslazione;

	// Token: 0x04000F93 RID: 3987
	public float velocitàSlittamentoIniziale;

	// Token: 0x04000F94 RID: 3988
	private float velocitàSlittamento;

	// Token: 0x04000F95 RID: 3989
	public float velocitàAutoRotazione;

	// Token: 0x04000F96 RID: 3990
	private Vector3 origineSensori;

	// Token: 0x04000F97 RID: 3991
	public GameObject target;

	// Token: 0x04000F98 RID: 3992
	private int layerNavigazione;

	// Token: 0x04000F99 RID: 3993
	private GameObject infoNeutreTattica;

	// Token: 0x04000F9A RID: 3994
	private GameObject terzaCamera;

	// Token: 0x04000F9B RID: 3995
	private GameObject primaCamera;

	// Token: 0x04000F9C RID: 3996
	private RaycastHit hitSensoreCentrale;

	// Token: 0x04000F9D RID: 3997
	private RaycastHit hitSensoreCircolareAnteriore;

	// Token: 0x04000F9E RID: 3998
	private RaycastHit hitSensoreCircolarePosteriore;

	// Token: 0x04000F9F RID: 3999
	private Vector3 destinazione;

	// Token: 0x04000FA0 RID: 4000
	private bool destinazioneInVista;

	// Token: 0x04000FA1 RID: 4001
	private int ampiezzaSensoreCircolare;

	// Token: 0x04000FA2 RID: 4002
	private int numeroRaggiTrue;

	// Token: 0x04000FA3 RID: 4003
	private float slittamentoVerticale1;

	// Token: 0x04000FA4 RID: 4004
	private float slittamentoOrizzontale1;

	// Token: 0x04000FA5 RID: 4005
	private float slittamentoVerticale2;

	// Token: 0x04000FA6 RID: 4006
	private float slittamentoOrizzontale2;

	// Token: 0x04000FA7 RID: 4007
	private float timerRotazione;

	// Token: 0x04000FA8 RID: 4008
	private Vector3 direzioneRaggioLibero;

	// Token: 0x04000FA9 RID: 4009
	public bool ripetitoreDiAttaccoOrdinato;

	// Token: 0x04000FAA RID: 4010
	private bool inAttesaDiOrdini;

	// Token: 0x04000FAB RID: 4011
	public bool ritornoSuBersaglio;

	// Token: 0x04000FAC RID: 4012
	private Vector3 puntoDiEntrata;
}
