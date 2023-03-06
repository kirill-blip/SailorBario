using System;

public class Tower : InteractionBase
{
    public event EventHandler PlayerPressed;

    public override void Interact()
    {
        PlayerPressed?.Invoke(this, null);
        // OnPlayerEnteredOrExited();
    }
}