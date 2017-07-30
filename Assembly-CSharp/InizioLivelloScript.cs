using System;
using UnityEngine;

// Token: 0x0200010A RID: 266
public class InizioLivelloScript : MonoBehaviour
{
	// Token: 0x06000860 RID: 2144 RVA: 0x00126E90 File Offset: 0x00125090
	private void Awake()
	{
		if (base.GetComponent<OltreScene>().scenaDiMenu)
		{
			if (GameObject.FindGameObjectWithTag("CanvasMenuIniz"))
			{
				UnityEngine.Object.Instantiate(this.CanvasManuale, Vector3.zero, Quaternion.identity);
			}
		}
		else
		{
			if (base.GetComponent<OltreScene>().èInStrategia)
			{
				UnityEngine.Object.Instantiate(this.CanvasStrategia, Vector3.zero, Quaternion.identity);
				UnityEngine.Object.Instantiate(this.PulsantiFitStrategia, Vector3.zero, Quaternion.identity);
			}
			else
			{
				UnityEngine.Object.Instantiate(this.CanvasFPS, Vector3.zero, Quaternion.identity);
				UnityEngine.Object.Instantiate(this.CanvasComandante, Vector3.zero, Quaternion.identity);
				UnityEngine.Object.Instantiate(this.InfoAlleati, Vector3.zero, Quaternion.identity);
				UnityEngine.Object.Instantiate(this.PulsantiFitBattaglia, Vector3.zero, Quaternion.identity);
				UnityEngine.Object.Instantiate(this.IANemico, Vector3.zero, Quaternion.identity);
				UnityEngine.Object.Instantiate(this.AttacchiAlleatiSpeciali, Vector3.zero, Quaternion.identity);
				UnityEngine.Object.Instantiate(this.InfoNeutreTattica, Vector3.zero, Quaternion.identity);
			}
			UnityEngine.Object.Instantiate(this.CanvasMenu, Vector3.zero, Quaternion.identity);
			UnityEngine.Object.Instantiate(this.CanvasManuale, Vector3.zero, Quaternion.identity);
		}
	}

	// Token: 0x04001F9D RID: 8093
	public GameObject CanvasStrategia;

	// Token: 0x04001F9E RID: 8094
	public GameObject PulsantiFitStrategia;

	// Token: 0x04001F9F RID: 8095
	public GameObject CanvasFPS;

	// Token: 0x04001FA0 RID: 8096
	public GameObject CanvasComandante;

	// Token: 0x04001FA1 RID: 8097
	public GameObject InfoAlleati;

	// Token: 0x04001FA2 RID: 8098
	public GameObject PulsantiFitBattaglia;

	// Token: 0x04001FA3 RID: 8099
	public GameObject IANemico;

	// Token: 0x04001FA4 RID: 8100
	public GameObject AttacchiAlleatiSpeciali;

	// Token: 0x04001FA5 RID: 8101
	public GameObject InfoNeutreTattica;

	// Token: 0x04001FA6 RID: 8102
	public GameObject CanvasMenu;

	// Token: 0x04001FA7 RID: 8103
	public GameObject CanvasManuale;
}
