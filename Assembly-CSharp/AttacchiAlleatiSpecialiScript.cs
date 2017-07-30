using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x02000003 RID: 3
public class AttacchiAlleatiSpecialiScript : MonoBehaviour
{
	// Token: 0x0600000A RID: 10 RVA: 0x000022D8 File Offset: 0x000004D8
	private void Start()
	{
		this.infoAlleati = GameObject.FindGameObjectWithTag("Info Alleati");
		this.CanvasComandante = GameObject.FindGameObjectWithTag("CanvasComandante");
		this.schermataSelezParà = this.CanvasComandante.transform.FindChild("Pannello Paracadutisti").gameObject;
		this.infoNeutreTattica = GameObject.FindGameObjectWithTag("InfoNeutreTattica");
		this.primaCamera = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaCamere[0];
		this.layerMirino = 256;
	}

	// Token: 0x0600000B RID: 11 RVA: 0x0000235C File Offset: 0x0000055C
	private void Update()
	{
		this.MostraPulsanti();
		this.AttaccoZonaBombardamento();
		this.AttaccoZonaArtiglieria();
		this.timerProssLancioParà += Time.deltaTime;
		if (this.infoAlleati.GetComponent<GestioneComandanteInUI>().mirinoParàAttivo && this.timerProssLancioParà > 0f)
		{
			this.AttaccoLancioParacadutisti();
		}
		else
		{
			this.infoAlleati.GetComponent<GestioneComandanteInUI>().mirinoParàAttivo = false;
		}
	}

	// Token: 0x0600000C RID: 12 RVA: 0x000023D0 File Offset: 0x000005D0
	private void MostraPulsanti()
	{
		if (Input.GetMouseButtonUp(0) || Input.GetKeyUp(KeyCode.Z) || this.pulsAereoPremuto)
		{
			this.pulsAereoPremuto = false;
			this.bombardiereInLista = false;
			this.artiglieriaInLista = false;
			for (int i = 0; i < this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaSelezionamento.Count; i++)
			{
				if (!this.bombardiereInLista && this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaSelezionamento[i].GetComponent<PresenzaAlleato>().èBombardiere)
				{
					this.bombardiereInLista = true;
				}
				if (!this.artiglieriaInLista && this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaSelezionamento[i].GetComponent<PresenzaAlleato>().èArtiglieria)
				{
					this.artiglieriaInLista = true;
				}
			}
			if (this.artiglieriaInLista)
			{
				this.CanvasComandante.transform.GetChild(0).transform.GetChild(3).transform.GetChild(0).GetComponent<Button>().interactable = true;
			}
			else
			{
				this.CanvasComandante.transform.GetChild(0).transform.GetChild(3).transform.GetChild(0).GetComponent<Button>().interactable = false;
			}
			if (this.bombardiereInLista)
			{
				this.CanvasComandante.transform.GetChild(0).transform.GetChild(3).transform.GetChild(1).GetComponent<Button>().interactable = true;
			}
			else
			{
				this.CanvasComandante.transform.GetChild(0).transform.GetChild(3).transform.GetChild(1).GetComponent<Button>().interactable = false;
			}
		}
		if (Input.GetKeyDown(KeyCode.T))
		{
			if (this.bombardiereInLista)
			{
				this.attaccoZonaBomb = !this.attaccoZonaBomb;
				if (!this.attaccoZonaBomb && this.mirinoBombardamento)
				{
					UnityEngine.Object.Destroy(this.mirinoBombardamento);
					this.mirinoBombCreato = false;
				}
			}
			if (this.artiglieriaInLista)
			{
				this.attaccoZonaArt = !this.attaccoZonaArt;
				if (!this.attaccoZonaArt && this.mirinoArtiglieria)
				{
					UnityEngine.Object.Destroy(this.mirinoArtiglieria);
					this.mirinoArtCreato = false;
				}
			}
			if (this.attaccoZonaArt || this.attaccoZonaBomb)
			{
				Selezionamento.selezioneInvalidata = true;
			}
			else if (!this.attaccoZonaArt && !this.attaccoZonaBomb)
			{
				Selezionamento.selezioneInvalidata = false;
			}
		}
	}

	// Token: 0x0600000D RID: 13 RVA: 0x00002668 File Offset: 0x00000868
	private void AttaccoZonaArtiglieria()
	{
		if (this.attaccoZonaArt)
		{
			this.timerAnnullamentoArt += Time.unscaledDeltaTime;
			if (!this.mirinoArtCreato)
			{
				Vector3 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
				this.mirinoArtiglieria = (UnityEngine.Object.Instantiate(this.mirinoArtPrefab, position, Quaternion.identity) as GameObject);
				this.mirinoArtiglieria.transform.localScale = Vector3.zero;
				this.mirinoArtCreato = true;
			}
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if (Physics.Raycast(ray, out this.mirinoArtTarget, 9999f, this.layerMirino))
			{
				GameObject gameObject = null;
				foreach (GameObject current in this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaSelezionamento)
				{
					if (current != null && current.GetComponent<PresenzaAlleato>().èArtiglieria)
					{
						gameObject = current;
						break;
					}
				}
				if (gameObject != null)
				{
					float num = Vector3.Distance(gameObject.transform.position, this.mirinoArtiglieria.transform.position);
					float num2 = gameObject.GetComponent<PresenzaAlleato>().valoreInizialePrecisione + num / gameObject.GetComponent<PresenzaAlleato>().valorePerditaPrecisione;
					if (num > gameObject.GetComponent<PresenzaAlleato>().ListaMunizioniAttive[0].GetComponent<DatiGeneraliMunizione>().portataMinima && num < gameObject.GetComponent<PresenzaAlleato>().ListaMunizioniAttive[0].GetComponent<DatiGeneraliMunizione>().portataMassima)
					{
						this.mirinoArtiglieria.GetComponent<MeshRenderer>().material = this.mirinoArtValido;
					}
					else
					{
						this.mirinoArtiglieria.GetComponent<MeshRenderer>().material = this.mirinoArtNonValido;
					}
					this.mirinoArtiglieria.transform.forward = -this.mirinoArtTarget.normal;
					this.mirinoArtiglieria.transform.position = this.mirinoArtTarget.point - this.mirinoArtiglieria.transform.forward * 2f;
					this.mirinoArtiglieria.transform.localScale = new Vector3(num2 * 2.1f, num2 * 2.1f, num2 * 2.1f);
				}
			}
			if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
			{
				this.primaCamera.GetComponent<GestioneSuoniCamera>().attivaVoce = true;
				this.primaCamera.GetComponent<GestioneSuoniCamera>().numListaVoceSelez = 6;
				foreach (GameObject current2 in this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaSelezionamento)
				{
					if (current2.GetComponent<PresenzaAlleato>().èArtiglieria)
					{
						current2.GetComponent<PresenzaAlleato>().attaccoZonaOrdinato = true;
						current2.GetComponent<PresenzaAlleato>().attaccoOrdinato = false;
						current2.GetComponent<PresenzaAlleato>().destinazioneOrdinata = false;
						current2.GetComponent<PresenzaAlleato>().luogoAttZonaArt = this.mirinoArtiglieria.transform.position;
					}
				}
			}
			if (Input.GetKeyDown(KeyCode.Q) || Input.GetMouseButtonDown(1))
			{
				UnityEngine.Object.Destroy(this.mirinoArtiglieria);
				this.mirinoArtCreato = false;
				this.attaccoZonaArt = false;
				this.timerAnnullamentoArt = 0f;
				Selezionamento.selezioneInvalidata = false;
			}
			if (Input.GetMouseButtonUp(0) && this.timerAnnullamentoArt > 0.4f)
			{
				UnityEngine.Object.Destroy(this.mirinoArtiglieria);
				this.mirinoArtCreato = false;
				this.attaccoZonaArt = false;
				Selezionamento.selezioneInvalidata = false;
				this.timerAnnullamentoArt = 0f;
			}
		}
	}

	// Token: 0x0600000E RID: 14 RVA: 0x00002A4C File Offset: 0x00000C4C
	private void AttaccoZonaBombardamento()
	{
		if (this.attaccoZonaBomb)
		{
			this.timerAnnullamentoBomb += Time.unscaledDeltaTime;
			if (!this.mirinoBombCreato)
			{
				Vector3 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
				this.mirinoBombardamento = (UnityEngine.Object.Instantiate(this.mirinoBombPrefab, position, Quaternion.identity) as GameObject);
				this.mirinoBombCreato = true;
			}
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if (Physics.Raycast(ray, out this.mirinoBombTarget, 9999f, this.layerMirino))
			{
				this.mirinoBombardamento.transform.forward = -this.mirinoBombTarget.normal;
				this.mirinoBombardamento.transform.position = this.mirinoBombTarget.point - this.mirinoBombardamento.transform.forward * 2f;
			}
			if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
			{
				this.primaCamera.GetComponent<GestioneSuoniCamera>().attivaVoce = true;
				this.primaCamera.GetComponent<GestioneSuoniCamera>().numListaVoceSelez = 5;
				foreach (GameObject current in this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaSelezionamento)
				{
					if (current.GetComponent<PresenzaAlleato>().èBombardiere)
					{
						current.GetComponent<PresenzaAlleato>().attaccoZonaOrdinato = true;
						current.GetComponent<PresenzaAlleato>().attaccoOrdinato = false;
						current.GetComponent<PresenzaAlleato>().destinazioneOrdinata = false;
						current.GetComponent<PresenzaAlleato>().luogoAttZonaBomb = this.mirinoBombardamento.transform.position;
					}
				}
			}
			if (Input.GetKeyDown(KeyCode.Q) || Input.GetMouseButtonDown(1))
			{
				UnityEngine.Object.Destroy(this.mirinoBombardamento);
				this.mirinoBombCreato = false;
				this.attaccoZonaBomb = false;
				this.timerAnnullamentoBomb = 0f;
				Selezionamento.selezioneInvalidata = false;
			}
			if (Input.GetMouseButtonUp(0) && this.timerAnnullamentoBomb > 0.4f)
			{
				UnityEngine.Object.Destroy(this.mirinoBombardamento);
				this.mirinoBombCreato = false;
				this.attaccoZonaBomb = false;
				Selezionamento.selezioneInvalidata = false;
				this.timerAnnullamentoBomb = 0f;
			}
		}
	}

	// Token: 0x0600000F RID: 15 RVA: 0x00002CA8 File Offset: 0x00000EA8
	private void AttaccoLancioParacadutisti()
	{
		this.timerAnnullamentoParà += Time.unscaledDeltaTime;
		if (!this.mirinoParàCreato)
		{
			Vector3 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			this.quadMirinoParà = (UnityEngine.Object.Instantiate(this.quadMirinoParàPrefab, position, Quaternion.identity) as GameObject);
			this.mirinoParàCreato = true;
		}
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		if (Physics.Raycast(ray, out this.hitMirino, 9999f, this.layerMirino))
		{
			this.quadMirinoParà.transform.forward = -this.hitMirino.normal;
			this.quadMirinoParà.transform.position = this.hitMirino.point - this.quadMirinoParà.transform.forward * 2f;
		}
		if (Input.GetKeyDown(KeyCode.Q) || Input.GetMouseButtonDown(1))
		{
			UnityEngine.Object.Destroy(this.quadMirinoParà);
			this.mirinoParàCreato = false;
			this.infoAlleati.GetComponent<GestioneComandanteInUI>().mirinoParàAttivo = false;
			this.timerAnnullamentoParà = 0f;
		}
		float num = Vector3.Dot(-Vector3.up, this.quadMirinoParà.transform.forward);
		if (this.quadMirinoParà.transform.GetChild(0).GetComponent<ColliderMirinoParà>().ListaAmbienteToccato.Count == 0 && num > 0.95f && this.quadMirinoParà.transform.position.y < 230f)
		{
			this.quadMirinoParà.GetComponent<MeshRenderer>().material = this.mirinoParàValido;
			if (Input.GetMouseButtonUp(0) && this.timerAnnullamentoParà > 0.4f)
			{
				UnityEngine.Object.Destroy(this.quadMirinoParà);
				this.mirinoParàCreato = false;
				this.infoAlleati.GetComponent<GestioneComandanteInUI>().mirinoParàAttivo = false;
				Selezionamento.selezioneInvalidata = false;
				this.timerAnnullamentoParà = 0f;
				this.timerProssLancioParà = 0f;
				this.infoAlleati.GetComponent<GestioneComandanteInUI>().preparLancioParàAttivo = true;
				this.infoAlleati.GetComponent<GestioneComandanteInUI>().aggListaPossParà = true;
				this.infoAlleati.GetComponent<GestioneComandanteInUI>().aggListaParà = true;
				this.infoAlleati.GetComponent<GestioneComandanteInUI>().ListaPossibiliParà = new List<GameObject>();
				this.infoAlleati.GetComponent<GestioneComandanteInUI>().ListaParàPerLancio = new List<GameObject>();
				if (this.quadMirinoParà.transform.position.y < 150f)
				{
					this.infoAlleati.GetComponent<InfoGenericheAlleati>().puntoDiLancioParà = new Vector3(this.hitMirino.point.x, 300f, this.hitMirino.point.z);
				}
				else
				{
					this.infoAlleati.GetComponent<InfoGenericheAlleati>().puntoDiLancioParà = this.hitMirino.point + Vector3.up * 160f;
				}
				this.schermataSelezParà.GetComponent<CanvasGroup>().alpha = 1f;
				this.schermataSelezParà.GetComponent<CanvasGroup>().interactable = true;
				this.schermataSelezParà.GetComponent<CanvasGroup>().blocksRaycasts = true;
			}
		}
		else
		{
			this.quadMirinoParà.GetComponent<MeshRenderer>().material = this.mirinoParàNonValido;
		}
	}

	// Token: 0x04000005 RID: 5
	private GameObject infoAlleati;

	// Token: 0x04000006 RID: 6
	private GameObject CanvasComandante;

	// Token: 0x04000007 RID: 7
	private GameObject schermataSelezParà;

	// Token: 0x04000008 RID: 8
	private GameObject primaCamera;

	// Token: 0x04000009 RID: 9
	private GameObject infoNeutreTattica;

	// Token: 0x0400000A RID: 10
	public GameObject unitàScelta;

	// Token: 0x0400000B RID: 11
	public bool attaccoZonaArt;

	// Token: 0x0400000C RID: 12
	public bool attaccoZonaBomb;

	// Token: 0x0400000D RID: 13
	public GameObject mirinoArtPrefab;

	// Token: 0x0400000E RID: 14
	public GameObject mirinoArtiglieria;

	// Token: 0x0400000F RID: 15
	public GameObject mirinoBombPrefab;

	// Token: 0x04000010 RID: 16
	public GameObject mirinoBombardamento;

	// Token: 0x04000011 RID: 17
	public bool mirinoArtCreato;

	// Token: 0x04000012 RID: 18
	public bool mirinoBombCreato;

	// Token: 0x04000013 RID: 19
	private RaycastHit mirinoArtTarget;

	// Token: 0x04000014 RID: 20
	private RaycastHit mirinoBombTarget;

	// Token: 0x04000015 RID: 21
	public Material mirinoArtValido;

	// Token: 0x04000016 RID: 22
	public Material mirinoArtNonValido;

	// Token: 0x04000017 RID: 23
	public Material mirinoBombValido;

	// Token: 0x04000018 RID: 24
	public Material mirinoBombNonValido;

	// Token: 0x04000019 RID: 25
	public bool artiglieriaInLista;

	// Token: 0x0400001A RID: 26
	public bool bombardiereInLista;

	// Token: 0x0400001B RID: 27
	public bool attZonaBombAttiva;

	// Token: 0x0400001C RID: 28
	public float timerAnnullamentoArt;

	// Token: 0x0400001D RID: 29
	public float timerAnnullamentoBomb;

	// Token: 0x0400001E RID: 30
	private bool mirinoParàCreato;

	// Token: 0x0400001F RID: 31
	public GameObject quadMirinoParàPrefab;

	// Token: 0x04000020 RID: 32
	public GameObject quadMirinoParà;

	// Token: 0x04000021 RID: 33
	public Material mirinoParàValido;

	// Token: 0x04000022 RID: 34
	public Material mirinoParàNonValido;

	// Token: 0x04000023 RID: 35
	private float timerAnnullamentoParà;

	// Token: 0x04000024 RID: 36
	public float timerProssLancioParà;

	// Token: 0x04000025 RID: 37
	private int layerMirino;

	// Token: 0x04000026 RID: 38
	private RaycastHit hitMirino;

	// Token: 0x04000027 RID: 39
	public bool pulsAereoPremuto;
}
