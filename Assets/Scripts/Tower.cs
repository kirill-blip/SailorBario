using System;

public class Tower : InteractionBase
{
    public event EventHandler PlayerPressed;

    public override void Interact()
    {
        if (FindObjectOfType<PlayerController>().HasKey())
        {
            PlayerPressed?.Invoke(this, null);
        }
        else
        {
            OnTipTextChanged(this, SecondTipText);
        }
    }
}