namespace BauMat.Client.Services;

public sealed class CompareState
{
    // Persona compares 2-6 materials at a time (see Issue #9 follow-up
    // stories); the "+ Add material" / catalog return-flow disables adding
    // beyond this cap.
    public const int MaxCount = 6;

    // Insertion order, not a HashSet, so the card view can default the
    // reference material to "the first one added" (CL-05).
    private readonly List<int> _order = new();
    private int? _referenceId;

    public IReadOnlyList<int> Ids => _order;

    public int Count => _order.Count;

    public bool IsFull => _order.Count >= MaxCount;

    public bool Contains(int id) => _order.Contains(id);

    // Falls back to the first added material when no reference has been
    // explicitly set (or the explicit one was removed from the list).
    public int? ReferenceId =>
        _referenceId is int id && _order.Contains(id)
            ? id
            : _order.Count > 0 ? _order[0] : null;

    public void Toggle(int id)
    {
        if (_order.Remove(id))
        {
            if (_referenceId == id)
            {
                _referenceId = null;
            }
        }
        else
        {
            if (IsFull)
            {
                return;
            }

            _order.Add(id);
        }

        NotifyChanged();
    }

    // CL-07: replaces the reference material rather than removing it. If the
    // chosen material is already a candidate it simply becomes the
    // reference; if it isn't in the list yet it is added.
    public void SetReference(int id)
    {
        if (!_order.Contains(id))
        {
            if (IsFull)
            {
                return;
            }

            _order.Add(id);
        }

        _referenceId = id;
        NotifyChanged();
    }

    public void Clear()
    {
        if (_order.Count == 0)
        {
            return;
        }

        _order.Clear();
        _referenceId = null;
        NotifyChanged();
    }

    public event Action? OnChanged;

    private void NotifyChanged()
    {
        OnChanged?.Invoke();
    }
}
