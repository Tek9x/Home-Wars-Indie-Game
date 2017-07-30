using System;
using UnityEngine;

// Token: 0x020000F6 RID: 246
public class HeliPerEstrazione : MonoBehaviour
{
	// Token: 0x060007E8 RID: 2024 RVA: 0x00118998 File Offset: 0x00116B98
	private void Start()
	{
		this.infoNeutreTattica = GameObject.FindGameObjectWithTag("InfoNeutreTattica");
		this.primaCamera = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaCamere[0];
		this.varieMappaLocale = GameObject.FindGameObjectWithTag("VarieMappaLocale");
		this.infoAlleati = GameObject.FindGameObjectWithTag("Info Alleati");
		this.velocitàTraslazione = this.velocitàTraslazioneIniziale;
		this.velocitàSlittamento = this.velocitàSlittamentoIniziale;
		this.layerNavigazione = 256;
		if (this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().tipoBattaglia == 3 || this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().tipoBattaglia == 4)
		{
			this.destinazione = new Vector3(this.varieMappaLocale.GetComponent<VarieMappaLocale>().centroPerEstrazione.x, 70f, this.varieMappaLocale.GetComponent<VarieMappaLocale>().centroPerEstrazione.z);
		}
		this.posizioneDiPartenza = base.transform.position;
		this.cavo = base.transform.FindChild("cavo").gameObject;
		this.quadCerchio1 = base.gameObject.transform.GetChild(0).gameObject;
		this.quadCerchio2 = base.gameObject.transform.GetChild(1).gameObject;
	}

	// Token: 0x060007E9 RID: 2025 RVA: 0x00118AE0 File Offset: 0x00116CE0
	private void Update()
	{
		this.origineSensori = base.transform.position;
		this.RotazionePale();
		this.SensoriAnteriori();
		this.SensoriPosteriori();
		if (this.atterraHeli || this.stazionario || this.decollaHeli)
		{
			this.AtterraEDecolla();
		}
		else
		{
			this.NavigazionePerEstrazione();
		}
		float num = Vector3.Distance(base.transform.position, this.posizioneDiPartenza);
		if (this.caricoPreso && num < 15f)
		{
			this.heliInSalvo = true;
		}
		if (this.infoAlleati.GetComponent<GestioneComandanteInUI>().evidenziaAlleatiENemici)
		{
			this.quadCerchio1.GetComponent<MeshRenderer>().enabled = true;
			this.quadCerchio2.GetComponent<MeshRenderer>().enabled = true;
		}
		else
		{
			this.quadCerchio1.GetComponent<MeshRenderer>().enabled = false;
			this.quadCerchio2.GetComponent<MeshRenderer>().enabled = false;
		}
	}

	// Token: 0x060007EA RID: 2026 RVA: 0x00118BD4 File Offset: 0x00116DD4
	private void RotazionePale()
	{
		base.transform.GetChild(2).GetChild(0).transform.Rotate(Vector3.forward * 1000f * Time.deltaTime);
		base.transform.GetChild(2).GetChild(1).transform.Rotate(-Vector3.forward * 1000f * Time.deltaTime);
	}

	// Token: 0x060007EB RID: 2027 RVA: 0x00118C50 File Offset: 0x00116E50
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
		int num2 = 10;
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
			}
			num2 += 40;
		}
	}

	// Token: 0x060007EC RID: 2028 RVA: 0x00118E28 File Offset: 0x00117028
	private void SensoriPosteriori()
	{
		this.slittamentoVerticale1 = 0f;
		this.slittamentoOrizzontale1 = 0f;
		for (int i = 40; i < 95; i += 45)
		{
			Quaternion rotation = Quaternion.AngleAxis((float)i, base.transform.right);
			for (int j = 0; j < 360; j += 90)
			{
				Quaternion rotation2 = Quaternion.AngleAxis((float)j, base.transform.forward);
				Ray ray = new Ray(this.origineSensori, rotation2 * (rotation * base.transform.forward));
				if (Physics.Raycast(ray, out this.hitSensoreCircolarePosteriore, 10f, this.layerNavigazione))
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

	// Token: 0x060007ED RID: 2029 RVA: 0x00118FB8 File Offset: 0x001171B8
	private void NavigazionePerEstrazione()
	{
		Vector3 normalized = (this.destinazione - base.transform.position).normalized;
		if (this.destinazioneInVista)
		{
			Quaternion to = Quaternion.LookRotation(this.destinazione - base.transform.position);
			base.transform.rotation = Quaternion.RotateTowards(base.transform.rotation, to, this.velocitàAutoRotazione * Time.deltaTime);
			base.transform.position += normalized * this.velocitàTraslazione * Time.deltaTime;
		}
		else
		{
			Quaternion to2 = Quaternion.LookRotation(this.direzioneRaggioLibero);
			base.transform.rotation = Quaternion.RotateTowards(base.transform.rotation, to2, this.velocitàAutoRotazione * Time.deltaTime);
		}
		base.transform.position += base.transform.up * (this.slittamentoVerticale1 + this.slittamentoVerticale2) * this.velocitàSlittamento * Time.deltaTime;
		base.transform.position += base.transform.right * (this.slittamentoOrizzontale1 + this.slittamentoOrizzontale2) * this.velocitàSlittamento * Time.deltaTime;
		if (this.numeroRaggiTrue == 8)
		{
			base.transform.Rotate(base.transform.up * this.velocitàAutoRotazione * Time.deltaTime);
		}
		else
		{
			base.transform.position += base.transform.forward * this.velocitàTraslazione * Time.deltaTime;
		}
		if (Physics.Raycast(this.origineSensori, base.transform.forward, 10f, this.layerNavigazione))
		{
			base.transform.Rotate(base.transform.up * 1000f * Time.deltaTime);
		}
		float num = Vector3.Distance(base.transform.position, this.destinazione);
		if (num < 3f)
		{
			this.atterraHeli = true;
			if (this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().tipoBattaglia == 4 && !this.allineatoConCarico)
			{
				this.allineatoConCarico = true;
				base.transform.position = new Vector3(this.destinazione.x, base.transform.position.y, this.destinazione.z);
			}
		}
		base.transform.rotation = Quaternion.Euler(new Vector3(0f, base.transform.localEulerAngles.y, 0f));
	}

	// Token: 0x060007EE RID: 2030 RVA: 0x001192AC File Offset: 0x001174AC
	private void AtterraEDecolla()
	{
		if (this.atterraHeli)
		{
			if (this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().tipoBattaglia == 3)
			{
				if (base.transform.position.y > 4f)
				{
					base.transform.position += -Vector3.up * 15f * Time.deltaTime;
					this.destinazione = base.transform.position;
				}
				else
				{
					this.atterraHeli = false;
					this.stazionario = true;
				}
			}
			else if (this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().tipoBattaglia == 4)
			{
				if (base.transform.position.y > 30f)
				{
					base.transform.position += -Vector3.up * 15f * Time.deltaTime;
					this.destinazione = base.transform.position;
				}
				else
				{
					this.atterraHeli = false;
					this.stazionario = true;
				}
			}
		}
		else if (this.stazionario)
		{
			if (!this.gridoGoFatto)
			{
				this.primaCamera.GetComponent<GestioneSuoniCamera>().attivaVoce = true;
				this.primaCamera.GetComponent<GestioneSuoniCamera>().nonèVoce = true;
				this.primaCamera.GetComponent<GestioneSuoniCamera>().suonoVoci.clip = this.suonoGo;
				this.gridoGoFatto = true;
			}
			if (this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().tipoBattaglia == 3)
			{
				this.timerCancellaMembro += Time.deltaTime;
				if (this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaèFanteria.Count > 0)
				{
					if (this.timerCancellaMembro > 2f)
					{
						foreach (GameObject current in this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaèFanteria)
						{
							float num = Vector3.Distance(base.transform.position, current.transform.position);
							if (current && num < 20f)
							{
								current.GetComponent<PresenzaAlleato>().estratto = true;
								current.GetComponent<PresenzaAlleato>().vita = 0f;
								this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().soldatiSalvatiInBatt3++;
								this.timerCancellaMembro = 0f;
								this.caricoPreso = true;
								break;
							}
						}
					}
				}
				else
				{
					this.stazionario = false;
					this.decollaHeli = true;
				}
			}
			else if (this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().tipoBattaglia == 4)
			{
				if (this.lunghezzaCavo < 53f)
				{
					this.lunghezzaCavo += 80f * Time.deltaTime;
					this.cavo.transform.localScale = new Vector3(1f, this.lunghezzaCavo, 1f);
				}
				else
				{
					this.stazionario = false;
					this.decollaHeli = true;
					this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().cassaSupply.transform.parent = base.transform;
					this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().cassaSupply.GetComponent<BoxCollider>().enabled = false;
					this.caricoPreso = true;
				}
			}
		}
		else if (this.decollaHeli)
		{
			if (base.transform.position.y < 70f)
			{
				base.transform.position += Vector3.up * 15f * Time.deltaTime;
				this.destinazione = base.transform.position;
				this.stazionario = false;
			}
			else
			{
				this.decollaHeli = false;
				this.destinazione = this.posizioneDiPartenza;
			}
		}
	}

	// Token: 0x04001DD4 RID: 7636
	public float velocitàTraslazioneIniziale;

	// Token: 0x04001DD5 RID: 7637
	private float velocitàTraslazione;

	// Token: 0x04001DD6 RID: 7638
	public float velocitàSlittamentoIniziale;

	// Token: 0x04001DD7 RID: 7639
	private float velocitàSlittamento;

	// Token: 0x04001DD8 RID: 7640
	public float velocitàAutoRotazione;

	// Token: 0x04001DD9 RID: 7641
	public AudioClip suonoGo;

	// Token: 0x04001DDA RID: 7642
	private GameObject infoNeutreTattica;

	// Token: 0x04001DDB RID: 7643
	private GameObject varieMappaLocale;

	// Token: 0x04001DDC RID: 7644
	private GameObject infoAlleati;

	// Token: 0x04001DDD RID: 7645
	private GameObject primaCamera;

	// Token: 0x04001DDE RID: 7646
	private Vector3 origineSensori;

	// Token: 0x04001DDF RID: 7647
	private int layerNavigazione;

	// Token: 0x04001DE0 RID: 7648
	private RaycastHit hitSensoreCentrale;

	// Token: 0x04001DE1 RID: 7649
	private RaycastHit hitSensoreCircolareAnteriore;

	// Token: 0x04001DE2 RID: 7650
	private RaycastHit hitSensoreCircolarePosteriore;

	// Token: 0x04001DE3 RID: 7651
	private Vector3 destinazione;

	// Token: 0x04001DE4 RID: 7652
	private bool destinazioneInVista;

	// Token: 0x04001DE5 RID: 7653
	private int ampiezzaSensoreCircolare;

	// Token: 0x04001DE6 RID: 7654
	private int numeroRaggiTrue;

	// Token: 0x04001DE7 RID: 7655
	private float slittamentoVerticale1;

	// Token: 0x04001DE8 RID: 7656
	private float slittamentoOrizzontale1;

	// Token: 0x04001DE9 RID: 7657
	private float slittamentoVerticale2;

	// Token: 0x04001DEA RID: 7658
	private float slittamentoOrizzontale2;

	// Token: 0x04001DEB RID: 7659
	private Vector3 direzioneRaggioLibero;

	// Token: 0x04001DEC RID: 7660
	private bool atterraHeli;

	// Token: 0x04001DED RID: 7661
	private bool decollaHeli;

	// Token: 0x04001DEE RID: 7662
	private bool stazionario;

	// Token: 0x04001DEF RID: 7663
	public bool caricoPreso;

	// Token: 0x04001DF0 RID: 7664
	private Vector3 posizioneDiPartenza;

	// Token: 0x04001DF1 RID: 7665
	private GameObject cavo;

	// Token: 0x04001DF2 RID: 7666
	private float lunghezzaCavo;

	// Token: 0x04001DF3 RID: 7667
	private float timerCancellaMembro;

	// Token: 0x04001DF4 RID: 7668
	public bool heliInSalvo;

	// Token: 0x04001DF5 RID: 7669
	private GameObject quadCerchio1;

	// Token: 0x04001DF6 RID: 7670
	private GameObject quadCerchio2;

	// Token: 0x04001DF7 RID: 7671
	private bool gridoGoFatto;

	// Token: 0x04001DF8 RID: 7672
	private bool allineatoConCarico;
}
