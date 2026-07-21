namespace BauMat.Client.Services;

public sealed class CompareState
{
    private readonly HashSet<int> _ids = new();

    public IReadOnlySet<int> Ids => _ids;

    public int Count => _ids.Count;

    public bool Contains(int id) => _ids.Contains(id);

    public void Toggle(int id)
    {
        if (!_ids.Remove(id))
        {
            _ids.Add(id);
        }

        NotifyChanged();
    }

    public void Clear()
    {
        if (_ids.Count == 0)
        {
            return;
        }

        _ids.Clear();
        NotifyChanged();
    }

    public event Action? OnChanged;

    private void NotifyChanged()
    {
        OnChanged?.Invoke();
    }
}
