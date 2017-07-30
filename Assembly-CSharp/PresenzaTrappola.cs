using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000100 RID: 256
public class PresenzaTrappola : MonoBehaviour
{
	// Token: 0x0600081E RID: 2078 RVA: 0x0011B108 File Offset: 0x00119308
	private void Start()
	{
		this.infoAlleati = GameObject.FindGameObjectWithTag("Info Alleati");
		this.infoNeutreTattica = GameObject.FindGameObjectWithTag("InfoNeutreTattica");
		this.primaCamera = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaCamere[0];
		this.IANemico = GameObject.FindGameObjectWithTag("IANemico");
		this.layerPosiz = 256;
		this.vitaIniziale = this.vita;
		this.quadSelezione = base.transform.GetChild(0).gameObject;
		this.dimensioneQuadSel = this.quadSelezione.transform.localScale.x;
		this.testoVita = base.transform.GetChild(1).gameObject;
	}

	// Token: 0x0600081F RID: 2079 RVA: 0x0011B1C4 File Offset: 0x001193C4
	private void Update()
	{
		if (this.trappolaAttiva)
		{
			if (!this.disposta)
			{
				this.Disposizione();
				this.disposta = true;
			}
			this.Evidenziazione();
			if (this.primaCamera.GetComponent<Selezionamento>().trappolaSelez == base.gameObject)
			{
				this.RiparazioneEDemolizione();
			}
			if (this.tipoTrappola == 1 || this.tipoTrappola == 2 || this.tipoTrappola == 8)
			{
				this.Danno();
			}
			if (this.vita <= 0f)
			{
				this.Distruzione();
			}
		}
		else
		{
			if (this.inPosizionamento)
			{
				this.VerificaPosizionamento();
			}
			if (this.inCostruzione)
			{
				this.Costruzione();
			}
		}
	}

	// Token: 0x06000820 RID: 2080 RVA: 0x0011B288 File Offset: 0x00119488
	private void VerificaPosizionamento()
	{
		if (this.tipoTrappola == 5)
		{
			if (base.transform.position.y < 290f)
			{
				this.èPosizionabile = true;
			}
			else
			{
				this.èPosizionabile = false;
			}
		}
		else if (Physics.Raycast(base.transform.position + Vector3.up * 2f, -Vector3.up, out this.hitTerreno, 5f, this.layerPosiz))
		{
			if (Vector3.Dot(Vector3.up, this.hitTerreno.normal) > 0.95f)
			{
				this.èPosizionabile = true;
			}
			else
			{
				this.èPosizionabile = false;
			}
		}
		else
		{
			this.èPosizionabile = false;
		}
	}

	// Token: 0x06000821 RID: 2081 RVA: 0x0011B358 File Offset: 0x00119558
	private void Disposizione()
	{
		if (base.GetComponent<NavMeshObstacle>())
		{
			base.GetComponent<NavMeshObstacle>().enabled = true;
			if (this.tipoTrappola == 6)
			{
				base.transform.GetChild(4).GetComponent<NavMeshObstacle>().enabled = true;
				base.transform.GetChild(5).GetComponent<NavMeshObstacle>().enabled = true;
			}
		}
		if (base.GetComponent<BoxCollider>())
		{
			base.GetComponent<BoxCollider>().enabled = true;
		}
	}

	// Token: 0x06000822 RID: 2082 RVA: 0x0011B3D8 File Offset: 0x001195D8
	private void Evidenziazione()
	{
		if (this.trappolaSelezionata || this.infoAlleati.GetComponent<GestioneComandanteInUI>().evidenziaAlleatiENemici)
		{
			this.cameraAttiva = this.primaCamera.GetComponent<PrimaCamera>().oggettoCameraAttiva;
			this.quadSelezione.GetComponent<MeshRenderer>().enabled = true;
			this.testoVita.GetComponent<MeshRenderer>().enabled = true;
			float num = Vector3.Distance(base.transform.position, this.cameraAttiva.transform.position);
			float num2 = num / 20f * (this.dimensioneQuadSel / 50f + 1f);
			if (num2 < this.dimensioneQuadSel)
			{
				this.quadSelezione.transform.localScale = new Vector3(this.dimensioneQuadSel, this.dimensioneQuadSel, 0f);
			}
			else
			{
				this.quadSelezione.transform.localScale = new Vector3(num2, num2, 0f);
			}
			if (this.vita > 0f)
			{
				this.testoVita.GetComponent<TextMesh>().text = (this.vita * 100f / this.vitaIniziale).ToString("F1") + "%";
			}
			else
			{
				this.testoVita.GetComponent<TextMesh>().text = "0%";
			}
			Vector3 normalized = (base.transform.position - this.cameraAttiva.transform.position).normalized;
			this.testoVita.transform.forward = normalized;
			float num3 = num / 400f;
			float num4 = 0.2f;
			if (num3 > num4)
			{
				this.testoVita.transform.localScale = new Vector3(num3, num3, num3);
			}
			else
			{
				this.testoVita.transform.localScale = new Vector3(num4, num4, num4);
			}
		}
		else
		{
			this.quadSelezione.GetComponent<MeshRenderer>().enabled = false;
			this.testoVita.GetComponent<MeshRenderer>().enabled = false;
		}
	}

	// Token: 0x06000823 RID: 2083 RVA: 0x0011B5E4 File Offset: 0x001197E4
	private void RiparazioneEDemolizione()
	{
		if (this.infoAlleati.GetComponent<InfoGenericheAlleati>().riparaTrappola)
		{
			this.infoAlleati.GetComponent<InfoGenericheAlleati>().riparaTrappola = false;
			bool flag = false;
			for (int i = 0; i < this.IANemico.GetComponent<InfoGenericheNemici>().ListaNemici.Count; i++)
			{
				if (this.IANemico.GetComponent<InfoGenericheNemici>().ListaNemici[i] != null && Vector3.Distance(this.IANemico.GetComponent<InfoGenericheNemici>().ListaNemici[i].transform.position, base.transform.position) < 30f)
				{
					flag = true;
					break;
				}
			}
			if (flag)
			{
				this.infoAlleati.GetComponent<GestioneComandanteInUI>().partenzaTimerScrAvvSapper = true;
			}
			else if (this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().puntiBattaglia >= this.costoPuntiBattaglia / 2f)
			{
				this.vita = this.vitaIniziale;
				this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().puntiBattaglia -= this.costoPuntiBattaglia / 2f;
			}
		}
		if (this.infoAlleati.GetComponent<InfoGenericheAlleati>().demolisciTrappola)
		{
			this.infoAlleati.GetComponent<InfoGenericheAlleati>().demolisciTrappola = false;
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	// Token: 0x06000824 RID: 2084 RVA: 0x0011B740 File Offset: 0x00119940
	private void Costruzione()
	{
		if (this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaèGeniere.Count > 0)
		{
			if (this.tipoTrappola == 5)
			{
				Vector3 position = base.transform.GetChild(3).transform.position;
				foreach (GameObject current in this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaèGeniere)
				{
					if (Vector3.Distance(current.transform.position, position) < this.raggioDiCostruzione)
					{
						this.inCostruzione = false;
						this.trappolaAttiva = true;
						base.transform.GetChild(2).GetComponent<MeshRenderer>().material = this.materialeFinito;
						base.transform.GetChild(3).GetComponent<ParticleSystem>().Play();
					}
				}
			}
			else
			{
				foreach (GameObject current2 in this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaèGeniere)
				{
					if (Vector3.Distance(current2.transform.position, base.transform.position) < this.raggioDiCostruzione)
					{
						this.inCostruzione = false;
						this.trappolaAttiva = true;
						base.transform.GetChild(3).GetComponent<ParticleSystem>().Play();
						if (this.tipoTrappola == 2)
						{
							base.transform.GetChild(2).GetChild(0).GetComponent<MeshRenderer>().material = this.materialeFinito;
							base.transform.GetChild(2).GetChild(1).GetComponent<MeshRenderer>().material = this.materialeFinito;
							base.transform.GetChild(2).GetChild(2).GetComponent<MeshRenderer>().material = this.materialeFinito;
						}
						else
						{
							base.transform.GetChild(2).GetComponent<MeshRenderer>().material = this.materialeFinito;
						}
					}
				}
			}
		}
	}

	// Token: 0x06000825 RID: 2085 RVA: 0x0011B980 File Offset: 0x00119B80
	private void Danno()
	{
		this.timerDanno += Time.deltaTime;
		if (this.timerDanno > this.frequenzaAttacco)
		{
			this.timerDanno = 0f;
			foreach (GameObject current in base.GetComponent<AreaTrappola>().ListaAreaTrappola)
			{
				if (current != null)
				{
					float num = 0f;
					if (current.GetComponent<PresenzaNemico>().vita > this.penetrazione)
					{
						num = this.penetrazione;
					}
					else if (current.GetComponent<PresenzaNemico>().vita > 0f)
					{
						num = current.GetComponent<PresenzaNemico>().vita;
					}
					current.GetComponent<PresenzaNemico>().vita -= this.penetrazione;
					if (current.GetComponent<PresenzaNemico>().vita > this.danno * (1f - current.GetComponent<PresenzaNemico>().armatura))
					{
						num += this.danno * (1f - current.GetComponent<PresenzaNemico>().armatura);
					}
					else if (current.GetComponent<PresenzaNemico>().vita > 0f)
					{
						num += current.GetComponent<PresenzaNemico>().vita;
					}
					current.GetComponent<PresenzaNemico>().vita -= this.danno * (1f - current.GetComponent<PresenzaNemico>().armatura);
					List<float> listaDanniAlleati;
					List<float> expr_15E = listaDanniAlleati = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaDanniAlleati;
					int index;
					int expr_162 = index = 11;
					float num2 = listaDanniAlleati[index];
					expr_15E[expr_162] = num2 + num;
					current.GetComponent<PresenzaNemico>().vita -= this.penetrazione;
					current.GetComponent<PresenzaNemico>().vita -= this.danno * (1f - current.GetComponent<PresenzaNemico>().armatura);
				}
			}
		}
	}

	// Token: 0x06000826 RID: 2086 RVA: 0x0011BB80 File Offset: 0x00119D80
	private void Distruzione()
	{
		if (base.GetComponent<AreaTrappola>())
		{
			foreach (GameObject current in base.GetComponent<AreaTrappola>().ListaAreaTrappola)
			{
				if (current != null && current.GetComponent<NavigazioneConCamminata>())
				{
					current.GetComponent<NavigazioneConCamminata>().rallDaTrappola = false;
				}
			}
		}
		if (this.rumoreInDistruzione)
		{
			this.timerDistruzione += Time.deltaTime;
			if (!this.suonoDistrAttivato)
			{
				this.suonoDistrAttivato = true;
				base.GetComponent<AudioSource>().Play();
				base.transform.GetChild(3).GetComponent<ParticleSystem>().Play();
				base.transform.GetChild(2).GetComponent<MeshRenderer>().enabled = false;
			}
			if (this.timerDistruzione > 2.5f)
			{
				if (this.tipoTrappola == 7 || this.tipoTrappola == 8)
				{
					foreach (GameObject current2 in base.GetComponent<AreaTrappola>().ListaAreaTrappola)
					{
						if (current2)
						{
							current2.GetComponent<NavigazioneConCamminata>().rallDaTrappola = false;
						}
					}
				}
				UnityEngine.Object.Destroy(base.gameObject);
			}
		}
		else
		{
			if (this.tipoTrappola == 7 || this.tipoTrappola == 8)
			{
				foreach (GameObject current3 in base.GetComponent<AreaTrappola>().ListaAreaTrappola)
				{
					if (current3 != null && current3.GetComponent<NavigazioneConCamminata>())
					{
						current3.GetComponent<NavigazioneConCamminata>().rallDaTrappola = false;
					}
				}
			}
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	// Token: 0x04001E30 RID: 7728
	private GameObject infoAlleati;

	// Token: 0x04001E31 RID: 7729
	private GameObject infoNeutreTattica;

	// Token: 0x04001E32 RID: 7730
	private GameObject primaCamera;

	// Token: 0x04001E33 RID: 7731
	private GameObject IANemico;

	// Token: 0x04001E34 RID: 7732
	public float costoPuntiBattaglia;

	// Token: 0x04001E35 RID: 7733
	public float vita;

	// Token: 0x04001E36 RID: 7734
	public bool rumoreInDistruzione;

	// Token: 0x04001E37 RID: 7735
	public float danno;

	// Token: 0x04001E38 RID: 7736
	public float penetrazione;

	// Token: 0x04001E39 RID: 7737
	public float portata;

	// Token: 0x04001E3A RID: 7738
	public float frequenzaAttacco;

	// Token: 0x04001E3B RID: 7739
	public float percDiRallentamento;

	// Token: 0x04001E3C RID: 7740
	public float dannoSubitoPerAzione;

	// Token: 0x04001E3D RID: 7741
	public int tipoTrappola;

	// Token: 0x04001E3E RID: 7742
	public string nomeTrappola;

	// Token: 0x04001E3F RID: 7743
	public Sprite immagineTrappola;

	// Token: 0x04001E40 RID: 7744
	public GameObject oggettoDescrizione;

	// Token: 0x04001E41 RID: 7745
	public float vitaIniziale;

	// Token: 0x04001E42 RID: 7746
	public bool inPosizionamento;

	// Token: 0x04001E43 RID: 7747
	public bool èPosizionabile;

	// Token: 0x04001E44 RID: 7748
	private RaycastHit hitTerreno;

	// Token: 0x04001E45 RID: 7749
	private int layerPosiz;

	// Token: 0x04001E46 RID: 7750
	public bool trappolaAttiva;

	// Token: 0x04001E47 RID: 7751
	public bool disposta;

	// Token: 0x04001E48 RID: 7752
	public bool trappolaSelezionata;

	// Token: 0x04001E49 RID: 7753
	private GameObject quadSelezione;

	// Token: 0x04001E4A RID: 7754
	private float dimensioneQuadSel;

	// Token: 0x04001E4B RID: 7755
	private GameObject testoVita;

	// Token: 0x04001E4C RID: 7756
	private GameObject cameraAttiva;

	// Token: 0x04001E4D RID: 7757
	public bool inCostruzione;

	// Token: 0x04001E4E RID: 7758
	public Material materialeFinito;

	// Token: 0x04001E4F RID: 7759
	public float raggioDiCostruzione;

	// Token: 0x04001E50 RID: 7760
	private float timerDistruzione;

	// Token: 0x04001E51 RID: 7761
	private bool suonoDistrAttivato;

	// Token: 0x04001E52 RID: 7762
	private float timerDanno;
}
