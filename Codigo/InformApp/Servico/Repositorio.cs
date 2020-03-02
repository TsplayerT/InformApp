using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using InformApp.Controle;
using InformApp.Modelo;
using InformApp.Utilidade;
using SQLite;
using Xamarin.Forms;

namespace InformApp.Servico
{
    public class Repositorio
    {
        private SQLiteAsyncConnection Conexao { get; }
        public static string DiretorioBancoDados => Path.Combine(DependencyService.Get<IArquivamento>().PegarDiretorio(), "InformApp.db");

        public Repositorio()
        {
            Conexao = new SQLiteAsyncConnection(DiretorioBancoDados, SQLiteOpenFlags.Create | SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.SharedCache | SQLiteOpenFlags.FullMutex);
        }

        public async Task<Dictionary<Type, bool>> Inicializar()
        {
            Miscelanea.DefinirAtributosPai(typeof(_ModeloBase));

            var modelos = Miscelanea.PegarTodasClassesEmNamespace(typeof(_ModeloBase).Namespace).Where(x => x.Name != nameof(SQLiteConnection.ColumnInfo) && x.IsClass && !x.IsAbstract && Conexao.TableMappings.All(p => p.TableName.ToLower() != x.Name.ToLower())).ToArray();

            await Conexao.EnableLoadExtensionAsync(true).ConfigureAwait(false);
            await Conexao.EnableWriteAheadLoggingAsync().ConfigureAwait(false);
            var resultado = await Conexao.CreateTablesAsync(CreateFlags.AutoIncPK, modelos).ConfigureAwait(false);

            return resultado.Results.ToDictionary(x => x.Key, x => x.Value == CreateTableResult.Created);
        }

        public async Task<T> PegarAsync<T>(Func<T, bool> funcao) where T : _ModeloBase, new()
        {
            try
            {
                var listaEntidades = await Conexao.Table<T>().ToListAsync().ConfigureAwait(false);
                var entidade = listaEntidades.FirstOrDefault(funcao ?? (x => true));

                return entidade;
            }
            catch (Exception ex)
            {
                await Principal.Mensagem(ex.Message);

                return default;
            }
        }

        public async Task<int?> AdicionarOuAtualizarAsync<T>(T entidade) where T : _ModeloBase, new()
        {
            if (entidade == null)
            {
                return default;
            }

            try
            {
                var resultado = await Conexao.InsertOrReplaceAsync(entidade, typeof(T)).ConfigureAwait(false);

                return resultado;
            }
            catch (Exception ex)
            {
                await Principal.Mensagem(ex.Message);

                return default;
            }
        }

        public async Task<List<T>> ListarAsync<T>(Expression<Func<T, bool>> funcao = null) where T : _ModeloBase, new()
        {
            try
            {
                var resultado = await Conexao.Table<T>().Where(funcao ?? (x => true)).ToListAsync().ConfigureAwait(false);

                return resultado;
            }
            catch (Exception ex)
            {
                await Principal.Mensagem(ex.Message);

                return default;
            }
        }
    }
}
