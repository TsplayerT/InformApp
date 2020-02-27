using SQLite;

namespace InformApp.Modelo
{
    public abstract class _ModeloBase
    {
        [PrimaryKey, AutoIncrement]
        public int? Id { get; set; }
    }
}
