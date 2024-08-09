namespace Suap.Triast.WebApi.Extensions;

public readonly ref struct SpanSplitter
{
    private readonly ReadOnlySpan<char> _input;
    private readonly char _splitOn;

    public SpanSplitter(ReadOnlySpan<char> input, char splitOn)
    {
        _input = input;
        _splitOn = splitOn;
    }

    public Enumerator GetEnumerator()
    {
        return new Enumerator(_input, _splitOn);
    }

    public ref struct Enumerator
    {
        private readonly ReadOnlySpan<char> _input;
        private readonly char _splitOn;
        private int _propertyPosition;
        public ReadOnlySpan<char> Current { get; private set; } = default;

        public Enumerator(ReadOnlySpan<char> input, char splitOn)
        {
            _input = input;
            _splitOn = splitOn;
            _propertyPosition = 0;
        }

        public bool MoveNext()
        {
            for (var i = _propertyPosition; i <= _input.Length; i++)
            {
                if (i != _input.Length && _input[i] != _splitOn)
                {
                    continue;
                }
                Current = _input[_propertyPosition..i];
                _propertyPosition = i + 1;
                return true;
            }

            return false;
        }
    }
}
