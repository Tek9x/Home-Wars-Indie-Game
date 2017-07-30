using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000F4 RID: 244
public class CamionPerConvoglio : MonoBehaviour
{
	// Token: 0x060007DD RID: 2013 RVA: 0x001181E4 File Offset: 0x001163E4
	private void Start()
	{
		this.varieMappaLocale = GameObject.FindGameObjectWithTag("VarieMappaLocale");
		this.infoNeutreTattica = GameObject.FindGameObjectWithTag("InfoNeutreTattica");
		this.ListaDestinazioni = new List<Vector3>();
		for (int i = 0; i < this.varieMappaLocale.transform.GetChild(1).childCount; i++)
		{
			this.ListaDestinazioni.Add(this.varieMappaLocale.transform.GetChild(1).GetChild(i).transform.position);
		}
		this.prossimoPuntoDest = 1;
		if (this.numConvoglio != 0)
		{
			for (int j = 0; j < 4; j++)
			{
				if (this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaCamionPerConvoglio[j].GetComponent<CamionPerConvoglio>().numConvoglio == this.numConvoglio - 1)
				{
					this.camionDavanti = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaCamionPerConvoglio[j];
				}
			}
		}
		this.ruote1 = base.transform.GetChild(1).transform.GetChild(0).gameObject;
		this.ruote2 = base.transform.GetChild(1).transform.GetChild(1).gameObject;
		this.ruote3 = base.transform.GetChild(1).transform.GetChild(2).gameObject;
		this.suonoRuote = base.GetComponent<AudioSource>();
	}

	// Token: 0x060007DE RID: 2014 RVA: 0x00118350 File Offset: 0x00116550
	private void Update()
	{
		this.ControlloCamionDavanti();
		this.VerificaPresenzaAlleata();
		if (this.spintaAttiva && this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().faseSchierInizTerminata && base.GetComponent<ObbiettivoTatticoScript>().vita > 0f)
		{
			this.Movimento();
			if (!this.suonoPartito)
			{
				this.suonoPartito = true;
				this.suonoRuote.Play();
			}
		}
		else if (!this.fumoPartito && base.GetComponent<ObbiettivoTatticoScript>().vita <= 0f)
		{
			this.fumoPartito = true;
			base.transform.FindChild("fumo").GetComponent<ParticleSystem>().Play();
			base.GetComponent<BoxCollider>().enabled = false;
		}
	}

	// Token: 0x060007DF RID: 2015 RVA: 0x00118414 File Offset: 0x00116614
	private void ControlloCamionDavanti()
	{
		if (this.numConvoglio != 0)
		{
			if (this.camionDavanti != null && this.camionDavanti.GetComponent<ObbiettivoTatticoScript>().vita <= 0f)
			{
				for (int i = 3; i >= 0; i--)
				{
					if (this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaCamionPerConvoglio[i] != null && this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaCamionPerConvoglio[i].GetComponent<ObbiettivoTatticoScript>().vita > 0f && this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaCamionPerConvoglio[i].GetComponent<CamionPerConvoglio>().numConvoglio < this.numConvoglio)
					{
						this.camionDavanti = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaCamionPerConvoglio[i];
						break;
					}
				}
			}
			else if (this.camionDavanti == null)
			{
				for (int j = 3; j >= 0; j--)
				{
					if (this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaCamionPerConvoglio[j] != null && this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaCamionPerConvoglio[j].GetComponent<CamionPerConvoglio>().numConvoglio < this.numConvoglio)
					{
						this.camionDavanti = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaCamionPerConvoglio[j];
						break;
					}
				}
			}
		}
	}

	// Token: 0x060007E0 RID: 2016 RVA: 0x00118598 File Offset: 0x00116798
	private void VerificaPresenzaAlleata()
	{
		this.spintaAttiva = true;
		if (this.spintaAttiva && this.camionDavanti != null && this.camionDavanti.GetComponent<ObbiettivoTatticoScript>().vita > 0f)
		{
			float num = Vector3.Distance(base.transform.position, this.camionDavanti.transform.position);
			if (num < 80f)
			{
				this.spintaAttiva = false;
				this.suonoRuote.Stop();
				this.suonoPartito = false;
			}
		}
		if (!this.spintaAttiva)
		{
			this.suonoRuote.Stop();
			this.suonoPartito = false;
		}
	}

	// Token: 0x060007E1 RID: 2017 RVA: 0x00118644 File Offset: 0x00116844
	private void Movimento()
	{
		base.transform.LookAt(this.ListaDestinazioni[this.prossimoPuntoDest]);
		base.transform.position += base.transform.forward * this.velocitàTraslazione * Time.deltaTime;
		this.ruote1.transform.Rotate(Vector3.right * 1f);
		this.ruote2.transform.Rotate(Vector3.right * 1f);
		this.ruote3.transform.Rotate(Vector3.right * 1f);
		float num = Vector3.Distance(base.transform.position, this.ListaDestinazioni[this.prossimoPuntoDest]);
		if (num < 2f)
		{
			if (this.prossimoPuntoDest < this.ListaDestinazioni.Count - 1)
			{
				this.prossimoPuntoDest++;
			}
			else
			{
				UnityEngine.Object.Destroy(base.gameObject);
			}
		}
	}

	// Token: 0x04001DC2 RID: 7618
	public float velocitàTraslazione;

	// Token: 0x04001DC3 RID: 7619
	private GameObject varieMappaLocale;

	// Token: 0x04001DC4 RID: 7620
	private GameObject infoNeutreTattica;

	// Token: 0x04001DC5 RID: 7621
	private List<Vector3> ListaDestinazioni;

	// Token: 0x04001DC6 RID: 7622
	private int prossimoPuntoDest;

	// Token: 0x04001DC7 RID: 7623
	private bool spintaAttiva;

	// Token: 0x04001DC8 RID: 7624
	private bool fumoPartito;

	// Token: 0x04001DC9 RID: 7625
	public int numConvoglio;

	// Token: 0x04001DCA RID: 7626
	public GameObject camionDavanti;

	// Token: 0x04001DCB RID: 7627
	private GameObject ruote1;

	// Token: 0x04001DCC RID: 7628
	private GameObject ruote2;

	// Token: 0x04001DCD RID: 7629
	private GameObject ruote3;

	// Token: 0x04001DCE RID: 7630
	private AudioSource suonoRuote;

	// Token: 0x04001DCF RID: 7631
	private bool suonoPartito;

	// Token: 0x04001DD0 RID: 7632
	public GameObject testoVita;

	// Token: 0x04001DD1 RID: 7633
	public GameObject quadRaggioSpinta;
}
