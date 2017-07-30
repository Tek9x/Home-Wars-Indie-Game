using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000032 RID: 50
public class ATT_Observer : MonoBehaviour
{
	// Token: 0x0600025E RID: 606 RVA: 0x00065750 File Offset: 0x00063950
	private void Start()
	{
		this.CanvasFPS = GameObject.FindGameObjectWithTag("CanvasFPS");
		this.mirinoBinocolo = this.CanvasFPS.transform.FindChild("Mirini a schermo intero").transform.FindChild("binocolo").gameObject;
		this.barraAgganciamento = this.mirinoBinocolo.transform.GetChild(5).GetChild(0).gameObject;
		this.infoNeutreTattica = GameObject.FindGameObjectWithTag("InfoNeutreTattica");
		this.primaCamera = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaCamere[0];
		this.terzaCamera = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaCamere[2];
		this.IANemico = GameObject.FindGameObjectWithTag("IANemico");
		this.InfoAlleati = GameObject.FindGameObjectWithTag("Info Alleati");
		this.varieMappaLocale = GameObject.FindGameObjectWithTag("VarieMappaLocale");
		this.alleatoNav = base.GetComponent<NavMeshAgent>();
		this.velocitàAlleatoNav = base.GetComponent<NavMeshAgent>().speed;
		this.campoCameraIniziale = this.terzaCamera.GetComponent<Camera>().fieldOfView;
		this.distFineOrdineMovimento = this.alleatoNav.stoppingDistance + 3f;
		this.velocitàIniziale = this.alleatoNav.speed;
		this.layerMirino = 256;
		this.tempoDiAggancio = 4f;
		this.tempoDiRiposo = 6f;
		this.avviaTimerRiposo = true;
		this.suoniObserver = base.GetComponent<AudioSource>();
		for (int i = 0; i < this.varieMappaLocale.GetComponent<VarieMappaLocale>().ListaDelleAreeDiSchieramento.Count; i++)
		{
			if (this.varieMappaLocale.GetComponent<VarieMappaLocale>().ListaDelleAreeDiSchieramento[i].transform.GetChild(0).tag == "AreaSchieramentoAlleato")
			{
				Vector3 position = this.varieMappaLocale.GetComponent<VarieMappaLocale>().ListaDelleAreeDiSchieramento[i].transform.position;
				this.origineColpiArt = new Vector3(position.x, 230f, position.z);
				break;
			}
		}
		this.volumeIniziale = this.suoniObserver.volume;
		this.ListaScritteQuantitàSupportoFPS = new List<GameObject>();
		this.ListaScritteQuantitàSupportoFPS.Add(this.mirinoBinocolo.transform.GetChild(0).GetChild(4).gameObject);
		this.ListaScritteQuantitàSupportoFPS.Add(this.mirinoBinocolo.transform.GetChild(1).GetChild(4).gameObject);
		this.ListaScritteQuantitàSupportoFPS.Add(this.mirinoBinocolo.transform.GetChild(2).GetChild(4).gameObject);
		this.ListaScritteQuantitàSupportoFPS.Add(this.mirinoBinocolo.transform.GetChild(3).GetChild(4).gameObject);
		this.ListaScritteQuantitàSupportoFPS.Add(this.mirinoBinocolo.transform.GetChild(4).GetChild(4).gameObject);
		this.mirinoBinocolo.transform.GetChild(0).GetChild(3).GetComponent<CanvasGroup>().alpha = 1f;
	}

	// Token: 0x0600025F RID: 607 RVA: 0x00065A6C File Offset: 0x00063C6C
	private void Update()
	{
		if (this.avviaTimerRiposo)
		{
			this.timerDiRiposo += Time.deltaTime;
		}
		if (base.gameObject != this.terzaCamera.GetComponent<TerzaCamera>().ospiteCamera)
		{
			if (!this.alleatoNav.isOnOffMeshLink)
			{
				this.calcoloJumpEffettuato = false;
				if (base.GetComponent<PresenzaAlleato>().destinazioneOrdinata)
				{
					this.alleatoNav.speed = this.velocitàIniziale;
				}
			}
			else
			{
				this.InJump();
			}
		}
		else
		{
			this.GestioneVisuali();
			if (this.terzaCamera.GetComponent<TerzaCamera>().èFPS)
			{
				this.MiriniInComune();
				if (Input.GetKeyDown(KeyCode.Alpha1))
				{
					this.mirinoBinocolo.transform.GetChild(0).GetChild(3).GetComponent<CanvasGroup>().alpha = 1f;
					this.mirinoBinocolo.transform.GetChild(1).GetChild(3).GetComponent<CanvasGroup>().alpha = 0f;
					this.mirinoBinocolo.transform.GetChild(2).GetChild(3).GetComponent<CanvasGroup>().alpha = 0f;
					this.mirinoBinocolo.transform.GetChild(3).GetChild(3).GetComponent<CanvasGroup>().alpha = 0f;
					this.mirinoBinocolo.transform.GetChild(4).GetChild(3).GetComponent<CanvasGroup>().alpha = 0f;
					this.numArmaAttiva = 0;
					this.timerDiAggancio = 0f;
					this.barraAgganciamento.GetComponent<Image>().fillAmount = 0f;
				}
				else if (Input.GetKeyDown(KeyCode.Alpha2))
				{
					this.mirinoBinocolo.transform.GetChild(0).GetChild(3).GetComponent<CanvasGroup>().alpha = 0f;
					this.mirinoBinocolo.transform.GetChild(1).GetChild(3).GetComponent<CanvasGroup>().alpha = 1f;
					this.mirinoBinocolo.transform.GetChild(2).GetChild(3).GetComponent<CanvasGroup>().alpha = 0f;
					this.mirinoBinocolo.transform.GetChild(3).GetChild(3).GetComponent<CanvasGroup>().alpha = 0f;
					this.mirinoBinocolo.transform.GetChild(4).GetChild(3).GetComponent<CanvasGroup>().alpha = 0f;
					this.numArmaAttiva = 1;
					this.timerDiAggancio = 0f;
					this.barraAgganciamento.GetComponent<Image>().fillAmount = 0f;
				}
				else if (Input.GetKeyDown(KeyCode.Alpha3))
				{
					this.mirinoBinocolo.transform.GetChild(0).GetChild(3).GetComponent<CanvasGroup>().alpha = 0f;
					this.mirinoBinocolo.transform.GetChild(1).GetChild(3).GetComponent<CanvasGroup>().alpha = 0f;
					this.mirinoBinocolo.transform.GetChild(2).GetChild(3).GetComponent<CanvasGroup>().alpha = 1f;
					this.mirinoBinocolo.transform.GetChild(3).GetChild(3).GetComponent<CanvasGroup>().alpha = 0f;
					this.mirinoBinocolo.transform.GetChild(4).GetChild(3).GetComponent<CanvasGroup>().alpha = 0f;
					this.numArmaAttiva = 2;
					this.timerDiAggancio = 0f;
					this.barraAgganciamento.GetComponent<Image>().fillAmount = 0f;
				}
				else if (Input.GetKeyDown(KeyCode.Alpha4))
				{
					this.mirinoBinocolo.transform.GetChild(0).GetChild(3).GetComponent<CanvasGroup>().alpha = 0f;
					this.mirinoBinocolo.transform.GetChild(1).GetChild(3).GetComponent<CanvasGroup>().alpha = 0f;
					this.mirinoBinocolo.transform.GetChild(2).GetChild(3).GetComponent<CanvasGroup>().alpha = 0f;
					this.mirinoBinocolo.transform.GetChild(3).GetChild(3).GetComponent<CanvasGroup>().alpha = 1f;
					this.mirinoBinocolo.transform.GetChild(4).GetChild(3).GetComponent<CanvasGroup>().alpha = 0f;
					this.numArmaAttiva = 3;
					this.timerDiAggancio = 0f;
					this.barraAgganciamento.GetComponent<Image>().fillAmount = 0f;
				}
				else if (Input.GetKeyDown(KeyCode.Alpha5))
				{
					this.mirinoBinocolo.transform.GetChild(0).GetChild(3).GetComponent<CanvasGroup>().alpha = 0f;
					this.mirinoBinocolo.transform.GetChild(1).GetChild(3).GetComponent<CanvasGroup>().alpha = 0f;
					this.mirinoBinocolo.transform.GetChild(2).GetChild(3).GetComponent<CanvasGroup>().alpha = 0f;
					this.mirinoBinocolo.transform.GetChild(3).GetChild(3).GetComponent<CanvasGroup>().alpha = 0f;
					this.mirinoBinocolo.transform.GetChild(4).GetChild(3).GetComponent<CanvasGroup>().alpha = 1f;
					this.numArmaAttiva = 4;
					this.timerDiAggancio = 0f;
					this.barraAgganciamento.GetComponent<Image>().fillAmount = 0f;
				}
				if (this.numArmaAttiva == 0)
				{
					this.HeliRifornimenti();
				}
				else if (this.numArmaAttiva == 1)
				{
					this.Fumogeno();
				}
				else if (this.numArmaAttiva == 2)
				{
					this.BombaLaser();
				}
				else if (this.numArmaAttiva == 3)
				{
					this.Artiglieria();
				}
				else if (this.numArmaAttiva == 4)
				{
					this.MissileCruise();
				}
				this.ListaScritteQuantitàSupportoFPS[0].GetComponent<Text>().text = this.InfoAlleati.GetComponent<InfoMunizionamento>().ListaQuantitàSupportoTattica[0].ToString();
				this.ListaScritteQuantitàSupportoFPS[1].GetComponent<Text>().text = this.InfoAlleati.GetComponent<InfoMunizionamento>().ListaQuantitàSupportoTattica[1].ToString();
				this.ListaScritteQuantitàSupportoFPS[2].GetComponent<Text>().text = this.InfoAlleati.GetComponent<InfoMunizionamento>().ListaQuantitàSupportoTattica[2].ToString();
				this.ListaScritteQuantitàSupportoFPS[3].GetComponent<Text>().text = this.InfoAlleati.GetComponent<InfoMunizionamento>().ListaQuantitàSupportoTattica[4].ToString();
				this.ListaScritteQuantitàSupportoFPS[4].GetComponent<Text>().text = this.InfoAlleati.GetComponent<InfoMunizionamento>().ListaQuantitàSupportoTattica[5].ToString();
			}
			else if (this.mirinoCreato)
			{
				UnityEngine.Object.Destroy(this.mirinoObserver);
				this.mirinoCreato = false;
			}
			base.GetComponent<NavMeshAgent>().enabled = false;
			if (this.terzaCamera.GetComponent<TerzaCamera>().èTPS)
			{
				this.mirinoBinocolo.GetComponent<CanvasGroup>().alpha = 0f;
			}
			if (this.primaCamera.GetComponent<PrimaCamera>().cameraAttiva == 3)
			{
				if (this.terzaCamera.GetComponent<TerzaCamera>().diventaFPS)
				{
					this.terzaCamera.GetComponent<TerzaCamera>().diventaFPS = false;
					this.mirinoBinocolo.GetComponent<CanvasGroup>().alpha = 1f;
					this.mirinoBinocolo.GetComponent<Image>().sprite = this.spriteBinocolo;
					this.terzaCamera.GetComponent<Camera>().fieldOfView = 30f;
					base.gameObject.transform.GetChild(1).GetComponent<SkinnedMeshRenderer>().enabled = false;
				}
				if (this.terzaCamera.GetComponent<TerzaCamera>().diventaTPS)
				{
					this.terzaCamera.GetComponent<TerzaCamera>().diventaTPS = false;
					base.gameObject.transform.GetChild(1).GetComponent<SkinnedMeshRenderer>().enabled = true;
				}
			}
		}
		if (this.terzaCamera.GetComponent<TerzaCamera>().esciDaPrimaPersona && this.InfoAlleati.GetComponent<InfoGenericheAlleati>().ListaSelezionamento.Count > 0 && this.InfoAlleati.GetComponent<InfoGenericheAlleati>().ListaSelezionamento[0] == base.gameObject)
		{
			this.terzaCamera.GetComponent<TerzaCamera>().esciDaPrimaPersona = false;
			this.terzaCamera.GetComponent<TerzaCamera>().ospiteCamera = null;
			this.timerPosizionamentoTPS = 0f;
			this.timerPosizionamentoFPS = 0f;
			base.GetComponent<NavMeshAgent>().enabled = true;
			base.gameObject.transform.GetChild(1).GetComponent<SkinnedMeshRenderer>().enabled = true;
			this.terzaCamera.GetComponent<Camera>().fieldOfView = this.campoCameraIniziale;
			if (this.mirinoCreato)
			{
				UnityEngine.Object.Destroy(this.mirinoObserver);
				this.mirinoCreato = false;
			}
		}
	}

	// Token: 0x06000260 RID: 608 RVA: 0x00066378 File Offset: 0x00064578
	private void InJump()
	{
		if (!this.calcoloJumpEffettuato)
		{
			this.calcoloDistJump = true;
		}
		if (this.calcoloDistJump)
		{
			this.calcoloDistJump = false;
			this.calcoloJumpEffettuato = true;
			float num = Mathf.Abs(this.alleatoNav.destination.y - base.transform.position.y);
			this.alleatoNav.speed = this.velocitàIniziale / (num / 80f) / 10f;
		}
	}

	// Token: 0x06000261 RID: 609 RVA: 0x000663FC File Offset: 0x000645FC
	private void GestioneVisuali()
	{
		if (!this.terzaCamera.GetComponent<TerzaCamera>().èFPS)
		{
			this.CameraTPS();
			this.timerPosizionamentoFPS = 0f;
		}
		if (this.terzaCamera.GetComponent<TerzaCamera>().èFPS)
		{
			this.CameraFPS();
			this.timerPosizionamentoTPS = 0f;
		}
	}

	// Token: 0x06000262 RID: 610 RVA: 0x00066458 File Offset: 0x00064658
	private void CameraTPS()
	{
		this.timerPosizionamentoTPS += Time.deltaTime;
		if (this.timerPosizionamentoTPS < 0.35f)
		{
			this.terzaCamera.GetComponent<TerzaCamera>().transform.parent = base.transform;
			this.terzaCamera.GetComponent<TerzaCamera>().transform.localPosition = this.posizionamentoCameraTPS;
			this.terzaCamera.GetComponent<Camera>().fieldOfView = this.campoCameraIniziale;
			this.terzaCamera.transform.rotation = base.transform.rotation;
		}
	}

	// Token: 0x06000263 RID: 611 RVA: 0x000664F0 File Offset: 0x000646F0
	private void CameraFPS()
	{
		this.timerPosizionamentoFPS += Time.deltaTime;
		if (this.timerPosizionamentoFPS < 0.35f)
		{
			this.terzaCamera.GetComponent<TerzaCamera>().transform.parent = base.transform;
			this.terzaCamera.GetComponent<TerzaCamera>().transform.localPosition = this.posizionamentoCameraFPS;
		}
		float num = 1f;
		if (this.terzaCamera.GetComponent<Camera>().fieldOfView < 40f && -Input.GetAxis("Mouse ScrollWheel") > 0f)
		{
			this.terzaCamera.GetComponent<Camera>().fieldOfView += -Input.GetAxis("Mouse ScrollWheel") * num * 10f;
		}
		else if (this.terzaCamera.GetComponent<Camera>().fieldOfView > 10f && -Input.GetAxis("Mouse ScrollWheel") < 0f)
		{
			this.terzaCamera.GetComponent<Camera>().fieldOfView += -Input.GetAxis("Mouse ScrollWheel") * num * 10f;
		}
	}

	// Token: 0x06000264 RID: 612 RVA: 0x00066614 File Offset: 0x00064814
	private void MiriniInComune()
	{
		if (!this.mirinoCreato)
		{
			this.mirinoCreato = true;
			Vector3 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			this.mirinoObserver = (UnityEngine.Object.Instantiate(this.mirinoObserverPrefab, position, Quaternion.identity) as GameObject);
			this.quadObserver1 = this.mirinoObserver.transform.GetChild(0).gameObject;
			this.quadObserver2 = this.mirinoObserver.transform.GetChild(1).gameObject;
		}
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		if (Physics.Raycast(ray, out this.hitMirino, 9999f, this.layerMirino))
		{
			this.mirinoObserver.transform.up = this.hitMirino.normal;
			this.mirinoObserver.transform.position = this.hitMirino.point;
		}
		if (Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.Tab))
		{
			UnityEngine.Object.Destroy(this.mirinoObserver);
			this.mirinoCreato = false;
		}
		if (Input.GetMouseButtonUp(0))
		{
			this.timerDiAggancio = 0f;
			this.barraAgganciamento.GetComponent<Image>().fillAmount = 0f;
		}
		if (this.avviaTimerRiposo && this.timerDiRiposo > 0f)
		{
			this.barraAgganciamento.GetComponent<Image>().fillAmount = 1f - this.timerDiRiposo / this.tempoDiRiposo;
			if (this.timerDiRiposo > this.tempoDiRiposo)
			{
				this.avviaTimerRiposo = false;
				this.attaccoDisponibile = true;
				this.timerDiAggancio = 0f;
				this.timerDiRiposo = 0f;
			}
		}
		float num = Vector3.Dot(Vector3.up, this.mirinoObserver.transform.up);
		if (this.mirinoObserver.transform.GetChild(2).GetComponent<ColliderMirinoParà>().ListaAmbienteToccato.Count == 0 && num > 0.95f)
		{
			if (!Input.GetMouseButton(0))
			{
				this.quadObserver1.GetComponent<MeshRenderer>().material = this.mirinoValidoNero;
				this.quadObserver2.GetComponent<MeshRenderer>().material = this.mirinoValidoNero;
				this.quadObserver2.GetComponent<MeshRenderer>().enabled = true;
				this.quadObserver1.transform.Rotate(new Vector3(0f, 0f, 1f));
				this.quadObserver2.transform.Rotate(new Vector3(0f, 0f, -1f));
			}
			else if (this.attaccoDisponibile)
			{
				this.timerDiAggancio += Time.deltaTime;
				if (this.timerDiAggancio > 0f)
				{
					this.quadObserver1.GetComponent<MeshRenderer>().material = this.mirinoValidoRosso;
					this.quadObserver2.GetComponent<MeshRenderer>().material = this.mirinoValidoRosso;
					this.quadObserver2.GetComponent<MeshRenderer>().enabled = true;
					float num2 = this.timerDiAggancio / this.tempoDiAggancio;
					this.quadObserver1.transform.Rotate(new Vector3(0f, 0f, 1f + 3f * num2));
					this.quadObserver2.transform.Rotate(new Vector3(0f, 0f, -1f - 3f * num2));
					this.barraAgganciamento.GetComponent<Image>().fillAmount = num2;
					if (this.timerDiAggancio > this.tempoDiAggancio)
					{
						if (this.numArmaAttiva <= 2 && this.InfoAlleati.GetComponent<InfoMunizionamento>().ListaQuantitàSupportoTattica[this.numArmaAttiva] > 0)
						{
							this.attaccoAvviato = true;
							this.attaccoDisponibile = false;
							this.avviaTimerRiposo = true;
							this.centroAttacco = this.hitMirino.point;
							if (this.numArmaAttiva == 0)
							{
								List<int> listaQuantitàSupportoTattica;
								List<int> expr_3E0 = listaQuantitàSupportoTattica = this.InfoAlleati.GetComponent<InfoMunizionamento>().ListaQuantitàSupportoTattica;
								int num3;
								int expr_3E4 = num3 = 0;
								num3 = listaQuantitàSupportoTattica[num3];
								expr_3E0[expr_3E4] = num3 - 1;
							}
							else if (this.numArmaAttiva == 1)
							{
								List<int> listaQuantitàSupportoTattica2;
								List<int> expr_41C = listaQuantitàSupportoTattica2 = this.InfoAlleati.GetComponent<InfoMunizionamento>().ListaQuantitàSupportoTattica;
								int num3;
								int expr_420 = num3 = 1;
								num3 = listaQuantitàSupportoTattica2[num3];
								expr_41C[expr_420] = num3 - 1;
							}
							else if (this.numArmaAttiva == 2)
							{
								List<int> listaQuantitàSupportoTattica3;
								List<int> expr_458 = listaQuantitàSupportoTattica3 = this.InfoAlleati.GetComponent<InfoMunizionamento>().ListaQuantitàSupportoTattica;
								int num3;
								int expr_45C = num3 = 2;
								num3 = listaQuantitàSupportoTattica3[num3];
								expr_458[expr_45C] = num3 - 1;
							}
						}
						else if (this.numArmaAttiva >= 3 && this.InfoAlleati.GetComponent<InfoMunizionamento>().ListaQuantitàSupportoTattica[this.numArmaAttiva + 1] > 0)
						{
							this.attaccoAvviato = true;
							this.attaccoDisponibile = false;
							this.avviaTimerRiposo = true;
							this.centroAttacco = this.hitMirino.point;
							if (this.numArmaAttiva == 3)
							{
								List<int> listaQuantitàSupportoTattica4;
								List<int> expr_4E9 = listaQuantitàSupportoTattica4 = this.InfoAlleati.GetComponent<InfoMunizionamento>().ListaQuantitàSupportoTattica;
								int num3;
								int expr_4ED = num3 = 4;
								num3 = listaQuantitàSupportoTattica4[num3];
								expr_4E9[expr_4ED] = num3 - 1;
							}
							else if (this.numArmaAttiva == 4)
							{
								List<int> listaQuantitàSupportoTattica5;
								List<int> expr_525 = listaQuantitàSupportoTattica5 = this.InfoAlleati.GetComponent<InfoMunizionamento>().ListaQuantitàSupportoTattica;
								int num3;
								int expr_529 = num3 = 5;
								num3 = listaQuantitàSupportoTattica5[num3];
								expr_525[expr_529] = num3 - 1;
							}
						}
					}
				}
				else
				{
					this.quadObserver1.GetComponent<MeshRenderer>().material = this.mirinoValidoNero;
					this.quadObserver2.GetComponent<MeshRenderer>().material = this.mirinoValidoNero;
					this.quadObserver2.GetComponent<MeshRenderer>().enabled = true;
					this.quadObserver1.transform.Rotate(new Vector3(0f, 0f, 1f));
					this.quadObserver2.transform.Rotate(new Vector3(0f, 0f, -1f));
				}
			}
			else
			{
				this.quadObserver1.GetComponent<MeshRenderer>().material = this.mirinoValidoNero;
				this.quadObserver2.GetComponent<MeshRenderer>().material = this.mirinoValidoNero;
				this.quadObserver2.GetComponent<MeshRenderer>().enabled = true;
				this.quadObserver1.transform.Rotate(new Vector3(0f, 0f, 1f));
				this.quadObserver2.transform.Rotate(new Vector3(0f, 0f, -1f));
			}
		}
		else
		{
			this.quadObserver1.GetComponent<MeshRenderer>().material = this.mirinoNonValido;
			this.quadObserver2.GetComponent<MeshRenderer>().enabled = false;
			if (this.timerDiAggancio > 0f)
			{
				this.timerDiAggancio = 0f;
				this.barraAgganciamento.GetComponent<Image>().fillAmount = 0f;
			}
			else if (this.timerDiRiposo > 0f)
			{
				this.timerDiAggancio = 0f;
			}
		}
	}

	// Token: 0x06000265 RID: 613 RVA: 0x00066CF4 File Offset: 0x00064EF4
	private void HeliRifornimenti()
	{
		if (this.attaccoAvviato)
		{
			this.attaccoAvviato = false;
			this.cassaRifor = (UnityEngine.Object.Instantiate(this.cassaRiforPrefab, new Vector3(this.centroAttacco.x, 280f, this.centroAttacco.z), Quaternion.identity) as GameObject);
			this.suoniObserver.clip = this.rumoreParacadute;
			this.suoniObserver.volume = this.volumeIniziale * 1f;
			this.suoniObserver.Play();
		}
	}

	// Token: 0x06000266 RID: 614 RVA: 0x00066D84 File Offset: 0x00064F84
	private void Fumogeno()
	{
		if (this.attaccoAvviato)
		{
			this.attaccoAvviato = false;
			this.fumogeno = (UnityEngine.Object.Instantiate(this.fumogenoPrefab, this.centroAttacco, Quaternion.identity) as GameObject);
			this.fumogeno.transform.forward = Vector3.up;
		}
	}

	// Token: 0x06000267 RID: 615 RVA: 0x00066DDC File Offset: 0x00064FDC
	private void BombaLaser()
	{
		if (this.attaccoAvviato)
		{
			this.attaccoAvviato = false;
			this.bombaLaser = (UnityEngine.Object.Instantiate(this.bombaLaserPrefab, new Vector3(this.centroAttacco.x, 280f, this.centroAttacco.z), Quaternion.identity) as GameObject);
			this.bombaLaser.transform.forward = -Vector3.up;
			this.suoniObserver.clip = this.rumoreSgancioBombaLaser;
			this.suoniObserver.volume = this.volumeIniziale * 2f;
			this.suoniObserver.Play();
		}
	}

	// Token: 0x06000268 RID: 616 RVA: 0x00066E84 File Offset: 0x00065084
	private void Artiglieria()
	{
		if (this.attaccoAvviato)
		{
			this.attaccoAvviato = false;
			this.oggettoArtiglieria = (UnityEngine.Object.Instantiate(this.oggettoArtiglieriaPrefab, this.origineColpiArt, Quaternion.identity) as GameObject);
			this.oggettoArtiglieria.GetComponent<ArtObserverPartenza>().destColpiArt = this.centroAttacco;
		}
	}

	// Token: 0x06000269 RID: 617 RVA: 0x00066EDC File Offset: 0x000650DC
	private void MissileCruise()
	{
		if (this.attaccoAvviato)
		{
			this.attaccoAvviato = false;
			this.missileDaCrociera = (UnityEngine.Object.Instantiate(this.missileDaCorcieraPrefab, this.origineColpiArt, Quaternion.identity) as GameObject);
			this.missileDaCrociera.GetComponent<MissileDaCrociera>().destMissile = this.centroAttacco + Vector3.up;
			this.suoniObserver.clip = this.rumoreMisisleCrociera;
			this.suoniObserver.volume = this.volumeIniziale * 0.5f;
			this.suoniObserver.Play();
		}
	}

	// Token: 0x04000A31 RID: 2609
	private GameObject infoNeutreTattica;

	// Token: 0x04000A32 RID: 2610
	private GameObject terzaCamera;

	// Token: 0x04000A33 RID: 2611
	private GameObject primaCamera;

	// Token: 0x04000A34 RID: 2612
	private GameObject CanvasFPS;

	// Token: 0x04000A35 RID: 2613
	private GameObject mirinoBinocolo;

	// Token: 0x04000A36 RID: 2614
	private GameObject barraAgganciamento;

	// Token: 0x04000A37 RID: 2615
	private GameObject varieMappaLocale;

	// Token: 0x04000A38 RID: 2616
	public Sprite spriteBinocolo;

	// Token: 0x04000A39 RID: 2617
	public Vector3 posizionamentoCameraTPS;

	// Token: 0x04000A3A RID: 2618
	public Vector3 posizionamentoCameraFPS;

	// Token: 0x04000A3B RID: 2619
	public Vector3 aggiustamentoTraslVisualeFPS;

	// Token: 0x04000A3C RID: 2620
	public Vector3 aggiustamentoRotVisualeFPS;

	// Token: 0x04000A3D RID: 2621
	private float timerPosizionamentoTPS;

	// Token: 0x04000A3E RID: 2622
	private float timerPosizionamentoFPS;

	// Token: 0x04000A3F RID: 2623
	private float campoCameraIniziale;

	// Token: 0x04000A40 RID: 2624
	private NavMeshAgent alleatoNav;

	// Token: 0x04000A41 RID: 2625
	private float velocitàAlleatoNav;

	// Token: 0x04000A42 RID: 2626
	private GameObject IANemico;

	// Token: 0x04000A43 RID: 2627
	private GameObject InfoAlleati;

	// Token: 0x04000A44 RID: 2628
	private float distFineOrdineMovimento;

	// Token: 0x04000A45 RID: 2629
	private bool calcoloDistJump;

	// Token: 0x04000A46 RID: 2630
	private bool calcoloJumpEffettuato;

	// Token: 0x04000A47 RID: 2631
	private float velocitàIniziale;

	// Token: 0x04000A48 RID: 2632
	private int numArmaAttiva;

	// Token: 0x04000A49 RID: 2633
	public bool mirinoCreato;

	// Token: 0x04000A4A RID: 2634
	public GameObject mirinoObserverPrefab;

	// Token: 0x04000A4B RID: 2635
	public GameObject mirinoObserver;

	// Token: 0x04000A4C RID: 2636
	private GameObject quadObserver1;

	// Token: 0x04000A4D RID: 2637
	private GameObject quadObserver2;

	// Token: 0x04000A4E RID: 2638
	public Material mirinoValidoNero;

	// Token: 0x04000A4F RID: 2639
	public Material mirinoValidoRosso;

	// Token: 0x04000A50 RID: 2640
	public Material mirinoNonValido;

	// Token: 0x04000A51 RID: 2641
	private RaycastHit hitMirino;

	// Token: 0x04000A52 RID: 2642
	private int layerMirino;

	// Token: 0x04000A53 RID: 2643
	private float timerDiAggancio;

	// Token: 0x04000A54 RID: 2644
	private float tempoDiAggancio;

	// Token: 0x04000A55 RID: 2645
	private bool attaccoAvviato;

	// Token: 0x04000A56 RID: 2646
	private float timerDiRiposo;

	// Token: 0x04000A57 RID: 2647
	private float tempoDiRiposo;

	// Token: 0x04000A58 RID: 2648
	private bool avviaTimerRiposo;

	// Token: 0x04000A59 RID: 2649
	private bool attaccoDisponibile;

	// Token: 0x04000A5A RID: 2650
	private Vector3 centroAttacco;

	// Token: 0x04000A5B RID: 2651
	private AudioSource suoniObserver;

	// Token: 0x04000A5C RID: 2652
	public AudioClip rumoreParacadute;

	// Token: 0x04000A5D RID: 2653
	public AudioClip rumoreFumogeno;

	// Token: 0x04000A5E RID: 2654
	public AudioClip rumoreSgancioBombaLaser;

	// Token: 0x04000A5F RID: 2655
	public AudioClip rumoreMisisleCrociera;

	// Token: 0x04000A60 RID: 2656
	private float volumeIniziale;

	// Token: 0x04000A61 RID: 2657
	public GameObject cassaRiforPrefab;

	// Token: 0x04000A62 RID: 2658
	private GameObject cassaRifor;

	// Token: 0x04000A63 RID: 2659
	public GameObject fumogenoPrefab;

	// Token: 0x04000A64 RID: 2660
	private GameObject fumogeno;

	// Token: 0x04000A65 RID: 2661
	public GameObject bombaLaserPrefab;

	// Token: 0x04000A66 RID: 2662
	private GameObject bombaLaser;

	// Token: 0x04000A67 RID: 2663
	public GameObject oggettoArtiglieriaPrefab;

	// Token: 0x04000A68 RID: 2664
	private GameObject oggettoArtiglieria;

	// Token: 0x04000A69 RID: 2665
	private Vector3 origineColpiArt;

	// Token: 0x04000A6A RID: 2666
	public GameObject missileDaCorcieraPrefab;

	// Token: 0x04000A6B RID: 2667
	private GameObject missileDaCrociera;

	// Token: 0x04000A6C RID: 2668
	private List<GameObject> ListaScritteQuantitàSupportoFPS;
}
