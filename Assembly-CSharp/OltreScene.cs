using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200010C RID: 268
public class OltreScene : MonoBehaviour
{
	// Token: 0x06000866 RID: 2150 RVA: 0x0012705C File Offset: 0x0012525C
	private void Start()
	{
		this.cameraGiocatore = GameObject.FindGameObjectWithTag("MainCamera");
		if (!this.scenaDiMenu)
		{
			if (this.èInStrategia)
			{
				this.menuInStrategia = GameObject.FindGameObjectWithTag("CanvasMenu").transform.FindChild("Menu Strategico").gameObject;
				this.CanvasStrategia = GameObject.FindGameObjectWithTag("CanvasStrategia");
				this.schede = GameObject.FindGameObjectWithTag("Schede");
				this.visualizzaEsercito = this.CanvasStrategia.transform.FindChild("Visualizza Esercito").gameObject;
				this.visualizzaSecondoEsercito = this.CanvasStrategia.transform.FindChild("Visualizza Esercito").FindChild("elenco secondo esercito").gameObject;
				this.CentroStanzaUI = this.CanvasStrategia.transform.FindChild("Centro Stanza").gameObject;
				this.visualEserNemico = this.CanvasStrategia.transform.FindChild("Visualizza Esercito Insetti").gameObject;
				this.visualDettVelAlleati = this.CanvasStrategia.transform.FindChild("Dettagli Veloci Unità").gameObject;
				this.visualDettVelNemici = this.CanvasStrategia.transform.FindChild("Dettagli Veloci Insetto").gameObject;
				this.Headquarters = GameObject.FindGameObjectWithTag("Headquarters");
				this.menuChiudiSalva = GameObject.FindGameObjectWithTag("CanvasMenu").transform.FindChild("Finestra Salva").gameObject;
				this.menuChiudiCarica = GameObject.FindGameObjectWithTag("CanvasMenu").transform.FindChild("Finestra Carica In Strategia").gameObject;
				this.menuChiudiSalvaConNome = GameObject.FindGameObjectWithTag("CanvasMenu").transform.FindChild("Finestra Salva").FindChild("domanda nome salvataggio").gameObject;
				this.menuChiudiDomandaCarica = GameObject.FindGameObjectWithTag("CanvasMenu").transform.FindChild("Finestra Carica In Strategia").FindChild("domanda per caricamento").gameObject;
				this.menuChiudiCancellamento = GameObject.FindGameObjectWithTag("CanvasMenu").transform.FindChild("Finestra Cancella").gameObject;
				this.menuChiudiTornaAMenuIniz = GameObject.FindGameObjectWithTag("CanvasMenu").transform.FindChild("Domande Uscita Strategia").FindChild("domanda torna al menu").gameObject;
				this.menuChiudiTornaADesktop = GameObject.FindGameObjectWithTag("CanvasMenu").transform.FindChild("Domande Uscita Strategia").FindChild("domanda torna al desktop").gameObject;
				this.menuChiudiImpost = GameObject.FindGameObjectWithTag("CanvasMenu").transform.FindChild("Impostazioni").gameObject;
				this.domandaLanciareOAbbattereSat = this.CentroStanzaUI.transform.FindChild("domanda lanciare o abbattere satellite").gameObject;
			}
			else
			{
				this.menuInTattica = GameObject.FindGameObjectWithTag("CanvasMenu").transform.FindChild("Menu Tattico").gameObject;
				this.infoNeutreTattica = GameObject.FindGameObjectWithTag("InfoNeutreTattica");
				this.scrittaPausa = GameObject.FindGameObjectWithTag("CanvasComandante").transform.FindChild("Varie Battaglia").FindChild("Scritta Pausa").gameObject;
				this.menuChiudiTornaAMenuIniz = GameObject.FindGameObjectWithTag("CanvasMenu").transform.FindChild("Domande Uscita Battaglia").FindChild("domanda torna al menu").gameObject;
				this.menuChiudiTornaADesktop = GameObject.FindGameObjectWithTag("CanvasMenu").transform.FindChild("Domande Uscita Battaglia").FindChild("domanda torna al desktop").gameObject;
				this.menuChiudiImpost = GameObject.FindGameObjectWithTag("CanvasMenu").transform.FindChild("Impostazioni").gameObject;
				this.menuChiudiTornaACasa = GameObject.FindGameObjectWithTag("CanvasMenu").transform.FindChild("Domande Uscita Battaglia").FindChild("domanda torna alla casa").gameObject;
				this.pulsantePausa = GameObject.FindGameObjectWithTag("CanvasComandante").transform.FindChild("Barra Superiore").FindChild("Timer").FindChild("pulsante Pausa").gameObject;
				this.pulsanteVelTempo = GameObject.FindGameObjectWithTag("CanvasComandante").transform.FindChild("Barra Superiore").FindChild("Timer").FindChild("pulsante Velocità").gameObject;
				this.scrittaVelTempo = this.pulsanteVelTempo.transform.GetChild(1).gameObject;
				if (GestoreNeutroTattica.èBattagliaVeloce)
				{
					this.menuInTattica.transform.GetChild(0).FindChild("Torna alla Casa").GetComponent<Button>().interactable = false;
				}
				this.ListaVelocitàTempo = new List<int>();
				this.ListaVelocitàTempo.Add(1);
				this.ListaVelocitàTempo.Add(2);
				this.ListaVelocitàTempo.Add(3);
				this.ListaVelocitàTempo.Add(4);
			}
		}
	}

	// Token: 0x06000867 RID: 2151 RVA: 0x00127528 File Offset: 0x00125728
	private void Update()
	{
		if (!this.scenaDiMenu)
		{
			if (!this.èInStrategia)
			{
				if (!this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().battagliaTerminata)
				{
					this.AperturaChiusuraMenuGioco();
					this.PausaSemplice();
					this.VisibilitàCursore();
					this.GestioneTempoGioco();
				}
				else
				{
					Cursor.lockState = CursorLockMode.None;
					Cursor.visible = true;
					Time.timeScale = 0f;
				}
			}
			else
			{
				this.AperturaChiusuraMenuGioco();
			}
		}
	}

	// Token: 0x06000868 RID: 2152 RVA: 0x001275A0 File Offset: 0x001257A0
	private void AperturaChiusuraMenuGioco()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			if (this.èInStrategia)
			{
				this.Headquarters.GetComponent<GestioneSuoniCasa>().attivaSuono = true;
				this.cameraGiocatore.GetComponent<AudioSource>().clip = this.Headquarters.GetComponent<GestioneSuoniCasa>().suonoClickGenerico1;
				if (!this.menuAperto)
				{
					if (this.visualizzaEsercito.GetComponent<CanvasGroup>().alpha == 0f && this.visualizzaSecondoEsercito.GetComponent<CanvasGroup>().alpha == 0f && this.visualEserNemico.GetComponent<CanvasGroup>().alpha == 0f && this.schede.GetComponent<CanvasGroup>().alpha == 0f && this.CentroStanzaUI.GetComponent<CanvasGroup>().alpha == 0f && this.visualDettVelAlleati.GetComponent<CanvasGroup>().alpha == 0f && this.visualDettVelNemici.GetComponent<CanvasGroup>().alpha == 0f)
					{
						this.menuInStrategia.GetComponent<CanvasGroup>().alpha = 1f;
						this.menuInStrategia.GetComponent<CanvasGroup>().interactable = true;
						this.menuInStrategia.GetComponent<CanvasGroup>().blocksRaycasts = true;
						this.menuAperto = true;
					}
					else
					{
						this.visualizzaEsercito.GetComponent<CanvasGroup>().alpha = 0f;
						this.visualizzaEsercito.GetComponent<CanvasGroup>().interactable = false;
						this.visualizzaEsercito.GetComponent<CanvasGroup>().blocksRaycasts = false;
						this.visualizzaSecondoEsercito.GetComponent<CanvasGroup>().alpha = 0f;
						this.visualizzaSecondoEsercito.GetComponent<CanvasGroup>().interactable = false;
						this.visualizzaSecondoEsercito.GetComponent<CanvasGroup>().blocksRaycasts = false;
						this.visualEserNemico.GetComponent<CanvasGroup>().alpha = 0f;
						this.visualEserNemico.GetComponent<CanvasGroup>().interactable = false;
						this.visualEserNemico.GetComponent<CanvasGroup>().blocksRaycasts = false;
						this.schede.GetComponent<CanvasGroup>().alpha = 0f;
						this.schede.GetComponent<CanvasGroup>().interactable = false;
						this.schede.GetComponent<CanvasGroup>().blocksRaycasts = false;
						this.CentroStanzaUI.GetComponent<CanvasGroup>().alpha = 0f;
						this.CentroStanzaUI.GetComponent<CanvasGroup>().interactable = false;
						this.CentroStanzaUI.GetComponent<CanvasGroup>().blocksRaycasts = false;
						this.visualDettVelAlleati.GetComponent<CanvasGroup>().alpha = 0f;
						this.visualDettVelAlleati.GetComponent<CanvasGroup>().interactable = false;
						this.visualDettVelAlleati.GetComponent<CanvasGroup>().blocksRaycasts = false;
						this.visualDettVelNemici.GetComponent<CanvasGroup>().alpha = 0f;
						this.visualDettVelNemici.GetComponent<CanvasGroup>().interactable = false;
						this.visualDettVelNemici.GetComponent<CanvasGroup>().blocksRaycasts = false;
						this.Headquarters.GetComponent<GestioneEsercitiAlleati>().visualizzaEser = false;
						this.Headquarters.GetComponent<GestioneEsercitiAlleati>().scambioFraEserciti = false;
						this.Headquarters.GetComponent<GestioneEsercitiAlleati>().controlloEserVuoti = true;
						this.domandaLanciareOAbbattereSat.GetComponent<CanvasGroup>().alpha = 0f;
						this.domandaLanciareOAbbattereSat.GetComponent<CanvasGroup>().interactable = false;
						this.domandaLanciareOAbbattereSat.GetComponent<CanvasGroup>().blocksRaycasts = false;
					}
				}
				else
				{
					this.menuInStrategia.GetComponent<CanvasGroup>().alpha = 0f;
					this.menuInStrategia.GetComponent<CanvasGroup>().interactable = false;
					this.menuInStrategia.GetComponent<CanvasGroup>().blocksRaycasts = false;
					this.menuAperto = false;
					this.menuChiudiSalva.GetComponent<CanvasGroup>().alpha = 0f;
					this.menuChiudiSalva.GetComponent<CanvasGroup>().interactable = false;
					this.menuChiudiSalva.GetComponent<CanvasGroup>().blocksRaycasts = false;
					this.menuChiudiSalvaConNome.GetComponent<CanvasGroup>().alpha = 0f;
					this.menuChiudiSalvaConNome.GetComponent<CanvasGroup>().interactable = false;
					this.menuChiudiSalvaConNome.GetComponent<CanvasGroup>().blocksRaycasts = false;
					this.menuChiudiCarica.GetComponent<CanvasGroup>().alpha = 0f;
					this.menuChiudiCarica.GetComponent<CanvasGroup>().interactable = false;
					this.menuChiudiCarica.GetComponent<CanvasGroup>().blocksRaycasts = false;
					this.menuChiudiDomandaCarica.GetComponent<CanvasGroup>().alpha = 0f;
					this.menuChiudiDomandaCarica.GetComponent<CanvasGroup>().interactable = false;
					this.menuChiudiDomandaCarica.GetComponent<CanvasGroup>().blocksRaycasts = false;
					this.menuChiudiCancellamento.GetComponent<CanvasGroup>().alpha = 0f;
					this.menuChiudiCancellamento.GetComponent<CanvasGroup>().interactable = false;
					this.menuChiudiCancellamento.GetComponent<CanvasGroup>().blocksRaycasts = false;
					this.menuChiudiTornaAMenuIniz.GetComponent<CanvasGroup>().alpha = 0f;
					this.menuChiudiTornaAMenuIniz.GetComponent<CanvasGroup>().interactable = false;
					this.menuChiudiTornaAMenuIniz.GetComponent<CanvasGroup>().blocksRaycasts = false;
					this.menuChiudiTornaAMenuIniz.transform.parent.GetComponent<CanvasGroup>().alpha = 0f;
					this.menuChiudiTornaAMenuIniz.transform.parent.GetComponent<CanvasGroup>().interactable = false;
					this.menuChiudiTornaAMenuIniz.transform.parent.GetComponent<CanvasGroup>().blocksRaycasts = false;
					this.menuChiudiTornaADesktop.GetComponent<CanvasGroup>().alpha = 0f;
					this.menuChiudiTornaADesktop.GetComponent<CanvasGroup>().interactable = false;
					this.menuChiudiTornaADesktop.GetComponent<CanvasGroup>().blocksRaycasts = false;
					this.menuChiudiTornaADesktop.transform.parent.GetComponent<CanvasGroup>().alpha = 0f;
					this.menuChiudiTornaADesktop.transform.parent.GetComponent<CanvasGroup>().interactable = false;
					this.menuChiudiTornaADesktop.transform.parent.GetComponent<CanvasGroup>().blocksRaycasts = false;
					this.menuChiudiImpost.GetComponent<CanvasGroup>().alpha = 0f;
					this.menuChiudiImpost.GetComponent<CanvasGroup>().interactable = false;
					this.menuChiudiImpost.GetComponent<CanvasGroup>().blocksRaycasts = false;
				}
			}
			else
			{
				this.cameraGiocatore.GetComponent<GestioneSuoniCamera>().attivaVoce = true;
				this.cameraGiocatore.GetComponent<GestioneSuoniCamera>().nonèVoce = true;
				this.cameraGiocatore.GetComponent<AudioSource>().clip = this.cameraGiocatore.GetComponent<GestioneSuoniCamera>().suonoClickGenerico1;
				if (!this.menuAperto)
				{
					this.menuInTattica.GetComponent<CanvasGroup>().alpha = 1f;
					this.menuInTattica.GetComponent<CanvasGroup>().interactable = true;
					this.menuInTattica.GetComponent<CanvasGroup>().blocksRaycasts = true;
					this.menuAperto = true;
					if (this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().faseSchierInizTerminata)
					{
						this.pausaSempliceAttiva = true;
						this.attivitàSuVelTempo = true;
					}
				}
				else
				{
					this.menuInTattica.GetComponent<CanvasGroup>().alpha = 0f;
					this.menuInTattica.GetComponent<CanvasGroup>().interactable = false;
					this.menuInTattica.GetComponent<CanvasGroup>().blocksRaycasts = false;
					this.menuAperto = false;
					this.menuChiudiTornaACasa.GetComponent<CanvasGroup>().alpha = 0f;
					this.menuChiudiTornaACasa.GetComponent<CanvasGroup>().interactable = false;
					this.menuChiudiTornaACasa.GetComponent<CanvasGroup>().blocksRaycasts = false;
					this.menuChiudiTornaACasa.transform.parent.GetComponent<CanvasGroup>().alpha = 0f;
					this.menuChiudiTornaACasa.transform.parent.GetComponent<CanvasGroup>().interactable = false;
					this.menuChiudiTornaACasa.transform.parent.GetComponent<CanvasGroup>().blocksRaycasts = false;
					this.menuChiudiTornaAMenuIniz.GetComponent<CanvasGroup>().alpha = 0f;
					this.menuChiudiTornaAMenuIniz.GetComponent<CanvasGroup>().interactable = false;
					this.menuChiudiTornaAMenuIniz.GetComponent<CanvasGroup>().blocksRaycasts = false;
					this.menuChiudiTornaAMenuIniz.transform.parent.GetComponent<CanvasGroup>().alpha = 0f;
					this.menuChiudiTornaAMenuIniz.transform.parent.GetComponent<CanvasGroup>().interactable = false;
					this.menuChiudiTornaAMenuIniz.transform.parent.GetComponent<CanvasGroup>().blocksRaycasts = false;
					this.menuChiudiTornaADesktop.GetComponent<CanvasGroup>().alpha = 0f;
					this.menuChiudiTornaADesktop.GetComponent<CanvasGroup>().interactable = false;
					this.menuChiudiTornaADesktop.GetComponent<CanvasGroup>().blocksRaycasts = false;
					this.menuChiudiTornaADesktop.transform.parent.GetComponent<CanvasGroup>().alpha = 0f;
					this.menuChiudiTornaADesktop.transform.parent.GetComponent<CanvasGroup>().interactable = false;
					this.menuChiudiTornaADesktop.transform.parent.GetComponent<CanvasGroup>().blocksRaycasts = false;
					this.menuChiudiImpost.GetComponent<CanvasGroup>().alpha = 0f;
					this.menuChiudiImpost.GetComponent<CanvasGroup>().interactable = false;
					this.menuChiudiImpost.GetComponent<CanvasGroup>().blocksRaycasts = false;
					if (this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().faseSchierInizTerminata)
					{
						this.pausaSempliceAttiva = false;
						this.attivitàSuVelTempo = true;
					}
				}
			}
		}
	}

	// Token: 0x06000869 RID: 2153 RVA: 0x00127E80 File Offset: 0x00126080
	private void VisibilitàCursore()
	{
		if (this.scenaDiMenu)
		{
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
		}
		else if (!this.èInStrategia)
		{
			if (this.cameraGiocatore.GetComponent<PrimaCamera>().cameraAttiva == 3 && !this.pausaSempliceAttiva)
			{
				Cursor.lockState = CursorLockMode.Locked;
				Cursor.visible = false;
			}
			else
			{
				Cursor.lockState = CursorLockMode.None;
				Cursor.visible = true;
			}
		}
	}

	// Token: 0x0600086A RID: 2154 RVA: 0x00127EF4 File Offset: 0x001260F4
	private void PausaSemplice()
	{
		if (Input.GetKeyDown(KeyCode.P) && this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().faseSchierInizTerminata)
		{
			this.pausaSempliceAttiva = !this.pausaSempliceAttiva;
			this.attivitàSuVelTempo = true;
		}
		if (this.pausaSempliceAttiva)
		{
			this.velocitàTempo = 0;
			this.scrittaPausa.GetComponent<Text>().enabled = true;
		}
		else
		{
			this.scrittaPausa.GetComponent<Text>().enabled = false;
			if (Input.GetKeyDown(KeyCode.B) && this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().faseSchierInizTerminata)
			{
				this.indiceListeVelTempo++;
				if (this.indiceListeVelTempo == this.ListaVelocitàTempo.Count)
				{
					this.indiceListeVelTempo = 0;
				}
				this.velocitàTempo = this.ListaVelocitàTempo[this.indiceListeVelTempo];
				this.attivitàSuVelTempo = true;
			}
			if (this.velocitàTempo == 0)
			{
				this.velocitàTempo = 1;
			}
		}
	}

	// Token: 0x0600086B RID: 2155 RVA: 0x00127FF0 File Offset: 0x001261F0
	private void GestioneTempoGioco()
	{
		if (this.velocitàTempo == 0)
		{
			Time.timeScale = 0f;
		}
		else if (this.velocitàTempo == 1)
		{
			Time.timeScale = 1f;
		}
		else if (this.velocitàTempo == 2)
		{
			Time.timeScale = 2f;
		}
		else if (this.velocitàTempo == 3)
		{
			Time.timeScale = 3f;
		}
		else if (this.velocitàTempo == 4)
		{
			Time.timeScale = 4f;
		}
		if (this.attivitàSuVelTempo)
		{
			this.attivitàSuVelTempo = false;
			if (this.velocitàTempo == 0)
			{
				this.pulsantePausa.GetComponent<Image>().color = Color.red;
			}
			else
			{
				this.pulsantePausa.GetComponent<Image>().color = this.colorePulsantiVelTempo;
			}
			if (this.velocitàTempo > 1)
			{
				this.pulsanteVelTempo.GetComponent<Image>().color = Color.red;
			}
			else
			{
				this.pulsanteVelTempo.GetComponent<Image>().color = this.colorePulsantiVelTempo;
			}
			this.scrittaVelTempo.GetComponent<Text>().text = "x" + Time.timeScale.ToString("F0");
		}
	}

	// Token: 0x04001FAD RID: 8109
	public bool scenaDiMenu;

	// Token: 0x04001FAE RID: 8110
	public bool èInStrategia;

	// Token: 0x04001FAF RID: 8111
	public bool èMenuIniziale;

	// Token: 0x04001FB0 RID: 8112
	private GameObject cameraGiocatore;

	// Token: 0x04001FB1 RID: 8113
	private GameObject menuInStrategia;

	// Token: 0x04001FB2 RID: 8114
	private GameObject CanvasStrategia;

	// Token: 0x04001FB3 RID: 8115
	private GameObject visualizzaEsercito;

	// Token: 0x04001FB4 RID: 8116
	private GameObject visualizzaSecondoEsercito;

	// Token: 0x04001FB5 RID: 8117
	private GameObject visualEserNemico;

	// Token: 0x04001FB6 RID: 8118
	private GameObject schede;

	// Token: 0x04001FB7 RID: 8119
	private GameObject CentroStanzaUI;

	// Token: 0x04001FB8 RID: 8120
	private GameObject visualDettVelAlleati;

	// Token: 0x04001FB9 RID: 8121
	private GameObject visualDettVelNemici;

	// Token: 0x04001FBA RID: 8122
	private GameObject Headquarters;

	// Token: 0x04001FBB RID: 8123
	private GameObject menuChiudiSalva;

	// Token: 0x04001FBC RID: 8124
	private GameObject menuChiudiCarica;

	// Token: 0x04001FBD RID: 8125
	private GameObject menuChiudiDomandaCarica;

	// Token: 0x04001FBE RID: 8126
	private GameObject menuChiudiSalvaConNome;

	// Token: 0x04001FBF RID: 8127
	private GameObject menuChiudiCancellamento;

	// Token: 0x04001FC0 RID: 8128
	private GameObject menuChiudiTornaAMenuIniz;

	// Token: 0x04001FC1 RID: 8129
	private GameObject menuChiudiTornaADesktop;

	// Token: 0x04001FC2 RID: 8130
	private GameObject menuChiudiImpost;

	// Token: 0x04001FC3 RID: 8131
	private GameObject menuChiudiTornaACasa;

	// Token: 0x04001FC4 RID: 8132
	private GameObject domandaLanciareOAbbattereSat;

	// Token: 0x04001FC5 RID: 8133
	private GameObject menuInTattica;

	// Token: 0x04001FC6 RID: 8134
	private GameObject infoNeutreTattica;

	// Token: 0x04001FC7 RID: 8135
	private GameObject scrittaPausa;

	// Token: 0x04001FC8 RID: 8136
	private GameObject scrittaVelTempo;

	// Token: 0x04001FC9 RID: 8137
	private GameObject pulsantePausa;

	// Token: 0x04001FCA RID: 8138
	private GameObject pulsanteVelTempo;

	// Token: 0x04001FCB RID: 8139
	public bool menuAperto;

	// Token: 0x04001FCC RID: 8140
	public bool pausaSempliceAttiva;

	// Token: 0x04001FCD RID: 8141
	public int velocitàTempo;

	// Token: 0x04001FCE RID: 8142
	public List<int> ListaVelocitàTempo;

	// Token: 0x04001FCF RID: 8143
	public int indiceListeVelTempo;

	// Token: 0x04001FD0 RID: 8144
	public bool attivitàSuVelTempo;

	// Token: 0x04001FD1 RID: 8145
	public Color colorePulsantiVelTempo;
}
