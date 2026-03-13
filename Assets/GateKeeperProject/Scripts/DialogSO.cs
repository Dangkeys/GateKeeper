using UnityEngine;

[CreateAssetMenu(fileName = "DialogSO", menuName = "Scriptable Objects/DialogSO")]
public class DialogSO : ScriptableObject
{
    public string header;
    public string description;
    public DialogSO dialogSO;
    public Sprite image;
}
