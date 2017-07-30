using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x020000BC RID: 188
public class SelezionamentoInStrategia : MonoBehaviour
{
	// Token: 0x060006AA RID: 1706 RVA: 0x000EBB94 File Offset: 0x000E9D94
	private void Start()
	{
		this.layerSelezione = 345344;
		this.cameraCasa = GameObject.FindGameObjectWithTag("MainCamera");
		this.inizioLivello = GameObject.FindGameObjectWithTag("InizioLivello");
		this.schede = GameObject.FindGameObjectWithTag("Schede");
		this.CanvasStrategia = GameObject.FindGameObjectWithTag("CanvasStrategia");
		this.visualizzaEsercito = this.CanvasStrategia.transform.FindChild("Visualizza Esercito").gameObject;
		this.visualizzaSecondoEsercito = this.CanvasStrategia.transform.FindChild("Visualizza Esercito").FindChild("elenco secondo esercito").gameObject;
		this.Headquarters = GameObject.FindGameObjectWithTag("Headquarters");
		this.CentroStanzaUI = this.CanvasStrategia.transform.FindChild("Centro Stanza").gameObject;
		this.visualEserNemico = this.CanvasStrategia.transform.FindChild("Visualizza Esercito Insetti").gameObject;
		this.Nest = GameObject.FindGameObjectWithTag("Nest");
		this.elencoEdifici = this.CanvasStrategia.transform.FindChild("Schermata Gestione Headquarters").FindChild("pannello costruzioni").gameObject;
		this.infoEdifici = this.CanvasStrategia.transform.FindChild("Schermata Gestione Headquarters").FindChild("info costruzioni").gameObject;
		this.pulsanteCostruisci = this.infoEdifici.transform.FindChild("pulsante costruisci").gameObject;
		this.pulsanteDemolisci = this.infoEdifici.transform.FindChild("pulsante demolisci").gameObject;
		this.pulsanteAccesoSpento = this.infoEdifici.transform.FindChild("pulsante acceso o spento").gameObject;
		this.iconaMovimento = this.CanvasStrategia.transform.FindChild("Varie").FindChild("icona movimento").gameObject;
		this.iconaScambio = this.CanvasStrategia.transform.FindChild("Varie").FindChild("icona scambio").gameObject;
		this.ListaSchede = new List<GameObject>();
		this.ListaSchede.Add(null);
		this.ListaSchede.Add(null);
		this.ListaSchede.Add(null);
		this.ListaSchede.Add(null);
		this.ListaSchede.Add(null);
	}

	// Token: 0x060006AB RID: 1707 RVA: 0x000EBDE8 File Offset: 0x000E9FE8
	private void Update()
	{
		this.raggioSelezionamento = base.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
		this.GestioneClick();
		this.SelezionamentoNellaStrategia();
		this.MovimentoEserciti();
	}

	// Token: 0x060006AC RID: 1708 RVA: 0x000EBE20 File Offset: 0x000EA020
	private void GestioneClick()
	{
		this.doppioClickEffettuato = false;
		if (!this.primoClickAvvenuto)
		{
			if (Input.GetMouseButtonDown(0))
			{
				this.primoClickAvvenuto = true;
			}
		}
		else
		{
			this.timerDoppioClick += Time.deltaTime;
			if (Input.GetMouseButtonDown(0) && this.timerDoppioClick > 0f && this.timerDoppioClick < 0.3f)
			{
				this.doppioClickEffettuato = true;
				this.timerDoppioClick = 0f;
				this.primoClickAvvenuto = false;
			}
		}
		if (this.timerDoppioClick > 0.3f)
		{
			this.timerDoppioClick = 0f;
			this.primoClickAvvenuto = false;
		}
	}

	// Token: 0x060006AD RID: 1709 RVA: 0x000EBED0 File Offset: 0x000EA0D0
	private void SelezionamentoNellaStrategia()
	{
		if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject() && Physics.Raycast(this.raggioSelezionamento, out this.hitSelezione, 9999f, this.layerSelezione))
		{
			this.visualizzaSecondoEsercito.GetComponent<CanvasGroup>().alpha = 0f;
			this.visualizzaSecondoEsercito.GetComponent<CanvasGroup>().interactable = false;
			this.visualizzaSecondoEsercito.GetComponent<CanvasGroup>().blocksRaycasts = false;
			if (this.hitSelezione.collider.tag == "Alleato")
			{
				if (this.esercitoSelezionato != null)
				{
					if (this.esercitoSelezionato.name.Contains("Army"))
					{
						this.esercitoSelezionato.GetComponent<PresenzaAlleataStrategica>().alleatoSelezionato = false;
					}
					this.esercitoSelezionato = null;
				}
				this.hitSelezione.collider.gameObject.GetComponent<PresenzaAlleataStrategica>().alleatoSelezionato = true;
				this.esercitoSelezionato = this.hitSelezione.collider.gameObject;
				this.esercitoSelezionato.GetComponent<PresenzaAlleataStrategica>().suonoDaEsercito.clip = this.esercitoSelezionato.GetComponent<PresenzaAlleataStrategica>().suonoSelezEsercito;
				this.esercitoSelezionato.GetComponent<PresenzaAlleataStrategica>().suonoDaEsercito.Play();
				if (this.doppioClickEffettuato)
				{
					this.visualizzaEsercito.GetComponent<CanvasGroup>().alpha = 1f;
					this.visualizzaEsercito.GetComponent<CanvasGroup>().interactable = true;
					this.visualizzaEsercito.GetComponent<CanvasGroup>().blocksRaycasts = true;
				}
				this.Headquarters.GetComponent<GestioneEsercitiAlleati>().visualizzaEser = true;
				this.Headquarters.GetComponent<GestioneEsercitiAlleati>().aggiornaEser = true;
				this.Headquarters.GetComponent<GestioneEsercitiAlleati>().aggiornaDettagliEser = false;
				this.Headquarters.GetComponent<GestioneSuoniCasa>().attivaSuono = true;
				this.Headquarters.GetComponent<GestioneSuoniCasa>().suoniDiCasa.clip = this.Headquarters.GetComponent<GestioneSuoniCasa>().suonoClickGenerico1;
			}
			else if (this.hitSelezione.collider.tag == "Nemico")
			{
				if (this.esercitoSelezionato != null)
				{
					if (this.esercitoSelezionato.name.Contains("Army"))
					{
						this.esercitoSelezionato.GetComponent<PresenzaAlleataStrategica>().alleatoSelezionato = false;
					}
					this.esercitoSelezionato = null;
				}
				this.esercitoSelezionato = this.hitSelezione.collider.gameObject;
				if (this.doppioClickEffettuato)
				{
					this.visualEserNemico.GetComponent<CanvasGroup>().alpha = 1f;
					this.visualEserNemico.GetComponent<CanvasGroup>().interactable = true;
					this.visualEserNemico.GetComponent<CanvasGroup>().blocksRaycasts = true;
				}
				this.Nest.GetComponent<IANemicoStrategia>().visualizzaEser = true;
				this.Nest.GetComponent<IANemicoStrategia>().aggiornaEser = true;
				this.Nest.GetComponent<IANemicoStrategia>().aggiornaDettagliEser = false;
				int index = UnityEngine.Random.Range(0, this.esercitoSelezionato.GetComponent<PresenzaNemicaStrategica>().ListaVersiInsetti.Count);
				GestoreNeutroStrategia.valoreRandomSeed++;
				UnityEngine.Random.InitState(GestoreNeutroStrategia.valoreRandomSeed);
				this.esercitoSelezionato.GetComponent<PresenzaNemicaStrategica>().suonoInsetto.clip = this.esercitoSelezionato.GetComponent<PresenzaNemicaStrategica>().ListaVersiInsetti[index];
				this.esercitoSelezionato.GetComponent<PresenzaNemicaStrategica>().suonoInsetto.Play();
			}
			else if (this.hitSelezione.collider.tag == "Headquarters")
			{
				if (this.schede.GetComponent<CanvasGroup>().alpha == 0f)
				{
					this.schede.GetComponent<CanvasGroup>().alpha = 1f;
					this.schede.GetComponent<CanvasGroup>().interactable = true;
					this.schede.GetComponent<CanvasGroup>().blocksRaycasts = true;
					for (int i = 0; i < this.ListaSchede.Count; i++)
					{
						if (this.schede.transform.GetChild(i).gameObject.name == "scheda 1")
						{
							this.schede.transform.GetChild(i).gameObject.transform.SetSiblingIndex(4);
						}
					}
				}
				this.Headquarters.GetComponent<GestioneSuoniCasa>().attivaSuono = true;
				this.Headquarters.GetComponent<GestioneSuoniCasa>().suoniDiCasa.clip = this.Headquarters.GetComponent<GestioneSuoniCasa>().suonoClickScheda;
			}
			else if (this.hitSelezione.collider.tag == "Nest")
			{
				if (this.schede.GetComponent<CanvasGroup>().alpha == 0f)
				{
					this.schede.GetComponent<CanvasGroup>().alpha = 1f;
					this.schede.GetComponent<CanvasGroup>().interactable = true;
					this.schede.GetComponent<CanvasGroup>().blocksRaycasts = true;
					for (int j = 0; j < this.ListaSchede.Count; j++)
					{
						if (this.schede.transform.GetChild(j).gameObject.name == "scheda 2")
						{
							this.schede.transform.GetChild(j).gameObject.transform.SetSiblingIndex(4);
						}
					}
				}
				else
				{
					this.schede.GetComponent<CanvasGroup>().alpha = 0f;
					this.schede.GetComponent<CanvasGroup>().interactable = false;
					this.schede.GetComponent<CanvasGroup>().blocksRaycasts = false;
				}
				this.Headquarters.GetComponent<GestioneSuoniCasa>().attivaSuono = true;
				this.Headquarters.GetComponent<GestioneSuoniCasa>().suoniDiCasa.clip = this.Headquarters.GetComponent<GestioneSuoniCasa>().suonoClickScheda;
			}
			else if (this.hitSelezione.collider.tag == "CentroStanza" || this.hitSelezione.collider.gameObject.transform.parent.tag == "CentroStanza")
			{
				if (this.schede.GetComponent<CanvasGroup>().alpha == 0f)
				{
					if (this.CentroStanzaUI.GetComponent<CanvasGroup>().alpha == 0f)
					{
						this.CentroStanzaUI.GetComponent<CanvasGroup>().alpha = 1f;
						this.CentroStanzaUI.GetComponent<CanvasGroup>().interactable = true;
						this.CentroStanzaUI.gameObject.GetComponent<CanvasGroup>().blocksRaycasts = true;
					}
					else if (this.bandieraSelezionata == this.hitSelezione.collider.transform.parent.gameObject)
					{
						this.CentroStanzaUI.GetComponent<CanvasGroup>().alpha = 0f;
						this.CentroStanzaUI.GetComponent<CanvasGroup>().interactable = false;
						this.CentroStanzaUI.gameObject.GetComponent<CanvasGroup>().blocksRaycasts = false;
					}
					if (this.hitSelezione.collider.gameObject.GetComponent<CapsuleCollider>())
					{
						this.bandieraSelezionata = this.hitSelezione.collider.gameObject;
					}
					else if (this.hitSelezione.collider.gameObject.GetComponent<BoxCollider>())
					{
						this.bandieraSelezionata = this.hitSelezione.collider.transform.parent.gameObject;
					}
				}
				this.Headquarters.GetComponent<GestioneRisorseEHeadquartiers>().aggiornaSatellite = true;
				this.Headquarters.GetComponent<GestioneSuoniCasa>().attivaSuono = true;
				this.Headquarters.GetComponent<GestioneSuoniCasa>().suoniDiCasa.clip = this.Headquarters.GetComponent<GestioneSuoniCasa>().suonoClickGenerico1;
			}
			else if (this.hitSelezione.collider.tag == "postoInHeadquarters")
			{
				int index2 = int.Parse(this.hitSelezione.collider.gameObject.name);
				this.Headquarters.GetComponent<GestioneRisorseEHeadquartiers>().numeroPosto = this.hitSelezione.collider.gameObject.transform.GetSiblingIndex();
				this.Headquarters.GetComponent<GestioneSuoniCasa>().attivaSuono = true;
				this.Headquarters.GetComponent<GestioneSuoniCasa>().suoniDiCasa.clip = this.Headquarters.GetComponent<GestioneSuoniCasa>().suonoClickGenerico1;
				GameObject gameObject = this.Headquarters.transform.FindChild("lista posti").gameObject;
				for (int k = 0; k < this.Headquarters.GetComponent<GestioneRisorseEHeadquartiers>().ListaPostiInHeadquarters.Count; k++)
				{
					gameObject.transform.GetChild(k).GetComponent<MeshRenderer>().material = this.Headquarters.GetComponent<GestioneRisorseEHeadquartiers>().colorePostoEdificioNormale;
				}
				this.hitSelezione.collider.gameObject.GetComponent<MeshRenderer>().material = this.Headquarters.GetComponent<GestioneRisorseEHeadquartiers>().colorePostoEdificioSelez;
				if (this.Headquarters.GetComponent<GestioneRisorseEHeadquartiers>().ListaPostiInHeadquarters[index2] == 100)
				{
					this.elencoEdifici.GetComponent<CanvasGroup>().alpha = 1f;
					this.elencoEdifici.GetComponent<CanvasGroup>().interactable = true;
					this.elencoEdifici.GetComponent<CanvasGroup>().blocksRaycasts = true;
					this.pulsanteCostruisci.GetComponent<Button>().interactable = true;
					this.pulsanteDemolisci.GetComponent<Button>().interactable = false;
					this.pulsanteAccesoSpento.GetComponent<CanvasGroup>().alpha = 0f;
					this.pulsanteAccesoSpento.GetComponent<CanvasGroup>().interactable = false;
					this.pulsanteAccesoSpento.GetComponent<CanvasGroup>().blocksRaycasts = false;
				}
				else
				{
					this.elencoEdifici.GetComponent<CanvasGroup>().alpha = 0f;
					this.elencoEdifici.GetComponent<CanvasGroup>().interactable = false;
					this.elencoEdifici.GetComponent<CanvasGroup>().blocksRaycasts = false;
					this.infoEdifici.GetComponent<CanvasGroup>().alpha = 1f;
					this.infoEdifici.GetComponent<CanvasGroup>().interactable = true;
					this.infoEdifici.GetComponent<CanvasGroup>().blocksRaycasts = true;
					this.pulsanteCostruisci.GetComponent<Button>().interactable = false;
					this.pulsanteDemolisci.GetComponent<Button>().interactable = true;
					this.pulsanteAccesoSpento.GetComponent<CanvasGroup>().alpha = 1f;
					this.pulsanteAccesoSpento.GetComponent<CanvasGroup>().interactable = true;
					this.pulsanteAccesoSpento.GetComponent<CanvasGroup>().blocksRaycasts = true;
					this.Headquarters.GetComponent<GestioneRisorseEHeadquartiers>().tipologiaEdificio = this.Headquarters.GetComponent<GestioneRisorseEHeadquartiers>().ListaPostiInHeadquarters[index2];
				}
			}
		}
		if (!EventSystem.current.IsPointerOverGameObject())
		{
			if (Physics.Raycast(this.raggioSelezionamento, out this.hitSelezione, 9999f, this.layerSelezione) && this.esercitoSelezionato != null && this.esercitoSelezionato.tag == "Alleato")
			{
				if (this.hitSelezione.collider.tag == "Alleato" && this.hitSelezione.collider.gameObject != this.esercitoSelezionato)
				{
					if (this.esercitoSelezionato.GetComponent<PresenzaAlleataStrategica>().posizioneAttuale == this.hitSelezione.collider.gameObject.GetComponent<PresenzaAlleataStrategica>().posizioneAttuale)
					{
						this.iconaScambio.GetComponent<Image>().enabled = true;
						this.iconaScambio.transform.position = Input.mousePosition;
						if (Input.GetMouseButtonDown(1))
						{
							this.visualizzaEsercito.GetComponent<CanvasGroup>().alpha = 1f;
							this.visualizzaEsercito.GetComponent<CanvasGroup>().interactable = true;
							this.visualizzaEsercito.GetComponent<CanvasGroup>().blocksRaycasts = true;
							this.visualizzaSecondoEsercito.GetComponent<CanvasGroup>().alpha = 1f;
							this.visualizzaSecondoEsercito.GetComponent<CanvasGroup>().interactable = true;
							this.visualizzaSecondoEsercito.GetComponent<CanvasGroup>().blocksRaycasts = true;
							this.Headquarters.GetComponent<GestioneEsercitiAlleati>().visualizzaEser = true;
							this.Headquarters.GetComponent<GestioneEsercitiAlleati>().aggScambioEserciti = true;
							this.Headquarters.GetComponent<GestioneEsercitiAlleati>().scambioFraEserciti = true;
							this.Headquarters.GetComponent<GestioneEsercitiAlleati>().numPosUnità = 100;
							this.secondoEsercitoSelezionato = this.hitSelezione.collider.gameObject;
							this.Headquarters.GetComponent<GestioneSuoniCasa>().attivaSuono = true;
							this.Headquarters.GetComponent<GestioneSuoniCasa>().suoniDiCasa.clip = this.Headquarters.GetComponent<GestioneSuoniCasa>().suonoClickGenerico1;
							this.iconaScambio.GetComponent<Image>().enabled = false;
						}
					}
					else
					{
						this.iconaScambio.GetComponent<Image>().enabled = false;
					}
				}
				else
				{
					this.iconaScambio.GetComponent<Image>().enabled = false;
				}
			}
			else
			{
				this.iconaScambio.GetComponent<Image>().enabled = false;
			}
		}
		if (Input.GetMouseButtonDown(0))
		{
			if (this.esercitoSelezionato != null && this.hitSelezione.collider && this.hitSelezione.collider.tag == "Ambiente" && !EventSystem.current.IsPointerOverGameObject())
			{
				if (this.esercitoSelezionato.name.Contains("Army"))
				{
					this.esercitoSelezionato.GetComponent<PresenzaAlleataStrategica>().alleatoSelezionato = false;
				}
				this.esercitoSelezionato = null;
				this.visualizzaEsercito.GetComponent<CanvasGroup>().alpha = 0f;
				this.visualizzaEsercito.GetComponent<CanvasGroup>().interactable = false;
				this.visualizzaEsercito.GetComponent<CanvasGroup>().blocksRaycasts = false;
			}
			if (this.hitSelezione.collider == null)
			{
				if (!EventSystem.current.IsPointerOverGameObject())
				{
					this.CentroStanzaUI.GetComponent<CanvasGroup>().alpha = 0f;
					this.CentroStanzaUI.GetComponent<CanvasGroup>().interactable = false;
					this.CentroStanzaUI.gameObject.GetComponent<CanvasGroup>().blocksRaycasts = false;
					if (this.esercitoSelezionato != null)
					{
						if (this.esercitoSelezionato.name.Contains("Army"))
						{
							this.esercitoSelezionato.GetComponent<PresenzaAlleataStrategica>().alleatoSelezionato = false;
						}
						this.esercitoSelezionato = null;
						this.visualizzaEsercito.GetComponent<CanvasGroup>().alpha = 0f;
						this.visualizzaEsercito.GetComponent<CanvasGroup>().interactable = false;
						this.visualizzaEsercito.GetComponent<CanvasGroup>().blocksRaycasts = false;
					}
				}
			}
			else if (this.hitSelezione.collider.tag == "MuroStrategia" && !EventSystem.current.IsPointerOverGameObject())
			{
				this.CentroStanzaUI.GetComponent<CanvasGroup>().alpha = 0f;
				this.CentroStanzaUI.GetComponent<CanvasGroup>().interactable = false;
				this.CentroStanzaUI.gameObject.GetComponent<CanvasGroup>().blocksRaycasts = false;
				if (this.esercitoSelezionato != null)
				{
					if (this.esercitoSelezionato.name.Contains("Army"))
					{
						this.esercitoSelezionato.GetComponent<PresenzaAlleataStrategica>().alleatoSelezionato = false;
					}
					this.esercitoSelezionato = null;
					this.visualizzaEsercito.GetComponent<CanvasGroup>().alpha = 0f;
					this.visualizzaEsercito.GetComponent<CanvasGroup>().interactable = false;
					this.visualizzaEsercito.GetComponent<CanvasGroup>().blocksRaycasts = false;
				}
			}
		}
	}

	// Token: 0x060006AE RID: 1710 RVA: 0x000ECE38 File Offset: 0x000EB038
	private void MovimentoEserciti()
	{
		if (!base.GetComponent<GestoreNeutroStrategia>().turnoNemicoAttivo && this.esercitoSelezionato != null && this.esercitoSelezionato.tag == "Alleato" && !EventSystem.current.IsPointerOverGameObject())
		{
			if (Physics.Raycast(this.raggioSelezionamento, out this.hitSelezione, 9999f, this.layerSelezione))
			{
				if (this.hitSelezione.collider.tag == "Ambiente" && this.hitSelezione.collider.gameObject.transform.parent && this.hitSelezione.collider.gameObject.transform.parent.tag == "CentroStanza")
				{
					for (int i = 0; i < base.GetComponent<GestoreNeutroStrategia>().ListaCentroStanze.Count; i++)
					{
						if (this.hitSelezione.collider.gameObject.transform.parent.gameObject == base.GetComponent<GestoreNeutroStrategia>().ListaCentroStanze[i])
						{
							GameObject gameObject = base.GetComponent<GestoreNeutroStrategia>().ListaCentroStanze[i];
							for (int j = 0; j < base.GetComponent<GestoreNeutroStrategia>().ListaDiListeDiVicinanze[i].Count; j++)
							{
								if (base.GetComponent<GestoreNeutroStrategia>().ListaDiListeDiVicinanze[i][j] == this.cameraCasa.GetComponent<GestoreNeutroStrategia>().ListaCentroStanze.IndexOf(this.esercitoSelezionato.GetComponent<PresenzaAlleataStrategica>().posizioneAttuale))
								{
									bool flag = false;
									if (this.esercitoSelezionato.GetComponent<PresenzaAlleataStrategica>().posizioneAttuale.GetComponent<CentroStanza>().appartenenzaBandiera == 0 || this.esercitoSelezionato.GetComponent<PresenzaAlleataStrategica>().posizioneAttuale.GetComponent<CentroStanza>().appartenenzaBandiera == 1)
									{
										flag = true;
									}
									else if (gameObject.GetComponent<CentroStanza>().appartenenzaBandiera == 1)
									{
										flag = true;
									}
									if (this.esercitoSelezionato.GetComponent<PresenzaAlleataStrategica>().puòAncoraMuoversi && flag)
									{
										if (Input.GetMouseButtonDown(1))
										{
											this.esercitoSelezionato.GetComponent<PresenzaAlleataStrategica>().destinazione = gameObject;
											this.esercitoSelezionato.GetComponent<PresenzaAlleataStrategica>().spostamentoAttivo = true;
											this.esercitoSelezionato.GetComponent<PresenzaAlleataStrategica>().puòAncoraMuoversi = false;
											this.cameraCasa.GetComponent<GestoreNeutroStrategia>().partenzaTimerAggMissioneExtra = true;
											this.cameraCasa.GetComponent<GestoreNeutroStrategia>().timerAggMissioneExtra = 0f;
											int index = UnityEngine.Random.Range(0, this.esercitoSelezionato.GetComponent<PresenzaAlleataStrategica>().ListaVoceEserAlleato.Count);
											GestoreNeutroStrategia.valoreRandomSeed++;
											UnityEngine.Random.InitState(GestoreNeutroStrategia.valoreRandomSeed);
											this.esercitoSelezionato.GetComponent<PresenzaAlleataStrategica>().suonoDaEsercito.clip = this.esercitoSelezionato.GetComponent<PresenzaAlleataStrategica>().ListaVoceEserAlleato[index];
											this.esercitoSelezionato.GetComponent<PresenzaAlleataStrategica>().suonoDaEsercito.Play();
										}
										this.iconaMovimento.GetComponent<Image>().enabled = true;
										this.iconaMovimento.GetComponent<Image>().color = Color.green;
										this.iconaMovimento.transform.position = Input.mousePosition;
									}
									else
									{
										this.iconaMovimento.GetComponent<Image>().enabled = true;
										this.iconaMovimento.GetComponent<Image>().color = Color.red;
										this.iconaMovimento.transform.position = Input.mousePosition;
									}
								}
							}
						}
					}
				}
				else
				{
					this.iconaMovimento.GetComponent<Image>().enabled = false;
				}
			}
			else
			{
				this.iconaMovimento.GetComponent<Image>().enabled = false;
			}
		}
		else
		{
			this.iconaMovimento.GetComponent<Image>().enabled = false;
		}
	}

	// Token: 0x040018B9 RID: 6329
	private GameObject cameraCasa;

	// Token: 0x040018BA RID: 6330
	private GameObject inizioLivello;

	// Token: 0x040018BB RID: 6331
	private RaycastHit hitSelezione;

	// Token: 0x040018BC RID: 6332
	private int layerSelezione;

	// Token: 0x040018BD RID: 6333
	private Ray raggioSelezionamento;

	// Token: 0x040018BE RID: 6334
	public GameObject esercitoSelezionato;

	// Token: 0x040018BF RID: 6335
	public GameObject bandieraSelezionata;

	// Token: 0x040018C0 RID: 6336
	public GameObject secondoEsercitoSelezionato;

	// Token: 0x040018C1 RID: 6337
	public Material materPerEvidAlleato;

	// Token: 0x040018C2 RID: 6338
	public Material materPerSelAlleato;

	// Token: 0x040018C3 RID: 6339
	public Material materPerEvidNemico;

	// Token: 0x040018C4 RID: 6340
	public Material materPerSelNemico;

	// Token: 0x040018C5 RID: 6341
	private GameObject schede;

	// Token: 0x040018C6 RID: 6342
	private GameObject scheda1;

	// Token: 0x040018C7 RID: 6343
	private GameObject scheda2;

	// Token: 0x040018C8 RID: 6344
	private GameObject scheda3;

	// Token: 0x040018C9 RID: 6345
	private GameObject scheda4;

	// Token: 0x040018CA RID: 6346
	private GameObject scheda5;

	// Token: 0x040018CB RID: 6347
	public List<GameObject> ListaSchede;

	// Token: 0x040018CC RID: 6348
	private float timerDoppioClick;

	// Token: 0x040018CD RID: 6349
	private bool primoClickAvvenuto;

	// Token: 0x040018CE RID: 6350
	private bool doppioClickEffettuato;

	// Token: 0x040018CF RID: 6351
	private GameObject CanvasStrategia;

	// Token: 0x040018D0 RID: 6352
	private GameObject visualizzaEsercito;

	// Token: 0x040018D1 RID: 6353
	private GameObject visualizzaSecondoEsercito;

	// Token: 0x040018D2 RID: 6354
	private GameObject Headquarters;

	// Token: 0x040018D3 RID: 6355
	private GameObject CentroStanzaUI;

	// Token: 0x040018D4 RID: 6356
	private GameObject visualEserNemico;

	// Token: 0x040018D5 RID: 6357
	private GameObject Nest;

	// Token: 0x040018D6 RID: 6358
	private GameObject elencoEdifici;

	// Token: 0x040018D7 RID: 6359
	private GameObject infoEdifici;

	// Token: 0x040018D8 RID: 6360
	private GameObject pulsanteCostruisci;

	// Token: 0x040018D9 RID: 6361
	private GameObject pulsanteDemolisci;

	// Token: 0x040018DA RID: 6362
	private GameObject pulsanteAccesoSpento;

	// Token: 0x040018DB RID: 6363
	private GameObject iconaMovimento;

	// Token: 0x040018DC RID: 6364
	private GameObject iconaScambio;
}
