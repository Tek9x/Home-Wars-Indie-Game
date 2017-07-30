using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000F8 RID: 248
public class PanePerConvoglio : MonoBehaviour
{
	// Token: 0x060007F2 RID: 2034 RVA: 0x001196E8 File Offset: 0x001178E8
	private void Start()
	{
		this.varieMappaLocale = GameObject.FindGameObjectWithTag("VarieMappaLocale");
		this.infoNeutreTattica = GameObject.FindGameObjectWithTag("InfoNeutreTattica");
		this.ListaDestinazioni = new List<Vector3>();
		for (int i = 0; i < this.varieMappaLocale.transform.GetChild(2).childCount; i++)
		{
			this.ListaDestinazioni.Add(this.varieMappaLocale.transform.GetChild(2).GetChild(i).transform.position);
		}
		this.prossimoPuntoDest = 1;
		if (this.numPane != 0)
		{
			for (int j = 0; j < 5; j++)
			{
				if (this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaPanePerConvoglio[j].GetComponent<PanePerConvoglio>().numPane == this.numPane - 1)
				{
					this.paneDavanti = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaPanePerConvoglio[j];
				}
			}
		}
	}

	// Token: 0x060007F3 RID: 2035 RVA: 0x001197E0 File Offset: 0x001179E0
	private void Update()
	{
		this.ControlloPaneDavanti();
		this.VerificaPresenzaNemica();
		if (this.spintaAttiva && this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().faseSchierInizTerminata && base.GetComponent<ObbiettivoTatticoScript>().vita > 0f)
		{
			this.Movimento();
			this.èPartito = true;
		}
		if (this.èPartito)
		{
			base.GetComponent<BoxCollider>().enabled = true;
		}
	}

	// Token: 0x060007F4 RID: 2036 RVA: 0x00119854 File Offset: 0x00117A54
	private void ControlloPaneDavanti()
	{
		if (this.numPane != 0)
		{
			if (this.paneDavanti != null && this.paneDavanti.GetComponent<ObbiettivoTatticoScript>().vita <= 0f)
			{
				for (int i = 4; i >= 0; i--)
				{
					if (this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaPanePerConvoglio[i] != null && this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaPanePerConvoglio[i].GetComponent<ObbiettivoTatticoScript>().vita > 0f && this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaPanePerConvoglio[i].GetComponent<PanePerConvoglio>().numPane < this.numPane)
					{
						this.paneDavanti = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaPanePerConvoglio[i];
						break;
					}
				}
			}
			else if (this.paneDavanti == null)
			{
				for (int j = 3; j >= 0; j--)
				{
					if (this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaPanePerConvoglio[j] != null && this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaPanePerConvoglio[j].GetComponent<PanePerConvoglio>().numPane < this.numPane)
					{
						this.paneDavanti = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaPanePerConvoglio[j];
						break;
					}
				}
			}
		}
	}

	// Token: 0x060007F5 RID: 2037 RVA: 0x001199D8 File Offset: 0x00117BD8
	private void VerificaPresenzaNemica()
	{
		this.spintaAttiva = false;
		for (int i = 0; i < base.transform.GetChild(1).GetComponent<ColliderObbiettivo>().ListaUnitàInObbiettivo.Count; i++)
		{
			if (base.transform.GetChild(1).GetComponent<ColliderObbiettivo>().ListaUnitàInObbiettivo[i] && base.transform.GetChild(1).GetComponent<ColliderObbiettivo>().ListaUnitàInObbiettivo[i].tag == "Nemico")
			{
				this.spintaAttiva = true;
				break;
			}
		}
		if (this.spintaAttiva && this.paneDavanti != null && this.paneDavanti.GetComponent<ObbiettivoTatticoScript>().vita > 0f)
		{
			float num = Vector3.Distance(base.transform.position, this.paneDavanti.transform.position);
			if (num < 80f)
			{
				this.spintaAttiva = false;
			}
		}
	}

	// Token: 0x060007F6 RID: 2038 RVA: 0x00119AE4 File Offset: 0x00117CE4
	private void Movimento()
	{
		base.transform.LookAt(this.ListaDestinazioni[this.prossimoPuntoDest]);
		base.transform.position += base.transform.forward * this.velocitàTraslazione * Time.deltaTime;
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

	// Token: 0x04001DFB RID: 7675
	public float velocitàTraslazione;

	// Token: 0x04001DFC RID: 7676
	private GameObject varieMappaLocale;

	// Token: 0x04001DFD RID: 7677
	private GameObject infoNeutreTattica;

	// Token: 0x04001DFE RID: 7678
	private List<Vector3> ListaDestinazioni;

	// Token: 0x04001DFF RID: 7679
	private int prossimoPuntoDest;

	// Token: 0x04001E00 RID: 7680
	private bool spintaAttiva;

	// Token: 0x04001E01 RID: 7681
	public int numPane;

	// Token: 0x04001E02 RID: 7682
	private GameObject paneDavanti;

	// Token: 0x04001E03 RID: 7683
	private bool èPartito;
}
