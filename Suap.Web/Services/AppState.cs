public class AppState
{
    private bool _loggedIn;
    private string? _userName  = null;
    public event Action OnChange;
    

    public string? Username
    {
        get { return _userName; }
        set
        {
              if (_userName != value)
            {
                _userName = value;
                NotifyStateChanged();
            }
        }
    }



    private void NotifyStateChanged() => OnChange?.Invoke();
}
