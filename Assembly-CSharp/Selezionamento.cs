using System;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x020000BB RID: 187
public class Selezionamento : MonoBehaviour
{
	// Token: 0x06000699 RID: 1689 RVA: 0x000E8EC0 File Offset: 0x000E70C0
	private void Start()
	{
		this.infoAlleati = GameObject.FindGameObjectWithTag("Info Alleati");
		this.IANemico = GameObject.FindGameObjectWithTag("IANemico");
		this.infoNeutreTattica = GameObject.FindGameObjectWithTag("InfoNeutreTattica");
		this.primaCamera = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaCamere[0];
		this.terzaCamera = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaCamere[2];
		this.CanvasComandante = GameObject.FindGameObjectWithTag("CanvasComandante");
		this.attacchiAlleatiSpeciali = GameObject.FindGameObjectWithTag("Attacchi Speciali Alleati");
		this.layerSelezione = 9626880;
		this.layerMovEAtt = 189696;
		if (base.name == "Prima Camera")
		{
			this.quadPerDestEBers = (UnityEngine.Object.Instantiate(this.quadPerDestEBersPrefab, Vector3.zero, Quaternion.identity) as GameObject);
			this.meshDiQuadDestEBers = this.quadPerDestEBers.GetComponent<MeshRenderer>();
			this.meshDiQuadDestEBers.enabled = false;
		}
	}

	// Token: 0x0600069A RID: 1690 RVA: 0x000E8FC0 File Offset: 0x000E71C0
	private void Update()
	{
		if (base.GetComponent<PrimaCamera>())
		{
			if (base.GetComponent<PrimaCamera>().cameraAttiva != 3)
			{
				this.raggioSelezionamento = Camera.main.ScreenPointToRay(Input.mousePosition);
				if (this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().faseSchierInizTerminata)
				{
					if (!this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().schieramentoAttivo)
					{
						this.GestioneClick();
						this.AzzeramentoLista();
						if (!Selezionamento.selezioneInvalidata && !this.infoAlleati.GetComponent<InfoGenericheAlleati>().posizTrapAttivo)
						{
							this.FunzioneSelezionamento();
						}
						this.MovimentoSelezionati();
						this.AttaccoSelezionati();
						this.SelezioneNemici();
					}
					else
					{
						this.SchieramentoAlleati();
					}
				}
				else
				{
					this.SchieramentoAlleati();
				}
				if (base.name == "Prima Camera")
				{
					if (this.quadDestAttivo || this.quadBersAttivo)
					{
						this.GestioneQuadDestEBers();
					}
					else
					{
						this.meshDiQuadDestEBers.enabled = false;
					}
				}
				if (this.infoAlleati.GetComponent<InfoGenericheAlleati>().posizTrapAttivo)
				{
					this.PerTrappole();
				}
				this.PuliziaListe();
			}
			else
			{
				this.meshDiQuadDestEBers.enabled = false;
			}
		}
		if (this.selezDaAereoParà)
		{
			this.selezDaAereoParà = false;
			this.CambioOspiteInParà();
		}
		if (Input.GetMouseButtonUp(1) && this.lineaCreata)
		{
			UnityEngine.Object.Destroy(this.lineaDestinazione);
			this.lineaCreata = false;
		}
	}

	// Token: 0x0600069B RID: 1691 RVA: 0x000E913C File Offset: 0x000E733C
	private void GestioneClick()
	{
		this.timerDoppioClick += Time.unscaledDeltaTime;
		if (this.timerDoppioClick > 0f && this.timerDoppioClick < 0.2f)
		{
			this.doppioClickChiamato = true;
		}
		else
		{
			this.doppioClickChiamato = false;
		}
		if (Input.GetMouseButtonDown(0))
		{
			this.timerDoppioClick = 0f;
		}
		if (Input.GetMouseButton(1))
		{
			this.timerPressioneClickDestro += Time.unscaledDeltaTime;
		}
		if (this.timerPressioneClickDestro > 0f && this.timerPressioneClickDestro < 0.2f)
		{
			this.clickDestroCorto = true;
		}
		else if (this.timerPressioneClickDestro > 0.2f)
		{
			this.clickDestroCorto = false;
		}
		if (Input.GetMouseButtonUp(1))
		{
			this.timerPressioneClickDestro = 0f;
		}
		if (Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(0))
		{
			this.destinazione = this.destinazioneDaSelezionare.point;
		}
	}

	// Token: 0x0600069C RID: 1692 RVA: 0x000E9244 File Offset: 0x000E7444
	private void AzzeramentoLista()
	{
		this.azzeramentoSelezione = false;
		if (Physics.Raycast(this.raggioSelezionamento, out this.oggettoDaSelezionare) && Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject() && !Input.GetKey(KeyCode.LeftControl))
		{
			this.azzeramentoSelezione = true;
		}
	}

	// Token: 0x0600069D RID: 1693 RVA: 0x000E92A0 File Offset: 0x000E74A0
	private void FunzioneSelezionamento()
	{
		if (this.aereoDaSelezionare)
		{
			foreach (GameObject current in this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaSelezionamento)
			{
				current.GetComponent<PresenzaAlleato>().truppaSelezionata = false;
			}
			this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaSelezionamento.Clear();
			this.trappolaSelez = null;
			bool flag = false;
			int num = 0;
			while (num < this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaèAereo.Count && !flag)
			{
				if (this.numPosAereoInQuadrato == this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaèAereo[num].GetComponent<PresenzaAlleato>().posInQuadratoAerei)
				{
					GameObject gameObject = this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaèAereo[num];
					if (this.infoAlleati.GetComponent<InfoGenericheAlleati>().aereoAppenaSpawnato)
					{
						gameObject.transform.forward = this.infoAlleati.GetComponent<InfoGenericheAlleati>().dirSpawnAereo;
						this.infoAlleati.GetComponent<InfoGenericheAlleati>().aereoAppenaSpawnato = false;
					}
					this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaSelezionamento.Add(gameObject);
					gameObject.GetComponent<PresenzaAlleato>().truppaSelezionata = true;
					this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaDisponibilitàAerei[this.numPosAereoInQuadrato] = false;
					flag = true;
					this.attacchiAlleatiSpeciali.GetComponent<AttacchiAlleatiSpecialiScript>().pulsAereoPremuto = true;
					break;
				}
				num++;
			}
			if (flag)
			{
				this.azzeramentoSelezione = true;
				this.aereoDaSelezionare = false;
			}
		}
		if (Input.GetKeyDown(KeyCode.Z))
		{
			foreach (GameObject current2 in this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaSelezionamento)
			{
				current2.GetComponent<PresenzaAlleato>().truppaSelezionata = false;
			}
			this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaSelezionamento.Clear();
			this.trappolaSelez = null;
			this.azzeramentoSelezione = true;
			foreach (GameObject current3 in this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaAlleati)
			{
				if (!this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaSelezionamento.Contains(current3))
				{
					this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaSelezionamento.Add(current3);
				}
			}
			base.GetComponent<GestioneSuoniCamera>().attivaVoce = true;
			base.GetComponent<GestioneSuoniCamera>().nonèVoce = true;
			base.GetComponent<AudioSource>().clip = this.primaCamera.GetComponent<GestioneSuoniCamera>().suonoClickGenerico1;
		}
		foreach (GameObject current4 in this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaSelezionamento)
		{
			current4.GetComponent<PresenzaAlleato>().truppaSelezionata = true;
		}
		if (Physics.Raycast(this.raggioSelezionamento, out this.oggettoDaSelezionare, 99999f, this.layerSelezione) && !EventSystem.current.IsPointerOverGameObject())
		{
			if (this.oggettoDaSelezionare.collider.gameObject.tag == "Alleato")
			{
				if (!Input.GetKey(KeyCode.LeftControl) && Input.GetMouseButtonDown(0))
				{
					foreach (GameObject current5 in this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaSelezionamento)
					{
						current5.GetComponent<PresenzaAlleato>().truppaSelezionata = false;
					}
					this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaSelezionamento.Clear();
					this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaSelezionamento.Add(this.oggettoDaSelezionare.collider.gameObject);
					this.trappolaSelez = null;
					base.GetComponent<GestioneSuoniCamera>().attivaVoce = true;
					base.GetComponent<GestioneSuoniCamera>().nonèVoce = true;
					base.GetComponent<AudioSource>().clip = this.primaCamera.GetComponent<GestioneSuoniCamera>().suonoClickGenerico1;
				}
				if (Input.GetKey(KeyCode.LeftControl) && Input.GetMouseButton(0) && !this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaSelezionamento.Contains(this.oggettoDaSelezionare.collider.gameObject))
				{
					this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaSelezionamento.Add(this.oggettoDaSelezionare.collider.gameObject);
					base.GetComponent<GestioneSuoniCamera>().attivaVoce = true;
					base.GetComponent<GestioneSuoniCamera>().nonèVoce = true;
					base.GetComponent<AudioSource>().clip = this.primaCamera.GetComponent<GestioneSuoniCamera>().suonoClickGenerico1;
				}
				if (Input.GetMouseButtonDown(0) && this.doppioClickChiamato)
				{
					foreach (GameObject current6 in this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaSelezionamento)
					{
						current6.GetComponent<PresenzaAlleato>().truppaSelezionata = false;
					}
					this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaSelezionamento.Clear();
					this.trappolaSelez = null;
					foreach (GameObject current7 in this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaAlleati)
					{
						int tipoTruppa = this.oggettoDaSelezionare.collider.gameObject.GetComponent<PresenzaAlleato>().tipoTruppa;
						if (current7.GetComponent<PresenzaAlleato>().tipoTruppa == tipoTruppa && !this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaSelezionamento.Contains(current7))
						{
							this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaSelezionamento.Add(current7);
						}
					}
					base.GetComponent<GestioneSuoniCamera>().attivaVoce = true;
					base.GetComponent<GestioneSuoniCamera>().nonèVoce = true;
					base.GetComponent<AudioSource>().clip = this.primaCamera.GetComponent<GestioneSuoniCamera>().suonoClickGenerico1;
				}
			}
			else if (Input.GetMouseButtonDown(0) && !Input.GetKey(KeyCode.LeftControl))
			{
				foreach (GameObject current8 in this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaSelezionamento)
				{
					current8.GetComponent<PresenzaAlleato>().truppaSelezionata = false;
				}
				this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaSelezionamento.Clear();
			}
			if (this.oggettoDaSelezionare.collider.gameObject.tag == "Trappola")
			{
				if (Input.GetMouseButtonDown(0))
				{
					if (this.trappolaSelez != null)
					{
						this.trappolaSelez.GetComponent<PresenzaTrappola>().trappolaSelezionata = false;
					}
					this.trappolaSelez = this.oggettoDaSelezionare.collider.transform.parent.gameObject;
					this.trappolaSelez.GetComponent<PresenzaTrappola>().trappolaSelezionata = true;
					base.GetComponent<GestioneSuoniCamera>().attivaVoce = true;
					base.GetComponent<GestioneSuoniCamera>().nonèVoce = true;
					base.GetComponent<AudioSource>().clip = this.primaCamera.GetComponent<GestioneSuoniCamera>().suonoClickGenerico1;
				}
			}
			else if (Input.GetMouseButtonDown(0) && this.trappolaSelez != null)
			{
				this.trappolaSelez.GetComponent<PresenzaTrappola>().trappolaSelezionata = false;
				this.trappolaSelez = null;
			}
		}
		if (Input.GetMouseButton(0) && !EventSystem.current.IsPointerOverGameObject() && !Input.GetKey(KeyCode.LeftControl))
		{
			this.timerRettSel += Time.unscaledDeltaTime;
			if (this.timerRettSel > 0.05f)
			{
				GameObject oggettoCameraAttiva = this.primaCamera.GetComponent<PrimaCamera>().oggettoCameraAttiva;
				if (!this.rettSelCreato)
				{
					this.inizioRett = oggettoCameraAttiva.GetComponent<Camera>().WorldToScreenPoint(this.oggettoDaSelezionare.point);
					this.rettSelCreato = true;
				}
				this.fineRett = oggettoCameraAttiva.GetComponent<Camera>().WorldToScreenPoint(this.oggettoDaSelezionare.point);
				this.larghezzaRett = this.fineRett.x - this.inizioRett.x;
				this.altezzaRett = this.fineRett.y - this.inizioRett.y;
				this.rettangoloSel = new Rect(this.inizioRett.x, this.inizioRett.y, this.larghezzaRett, this.altezzaRett);
				if (Mathf.Abs(this.larghezzaRett) > 10f)
				{
					foreach (GameObject current9 in this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaAlleati)
					{
						Vector3 vector = oggettoCameraAttiva.GetComponent<Camera>().WorldToScreenPoint(current9.transform.position);
						Vector2 v = new Vector2(vector.x, vector.y);
						if (this.rettangoloSel.Contains(v, true) && !this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaSelezionamento.Contains(current9))
						{
							this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaSelezionamento.Add(current9);
						}
						if (!this.rettangoloSel.Contains(v, true) && this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaSelezionamento.Contains(current9))
						{
							current9.GetComponent<PresenzaAlleato>().truppaSelezionata = false;
							this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaSelezionamento.Remove(current9);
							this.azzeramentoSelezione = true;
						}
					}
				}
			}
		}
		if (Input.GetMouseButtonUp(0))
		{
			this.timerRettSel = 0f;
			this.rettSelCreato = false;
		}
	}

	// Token: 0x0600069E RID: 1694 RVA: 0x000E9D90 File Offset: 0x000E7F90
	private void OnGUI()
	{
		if (this.timerRettSel > 0.2f)
		{
			GUI.skin = this.GUISkinRettangoloSel;
			Rect position = new Rect(this.inizioRett.x, (float)Screen.height - this.inizioRett.y, this.larghezzaRett, -this.altezzaRett);
			GUI.Box(position, " ", this.GUISkinRettangoloSel.customStyles[0]);
		}
	}

	// Token: 0x0600069F RID: 1695 RVA: 0x000E9E04 File Offset: 0x000E8004
	private void MovimentoSelezionati()
	{
		if (this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaSelezionamento.Count > 0 && !this.infoAlleati.GetComponent<InfoGenericheAlleati>().posizTrapAttivo)
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if (Physics.Raycast(ray, out this.destinazioneDaSelezionare, 99999f, this.layerMovEAtt) && this.destinazioneDaSelezionare.collider.gameObject.tag == "Ambiente" && this.infoAlleati.GetComponent<InfoGenericheAlleati>().timerAnnullTrap == 0f && (Input.GetMouseButton(1) || Input.GetMouseButtonUp(1)) && this.destinazioneDaSelezionare.point.y < 390f)
			{
				this.primaCamera.GetComponent<GestioneSuoniCamera>().attivaVoce = true;
				this.primaCamera.GetComponent<GestioneSuoniCamera>().numListaVoceSelez = 0;
				bool flag = false;
				bool flag2 = false;
				foreach (GameObject current in this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaSelezionamento)
				{
					float num = Mathf.Abs(this.destinazioneDaSelezionare.point.y - current.transform.position.y);
					current.GetComponent<PresenzaAlleato>().destinazioneOrdinata = true;
					current.GetComponent<PresenzaAlleato>().attaccoOrdinato = false;
					current.GetComponent<PresenzaAlleato>().attaccoZonaOrdinato = false;
					if (!current.GetComponent<PresenzaAlleato>().volante)
					{
						flag = true;
					}
					else
					{
						flag2 = true;
					}
				}
				if (flag)
				{
					this.DisposizioneDestinazioneConNavMesh();
				}
				if (flag2)
				{
					this.DisposizioneDestinazioneSenzaNavMesh();
				}
			}
		}
	}

	// Token: 0x060006A0 RID: 1696 RVA: 0x000E9FE8 File Offset: 0x000E81E8
	private void AttaccoSelezionati()
	{
		if (this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaSelezionamento.Count > 0)
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if (Physics.Raycast(ray, out this.oggettoDaSelezionare, 99999f, this.layerMovEAtt) && (this.oggettoDaSelezionare.collider.gameObject.tag == "Nemico" || this.oggettoDaSelezionare.collider.gameObject.tag == "Nemico Testa" || this.oggettoDaSelezionare.collider.gameObject.tag == "SelNemico") && Input.GetMouseButtonDown(1))
			{
				this.primaCamera.GetComponent<GestioneSuoniCamera>().attivaVoce = true;
				if (this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaSelezionamento[0] == null)
				{
					this.primaCamera.GetComponent<GestioneSuoniCamera>().numListaVoceSelez = 1;
				}
				else if (!this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaSelezionamento[0].GetComponent<PresenzaAlleato>().volante)
				{
					this.primaCamera.GetComponent<GestioneSuoniCamera>().numListaVoceSelez = 1;
				}
				else
				{
					this.primaCamera.GetComponent<GestioneSuoniCamera>().numListaVoceSelez = 4;
				}
				foreach (GameObject current in this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaSelezionamento)
				{
					if (this.oggettoDaSelezionare.collider.gameObject.tag == "Nemico")
					{
						this.oggettoBersaglio = this.oggettoDaSelezionare.collider.gameObject;
					}
					if (this.oggettoDaSelezionare.collider.gameObject.tag == "Nemico Testa")
					{
						this.oggettoBersaglio = this.oggettoDaSelezionare.collider.transform.parent.gameObject;
					}
					if (this.oggettoDaSelezionare.collider.gameObject.tag == "SelNemico")
					{
						this.oggettoBersaglio = this.oggettoDaSelezionare.collider.transform.parent.gameObject;
					}
					current.GetComponent<PresenzaAlleato>().attaccoOrdinato = true;
					current.GetComponent<PresenzaAlleato>().destinazioneOrdinata = false;
					current.GetComponent<PresenzaAlleato>().attaccoZonaOrdinato = false;
				}
			}
		}
	}

	// Token: 0x060006A1 RID: 1697 RVA: 0x000EA28C File Offset: 0x000E848C
	private void DisposizioneDestinazioneConNavMesh()
	{
		if (this.destinazioneDaSelezionare.collider.gameObject.tag == "Ambiente" && !this.clickDestroCorto)
		{
			if (Input.GetMouseButton(1))
			{
				if (!this.lineaCreata)
				{
					this.lineaDestinazione = (UnityEngine.Object.Instantiate(this.lineaDestinazionePrefab, this.destinazione + Vector3.up, Quaternion.identity) as GameObject);
					this.lineaCreata = true;
				}
				this.lineaDestinazione.GetComponent<LineRenderer>().SetPosition(0, this.destinazione);
				this.lineaDestinazione.GetComponent<LineRenderer>().SetPosition(1, this.destinazioneDaSelezionare.point);
			}
			if (Input.GetMouseButtonUp(1))
			{
				float f = Vector3.Distance(this.destinazione, this.destinazioneDaSelezionare.point);
				int num = Mathf.RoundToInt(f);
				int num2 = num / 7;
				Vector3 vector = -(this.destinazione - this.destinazioneDaSelezionare.point).normalized;
				Vector3 normalized = Vector3.Cross(vector, Vector3.up).normalized;
				bool flag = false;
				foreach (GameObject current in this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaSelezionamento)
				{
					if ((current.GetComponent<PresenzaAlleato>().èMezzo || current.GetComponent<PresenzaAlleato>().èArtiglieria) && current.GetComponent<PresenzaAlleato>().tipoTruppa != 6)
					{
						flag = true;
						break;
					}
				}
				int num3 = 0;
				int num4 = 0;
				for (int i = 0; i < this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaSelezionamento.Count; i++)
				{
					GameObject gameObject = this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaSelezionamento[i];
					bool flag2 = false;
					float num5 = Mathf.Abs(this.destinazioneDaSelezionare.point.y - gameObject.transform.position.y);
					if (num5 <= 3f)
					{
						flag2 = true;
					}
					else if (num5 > 3f && gameObject.GetComponent<PresenzaAlleato>().scalatrice)
					{
						flag2 = true;
					}
					if (flag2)
					{
						if (flag)
						{
							if (num4 >= num2 / 3)
							{
								num4 = 0;
								num3++;
							}
						}
						else if (num4 >= num2)
						{
							num4 = 0;
							num3++;
						}
						Vector3 destination = Vector3.zero;
						if (flag)
						{
							destination = this.destinazione + vector * (float)num4 * 21f - normalized * (float)num3 * 21f;
						}
						else
						{
							destination = this.destinazione + vector * (float)num4 * 6f - normalized * (float)num3 * 6f;
						}
						if (!gameObject.GetComponent<PresenzaAlleato>().volante && gameObject.GetComponent<PresenzaAlleato>().tipoTruppa != 32 && gameObject.GetComponent<NavMeshAgent>().isOnNavMesh)
						{
							gameObject.GetComponent<NavMeshAgent>().SetDestination(destination);
						}
						num4++;
					}
				}
			}
		}
		if (this.destinazioneDaSelezionare.collider.gameObject.tag == "Ambiente" && Input.GetMouseButtonUp(1) && this.clickDestroCorto)
		{
			if (base.name == "Prima Camera")
			{
				this.quadDestAttivo = true;
				this.quadBersAttivo = false;
				this.destOBersPerQuad = 0;
			}
			else if (base.name == "Seconda Camera")
			{
				this.primaCamera.GetComponent<Selezionamento>().quadDestAttivo = true;
				this.primaCamera.GetComponent<Selezionamento>().quadBersAttivo = false;
				this.primaCamera.GetComponent<Selezionamento>().destOBersPerQuad = 0;
			}
			int num6 = 7;
			Vector3 lhs = this.destinazioneDaSelezionare.point - this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaSelezionamento[0].transform.position;
			Vector3 normalized2 = Vector3.Cross(lhs, Vector3.up).normalized;
			Vector3 normalized3 = Vector3.Cross(normalized2, Vector3.up).normalized;
			bool flag3 = false;
			foreach (GameObject current2 in this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaSelezionamento)
			{
				if ((current2.GetComponent<PresenzaAlleato>().èMezzo || current2.GetComponent<PresenzaAlleato>().èArtiglieria) && current2.GetComponent<PresenzaAlleato>().tipoTruppa != 6)
				{
					flag3 = true;
					break;
				}
			}
			int num7 = 0;
			int num8 = 0;
			int count = this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaSelezionamento.Count;
			for (int j = 0; j < this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaSelezionamento.Count; j++)
			{
				GameObject gameObject2 = this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaSelezionamento[j];
				bool flag4 = false;
				float num9 = Mathf.Abs(this.destinazioneDaSelezionare.point.y - gameObject2.transform.position.y);
				if (num9 <= 3f)
				{
					flag4 = true;
				}
				else if (num9 > 3f && gameObject2.GetComponent<PresenzaAlleato>().scalatrice)
				{
					flag4 = true;
				}
				if (flag4)
				{
					if (flag3)
					{
						if (num8 >= num6 / 1)
						{
							num8 = 0;
							num7++;
						}
					}
					else if (num8 >= num6)
					{
						num8 = 0;
						num7++;
					}
					Vector3 destination2 = Vector3.zero;
					if (count == 1)
					{
						destination2 = this.destinazione;
					}
					else if (count <= num6)
					{
						if (flag3)
						{
							destination2 = this.destinazione + normalized2 * (float)num8 * 21f + normalized3 * (float)num7 * 21f - normalized2 * (float)count * 10f;
						}
						else
						{
							destination2 = this.destinazione + normalized2 * (float)num8 * 6f + normalized3 * (float)num7 * 6f - normalized2 * (float)count * 3f;
						}
					}
					else if (flag3)
					{
						destination2 = this.destinazione + normalized2 * (float)num8 * 21f + normalized3 * (float)num7 * 21f - normalized2 * 45f;
					}
					else
					{
						destination2 = this.destinazione + normalized2 * (float)num8 * 6f + normalized3 * (float)num7 * 6f - normalized2 * 15f;
					}
					if (!gameObject2.GetComponent<PresenzaAlleato>().volante && gameObject2.GetComponent<PresenzaAlleato>().tipoTruppa != 32 && gameObject2.GetComponent<NavMeshAgent>().isOnNavMesh)
					{
						if (!gameObject2.GetComponent<PresenzaAlleato>().èFanteria)
						{
							gameObject2.GetComponent<NavMeshAgent>().SetDestination(destination2);
						}
						else if (gameObject2.GetComponent<PresenzaAlleato>().toccaTerreno && gameObject2.GetComponent<NavMeshAgent>().enabled)
						{
							gameObject2.GetComponent<NavMeshAgent>().SetDestination(destination2);
						}
					}
					num8++;
				}
			}
		}
	}

	// Token: 0x060006A2 RID: 1698 RVA: 0x000EAAC0 File Offset: 0x000E8CC0
	private void DisposizioneDestinazioneSenzaNavMesh()
	{
		if (this.destinazioneDaSelezionare.collider.gameObject.tag == "Ambiente" && !this.clickDestroCorto)
		{
			if (Input.GetMouseButton(1))
			{
				if (!this.lineaCreata)
				{
					this.lineaDestinazione = (UnityEngine.Object.Instantiate(this.lineaDestinazionePrefab, this.destinazione, Quaternion.identity) as GameObject);
					this.lineaCreata = true;
				}
				this.lineaDestinazione.GetComponent<LineRenderer>().SetPosition(0, this.destinazione);
				this.lineaDestinazione.GetComponent<LineRenderer>().SetPosition(1, this.destinazioneDaSelezionare.point);
			}
			if (Input.GetMouseButtonUp(1))
			{
				float f = Vector3.Distance(this.destinazione, this.destinazioneDaSelezionare.point);
				int num = Mathf.RoundToInt(f);
				int num2 = num / 15;
				Vector3 vector = -(this.destinazione - this.destinazioneDaSelezionare.point).normalized;
				Vector3 normalized = Vector3.Cross(vector, Vector3.up).normalized;
				bool flag = false;
				foreach (GameObject current in this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaSelezionamento)
				{
					if (current.GetComponent<PresenzaAlleato>().tipoTruppa == 37 || current.GetComponent<PresenzaAlleato>().tipoTruppa == 43 || current.GetComponent<PresenzaAlleato>().tipoTruppa == 47)
					{
						flag = true;
						break;
					}
				}
				int num3 = 0;
				int num4 = 0;
				for (int i = 0; i < this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaSelezionamento.Count; i++)
				{
					GameObject gameObject = this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaSelezionamento[i];
					if (flag)
					{
						if (num4 >= num2 / 5)
						{
							num4 = 0;
							num3++;
						}
					}
					else if (num4 >= num2)
					{
						num4 = 0;
						num3++;
					}
					Vector3 destinazioneSenzaNavMesh = Vector3.zero;
					if (flag)
					{
						destinazioneSenzaNavMesh = this.destinazione + vector * (float)num4 * 50f - normalized * (float)num3 * 50f;
					}
					else
					{
						destinazioneSenzaNavMesh = this.destinazione + vector * (float)num4 * 17f - normalized * (float)num3 * 17f;
					}
					if (gameObject.GetComponent<PresenzaAlleato>().volante)
					{
						gameObject.GetComponent<PresenzaAlleato>().destinazioneSenzaNavMesh = destinazioneSenzaNavMesh;
					}
					num4++;
				}
			}
		}
		if (this.destinazioneDaSelezionare.collider.gameObject.tag == "Ambiente" && Input.GetMouseButtonUp(1) && this.clickDestroCorto)
		{
			if (base.name == "Prima Camera")
			{
				this.quadDestAttivo = true;
				this.quadBersAttivo = false;
				this.destOBersPerQuad = 0;
			}
			else if (base.name == "Seconda Camera")
			{
				this.primaCamera.GetComponent<Selezionamento>().quadDestAttivo = true;
				this.primaCamera.GetComponent<Selezionamento>().quadBersAttivo = false;
				this.primaCamera.GetComponent<Selezionamento>().destOBersPerQuad = 0;
			}
			int num5 = 7;
			Vector3 lhs = this.destinazioneDaSelezionare.point - this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaSelezionamento[0].transform.position;
			Vector3 normalized2 = Vector3.Cross(lhs, Vector3.up).normalized;
			Vector3 normalized3 = Vector3.Cross(normalized2, Vector3.up).normalized;
			int num6 = 0;
			int num7 = 0;
			int count = this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaSelezionamento.Count;
			for (int j = 0; j < this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaSelezionamento.Count; j++)
			{
				GameObject gameObject2 = this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaSelezionamento[j];
				Vector3 destinazioneSenzaNavMesh2 = Vector3.zero;
				if (num7 >= num5)
				{
					num7 = 0;
					num6++;
				}
				if (count == 1)
				{
					destinazioneSenzaNavMesh2 = this.destinazione;
				}
				else if (count <= num5)
				{
					bool flag2 = false;
					foreach (GameObject current2 in this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaSelezionamento)
					{
						if (current2.GetComponent<PresenzaAlleato>().tipoTruppa == 37 || current2.GetComponent<PresenzaAlleato>().tipoTruppa == 43 || current2.GetComponent<PresenzaAlleato>().tipoTruppa == 47)
						{
							flag2 = true;
							break;
						}
					}
					if (flag2)
					{
						destinazioneSenzaNavMesh2 = this.destinazione + normalized2 * (float)num7 * 50f + normalized3 * (float)num6 * 50f - normalized2 * (float)count * 10f;
					}
					else
					{
						destinazioneSenzaNavMesh2 = this.destinazione + normalized2 * (float)num7 * 17f + normalized3 * (float)num6 * 17f - normalized2 * (float)count * 5f;
					}
				}
				else
				{
					bool flag3 = false;
					foreach (GameObject current3 in this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaSelezionamento)
					{
						if (current3.GetComponent<PresenzaAlleato>().tipoTruppa == 37 || current3.GetComponent<PresenzaAlleato>().tipoTruppa == 43 || current3.GetComponent<PresenzaAlleato>().tipoTruppa == 47)
						{
							flag3 = true;
							break;
						}
					}
					if (flag3)
					{
						destinazioneSenzaNavMesh2 = this.destinazione + normalized2 * (float)num7 * 50f + normalized3 * (float)num6 * 50f - normalized2 * 100f;
					}
					else
					{
						destinazioneSenzaNavMesh2 = this.destinazione + normalized2 * (float)num7 * 17f + normalized3 * (float)num6 * 17f - normalized2 * 50f;
					}
				}
				if (gameObject2.GetComponent<PresenzaAlleato>().volante)
				{
					gameObject2.GetComponent<PresenzaAlleato>().destinazioneSenzaNavMesh = destinazioneSenzaNavMesh2;
				}
				num7++;
			}
		}
	}

	// Token: 0x060006A3 RID: 1699 RVA: 0x000EB204 File Offset: 0x000E9404
	private void SelezioneNemici()
	{
		if (Physics.Raycast(this.raggioSelezionamento, out this.oggettoDaSelezionare, 99999f, this.layerSelezione))
		{
			if (this.oggettoDaSelezionare.collider.gameObject.tag == "Nemico")
			{
				if (base.name == "Prima Camera")
				{
					this.quadDestAttivo = false;
					this.quadBersAttivo = true;
					this.destOBersPerQuad = 1;
					this.nemicoDelQuad = this.oggettoDaSelezionare.collider.gameObject;
				}
				else if (base.name == "Seconda Camera")
				{
					this.primaCamera.GetComponent<Selezionamento>().quadDestAttivo = false;
					this.primaCamera.GetComponent<Selezionamento>().quadBersAttivo = true;
					this.primaCamera.GetComponent<Selezionamento>().destOBersPerQuad = 1;
					this.primaCamera.GetComponent<Selezionamento>().nemicoDelQuad = this.oggettoDaSelezionare.collider.gameObject;
				}
				if (Input.GetMouseButtonDown(0))
				{
					this.IANemico.GetComponent<GestioneUIPerNemici>().mostraPannelloInfoNemico = true;
					this.IANemico.GetComponent<GestioneUIPerNemici>().nemicoPerPannelloInfo = this.oggettoDaSelezionare.collider.gameObject;
					base.GetComponent<GestioneSuoniCamera>().attivaVoce = true;
					base.GetComponent<GestioneSuoniCamera>().nonèVoce = true;
					base.GetComponent<AudioSource>().clip = this.primaCamera.GetComponent<GestioneSuoniCamera>().suonoClickGenerico1;
				}
			}
			else if (this.oggettoDaSelezionare.collider.gameObject.tag == "Nemico Testa" || this.oggettoDaSelezionare.collider.gameObject.tag == "SelNemico" || this.oggettoDaSelezionare.collider.gameObject.tag == "Nemico Coll Suppl")
			{
				if (base.name == "Prima Camera")
				{
					this.quadDestAttivo = false;
					this.quadBersAttivo = true;
					this.destOBersPerQuad = 1;
					this.nemicoDelQuad = this.oggettoDaSelezionare.collider.transform.parent.gameObject;
				}
				else if (base.name == "Seconda Camera")
				{
					this.primaCamera.GetComponent<Selezionamento>().quadDestAttivo = false;
					this.primaCamera.GetComponent<Selezionamento>().quadBersAttivo = true;
					this.primaCamera.GetComponent<Selezionamento>().destOBersPerQuad = 1;
					this.primaCamera.GetComponent<Selezionamento>().nemicoDelQuad = this.oggettoDaSelezionare.collider.transform.parent.gameObject;
				}
				if (Input.GetMouseButtonDown(0))
				{
					this.IANemico.GetComponent<GestioneUIPerNemici>().mostraPannelloInfoNemico = true;
					this.IANemico.GetComponent<GestioneUIPerNemici>().nemicoPerPannelloInfo = this.oggettoDaSelezionare.collider.transform.parent.gameObject;
					base.GetComponent<GestioneSuoniCamera>().attivaVoce = true;
					base.GetComponent<GestioneSuoniCamera>().nonèVoce = true;
					base.GetComponent<AudioSource>().clip = this.primaCamera.GetComponent<GestioneSuoniCamera>().suonoClickGenerico1;
				}
			}
			else
			{
				if (base.name == "Prima Camera")
				{
					this.quadBersAttivo = false;
					this.destOBersPerQuad = 0;
				}
				else if (base.name == "Seconda Camera")
				{
					this.primaCamera.GetComponent<Selezionamento>().quadBersAttivo = false;
					this.primaCamera.GetComponent<Selezionamento>().destOBersPerQuad = 0;
				}
				if (Input.GetMouseButtonDown(0))
				{
					this.IANemico.GetComponent<GestioneUIPerNemici>().mostraPannelloInfoNemico = false;
					this.IANemico.GetComponent<GestioneUIPerNemici>().nemicoPerPannelloInfo = null;
				}
			}
		}
	}

	// Token: 0x060006A4 RID: 1700 RVA: 0x000EB5B0 File Offset: 0x000E97B0
	private void SchieramentoAlleati()
	{
		if (this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().schieramentoAttivo && Physics.Raycast(this.raggioSelezionamento, out this.oggettoDaSelezionare, 99999f, this.layerSelezione) && this.oggettoDaSelezionare.collider.gameObject.tag == "Ambiente")
		{
			this.infoAlleati.GetComponent<GestioneComandanteInUI>().posSchierUnità = this.oggettoDaSelezionare.point;
			if (this.oggettoDaSelezionare.collider.gameObject.transform.childCount > 0 && this.oggettoDaSelezionare.collider.gameObject.transform.GetChild(0).tag == "AreaSchieramentoAlleato")
			{
				this.infoAlleati.GetComponent<GestioneComandanteInUI>().posPerSchierValida = true;
				this.ultimaAereaSchierToccata = this.oggettoDaSelezionare.collider.gameObject;
			}
			else
			{
				this.infoAlleati.GetComponent<GestioneComandanteInUI>().posPerSchierValida = false;
			}
		}
	}

	// Token: 0x060006A5 RID: 1701 RVA: 0x000EB6C0 File Offset: 0x000E98C0
	private void GestioneQuadDestEBers()
	{
		if (this.quadDestAttivo)
		{
			this.timerQuad += Time.unscaledDeltaTime;
			if (!this.quadPartito)
			{
				this.meshDiQuadDestEBers.material = this.matDestAssegnata;
				this.quadPerDestEBers.transform.position = this.destinazioneDaSelezionare.point + Vector3.up * 2f;
				this.quadPerDestEBers.transform.forward = -Vector3.up;
				float num = Vector3.Distance(this.destinazioneDaSelezionare.point, this.primaCamera.GetComponent<PrimaCamera>().oggettoCameraAttiva.transform.position);
				float num2 = num / 20f;
				this.quadPerDestEBers.transform.localScale = new Vector3(num2, num2, num2);
				this.meshDiQuadDestEBers.enabled = true;
				this.quadPartito = true;
			}
			if (this.timerQuad < 0.5f)
			{
				float num3 = this.quadPerDestEBers.transform.localScale.x - 50f * Time.unscaledDeltaTime;
				this.quadPerDestEBers.transform.localScale = new Vector3(num3, num3, num3);
			}
			else
			{
				this.meshDiQuadDestEBers.enabled = false;
				this.quadPartito = false;
				this.quadDestAttivo = false;
				this.timerQuad = 0f;
			}
		}
		else if (this.quadBersAttivo && this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaSelezionamento.Count > 0)
		{
			this.meshDiQuadDestEBers.material = this.matBersAssegnato;
			this.quadPerDestEBers.transform.position = this.oggettoDaSelezionare.point;
			if (this.nemicoDelQuad != null && !this.nemicoDelQuad.GetComponent<PresenzaNemico>().insettoVolante)
			{
				this.quadPerDestEBers.transform.forward = -Vector3.up;
			}
			else if (this.nemicoDelQuad != null && this.nemicoDelQuad.GetComponent<PresenzaNemico>().insettoVolante)
			{
				this.quadPerDestEBers.transform.forward = -(this.primaCamera.GetComponent<PrimaCamera>().oggettoCameraAttiva.transform.position - this.oggettoDaSelezionare.point);
			}
			float num4 = Vector3.Distance(this.destinazioneDaSelezionare.point, this.primaCamera.GetComponent<PrimaCamera>().oggettoCameraAttiva.transform.position);
			float num5 = num4 / 15f;
			this.quadPerDestEBers.transform.localScale = new Vector3(num5, num5, num5);
			this.meshDiQuadDestEBers.enabled = true;
		}
	}

	// Token: 0x060006A6 RID: 1702 RVA: 0x000EB984 File Offset: 0x000E9B84
	private void PerTrappole()
	{
		if (Physics.Raycast(this.raggioSelezionamento, out this.oggettoDaSelezionare, 99999f, 256) && this.oggettoDaSelezionare.collider.gameObject.tag == "Ambiente")
		{
			this.posPerTrappola = this.oggettoDaSelezionare.point;
		}
	}

	// Token: 0x060006A7 RID: 1703 RVA: 0x000EB9E8 File Offset: 0x000E9BE8
	private void CambioOspiteInParà()
	{
		foreach (GameObject current in this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaSelezionamento)
		{
			current.GetComponent<PresenzaAlleato>().truppaSelezionata = false;
		}
		this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaSelezionamento.Clear();
		this.trappolaSelez = null;
		this.azzeramentoSelezione = true;
		GameObject gameObject = this.aereoOrigineParà.GetComponent<ATT_ParaTransport>().ListaParàPresenti[this.numInListaParàDaSelez];
		this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaSelezionamento.Add(gameObject);
		gameObject.GetComponent<PresenzaAlleato>().truppaSelezionata = true;
		this.terzaCamera.GetComponent<TerzaCamera>().ospiteCamera = gameObject;
		this.terzaCamera.GetComponent<TerzaCamera>().diventaTPS = true;
		this.terzaCamera.GetComponent<TerzaCamera>().èTPS = true;
		this.terzaCamera.GetComponent<TerzaCamera>().èFPS = false;
		base.GetComponent<GestioneSuoniCamera>().attivaVoce = true;
		base.GetComponent<GestioneSuoniCamera>().nonèVoce = true;
		base.GetComponent<AudioSource>().clip = this.primaCamera.GetComponent<GestioneSuoniCamera>().suonoClickGenerico1;
	}

	// Token: 0x060006A8 RID: 1704 RVA: 0x000EBB38 File Offset: 0x000E9D38
	private void PuliziaListe()
	{
		if (this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaSelezionamento.Count <= 0)
		{
			this.infoAlleati.GetComponent<GestioneComandanteInUI>().ListaTipiTruppeSel.Clear();
			this.infoAlleati.GetComponent<GestioneComandanteInUI>().ListaPosizione0.Clear();
		}
	}

	// Token: 0x04001886 RID: 6278
	public GameObject lineaDestinazionePrefab;

	// Token: 0x04001887 RID: 6279
	private bool lineaCreata;

	// Token: 0x04001888 RID: 6280
	private GameObject lineaDestinazione;

	// Token: 0x04001889 RID: 6281
	private RaycastHit oggettoDaSelezionare;

	// Token: 0x0400188A RID: 6282
	private RaycastHit destinazioneDaSelezionare;

	// Token: 0x0400188B RID: 6283
	public GameObject oggettoBersaglio;

	// Token: 0x0400188C RID: 6284
	private GameObject infoAlleati;

	// Token: 0x0400188D RID: 6285
	private GameObject IANemico;

	// Token: 0x0400188E RID: 6286
	private GameObject infoNeutreTattica;

	// Token: 0x0400188F RID: 6287
	private GameObject primaCamera;

	// Token: 0x04001890 RID: 6288
	private GameObject terzaCamera;

	// Token: 0x04001891 RID: 6289
	private GameObject CanvasComandante;

	// Token: 0x04001892 RID: 6290
	private GameObject attacchiAlleatiSpeciali;

	// Token: 0x04001893 RID: 6291
	public bool azzeramentoSelezione;

	// Token: 0x04001894 RID: 6292
	public static bool selezioneInvalidata;

	// Token: 0x04001895 RID: 6293
	private float timerDoppioClick;

	// Token: 0x04001896 RID: 6294
	private bool doppioClickChiamato;

	// Token: 0x04001897 RID: 6295
	private float timerPressioneClickDestro;

	// Token: 0x04001898 RID: 6296
	public bool clickDestroCorto;

	// Token: 0x04001899 RID: 6297
	private Vector3 destinazione;

	// Token: 0x0400189A RID: 6298
	private int layerSelezione;

	// Token: 0x0400189B RID: 6299
	private int layerMovEAtt;

	// Token: 0x0400189C RID: 6300
	private Ray raggioSelezionamento;

	// Token: 0x0400189D RID: 6301
	private float timerRettSel;

	// Token: 0x0400189E RID: 6302
	private Vector3 inizioRett;

	// Token: 0x0400189F RID: 6303
	private Vector3 fineRett;

	// Token: 0x040018A0 RID: 6304
	private Rect rettangoloSel;

	// Token: 0x040018A1 RID: 6305
	private float larghezzaRett;

	// Token: 0x040018A2 RID: 6306
	private float altezzaRett;

	// Token: 0x040018A3 RID: 6307
	private bool rettSelCreato;

	// Token: 0x040018A4 RID: 6308
	public GUISkin GUISkinRettangoloSel;

	// Token: 0x040018A5 RID: 6309
	public bool aereoDaSelezionare;

	// Token: 0x040018A6 RID: 6310
	public GameObject aereoScelto;

	// Token: 0x040018A7 RID: 6311
	public int numPosAereoInQuadrato;

	// Token: 0x040018A8 RID: 6312
	public GameObject ultimaAereaSchierToccata;

	// Token: 0x040018A9 RID: 6313
	public bool quadDestAttivo;

	// Token: 0x040018AA RID: 6314
	public bool quadBersAttivo;

	// Token: 0x040018AB RID: 6315
	public bool quadPartito;

	// Token: 0x040018AC RID: 6316
	public int destOBersPerQuad;

	// Token: 0x040018AD RID: 6317
	public Material matDestAssegnata;

	// Token: 0x040018AE RID: 6318
	public Material matBersAssegnato;

	// Token: 0x040018AF RID: 6319
	public GameObject quadPerDestEBersPrefab;

	// Token: 0x040018B0 RID: 6320
	public GameObject quadPerDestEBers;

	// Token: 0x040018B1 RID: 6321
	public MeshRenderer meshDiQuadDestEBers;

	// Token: 0x040018B2 RID: 6322
	public float timerQuad;

	// Token: 0x040018B3 RID: 6323
	public GameObject nemicoDelQuad;

	// Token: 0x040018B4 RID: 6324
	public Vector3 posPerTrappola;

	// Token: 0x040018B5 RID: 6325
	public GameObject trappolaSelez;

	// Token: 0x040018B6 RID: 6326
	public bool selezDaAereoParà;

	// Token: 0x040018B7 RID: 6327
	public int numInListaParàDaSelez;

	// Token: 0x040018B8 RID: 6328
	public GameObject aereoOrigineParà;
}
