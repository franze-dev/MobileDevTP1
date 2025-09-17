using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BonusBolsaUI : MonoBehaviour
{
    [SerializeField] private Jugador Pj;
    [SerializeField] private Image Relleno;
    [SerializeField] private ControladorDeDescarga ContrDesc;
    [SerializeField] private int MaxBonus = (int)BolsaLogica.Valores.Valor2;

    [SerializeField] string PrefijoBonus = "$ ";
    [SerializeField] TextMeshProUGUI TextoBonus;

    private void Awake()
    {
        Relleno.fillAmount = 1;
        if (TextoBonus == null)
            TextoBonus = GetComponentInChildren<TextMeshProUGUI>();

        TextoBonus.text = PrefijoBonus + "0";
    }

    private void Update()
    {
        if (Pj.ContrDesc?.PEnMov != null)
        {
            MaxBonus = (int)Pj.ContrDesc.PEnMov.Valor;

            float fill = Mathf.Clamp01((float)Pj.ContrDesc.Bonus / MaxBonus);

            Relleno.fillAmount = Mathf.Lerp(Relleno.fillAmount, fill, Time.deltaTime * 5f);

            TextoBonus.text = PrefijoBonus + ((int)Pj.ContrDesc.Bonus).ToString("N0");
        }
        else
        {
            Relleno.fillAmount = 0;
            TextoBonus.text = PrefijoBonus + "0";
        }
    }
}
