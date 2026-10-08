namespace Generics.Tables;

public class Table<TRow, TColumn, TValue>
{
    private readonly Dictionary<(TRow, TColumn), TValue> _table;
    private readonly HashSet<TRow> _rows;
    private readonly HashSet<TColumn> _columns;
    
    public IReadOnlySet<TRow> Rows { get; }
    public IReadOnlySet<TColumn> Columns { get; }
    public readonly OpenIndexer Open;
    public readonly ExistedIndexer Existed;

    public Table()
    {
        _table = new Dictionary<(TRow, TColumn), TValue>();
        _rows = [];
        _columns = [];
        Open = new OpenIndexer(_table, _rows, _columns);
        Existed = new ExistedIndexer(Open);
        
        Rows = _rows;//.AsReadOnly();
        Columns = _columns;//.AsReadOnly();
    }

    public void AddRow(TRow row) => _rows.Add(row);
    public void AddColumn(TColumn column) => _columns.Add(column);

    public class OpenIndexer(Dictionary<(TRow, TColumn), TValue> table, HashSet<TRow> rows, HashSet<TColumn> columns)
    {
        private readonly Dictionary<(TRow, TColumn), TValue> _table = table;
        private readonly HashSet<TRow> _rows = rows;
        private readonly HashSet<TColumn> _columns = columns;
        
        public TValue this[TRow row, TColumn column]
        {
            get
            {
                return !_table.TryGetValue((row, column), out var value) 
                    ? default 
                    : value;
            }
            set
            {
                _table[(row, column)] = value;
                _rows.Add(row);
                _columns.Add(column);
            }
        }
        
        public bool ContainsCell(TRow row, TColumn column)
            => _rows.Contains(row) && _columns.Contains(column);
    }

    public class ExistedIndexer(OpenIndexer open)
    {
        private readonly OpenIndexer _open = open;

        public TValue this[TRow row, TColumn column]
        {
            get
            {
                EnsureRowAndColumnExist(row, column);
                return _open[row, column];
            }
            set
            {
                EnsureRowAndColumnExist(row, column);
                _open[row, column] = value;
            }
        }

        private void EnsureRowAndColumnExist(TRow row, TColumn column)
        {
            if (!_open.ContainsCell(row, column))
                throw new ArgumentException();
        }
    }
}
