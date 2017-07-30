using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200010D RID: 269
public class PulsFitPerBattaglia : MonoBehaviour
{
	// Token: 0x0600086D RID: 2157 RVA: 0x0012813C File Offset: 0x0012633C
	public void AttivazioneArma(int numeroArma)
	{
		this.infoAlleati = GameObject.FindGameObjectWithTag("Info Alleati");
		GameObject gameObject = GameObject.FindGameObjectWithTag("InfoNeutreTattica");
		GameObject gameObject2 = gameObject.GetComponent<GestoreNeutroTattica>().ListaCamere[0];
		gameObject2.GetComponent<GestioneSuoniCamera>().attivaVoce = true;
		gameObject2.GetComponent<GestioneSuoniCamera>().nonèVoce = true;
		gameObject2.GetComponent<AudioSource>().clip = gameObject2.GetComponent<GestioneSuoniCamera>().suonoClickGenerico1;
		for (int i = 0; i < 4; i++)
		{
			if (numeroArma == i)
			{
				if (this.infoAlleati.GetComponent<GestioneComandanteInUI>().ListaPosizione0[0].GetComponent<PresenzaAlleato>().ListaArmiAttivate[i])
				{
					foreach (GameObject current in this.infoAlleati.GetComponent<GestioneComandanteInUI>().ListaPosizione0)
					{
						current.GetComponent<PresenzaAlleato>().ListaArmiAttivate[i] = false;
					}
				}
				else
				{
					foreach (GameObject current2 in this.infoAlleati.GetComponent<GestioneComandanteInUI>().ListaPosizione0)
					{
						current2.GetComponent<PresenzaAlleato>().ListaArmiAttivate[i] = true;
					}
				}
				break;
			}
		}
	}

	// Token: 0x0600086E RID: 2158 RVA: 0x001282D0 File Offset: 0x001264D0
	public void VisualizzaMunizionamento(int numeroArmaSelPulsante)
	{
		this.infoAlleati = GameObject.FindGameObjectWithTag("Info Alleati");
		GameObject gameObject = GameObject.FindGameObjectWithTag("InfoNeutreTattica");
		GameObject gameObject2 = gameObject.GetComponent<GestoreNeutroTattica>().ListaCamere[0];
		gameObject2.GetComponent<GestioneSuoniCamera>().attivaVoce = true;
		gameObject2.GetComponent<GestioneSuoniCamera>().nonèVoce = true;
		gameObject2.GetComponent<AudioSource>().clip = gameObject2.GetComponent<GestioneSuoniCamera>().suonoClickGenerico1;
		this.infoAlleati.GetComponent<GestioneComandanteInUI>().numeroArmaSelezionata = numeroArmaSelPulsante;
		for (int i = 0; i < 5; i++)
		{
			this.infoAlleati.GetComponent<GestioneComandanteInUI>().ListaTipiMunizioni[i].GetComponent<CanvasGroup>().alpha = 0f;
			this.infoAlleati.GetComponent<GestioneComandanteInUI>().ListaTipiMunizioni[i].GetComponent<CanvasGroup>().interactable = false;
			this.infoAlleati.GetComponent<GestioneComandanteInUI>().ListaTipiMunizioni[i].GetComponent<CanvasGroup>().blocksRaycasts = false;
		}
	}

	// Token: 0x0600086F RID: 2159 RVA: 0x001283C4 File Offset: 0x001265C4
	public void SpuntaComportDifensivo()
	{
		this.infoAlleati = GameObject.FindGameObjectWithTag("Info Alleati");
		GameObject gameObject = GameObject.FindGameObjectWithTag("InfoNeutreTattica");
		GameObject gameObject2 = gameObject.GetComponent<GestoreNeutroTattica>().ListaCamere[0];
		if (this.infoAlleati.GetComponent<GestioneComandanteInUI>().ListaPosizione0[0].GetComponent<PresenzaAlleato>().comportamentoDifensivo)
		{
			foreach (GameObject current in this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaSelezionamento)
			{
				current.GetComponent<PresenzaAlleato>().comportamentoDifensivo = false;
				current.GetComponent<PresenzaAlleato>().ricercaAutomaticaBersaglio = false;
				current.GetComponent<PresenzaAlleato>().ricercaAutomDifensivaBers = false;
				current.GetComponent<PresenzaAlleato>().ricercaAutomDifensivaBersVicino = false;
			}
		}
		else
		{
			gameObject2.GetComponent<GestioneSuoniCamera>().attivaVoce = true;
			gameObject2.GetComponent<GestioneSuoniCamera>().numListaVoceSelez = 9;
			foreach (GameObject current2 in this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaSelezionamento)
			{
				current2.GetComponent<PresenzaAlleato>().comportamentoDifensivo = false;
				current2.GetComponent<PresenzaAlleato>().ricercaAutomaticaBersaglio = false;
				current2.GetComponent<PresenzaAlleato>().ricercaAutomDifensivaBers = false;
				current2.GetComponent<PresenzaAlleato>().ricercaAutomDifensivaBersVicino = false;
				current2.GetComponent<PresenzaAlleato>().comportamentoDifensivo = true;
			}
		}
	}

	// Token: 0x06000870 RID: 2160 RVA: 0x0012856C File Offset: 0x0012676C
	public void SpuntaRicerBersAutomatica()
	{
		this.infoAlleati = GameObject.FindGameObjectWithTag("Info Alleati");
		GameObject gameObject = GameObject.FindGameObjectWithTag("InfoNeutreTattica");
		GameObject gameObject2 = gameObject.GetComponent<GestoreNeutroTattica>().ListaCamere[0];
		if (this.infoAlleati.GetComponent<GestioneComandanteInUI>().ListaPosizione0[0].GetComponent<PresenzaAlleato>().ricercaAutomaticaBersaglio)
		{
			foreach (GameObject current in this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaSelezionamento)
			{
				current.GetComponent<PresenzaAlleato>().ricercaAutomaticaBersaglio = false;
				current.GetComponent<PresenzaAlleato>().ricercaAutomaticaBersaglio = false;
				current.GetComponent<PresenzaAlleato>().ricercaAutomDifensivaBers = false;
				current.GetComponent<PresenzaAlleato>().ricercaAutomDifensivaBersVicino = false;
			}
		}
		else
		{
			gameObject2.GetComponent<GestioneSuoniCamera>().attivaVoce = true;
			gameObject2.GetComponent<GestioneSuoniCamera>().numListaVoceSelez = 10;
			foreach (GameObject current2 in this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaSelezionamento)
			{
				current2.GetComponent<PresenzaAlleato>().comportamentoDifensivo = false;
				current2.GetComponent<PresenzaAlleato>().ricercaAutomaticaBersaglio = false;
				current2.GetComponent<PresenzaAlleato>().ricercaAutomDifensivaBers = false;
				current2.GetComponent<PresenzaAlleato>().ricercaAutomDifensivaBersVicino = false;
				current2.GetComponent<PresenzaAlleato>().ricercaAutomaticaBersaglio = true;
			}
		}
	}

	// Token: 0x06000871 RID: 2161 RVA: 0x00128714 File Offset: 0x00126914
	public void SpuntaRicerBersAutomaticaDifensiva()
	{
		this.infoAlleati = GameObject.FindGameObjectWithTag("Info Alleati");
		GameObject gameObject = GameObject.FindGameObjectWithTag("InfoNeutreTattica");
		GameObject gameObject2 = gameObject.GetComponent<GestoreNeutroTattica>().ListaCamere[0];
		if (this.infoAlleati.GetComponent<GestioneComandanteInUI>().ListaPosizione0[0].GetComponent<PresenzaAlleato>().ricercaAutomDifensivaBers)
		{
			foreach (GameObject current in this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaSelezionamento)
			{
				current.GetComponent<PresenzaAlleato>().ricercaAutomDifensivaBers = false;
				current.GetComponent<PresenzaAlleato>().ricercaAutomaticaBersaglio = false;
				current.GetComponent<PresenzaAlleato>().ricercaAutomDifensivaBers = false;
				current.GetComponent<PresenzaAlleato>().ricercaAutomDifensivaBersVicino = false;
			}
		}
		else
		{
			gameObject2.GetComponent<GestioneSuoniCamera>().attivaVoce = true;
			gameObject2.GetComponent<GestioneSuoniCamera>().numListaVoceSelez = 11;
			foreach (GameObject current2 in this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaSelezionamento)
			{
				current2.GetComponent<PresenzaAlleato>().comportamentoDifensivo = false;
				current2.GetComponent<PresenzaAlleato>().ricercaAutomaticaBersaglio = false;
				current2.GetComponent<PresenzaAlleato>().ricercaAutomDifensivaBers = false;
				current2.GetComponent<PresenzaAlleato>().ricercaAutomDifensivaBersVicino = false;
				current2.GetComponent<PresenzaAlleato>().ricercaAutomDifensivaBers = true;
			}
		}
	}

	// Token: 0x06000872 RID: 2162 RVA: 0x001288BC File Offset: 0x00126ABC
	public void SpuntaRicerBersAutomaticaDifensivaVicin()
	{
		this.infoAlleati = GameObject.FindGameObjectWithTag("Info Alleati");
		GameObject gameObject = GameObject.FindGameObjectWithTag("InfoNeutreTattica");
		GameObject gameObject2 = gameObject.GetComponent<GestoreNeutroTattica>().ListaCamere[0];
		if (this.infoAlleati.GetComponent<GestioneComandanteInUI>().ListaPosizione0[0].GetComponent<PresenzaAlleato>().ricercaAutomDifensivaBersVicino)
		{
			foreach (GameObject current in this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaSelezionamento)
			{
				current.GetComponent<PresenzaAlleato>().ricercaAutomDifensivaBersVicino = false;
				current.GetComponent<PresenzaAlleato>().ricercaAutomaticaBersaglio = false;
				current.GetComponent<PresenzaAlleato>().ricercaAutomDifensivaBers = false;
				current.GetComponent<PresenzaAlleato>().ricercaAutomDifensivaBersVicino = false;
			}
		}
		else
		{
			gameObject2.GetComponent<GestioneSuoniCamera>().attivaVoce = true;
			gameObject2.GetComponent<GestioneSuoniCamera>().numListaVoceSelez = 12;
			foreach (GameObject current2 in this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaSelezionamento)
			{
				current2.GetComponent<PresenzaAlleato>().comportamentoDifensivo = false;
				current2.GetComponent<PresenzaAlleato>().ricercaAutomaticaBersaglio = false;
				current2.GetComponent<PresenzaAlleato>().ricercaAutomDifensivaBers = false;
				current2.GetComponent<PresenzaAlleato>().ricercaAutomDifensivaBersVicino = false;
				current2.GetComponent<PresenzaAlleato>().ricercaAutomDifensivaBersVicino = true;
			}
		}
	}

	// Token: 0x06000873 RID: 2163 RVA: 0x00128A64 File Offset: 0x00126C64
	public void SpuntaTipoMunizione(int numeroTipo)
	{
		this.infoAlleati = GameObject.FindGameObjectWithTag("Info Alleati");
		GameObject gameObject = GameObject.FindGameObjectWithTag("InfoNeutreTattica");
		GameObject gameObject2 = gameObject.GetComponent<GestoreNeutroTattica>().ListaCamere[0];
		gameObject2.GetComponent<GestioneSuoniCamera>().attivaVoce = true;
		gameObject2.GetComponent<GestioneSuoniCamera>().nonèVoce = true;
		gameObject2.GetComponent<AudioSource>().clip = gameObject2.GetComponent<GestioneSuoniCamera>().suonoClickGenerico1;
		int numeroArmaSelezionata = this.infoAlleati.GetComponent<GestioneComandanteInUI>().numeroArmaSelezionata;
		if (numeroTipo == 1)
		{
			foreach (GameObject current in this.infoAlleati.GetComponent<GestioneComandanteInUI>().ListaPosizione0)
			{
				current.GetComponent<PresenzaAlleato>().ListaMunizioniAttive[numeroArmaSelezionata] = current.GetComponent<PresenzaAlleato>().ListaMunizioneArmi[numeroArmaSelezionata][0];
			}
		}
		else if (numeroTipo == 2)
		{
			foreach (GameObject current2 in this.infoAlleati.GetComponent<GestioneComandanteInUI>().ListaPosizione0)
			{
				current2.GetComponent<PresenzaAlleato>().ListaMunizioniAttive[numeroArmaSelezionata] = current2.GetComponent<PresenzaAlleato>().ListaMunizioneArmi[numeroArmaSelezionata][1];
			}
		}
		else if (numeroTipo == 3)
		{
			foreach (GameObject current3 in this.infoAlleati.GetComponent<GestioneComandanteInUI>().ListaPosizione0)
			{
				current3.GetComponent<PresenzaAlleato>().ListaMunizioniAttive[numeroArmaSelezionata] = current3.GetComponent<PresenzaAlleato>().ListaMunizioneArmi[numeroArmaSelezionata][2];
			}
		}
		else if (numeroTipo == 4)
		{
			foreach (GameObject current4 in this.infoAlleati.GetComponent<GestioneComandanteInUI>().ListaPosizione0)
			{
				current4.GetComponent<PresenzaAlleato>().ListaMunizioniAttive[numeroArmaSelezionata] = current4.GetComponent<PresenzaAlleato>().ListaMunizioneArmi[numeroArmaSelezionata][3];
			}
		}
		else if (numeroTipo == 5)
		{
			foreach (GameObject current5 in this.infoAlleati.GetComponent<GestioneComandanteInUI>().ListaPosizione0)
			{
				current5.GetComponent<PresenzaAlleato>().ListaMunizioniAttive[numeroArmaSelezionata] = current5.GetComponent<PresenzaAlleato>().ListaMunizioneArmi[numeroArmaSelezionata][4];
			}
		}
	}

	// Token: 0x06000874 RID: 2164 RVA: 0x00128DB0 File Offset: 0x00126FB0
	public void InformazioniUnità(int quadrato)
	{
		this.infoAlleati = GameObject.FindGameObjectWithTag("Info Alleati");
		GameObject gameObject = GameObject.FindGameObjectWithTag("CanvasComandante").transform.FindChild("Informazioni Unità").gameObject;
		GameObject gameObject2 = GameObject.FindGameObjectWithTag("InfoNeutreTattica");
		GameObject gameObject3 = gameObject2.GetComponent<GestoreNeutroTattica>().ListaCamere[0];
		gameObject3.GetComponent<GestioneSuoniCamera>().attivaVoce = true;
		gameObject3.GetComponent<GestioneSuoniCamera>().nonèVoce = true;
		gameObject3.GetComponent<AudioSource>().clip = gameObject3.GetComponent<GestioneSuoniCamera>().suonoClickGenerico1;
		gameObject.GetComponent<CanvasGroup>().alpha = 1f;
		gameObject.GetComponent<CanvasGroup>().interactable = false;
		gameObject.GetComponent<CanvasGroup>().blocksRaycasts = false;
		this.infoAlleati.GetComponent<GestioneComandanteInUI>().visualInfoTrappola = false;
		this.infoAlleati.GetComponent<GestioneComandanteInUI>().infoDaRinforzi = false;
		this.infoAlleati.GetComponent<GestioneComandanteInUI>().aggiornaAlleatoPerInfo = true;
	}

	// Token: 0x06000875 RID: 2165 RVA: 0x00128E94 File Offset: 0x00127094
	public void AttaccoZonaBombardamentoPulsante()
	{
		GameObject gameObject = GameObject.FindGameObjectWithTag("Attacchi Speciali Alleati");
		GameObject gameObject2 = GameObject.FindGameObjectWithTag("InfoNeutreTattica");
		GameObject gameObject3 = gameObject2.GetComponent<GestoreNeutroTattica>().ListaCamere[0];
		gameObject3.GetComponent<GestioneSuoniCamera>().attivaVoce = true;
		gameObject3.GetComponent<GestioneSuoniCamera>().nonèVoce = true;
		gameObject3.GetComponent<AudioSource>().clip = gameObject3.GetComponent<GestioneSuoniCamera>().suonoClickGenerico1;
		if (gameObject.GetComponent<AttacchiAlleatiSpecialiScript>().bombardiereInLista)
		{
			gameObject.GetComponent<AttacchiAlleatiSpecialiScript>().attaccoZonaBomb = !gameObject.GetComponent<AttacchiAlleatiSpecialiScript>().attaccoZonaBomb;
			if (gameObject.GetComponent<AttacchiAlleatiSpecialiScript>().attaccoZonaBomb)
			{
				Selezionamento.selezioneInvalidata = true;
				gameObject.GetComponent<AttacchiAlleatiSpecialiScript>().timerAnnullamentoBomb = 0f;
			}
			else
			{
				Selezionamento.selezioneInvalidata = false;
			}
			if (!gameObject.GetComponent<AttacchiAlleatiSpecialiScript>().attaccoZonaBomb && gameObject.GetComponent<AttacchiAlleatiSpecialiScript>().mirinoBombardamento)
			{
				UnityEngine.Object.Destroy(gameObject.GetComponent<AttacchiAlleatiSpecialiScript>().mirinoBombardamento);
				gameObject.GetComponent<AttacchiAlleatiSpecialiScript>().mirinoBombCreato = false;
			}
		}
	}

	// Token: 0x06000876 RID: 2166 RVA: 0x00128F94 File Offset: 0x00127194
	public void AttaccoZonaArtiglieriaPulsante()
	{
		GameObject gameObject = GameObject.FindGameObjectWithTag("Attacchi Speciali Alleati");
		GameObject gameObject2 = GameObject.FindGameObjectWithTag("InfoNeutreTattica");
		GameObject gameObject3 = gameObject2.GetComponent<GestoreNeutroTattica>().ListaCamere[0];
		gameObject3.GetComponent<GestioneSuoniCamera>().attivaVoce = true;
		gameObject3.GetComponent<GestioneSuoniCamera>().nonèVoce = true;
		gameObject3.GetComponent<AudioSource>().clip = gameObject3.GetComponent<GestioneSuoniCamera>().suonoClickGenerico1;
		if (gameObject.GetComponent<AttacchiAlleatiSpecialiScript>().artiglieriaInLista)
		{
			gameObject.GetComponent<AttacchiAlleatiSpecialiScript>().attaccoZonaArt = !gameObject.GetComponent<AttacchiAlleatiSpecialiScript>().attaccoZonaArt;
			if (gameObject.GetComponent<AttacchiAlleatiSpecialiScript>().attaccoZonaArt)
			{
				Selezionamento.selezioneInvalidata = true;
				gameObject.GetComponent<AttacchiAlleatiSpecialiScript>().timerAnnullamentoArt = 0f;
			}
			else
			{
				Selezionamento.selezioneInvalidata = false;
			}
			if (!gameObject.GetComponent<AttacchiAlleatiSpecialiScript>().attaccoZonaArt && gameObject.GetComponent<AttacchiAlleatiSpecialiScript>().mirinoArtiglieria)
			{
				UnityEngine.Object.Destroy(gameObject.GetComponent<AttacchiAlleatiSpecialiScript>().mirinoArtiglieria);
				gameObject.GetComponent<AttacchiAlleatiSpecialiScript>().mirinoArtCreato = false;
			}
		}
	}

	// Token: 0x06000877 RID: 2167 RVA: 0x00129094 File Offset: 0x00127294
	public void StopAzioniPulsante()
	{
		this.infoAlleati = GameObject.FindGameObjectWithTag("Info Alleati");
		GameObject gameObject = GameObject.FindGameObjectWithTag("InfoNeutreTattica");
		GameObject gameObject2 = gameObject.GetComponent<GestoreNeutroTattica>().ListaCamere[0];
		gameObject2.GetComponent<GestioneSuoniCamera>().attivaVoce = true;
		gameObject2.GetComponent<GestioneSuoniCamera>().numListaVoceSelez = 8;
		foreach (GameObject current in this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaSelezionamento)
		{
			current.GetComponent<PresenzaAlleato>().attaccoOrdinato = false;
			current.GetComponent<PresenzaAlleato>().unitàBersaglio = null;
			current.GetComponent<PresenzaAlleato>().attaccoZonaOrdinato = false;
			current.GetComponent<PresenzaAlleato>().destinazioneOrdinata = false;
			if (current.GetComponent<NavMeshAgent>() && current.GetComponent<PresenzaAlleato>().tipoTruppa != 32)
			{
				current.GetComponent<NavMeshAgent>().SetDestination(current.transform.position);
			}
		}
	}

	// Token: 0x06000878 RID: 2168 RVA: 0x001291AC File Offset: 0x001273AC
	public void AttivazioneDinamite()
	{
		this.infoAlleati = GameObject.FindGameObjectWithTag("Info Alleati");
		GameObject gameObject = GameObject.FindGameObjectWithTag("InfoNeutreTattica");
		GameObject gameObject2 = gameObject.GetComponent<GestoreNeutroTattica>().ListaCamere[0];
		gameObject2.GetComponent<GestioneSuoniCamera>().attivaVoce = true;
		gameObject2.GetComponent<GestioneSuoniCamera>().nonèVoce = true;
		gameObject2.GetComponent<AudioSource>().clip = gameObject2.GetComponent<GestioneSuoniCamera>().suonoClickGenerico1;
		foreach (GameObject current in this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaSelezionamento)
		{
			if (current.GetComponent<PresenzaAlleato>().èGeniere && current.GetComponent<ATT_Sapper>().dinamitePossibile)
			{
				current.GetComponent<ATT_Sapper>().piazzDinamiteAttivo = true;
			}
		}
	}

	// Token: 0x06000879 RID: 2169 RVA: 0x0012929C File Offset: 0x0012749C
	public void ApriRinforzi()
	{
		GameObject gameObject = GameObject.FindGameObjectWithTag("InfoNeutreTattica");
		GameObject gameObject2 = GameObject.FindGameObjectWithTag("CanvasComandante").transform.FindChild("Barra Superiore").FindChild("Barra Rinforzi").FindChild("barra tipologie").gameObject;
		GameObject gameObject3 = GameObject.FindGameObjectWithTag("CanvasComandante").transform.FindChild("Barra Superiore").FindChild("Barra Rinforzi").FindChild("elenco unità").gameObject;
		this.infoAlleati = GameObject.FindGameObjectWithTag("Info Alleati");
		GameObject gameObject4 = gameObject.GetComponent<GestoreNeutroTattica>().ListaCamere[0];
		gameObject4.GetComponent<GestioneSuoniCamera>().attivaVoce = true;
		gameObject4.GetComponent<GestioneSuoniCamera>().nonèVoce = true;
		gameObject4.GetComponent<AudioSource>().clip = gameObject4.GetComponent<GestioneSuoniCamera>().suonoClickGenerico1;
		if (!this.infoAlleati.GetComponent<GestioneComandanteInUI>().preparLancioParàAttivo)
		{
			if (gameObject2.GetComponent<CanvasGroup>().alpha == 0f)
			{
				gameObject2.GetComponent<CanvasGroup>().alpha = 1f;
				gameObject2.GetComponent<CanvasGroup>().interactable = true;
				gameObject2.GetComponent<CanvasGroup>().blocksRaycasts = true;
				gameObject.GetComponent<GestoreNeutroTattica>().scattoInSchierAttivo = true;
			}
			else
			{
				gameObject2.GetComponent<CanvasGroup>().alpha = 0f;
				gameObject2.GetComponent<CanvasGroup>().interactable = false;
				gameObject2.GetComponent<CanvasGroup>().blocksRaycasts = false;
				for (int i = 0; i < gameObject3.transform.childCount; i++)
				{
					gameObject3.transform.GetChild(i).GetComponent<CanvasGroup>().alpha = 0f;
					gameObject3.transform.GetChild(i).GetComponent<CanvasGroup>().interactable = false;
					gameObject3.transform.GetChild(i).GetComponent<CanvasGroup>().blocksRaycasts = false;
				}
				gameObject.GetComponent<GestoreNeutroTattica>().scattoInSchierNonAttivo = true;
				this.infoAlleati.GetComponent<GestioneComandanteInUI>().chiudiRinforzi = true;
			}
		}
	}

	// Token: 0x0600087A RID: 2170 RVA: 0x00129480 File Offset: 0x00127680
	public void TipoDiElencoRinforzi(int tipo)
	{
		this.infoAlleati = GameObject.FindGameObjectWithTag("Info Alleati");
		GameObject gameObject = GameObject.FindGameObjectWithTag("InfoNeutreTattica");
		GameObject gameObject2 = gameObject.GetComponent<GestoreNeutroTattica>().ListaCamere[0];
		gameObject2.GetComponent<GestioneSuoniCamera>().attivaVoce = true;
		gameObject2.GetComponent<GestioneSuoniCamera>().nonèVoce = true;
		gameObject2.GetComponent<AudioSource>().clip = gameObject2.GetComponent<GestioneSuoniCamera>().suonoClickGenerico1;
		this.infoAlleati.GetComponent<GestioneComandanteInUI>().tipoElencoRinforzi = tipo;
		this.infoAlleati.GetComponent<GestioneComandanteInUI>().aggiornaElencoRinforzi = true;
	}

	// Token: 0x0600087B RID: 2171 RVA: 0x0012950C File Offset: 0x0012770C
	public void SelezUnitàRinforzo(int numSlot)
	{
		this.infoAlleati = GameObject.FindGameObjectWithTag("Info Alleati");
		GameObject gameObject = GameObject.FindGameObjectWithTag("InfoNeutreTattica");
		GameObject gameObject2 = gameObject.GetComponent<GestoreNeutroTattica>().ListaCamere[0];
		this.infoAlleati.GetComponent<GestioneComandanteInUI>().slotRinforzoSelez = numSlot;
		this.infoAlleati.GetComponent<GestioneComandanteInUI>().aggiornaElencoRinforzi = true;
		this.infoAlleati.GetComponent<GestioneComandanteInUI>().creaAlleatoPerSchieramento = true;
		gameObject2.GetComponent<GestioneSuoniCamera>().attivaVoce = true;
		gameObject2.GetComponent<GestioneSuoniCamera>().nonèVoce = true;
		gameObject2.GetComponent<AudioSource>().clip = gameObject2.GetComponent<GestioneSuoniCamera>().suonoSelezRinforzo;
	}

	// Token: 0x0600087C RID: 2172 RVA: 0x001295A8 File Offset: 0x001277A8
	public void ApriOChiudiQuadroAerei()
	{
		this.infoAlleati = GameObject.FindGameObjectWithTag("Info Alleati");
		GameObject gameObject = GameObject.FindGameObjectWithTag("InfoNeutreTattica");
		GameObject gameObject2 = gameObject.GetComponent<GestoreNeutroTattica>().ListaCamere[0];
		gameObject2.GetComponent<GestioneSuoniCamera>().attivaVoce = true;
		gameObject2.GetComponent<GestioneSuoniCamera>().nonèVoce = true;
		gameObject2.GetComponent<AudioSource>().clip = gameObject2.GetComponent<GestioneSuoniCamera>().suonoClickGenerico1;
		this.infoAlleati.GetComponent<GestioneComandanteInUI>().pulsApriChiudiQAPremuto = true;
	}

	// Token: 0x0600087D RID: 2173 RVA: 0x00129624 File Offset: 0x00127824
	public void SelezAereoDaQuadro(int numSlot)
	{
		this.infoAlleati = GameObject.FindGameObjectWithTag("Info Alleati");
		GameObject gameObject = GameObject.FindGameObjectWithTag("InfoNeutreTattica");
		GameObject gameObject2 = gameObject.GetComponent<GestoreNeutroTattica>().ListaCamere[0];
		gameObject2.GetComponent<GestioneSuoniCamera>().attivaVoce = true;
		gameObject2.GetComponent<GestioneSuoniCamera>().nonèVoce = true;
		gameObject2.GetComponent<AudioSource>().clip = gameObject2.GetComponent<GestioneSuoniCamera>().suonoClickGenerico1;
		this.infoAlleati.GetComponent<InfoGenericheAlleati>().cliccatoSuQuadroAerei = true;
		this.infoAlleati.GetComponent<InfoGenericheAlleati>().numAereoInQuadro = numSlot;
	}

	// Token: 0x0600087E RID: 2174 RVA: 0x001296B0 File Offset: 0x001278B0
	public void RichiamaAereo()
	{
		this.infoAlleati = GameObject.FindGameObjectWithTag("Info Alleati");
		GameObject gameObject = GameObject.FindGameObjectWithTag("InfoNeutreTattica");
		GameObject gameObject2 = gameObject.GetComponent<GestoreNeutroTattica>().ListaCamere[0];
		gameObject2.GetComponent<GestioneSuoniCamera>().attivaVoce = true;
		gameObject2.GetComponent<GestioneSuoniCamera>().nonèVoce = true;
		gameObject2.GetComponent<AudioSource>().clip = gameObject2.GetComponent<GestioneSuoniCamera>().suonoClickGenerico1;
		this.infoAlleati.GetComponent<InfoGenericheAlleati>().richiamoAereo = true;
	}

	// Token: 0x0600087F RID: 2175 RVA: 0x0012972C File Offset: 0x0012792C
	public void InfoInRinforzi(int numSlot)
	{
		this.infoAlleati = GameObject.FindGameObjectWithTag("Info Alleati");
		GameObject gameObject = GameObject.FindGameObjectWithTag("CanvasComandante").transform.FindChild("Informazioni Unità").gameObject;
		GameObject gameObject2 = GameObject.FindGameObjectWithTag("InfoNeutreTattica");
		GameObject gameObject3 = gameObject2.GetComponent<GestoreNeutroTattica>().ListaCamere[0];
		gameObject3.GetComponent<GestioneSuoniCamera>().attivaVoce = true;
		gameObject3.GetComponent<GestioneSuoniCamera>().nonèVoce = true;
		gameObject3.GetComponent<AudioSource>().clip = gameObject3.GetComponent<GestioneSuoniCamera>().suonoClickGenerico1;
		gameObject.GetComponent<CanvasGroup>().alpha = 1f;
		gameObject.GetComponent<CanvasGroup>().interactable = false;
		gameObject.GetComponent<CanvasGroup>().blocksRaycasts = false;
		this.infoAlleati.GetComponent<GestioneComandanteInUI>().visualInfoTrappola = false;
		this.infoAlleati.GetComponent<GestioneComandanteInUI>().slotPerInfoRinforzi = numSlot;
		this.infoAlleati.GetComponent<GestioneComandanteInUI>().infoDaRinforzi = true;
		this.infoAlleati.GetComponent<GestioneComandanteInUI>().aggiornaAlleatoPerInfo = true;
	}

	// Token: 0x06000880 RID: 2176 RVA: 0x00129820 File Offset: 0x00127A20
	public void IniziaBattaglia()
	{
		GameObject gameObject = GameObject.FindGameObjectWithTag("InfoNeutreTattica");
		GameObject gameObject2 = GameObject.FindGameObjectWithTag("CanvasComandante").transform.FindChild("Varie Battaglia").FindChild("Sfondo scritta inizio").gameObject;
		GameObject gameObject3 = GameObject.FindGameObjectWithTag("CanvasComandante").transform.FindChild("Barra Superiore").FindChild("Barra Rinforzi").FindChild("barra tipologie").gameObject;
		GameObject gameObject4 = GameObject.FindGameObjectWithTag("CanvasComandante").transform.FindChild("Barra Superiore").FindChild("Barra Rinforzi").FindChild("elenco unità").gameObject;
		GameObject gameObject5 = gameObject.GetComponent<GestoreNeutroTattica>().ListaCamere[0];
		GameObject gameObject6 = GameObject.FindGameObjectWithTag("InizioLivello");
		GameObject gameObject7 = GameObject.FindGameObjectWithTag("CanvasComandante").transform.FindChild("Barra Superiore").FindChild("Timer").FindChild("pulsante Pausa").gameObject;
		GameObject gameObject8 = GameObject.FindGameObjectWithTag("CanvasComandante").transform.FindChild("Barra Superiore").FindChild("Timer").FindChild("pulsante Velocità").gameObject;
		this.infoAlleati = GameObject.FindGameObjectWithTag("Info Alleati");
		gameObject5.GetComponent<GestioneSuoniCamera>().attivaVoce = true;
		gameObject5.GetComponent<GestioneSuoniCamera>().nonèVoce = true;
		gameObject5.GetComponent<AudioSource>().clip = gameObject5.GetComponent<GestioneSuoniCamera>().suonoClickGenerico1;
		gameObject.GetComponent<GestoreNeutroTattica>().faseSchierInizTerminata = true;
		gameObject.GetComponent<GestoreNeutroTattica>().schieramentoAttivo = false;
		gameObject2.GetComponent<CanvasGroup>().alpha = 0f;
		gameObject2.GetComponent<CanvasGroup>().interactable = false;
		gameObject2.GetComponent<CanvasGroup>().blocksRaycasts = false;
		gameObject3.GetComponent<CanvasGroup>().alpha = 0f;
		gameObject3.GetComponent<CanvasGroup>().interactable = false;
		gameObject3.GetComponent<CanvasGroup>().blocksRaycasts = false;
		for (int i = 0; i < gameObject4.transform.childCount; i++)
		{
			gameObject4.transform.GetChild(i).GetComponent<CanvasGroup>().alpha = 0f;
			gameObject4.transform.GetChild(i).GetComponent<CanvasGroup>().interactable = false;
			gameObject4.transform.GetChild(i).GetComponent<CanvasGroup>().blocksRaycasts = false;
		}
		gameObject.GetComponent<GestoreNeutroTattica>().scattoInSchierNonAttivo = true;
		gameObject6.GetComponent<OltreScene>().attivitàSuVelTempo = true;
		gameObject7.GetComponent<Button>().interactable = true;
		gameObject8.GetComponent<Button>().interactable = true;
		UnityEngine.Object.Destroy(this.infoAlleati.GetComponent<GestioneComandanteInUI>().alleatoèDaSchier);
		this.infoAlleati.GetComponent<GestioneComandanteInUI>().alleatoèDaSchier = null;
		this.infoAlleati.GetComponent<GestioneComandanteInUI>().alleatoRealeèDaSchier = null;
		this.infoAlleati.GetComponent<GestioneComandanteInUI>().alleatoDaPosizionare = false;
	}

	// Token: 0x06000881 RID: 2177 RVA: 0x00129AE0 File Offset: 0x00127CE0
	public void ApriChiudiMenu()
	{
		GameObject gameObject = GameObject.FindGameObjectWithTag("InizioLivello");
		GameObject gameObject2 = GameObject.FindGameObjectWithTag("CanvasMenu").transform.FindChild("Menu Tattico").gameObject;
		GameObject gameObject3 = GameObject.FindGameObjectWithTag("InfoNeutreTattica");
		GameObject gameObject4 = gameObject3.GetComponent<GestoreNeutroTattica>().ListaCamere[0];
		gameObject4.GetComponent<GestioneSuoniCamera>().attivaVoce = true;
		gameObject4.GetComponent<GestioneSuoniCamera>().nonèVoce = true;
		gameObject4.GetComponent<AudioSource>().clip = gameObject4.GetComponent<GestioneSuoniCamera>().suonoClickGenerico1;
		if (!gameObject.GetComponent<OltreScene>().menuAperto)
		{
			gameObject2.GetComponent<CanvasGroup>().alpha = 1f;
			gameObject2.GetComponent<CanvasGroup>().interactable = true;
			gameObject2.GetComponent<CanvasGroup>().blocksRaycasts = true;
			gameObject.GetComponent<OltreScene>().menuAperto = true;
			gameObject.GetComponent<OltreScene>().pausaSempliceAttiva = true;
		}
		else
		{
			gameObject2.GetComponent<CanvasGroup>().alpha = 0f;
			gameObject2.GetComponent<CanvasGroup>().interactable = false;
			gameObject2.GetComponent<CanvasGroup>().blocksRaycasts = false;
			gameObject.GetComponent<OltreScene>().menuAperto = false;
			gameObject.GetComponent<OltreScene>().pausaSempliceAttiva = false;
		}
	}

	// Token: 0x06000882 RID: 2178 RVA: 0x00129BF8 File Offset: 0x00127DF8
	public void MenuApriChiudiDomandaTornaACasa()
	{
		GameObject gameObject = GameObject.FindGameObjectWithTag("CanvasMenu").transform.FindChild("Domande Uscita Battaglia").FindChild("domanda torna alla casa").gameObject;
		GameObject gameObject2 = gameObject.transform.parent.gameObject;
		GameObject gameObject3 = GameObject.FindGameObjectWithTag("InfoNeutreTattica");
		GameObject gameObject4 = gameObject3.GetComponent<GestoreNeutroTattica>().ListaCamere[0];
		gameObject4.GetComponent<GestioneSuoniCamera>().attivaVoce = true;
		gameObject4.GetComponent<GestioneSuoniCamera>().nonèVoce = true;
		gameObject4.GetComponent<AudioSource>().clip = gameObject4.GetComponent<GestioneSuoniCamera>().suonoClickGenerico1;
		if (gameObject.GetComponent<CanvasGroup>().alpha == 0f)
		{
			gameObject2.GetComponent<CanvasGroup>().alpha = 1f;
			gameObject2.GetComponent<CanvasGroup>().interactable = true;
			gameObject2.GetComponent<CanvasGroup>().blocksRaycasts = true;
			gameObject.GetComponent<CanvasGroup>().alpha = 1f;
			gameObject.GetComponent<CanvasGroup>().interactable = true;
			gameObject.GetComponent<CanvasGroup>().blocksRaycasts = true;
		}
		else
		{
			gameObject2.GetComponent<CanvasGroup>().alpha = 0f;
			gameObject2.GetComponent<CanvasGroup>().interactable = false;
			gameObject2.GetComponent<CanvasGroup>().blocksRaycasts = false;
			gameObject.GetComponent<CanvasGroup>().alpha = 0f;
			gameObject.GetComponent<CanvasGroup>().interactable = false;
			gameObject.GetComponent<CanvasGroup>().blocksRaycasts = false;
		}
	}

	// Token: 0x06000883 RID: 2179 RVA: 0x00129D44 File Offset: 0x00127F44
	public void MenuTornaAllaCasa()
	{
		GameObject gameObject = GameObject.FindGameObjectWithTag("InfoNeutreTattica");
		GameObject gameObject2 = gameObject.GetComponent<GestoreNeutroTattica>().ListaCamere[0];
		gameObject2.GetComponent<GestioneSuoniCamera>().attivaVoce = true;
		gameObject2.GetComponent<GestioneSuoniCamera>().nonèVoce = true;
		gameObject2.GetComponent<AudioSource>().clip = gameObject2.GetComponent<GestioneSuoniCamera>().suonoClickGenerico1;
		gameObject.GetComponent<GestoreNeutroTattica>().salvaDatiBattaglia = true;
		if (GestoreNeutroStrategia.tipoBattaglia == 0 || GestoreNeutroStrategia.tipoBattaglia == 1 || GestoreNeutroStrategia.tipoBattaglia == 2)
		{
			GestoreNeutroStrategia.mostraResocontoBattaglia = true;
		}
		else
		{
			GestoreNeutroStrategia.mostraResocontoMissione = true;
		}
		GestoreNeutroStrategia.mostraElencoResoconto = true;
		GestoreNeutroStrategia.aggElencoBattaglia = true;
		GestoreNeutroStrategia.ripristinaBarraVert = true;
		GestoreNeutroStrategia.inTattica = false;
	}

	// Token: 0x06000884 RID: 2180 RVA: 0x00129DF4 File Offset: 0x00127FF4
	public void MenuTornaAllaCasaIAnticipo()
	{
		GameObject gameObject = GameObject.FindGameObjectWithTag("InfoNeutreTattica");
		GameObject gameObject2 = GameObject.FindGameObjectWithTag("CanvasMenu").transform.FindChild("Domande Uscita Battaglia").FindChild("domanda torna alla casa").gameObject;
		GameObject gameObject3 = gameObject2.transform.parent.gameObject;
		GameObject gameObject4 = GameObject.FindGameObjectWithTag("InizioLivello");
		GameObject gameObject5 = GameObject.FindGameObjectWithTag("CanvasMenu").transform.FindChild("Menu Tattico").gameObject;
		GameObject gameObject6 = gameObject.GetComponent<GestoreNeutroTattica>().ListaCamere[0];
		gameObject6.GetComponent<GestioneSuoniCamera>().attivaVoce = true;
		gameObject6.GetComponent<GestioneSuoniCamera>().nonèVoce = true;
		gameObject6.GetComponent<AudioSource>().clip = gameObject6.GetComponent<GestioneSuoniCamera>().suonoClickGenerico1;
		gameObject.GetComponent<GestoreNeutroTattica>().termineInAnticipo = true;
		gameObject3.GetComponent<CanvasGroup>().alpha = 0f;
		gameObject3.GetComponent<CanvasGroup>().interactable = false;
		gameObject3.GetComponent<CanvasGroup>().blocksRaycasts = false;
		gameObject2.GetComponent<CanvasGroup>().alpha = 0f;
		gameObject2.GetComponent<CanvasGroup>().interactable = false;
		gameObject2.GetComponent<CanvasGroup>().blocksRaycasts = false;
		gameObject5.GetComponent<CanvasGroup>().alpha = 0f;
		gameObject5.GetComponent<CanvasGroup>().interactable = false;
		gameObject5.GetComponent<CanvasGroup>().blocksRaycasts = false;
		gameObject4.GetComponent<OltreScene>().menuAperto = false;
		GestoreNeutroStrategia.inTattica = false;
	}

	// Token: 0x06000885 RID: 2181 RVA: 0x00129F50 File Offset: 0x00128150
	public void MenuApriChiudiDomandaTornaAMenu()
	{
		if (GestoreNeutroTattica.èBattagliaVeloce)
		{
			GameObject gameObject = GameObject.FindGameObjectWithTag("CanvasMenu").transform.FindChild("Domanda Fine Batt Vel").gameObject;
			GameObject gameObject2 = GameObject.FindGameObjectWithTag("InfoNeutreTattica");
			GameObject gameObject3 = gameObject2.GetComponent<GestoreNeutroTattica>().ListaCamere[0];
			gameObject3.GetComponent<GestioneSuoniCamera>().attivaVoce = true;
			gameObject3.GetComponent<GestioneSuoniCamera>().nonèVoce = true;
			gameObject3.GetComponent<AudioSource>().clip = gameObject3.GetComponent<GestioneSuoniCamera>().suonoClickGenerico1;
			if (gameObject.GetComponent<CanvasGroup>().alpha == 0f)
			{
				gameObject.GetComponent<CanvasGroup>().alpha = 1f;
				gameObject.GetComponent<CanvasGroup>().interactable = true;
				gameObject.GetComponent<CanvasGroup>().blocksRaycasts = true;
			}
			else
			{
				gameObject.GetComponent<CanvasGroup>().alpha = 0f;
				gameObject.GetComponent<CanvasGroup>().interactable = false;
				gameObject.GetComponent<CanvasGroup>().blocksRaycasts = false;
			}
		}
		else
		{
			GameObject gameObject4 = GameObject.FindGameObjectWithTag("CanvasMenu").transform.FindChild("Domande Uscita Battaglia").FindChild("domanda torna al menu").gameObject;
			GameObject gameObject5 = gameObject4.transform.parent.gameObject;
			GameObject gameObject6 = GameObject.FindGameObjectWithTag("InfoNeutreTattica");
			GameObject gameObject7 = gameObject6.GetComponent<GestoreNeutroTattica>().ListaCamere[0];
			gameObject7.GetComponent<GestioneSuoniCamera>().attivaVoce = true;
			gameObject7.GetComponent<GestioneSuoniCamera>().nonèVoce = true;
			gameObject7.GetComponent<AudioSource>().clip = gameObject7.GetComponent<GestioneSuoniCamera>().suonoClickGenerico1;
			if (gameObject4.GetComponent<CanvasGroup>().alpha == 0f)
			{
				gameObject5.GetComponent<CanvasGroup>().alpha = 1f;
				gameObject5.GetComponent<CanvasGroup>().interactable = true;
				gameObject5.GetComponent<CanvasGroup>().blocksRaycasts = true;
				gameObject4.GetComponent<CanvasGroup>().alpha = 1f;
				gameObject4.GetComponent<CanvasGroup>().interactable = true;
				gameObject4.GetComponent<CanvasGroup>().blocksRaycasts = true;
			}
			else
			{
				gameObject5.GetComponent<CanvasGroup>().alpha = 0f;
				gameObject5.GetComponent<CanvasGroup>().interactable = false;
				gameObject5.GetComponent<CanvasGroup>().blocksRaycasts = false;
				gameObject4.GetComponent<CanvasGroup>().alpha = 0f;
				gameObject4.GetComponent<CanvasGroup>().interactable = false;
				gameObject4.GetComponent<CanvasGroup>().blocksRaycasts = false;
			}
		}
	}

	// Token: 0x06000886 RID: 2182 RVA: 0x0012A190 File Offset: 0x00128390
	public void MenuResocontoBattVel()
	{
		GameObject gameObject = GameObject.FindGameObjectWithTag("InfoNeutreTattica");
		GameObject gameObject2 = GameObject.FindGameObjectWithTag("InizioLivello");
		GameObject gameObject3 = GameObject.FindGameObjectWithTag("CanvasMenu").transform.FindChild("Menu Tattico").gameObject;
		GameObject gameObject4 = gameObject.GetComponent<GestoreNeutroTattica>().ListaCamere[0];
		GameObject gameObject5 = GameObject.FindGameObjectWithTag("CanvasMenu").transform.FindChild("Domanda Fine Batt Vel").gameObject;
		GameObject gameObject6 = GameObject.FindGameObjectWithTag("CanvasComandante").transform.FindChild("Varie Battaglia").FindChild("Scritta Pausa").gameObject;
		gameObject6.GetComponent<Text>().enabled = false;
		gameObject5.GetComponent<CanvasGroup>().alpha = 0f;
		gameObject5.GetComponent<CanvasGroup>().interactable = false;
		gameObject5.GetComponent<CanvasGroup>().blocksRaycasts = false;
		gameObject3.GetComponent<CanvasGroup>().alpha = 0f;
		gameObject3.GetComponent<CanvasGroup>().interactable = false;
		gameObject3.GetComponent<CanvasGroup>().blocksRaycasts = false;
		gameObject2.GetComponent<OltreScene>().menuAperto = false;
		gameObject2.GetComponent<OltreScene>().pausaSempliceAttiva = false;
		gameObject4.GetComponent<GestioneSuoniCamera>().attivaVoce = true;
		gameObject4.GetComponent<GestioneSuoniCamera>().nonèVoce = true;
		gameObject4.GetComponent<AudioSource>().clip = gameObject4.GetComponent<GestioneSuoniCamera>().suonoClickGenerico1;
		gameObject.GetComponent<GestoreNeutroTattica>().battagliaTerminata = true;
		GestoreNeutroStrategia.vincitore = 2;
	}

	// Token: 0x06000887 RID: 2183 RVA: 0x0012A2E8 File Offset: 0x001284E8
	public void MenuApriChiudiDomandaTornaADesktop()
	{
		GameObject gameObject = GameObject.FindGameObjectWithTag("CanvasMenu").transform.FindChild("Domande Uscita Battaglia").FindChild("domanda torna al desktop").gameObject;
		GameObject gameObject2 = gameObject.transform.parent.gameObject;
		GameObject gameObject3 = GameObject.FindGameObjectWithTag("InfoNeutreTattica");
		GameObject gameObject4 = gameObject3.GetComponent<GestoreNeutroTattica>().ListaCamere[0];
		gameObject4.GetComponent<GestioneSuoniCamera>().attivaVoce = true;
		gameObject4.GetComponent<GestioneSuoniCamera>().nonèVoce = true;
		gameObject4.GetComponent<AudioSource>().clip = gameObject4.GetComponent<GestioneSuoniCamera>().suonoClickGenerico1;
		if (gameObject.GetComponent<CanvasGroup>().alpha == 0f)
		{
			gameObject2.GetComponent<CanvasGroup>().alpha = 1f;
			gameObject2.GetComponent<CanvasGroup>().interactable = true;
			gameObject2.GetComponent<CanvasGroup>().blocksRaycasts = true;
			gameObject.GetComponent<CanvasGroup>().alpha = 1f;
			gameObject.GetComponent<CanvasGroup>().interactable = true;
			gameObject.GetComponent<CanvasGroup>().blocksRaycasts = true;
		}
		else
		{
			gameObject2.GetComponent<CanvasGroup>().alpha = 0f;
			gameObject2.GetComponent<CanvasGroup>().interactable = false;
			gameObject2.GetComponent<CanvasGroup>().blocksRaycasts = false;
			gameObject.GetComponent<CanvasGroup>().alpha = 0f;
			gameObject.GetComponent<CanvasGroup>().interactable = false;
			gameObject.GetComponent<CanvasGroup>().blocksRaycasts = false;
		}
	}

	// Token: 0x06000888 RID: 2184 RVA: 0x0012A434 File Offset: 0x00128634
	public void MenuTornaAlDesktop()
	{
	}

	// Token: 0x06000889 RID: 2185 RVA: 0x0012A438 File Offset: 0x00128638
	public void MenuApriImpostazioni()
	{
		GameObject gameObject = GameObject.FindGameObjectWithTag("CanvasMenu").transform.FindChild("Impostazioni").gameObject;
		GameObject gameObject2 = GameObject.FindGameObjectWithTag("InfoNeutreTattica");
		GameObject gameObject3 = gameObject2.GetComponent<GestoreNeutroTattica>().ListaCamere[0];
		gameObject3.GetComponent<GestioneSuoniCamera>().attivaVoce = true;
		gameObject3.GetComponent<GestioneSuoniCamera>().nonèVoce = true;
		gameObject3.GetComponent<AudioSource>().clip = gameObject3.GetComponent<GestioneSuoniCamera>().suonoClickGenerico1;
		gameObject.GetComponent<CanvasGroup>().alpha = 1f;
		gameObject.GetComponent<CanvasGroup>().interactable = true;
		gameObject.GetComponent<CanvasGroup>().blocksRaycasts = true;
	}

	// Token: 0x0600088A RID: 2186 RVA: 0x0012A4D8 File Offset: 0x001286D8
	public void ImpostazioniScelta(int numero)
	{
		GameObject gameObject = GameObject.FindGameObjectWithTag("InizioLivello").gameObject;
		GameObject gameObject2 = GameObject.FindGameObjectWithTag("InfoNeutreTattica");
		GameObject gameObject3 = gameObject2.GetComponent<GestoreNeutroTattica>().ListaCamere[0];
		gameObject3.GetComponent<GestioneSuoniCamera>().attivaVoce = true;
		gameObject3.GetComponent<GestioneSuoniCamera>().nonèVoce = true;
		gameObject3.GetComponent<AudioSource>().clip = gameObject3.GetComponent<GestioneSuoniCamera>().suonoClickGenerico1;
		gameObject.GetComponent<ImpostazioniScript>().impostSelez = numero;
		gameObject.GetComponent<ImpostazioniScript>().impostGiàScelta = true;
	}

	// Token: 0x0600088B RID: 2187 RVA: 0x0012A558 File Offset: 0x00128758
	public void ImpostazioniApplica()
	{
		GameObject gameObject = GameObject.FindGameObjectWithTag("InizioLivello").gameObject;
		GameObject gameObject2 = GameObject.FindGameObjectWithTag("InfoNeutreTattica");
		GameObject gameObject3 = gameObject2.GetComponent<GestoreNeutroTattica>().ListaCamere[0];
		gameObject3.GetComponent<GestioneSuoniCamera>().attivaVoce = true;
		gameObject3.GetComponent<GestioneSuoniCamera>().nonèVoce = true;
		gameObject3.GetComponent<AudioSource>().clip = gameObject3.GetComponent<GestioneSuoniCamera>().suonoClickGenerico1;
		gameObject.GetComponent<ImpostazioniScript>().impApplicate = true;
	}

	// Token: 0x0600088C RID: 2188 RVA: 0x0012A5CC File Offset: 0x001287CC
	public void ImpostazioniChiudi()
	{
		GameObject gameObject = GameObject.FindGameObjectWithTag("CanvasMenu").transform.FindChild("Impostazioni").gameObject;
		GameObject gameObject2 = GameObject.FindGameObjectWithTag("InizioLivello").gameObject;
		GameObject gameObject3 = GameObject.FindGameObjectWithTag("InfoNeutreTattica");
		GameObject gameObject4 = gameObject3.GetComponent<GestoreNeutroTattica>().ListaCamere[0];
		gameObject4.GetComponent<GestioneSuoniCamera>().attivaVoce = true;
		gameObject4.GetComponent<GestioneSuoniCamera>().nonèVoce = true;
		gameObject4.GetComponent<AudioSource>().clip = gameObject4.GetComponent<GestioneSuoniCamera>().suonoClickGenerico1;
		gameObject.GetComponent<CanvasGroup>().alpha = 0f;
		gameObject.GetComponent<CanvasGroup>().interactable = false;
		gameObject.GetComponent<CanvasGroup>().blocksRaycasts = false;
		gameObject2.GetComponent<ImpostazioniScript>().impostSelez = 2;
		gameObject2.GetComponent<ImpostazioniScript>().impostGiàScelta = true;
		gameObject2.GetComponent<ImpostazioniScript>().impChiuse = true;
	}

	// Token: 0x0600088D RID: 2189 RVA: 0x0012A6A0 File Offset: 0x001288A0
	public void ApriChiudiObbiettivi()
	{
		GameObject gameObject = GameObject.FindGameObjectWithTag("CanvasComandante").transform.FindChild("Barra Superiore").FindChild("Obbiettivi").FindChild("sfondo").gameObject;
		GameObject gameObject2 = GameObject.FindGameObjectWithTag("InfoNeutreTattica");
		GameObject gameObject3 = gameObject2.GetComponent<GestoreNeutroTattica>().ListaCamere[0];
		gameObject3.GetComponent<GestioneSuoniCamera>().attivaVoce = true;
		gameObject3.GetComponent<GestioneSuoniCamera>().nonèVoce = true;
		gameObject3.GetComponent<AudioSource>().clip = gameObject3.GetComponent<GestioneSuoniCamera>().suonoClickGenerico1;
		if (gameObject.GetComponent<CanvasGroup>().alpha == 0f)
		{
			gameObject.GetComponent<CanvasGroup>().alpha = 1f;
		}
		else
		{
			gameObject.GetComponent<CanvasGroup>().alpha = 0f;
		}
	}

	// Token: 0x0600088E RID: 2190 RVA: 0x0012A768 File Offset: 0x00128968
	public void InfoTrappola(int numTrappola)
	{
		this.infoAlleati = GameObject.FindGameObjectWithTag("Info Alleati");
		GameObject gameObject = GameObject.FindGameObjectWithTag("InfoNeutreTattica");
		GameObject gameObject2 = gameObject.GetComponent<GestoreNeutroTattica>().ListaCamere[0];
		gameObject2.GetComponent<GestioneSuoniCamera>().attivaVoce = true;
		gameObject2.GetComponent<GestioneSuoniCamera>().nonèVoce = true;
		gameObject2.GetComponent<AudioSource>().clip = gameObject2.GetComponent<GestioneSuoniCamera>().suonoClickGenerico1;
		this.infoAlleati.GetComponent<GestioneComandanteInUI>().numInfoTrappola = numTrappola;
	}

	// Token: 0x0600088F RID: 2191 RVA: 0x0012A7E4 File Offset: 0x001289E4
	public void PiùInfoTrappola(int origineInfo)
	{
		this.infoAlleati = GameObject.FindGameObjectWithTag("Info Alleati");
		GameObject gameObject = GameObject.FindGameObjectWithTag("CanvasComandante").transform.FindChild("Informazioni Unità").gameObject;
		GameObject gameObject2 = GameObject.FindGameObjectWithTag("InfoNeutreTattica");
		GameObject gameObject3 = gameObject2.GetComponent<GestoreNeutroTattica>().ListaCamere[0];
		gameObject3.GetComponent<GestioneSuoniCamera>().attivaVoce = true;
		gameObject3.GetComponent<GestioneSuoniCamera>().nonèVoce = true;
		gameObject3.GetComponent<AudioSource>().clip = gameObject3.GetComponent<GestioneSuoniCamera>().suonoClickGenerico1;
		this.infoAlleati.GetComponent<GestioneComandanteInUI>().visualInfoTrappola = true;
		this.infoAlleati.GetComponent<GestioneComandanteInUI>().aggiornaAlleatoPerInfo = true;
		this.infoAlleati.GetComponent<GestioneComandanteInUI>().origineInfoTrappola = origineInfo;
		gameObject.GetComponent<CanvasGroup>().alpha = 1f;
		gameObject.GetComponent<CanvasGroup>().interactable = false;
		gameObject.GetComponent<CanvasGroup>().blocksRaycasts = false;
	}

	// Token: 0x06000890 RID: 2192 RVA: 0x0012A8C8 File Offset: 0x00128AC8
	public void SelezTrappola(int tipoTrappola)
	{
		this.infoAlleati = GameObject.FindGameObjectWithTag("Info Alleati");
		GameObject gameObject = GameObject.FindGameObjectWithTag("InfoNeutreTattica");
		GameObject gameObject2 = gameObject.GetComponent<GestoreNeutroTattica>().ListaCamere[0];
		gameObject2.GetComponent<GestioneSuoniCamera>().attivaVoce = true;
		gameObject2.GetComponent<GestioneSuoniCamera>().nonèVoce = true;
		gameObject2.GetComponent<AudioSource>().clip = gameObject2.GetComponent<GestioneSuoniCamera>().suonoClickGenerico1;
		this.infoAlleati.GetComponent<InfoGenericheAlleati>().trappolaScelta = tipoTrappola;
		this.infoAlleati.GetComponent<InfoGenericheAlleati>().posizTrapAttivo = true;
		this.infoAlleati.GetComponent<InfoGenericheAlleati>().trappolaCreata = false;
	}

	// Token: 0x06000891 RID: 2193 RVA: 0x0012A964 File Offset: 0x00128B64
	public void RiparaTrappola()
	{
		this.infoAlleati = GameObject.FindGameObjectWithTag("Info Alleati");
		GameObject gameObject = GameObject.FindGameObjectWithTag("InfoNeutreTattica");
		GameObject gameObject2 = gameObject.GetComponent<GestoreNeutroTattica>().ListaCamere[0];
		gameObject2.GetComponent<GestioneSuoniCamera>().attivaVoce = true;
		gameObject2.GetComponent<GestioneSuoniCamera>().nonèVoce = true;
		gameObject2.GetComponent<AudioSource>().clip = gameObject2.GetComponent<GestioneSuoniCamera>().suonoClickGenerico1;
		this.infoAlleati.GetComponent<InfoGenericheAlleati>().riparaTrappola = true;
	}

	// Token: 0x06000892 RID: 2194 RVA: 0x0012A9E0 File Offset: 0x00128BE0
	public void DemolisciTrappola()
	{
		this.infoAlleati = GameObject.FindGameObjectWithTag("Info Alleati");
		GameObject gameObject = GameObject.FindGameObjectWithTag("InfoNeutreTattica");
		GameObject gameObject2 = gameObject.GetComponent<GestoreNeutroTattica>().ListaCamere[0];
		gameObject2.GetComponent<GestioneSuoniCamera>().attivaVoce = true;
		gameObject2.GetComponent<GestioneSuoniCamera>().nonèVoce = true;
		gameObject2.GetComponent<AudioSource>().clip = gameObject2.GetComponent<GestioneSuoniCamera>().suonoClickGenerico1;
		this.infoAlleati.GetComponent<InfoGenericheAlleati>().demolisciTrappola = true;
	}

	// Token: 0x06000893 RID: 2195 RVA: 0x0012AA5C File Offset: 0x00128C5C
	public void TrappolaQuota(int suOGiù)
	{
		this.infoAlleati = GameObject.FindGameObjectWithTag("Info Alleati");
		GameObject gameObject = GameObject.FindGameObjectWithTag("InfoNeutreTattica");
		GameObject gameObject2 = gameObject.GetComponent<GestoreNeutroTattica>().ListaCamere[0];
		gameObject2.GetComponent<GestioneSuoniCamera>().attivaVoce = true;
		gameObject2.GetComponent<GestioneSuoniCamera>().nonèVoce = true;
		gameObject2.GetComponent<AudioSource>().clip = gameObject2.GetComponent<GestioneSuoniCamera>().suonoClickGenerico1;
		this.infoAlleati.GetComponent<InfoGenericheAlleati>().coeffMoltiplPerQuota = (float)suOGiù;
		this.infoAlleati.GetComponent<InfoGenericheAlleati>().aggiorQuota = true;
	}

	// Token: 0x06000894 RID: 2196 RVA: 0x0012AAE8 File Offset: 0x00128CE8
	public void AtterraSupplyHeli()
	{
		this.infoAlleati = GameObject.FindGameObjectWithTag("Info Alleati");
		GameObject gameObject = GameObject.FindGameObjectWithTag("InfoNeutreTattica");
		GameObject gameObject2 = gameObject.GetComponent<GestoreNeutroTattica>().ListaCamere[0];
		gameObject2.GetComponent<GestioneSuoniCamera>().attivaVoce = true;
		gameObject2.GetComponent<GestioneSuoniCamera>().nonèVoce = true;
		gameObject2.GetComponent<AudioSource>().clip = gameObject2.GetComponent<GestioneSuoniCamera>().suonoClickGenerico1;
		this.infoAlleati.GetComponent<InfoGenericheAlleati>().atterraSupplyHeli = true;
	}

	// Token: 0x06000895 RID: 2197 RVA: 0x0012AB64 File Offset: 0x00128D64
	public void LancioParacadutisti()
	{
		this.infoAlleati = GameObject.FindGameObjectWithTag("Info Alleati");
		GameObject gameObject = GameObject.FindGameObjectWithTag("InfoNeutreTattica");
		GameObject gameObject2 = gameObject.GetComponent<GestoreNeutroTattica>().ListaCamere[0];
		gameObject2.GetComponent<GestioneSuoniCamera>().attivaVoce = true;
		gameObject2.GetComponent<GestioneSuoniCamera>().nonèVoce = true;
		gameObject2.GetComponent<AudioSource>().clip = gameObject2.GetComponent<GestioneSuoniCamera>().suonoClickGenerico1;
		this.infoAlleati.GetComponent<GestioneComandanteInUI>().mirinoParàAttivo = true;
	}

	// Token: 0x06000896 RID: 2198 RVA: 0x0012ABE0 File Offset: 0x00128DE0
	public void AnnullaLancio()
	{
		this.infoAlleati = GameObject.FindGameObjectWithTag("Info Alleati");
		GameObject gameObject = GameObject.FindGameObjectWithTag("CanvasComandante").transform.FindChild("Pannello Paracadutisti").gameObject;
		GameObject gameObject2 = GameObject.FindGameObjectWithTag("InfoNeutreTattica");
		GameObject gameObject3 = gameObject2.GetComponent<GestoreNeutroTattica>().ListaCamere[0];
		gameObject3.GetComponent<GestioneSuoniCamera>().attivaVoce = true;
		gameObject3.GetComponent<GestioneSuoniCamera>().nonèVoce = true;
		gameObject3.GetComponent<AudioSource>().clip = gameObject3.GetComponent<GestioneSuoniCamera>().suonoClickGenerico1;
		this.infoAlleati.GetComponent<GestioneComandanteInUI>().preparLancioParàAttivo = false;
		this.infoAlleati.GetComponent<GestioneComandanteInUI>().annullaLancio = true;
		gameObject.GetComponent<CanvasGroup>().alpha = 0f;
		gameObject.GetComponent<CanvasGroup>().interactable = false;
		gameObject.GetComponent<CanvasGroup>().blocksRaycasts = false;
	}

	// Token: 0x06000897 RID: 2199 RVA: 0x0012ACB4 File Offset: 0x00128EB4
	public void ProntoAlLAncio()
	{
		this.infoAlleati = GameObject.FindGameObjectWithTag("Info Alleati");
		GameObject gameObject = GameObject.FindGameObjectWithTag("CanvasComandante").transform.FindChild("Pannello Paracadutisti").gameObject;
		GameObject gameObject2 = GameObject.FindGameObjectWithTag("InfoNeutreTattica");
		GameObject gameObject3 = gameObject2.GetComponent<GestoreNeutroTattica>().ListaCamere[0];
		this.infoAlleati.GetComponent<GestioneComandanteInUI>().preparLancioParàAttivo = false;
		this.infoAlleati.GetComponent<GestioneComandanteInUI>().partenzaAereoParà = true;
		gameObject.GetComponent<CanvasGroup>().alpha = 0f;
		gameObject.GetComponent<CanvasGroup>().interactable = false;
		gameObject.GetComponent<CanvasGroup>().blocksRaycasts = false;
		gameObject3.GetComponent<GestioneSuoniCamera>().attivaVoce = true;
		gameObject3.GetComponent<GestioneSuoniCamera>().nonèVoce = true;
		gameObject3.GetComponent<GestioneSuoniCamera>().suonoVoci.clip = gameObject3.GetComponent<GestioneSuoniCamera>().suonoPassaggioAereoParà;
	}

	// Token: 0x06000898 RID: 2200 RVA: 0x0012AD8C File Offset: 0x00128F8C
	public void SelezPossibileParà(int numPossParà)
	{
		this.infoAlleati = GameObject.FindGameObjectWithTag("Info Alleati");
		GameObject gameObject = GameObject.FindGameObjectWithTag("InfoNeutreTattica");
		GameObject gameObject2 = gameObject.GetComponent<GestoreNeutroTattica>().ListaCamere[0];
		gameObject2.GetComponent<GestioneSuoniCamera>().attivaVoce = true;
		gameObject2.GetComponent<GestioneSuoniCamera>().nonèVoce = true;
		gameObject2.GetComponent<AudioSource>().clip = gameObject2.GetComponent<GestioneSuoniCamera>().suonoClickGenerico1;
		this.infoAlleati.GetComponent<GestioneComandanteInUI>().numInListaPossParà = numPossParà;
		this.infoAlleati.GetComponent<GestioneComandanteInUI>().aggiungiParà = true;
		this.infoAlleati.GetComponent<GestioneComandanteInUI>().aggListaPossParà = true;
		this.infoAlleati.GetComponent<GestioneComandanteInUI>().aggListaParà = true;
	}

	// Token: 0x06000899 RID: 2201 RVA: 0x0012AE38 File Offset: 0x00129038
	public void SelezParà(int numParà)
	{
		this.infoAlleati = GameObject.FindGameObjectWithTag("Info Alleati");
		GameObject gameObject = GameObject.FindGameObjectWithTag("InfoNeutreTattica");
		GameObject gameObject2 = gameObject.GetComponent<GestoreNeutroTattica>().ListaCamere[0];
		gameObject2.GetComponent<GestioneSuoniCamera>().attivaVoce = true;
		gameObject2.GetComponent<GestioneSuoniCamera>().nonèVoce = true;
		gameObject2.GetComponent<AudioSource>().clip = gameObject2.GetComponent<GestioneSuoniCamera>().suonoClickGenerico1;
		this.infoAlleati.GetComponent<GestioneComandanteInUI>().numInListaParà = numParà;
		this.infoAlleati.GetComponent<GestioneComandanteInUI>().togliParà = true;
		this.infoAlleati.GetComponent<GestioneComandanteInUI>().aggListaPossParà = true;
		this.infoAlleati.GetComponent<GestioneComandanteInUI>().aggListaParà = true;
	}

	// Token: 0x0600089A RID: 2202 RVA: 0x0012AEE4 File Offset: 0x001290E4
	public void ApriChiudiMunizioniPannello()
	{
		this.infoAlleati = GameObject.FindGameObjectWithTag("Info Alleati");
		GameObject gameObject = GameObject.FindGameObjectWithTag("CanvasComandante").transform.FindChild("Barra Superiore").FindChild("Munizioni").FindChild("sfondo").gameObject;
		GameObject gameObject2 = GameObject.FindGameObjectWithTag("InfoNeutreTattica");
		GameObject gameObject3 = gameObject2.GetComponent<GestoreNeutroTattica>().ListaCamere[0];
		gameObject3.GetComponent<GestioneSuoniCamera>().attivaVoce = true;
		gameObject3.GetComponent<GestioneSuoniCamera>().nonèVoce = true;
		gameObject3.GetComponent<AudioSource>().clip = gameObject3.GetComponent<GestioneSuoniCamera>().suonoClickGenerico1;
		if (gameObject.GetComponent<CanvasGroup>().alpha == 0f)
		{
			gameObject.GetComponent<CanvasGroup>().alpha = 1f;
			this.infoAlleati.GetComponent<GestioneComandanteInUI>().aggPannMuniz = true;
		}
		else
		{
			gameObject.GetComponent<CanvasGroup>().alpha = 0f;
		}
	}

	// Token: 0x0600089B RID: 2203 RVA: 0x0012AFCC File Offset: 0x001291CC
	public void FuocoSalvaDiRocketArtillery(int tipoFuoco)
	{
		this.infoAlleati = GameObject.FindGameObjectWithTag("Info Alleati");
		GameObject gameObject = GameObject.FindGameObjectWithTag("InfoNeutreTattica");
		GameObject gameObject2 = gameObject.GetComponent<GestoreNeutroTattica>().ListaCamere[0];
		gameObject2.GetComponent<GestioneSuoniCamera>().attivaVoce = true;
		gameObject2.GetComponent<GestioneSuoniCamera>().nonèVoce = true;
		gameObject2.GetComponent<AudioSource>().clip = gameObject2.GetComponent<GestioneSuoniCamera>().suonoClickGenerico1;
		this.infoAlleati.GetComponent<InfoGenericheAlleati>().tipoFuocoSalvaRockArt = tipoFuoco;
	}

	// Token: 0x0600089C RID: 2204 RVA: 0x0012B048 File Offset: 0x00129248
	public void ApriChiudiStatistiche()
	{
		GameObject gameObject = GameObject.FindGameObjectWithTag("CanvasComandante").transform.FindChild("Barra Superiore").FindChild("Statistiche").GetChild(1).gameObject;
		this.infoAlleati = GameObject.FindGameObjectWithTag("Info Alleati");
		GameObject gameObject2 = GameObject.FindGameObjectWithTag("InfoNeutreTattica");
		GameObject gameObject3 = gameObject2.GetComponent<GestoreNeutroTattica>().ListaCamere[0];
		gameObject3.GetComponent<GestioneSuoniCamera>().attivaVoce = true;
		gameObject3.GetComponent<GestioneSuoniCamera>().nonèVoce = true;
		gameObject3.GetComponent<AudioSource>().clip = gameObject3.GetComponent<GestioneSuoniCamera>().suonoClickGenerico1;
		this.infoAlleati.GetComponent<GestioneComandanteInUI>().statisticheAperte = !this.infoAlleati.GetComponent<GestioneComandanteInUI>().statisticheAperte;
		if (this.infoAlleati.GetComponent<GestioneComandanteInUI>().statisticheAperte)
		{
			gameObject.GetComponent<CanvasGroup>().alpha = 1f;
		}
		else
		{
			gameObject.GetComponent<CanvasGroup>().alpha = 0f;
		}
	}

	// Token: 0x0600089D RID: 2205 RVA: 0x0012B13C File Offset: 0x0012933C
	public void SaltaCountdown()
	{
		this.infoAlleati = GameObject.FindGameObjectWithTag("Info Alleati");
		GameObject gameObject = GameObject.FindGameObjectWithTag("InfoNeutreTattica");
		GameObject gameObject2 = gameObject.GetComponent<GestoreNeutroTattica>().ListaCamere[0];
		gameObject2.GetComponent<GestioneSuoniCamera>().attivaVoce = true;
		gameObject2.GetComponent<GestioneSuoniCamera>().nonèVoce = true;
		gameObject2.GetComponent<AudioSource>().clip = gameObject2.GetComponent<GestioneSuoniCamera>().suonoClickGenerico1;
		this.infoAlleati.GetComponent<GestioneComandanteInUI>().timerCountdown = 999f;
	}

	// Token: 0x0600089E RID: 2206 RVA: 0x0012B1BC File Offset: 0x001293BC
	public void EsciDaGioco()
	{
		GameObject gameObject = GameObject.FindGameObjectWithTag("InfoNeutreTattica");
		GameObject gameObject2 = gameObject.GetComponent<GestoreNeutroTattica>().ListaCamere[0];
		gameObject2.GetComponent<GestioneSuoniCamera>().attivaVoce = true;
		gameObject2.GetComponent<GestioneSuoniCamera>().nonèVoce = true;
		gameObject2.GetComponent<AudioSource>().clip = gameObject2.GetComponent<GestioneSuoniCamera>().suonoClickGenerico1;
		Application.Quit();
	}

	// Token: 0x0600089F RID: 2207 RVA: 0x0012B21C File Offset: 0x0012941C
	public void AttivaPausa()
	{
		GameObject gameObject = GameObject.FindGameObjectWithTag("InizioLivello");
		gameObject.GetComponent<OltreScene>().pausaSempliceAttiva = !gameObject.GetComponent<OltreScene>().pausaSempliceAttiva;
		gameObject.GetComponent<OltreScene>().attivitàSuVelTempo = true;
	}

	// Token: 0x060008A0 RID: 2208 RVA: 0x0012B25C File Offset: 0x0012945C
	public void AttivaCambioVelocitàTempo()
	{
		GameObject gameObject = GameObject.FindGameObjectWithTag("InizioLivello");
		gameObject.GetComponent<OltreScene>().indiceListeVelTempo++;
		if (gameObject.GetComponent<OltreScene>().indiceListeVelTempo == gameObject.GetComponent<OltreScene>().ListaVelocitàTempo.Count)
		{
			gameObject.GetComponent<OltreScene>().indiceListeVelTempo = 0;
		}
		gameObject.GetComponent<OltreScene>().velocitàTempo = gameObject.GetComponent<OltreScene>().ListaVelocitàTempo[gameObject.GetComponent<OltreScene>().indiceListeVelTempo];
		gameObject.GetComponent<OltreScene>().attivitàSuVelTempo = true;
	}

	// Token: 0x060008A1 RID: 2209 RVA: 0x0012B2E8 File Offset: 0x001294E8
	public void SelezGruppo(int numGruppo)
	{
		this.infoAlleati = GameObject.FindGameObjectWithTag("Info Alleati");
		GameObject gameObject = GameObject.FindGameObjectWithTag("InfoNeutreTattica");
		GameObject gameObject2 = gameObject.GetComponent<GestoreNeutroTattica>().ListaCamere[0];
		gameObject2.GetComponent<GestioneSuoniCamera>().attivaVoce = true;
		gameObject2.GetComponent<GestioneSuoniCamera>().nonèVoce = true;
		gameObject2.GetComponent<AudioSource>().clip = gameObject2.GetComponent<GestioneSuoniCamera>().suonoClickGenerico1;
		this.infoAlleati.GetComponent<InfoGenericheAlleati>().gruppoPremuto = numGruppo;
	}

	// Token: 0x060008A2 RID: 2210 RVA: 0x0012B364 File Offset: 0x00129564
	public void VinciBattInAnticipo()
	{
		GameObject gameObject = GameObject.FindGameObjectWithTag("InfoNeutreTattica");
		GameObject gameObject2 = GameObject.FindWithTag("CanvasComandante").transform.FindChild("Varie Battaglia").FindChild("Vinci battaglia in anticipo").gameObject;
		gameObject.GetComponent<GestoreNeutroTattica>().battagliaTerminata = true;
		GestoreNeutroStrategia.vincitore = 1;
		gameObject2.GetComponent<CanvasGroup>().alpha = 0f;
		gameObject2.GetComponent<CanvasGroup>().interactable = false;
		gameObject2.GetComponent<CanvasGroup>().blocksRaycasts = false;
	}

	// Token: 0x04001FD2 RID: 8146
	private GameObject infoAlleati;
}
