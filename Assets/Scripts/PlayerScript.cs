using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour, IDamageable
{
    public static float cleartime;
    public int playerHP = 3;
    public Text HPLabel;
    public static string scenename;

    public void TakeDamage(int damage)
    {
        playerHP -= damage;

        if (HPLabel != null)
        {
            HPLabel.text = "HP : " + playerHP;
        }

        if (playerHP <= 0)
        {
            Debug.Log("Player Dead");
        }
    }
}
