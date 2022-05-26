using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.IO;
using System.Data.SQLite;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ControleDizimistas
{
    public class class_banco
    {
        private String nomeBanco;
        private String caminhoBanco;
        private String usuarioBanco;
        private String senhaBanco;        
        public class_banco()
        {

        }

        //-------------Conexão com Banco de Dados------------
        public void InicializaBancoSQLite()
        {          
            if(File.Exists(Directory.GetCurrentDirectory() + @"\database.ini") == false)
            {
                MessageBox.Show("Erro na leitura do arquivo de configurações (.ini)! ", "Erro!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            class_iniFile ini = new class_iniFile(Directory.GetCurrentDirectory() + @"\database.ini");

            this.nomeBanco = ini.IniReadValue("DATABASE", "DATABASE");
            this.caminhoBanco = ini.IniReadValue("PATH", "PATH");
            this.usuarioBanco = ini.IniReadValue("DATABASE", "UID");
            this.senhaBanco = ini.IniReadValue("DATABASE", "PWD");              

        }

        public SQLiteConnection ConectarBancoSQLite()
        {
            SQLiteConnection sqliteConnection = new SQLiteConnection(@"Data Source=" + caminhoBanco + "\\" + nomeBanco + ".db");
            sqliteConnection.Open();
            return sqliteConnection;
        }

        //-------------Cria o Banco de Dados------------
        public void CriarBancoSQLite()
        {
            try
            {
                if (File.Exists(@caminhoBanco + "\\" + nomeBanco + ".db"))
                {
                    return;
                }
                else
                {
                    SQLiteConnection.CreateFile(@caminhoBanco + "\\" + nomeBanco + ".db");

                    var con = ConectarBancoSQLite();
                    var cmd = con.CreateCommand();
                    cmd.CommandText = ("CREATE TABLE IF NOT EXISTS Clientes(" +
                        "id int, " +
                        "Nome Varchar(50), " +
                        "email VarChar(80))");
                    cmd.ExecuteNonQuery();

                    cmd.CommandText = ("CREATE TABLE IF NOT EXISTS Parametros(" +
                        "nome_entidade VARCHAR (40)," +
                        "cnpj VARCHAR(14), " +
                        "nome_banco VARCHAR(20), " +
                        "caminho_banco VARCHAR(60), " +
                        "usuario_banco VARCHAR(20), " +
                        "senha_banco VARCHAR(20))");
                    cmd.ExecuteNonQuery();

                    con.Close();
                }           
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocorreu um erro ao criar o banco de dados! " + ex.Message, "Erro!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //-------------Gravar parâmetros do Sistema----------

        public void GravarConfiguracoes(class_parametros c)
        {
            if (ExisteInformacoes("PARAMETROS"))
            {
                //--------UPDATE---------

                try
                {
                    var con = ConectarBancoSQLite();
                    var cmd = con.CreateCommand();
                    cmd.CommandText = "UPDATE PARAMETROS SET " +
                        "NOME_ENTIDADE = @nomeEntidade, " +
                        "CNPJ = @cnpj, " +
                        "NOME_BANCO = @nomeBanco, " +
                        "CAMINHO_BANCO = @caminhoBanco, " +
                        "USUARIO_BANCO = @usuarioBanco, " +
                        "SENHA_BANCO = @senhaBanco";

                    cmd.Parameters.AddWithValue("@nomeEntidade", c.NOME_ENTIDADE);
                    cmd.Parameters.AddWithValue("@cnpj", c.CNPJ);
                    cmd.Parameters.AddWithValue("@nomeBanco", c.NOME_BANCO);
                    cmd.Parameters.AddWithValue("@caminhoBanco", c.CAMINHO_BANCO);
                    cmd.Parameters.AddWithValue("@usuarioBanco", c.USUARIO_BANCO);
                    cmd.Parameters.AddWithValue("@senhaBanco", c.SENHA_BANCO);

                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Configurações gravadas com sucesso!", "Sucesso!", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    con.Close();
                }
                catch (Exception ex)
                {

                    MessageBox.Show("Ocorreu um erro ao gravar as configurações " + ex.Message, "Erro!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
            else
            {
                //--------INSERT--------
                try
                {
                    var con = ConectarBancoSQLite();
                    var cmd = con.CreateCommand();
                    cmd.CommandText = "INSERT INTO PARAMETROS (" +
                        "NOME_ENTIDADE, " +
                        "CNPJ, " +
                        "NOME_BANCO, " +
                        "CAMINHO_BANCO, " +
                        "USUARIO_BANCO," +
                        "SENHA_BANCO) VALUES (@nomeEntidade, @cnpj, @nomeBanco, @caminhoBanco, @usuarioBanco, @senhaBanco)";

                    cmd.Parameters.AddWithValue("@nomeEntidade", c.NOME_ENTIDADE);
                    cmd.Parameters.AddWithValue("@cnpj", c.CNPJ);
                    cmd.Parameters.AddWithValue("@nomeBanco", c.NOME_BANCO);
                    cmd.Parameters.AddWithValue("@caminhoBanco", c.CAMINHO_BANCO);
                    cmd.Parameters.AddWithValue("@usuarioBanco", c.USUARIO_BANCO);
                    cmd.Parameters.AddWithValue("@senhaBanco", c.SENHA_BANCO);

                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Configurações gravadas com sucesso!", "Sucesso!", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    con.Close();

                }
                catch (Exception ex)
                {

                    MessageBox.Show("Ocorreu um erro ao gravar as configurações " + ex.Message, "Erro!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        //----------Consultar informações no banco de dados------------

        public DataTable ConsultaSQL(string sql)
        {
            //--------SELECT---------

            InicializaBancoSQLite();

            SQLiteDataAdapter adaptador = null;
            DataTable tabela = new DataTable();

            try
            {
                var con = ConectarBancoSQLite();
                var cmd = con.CreateCommand();
                cmd.CommandText = sql;

                adaptador = new SQLiteDataAdapter(cmd.CommandText, con);
                adaptador.Fill(tabela);
                
                con.Close();
                return tabela;

            }
            catch (Exception ex)
            {              
                MessageBox.Show("Ocorreu um erro ao consultar os dados! " + ex.Message, "Erro!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return tabela;
            }
          
        }


        //-------------Verifica se existe informações na tabela-------
        public bool ExisteInformacoes(string tabelaBanco)
        {
            bool res;

            SQLiteDataAdapter da = null;
            DataTable dt = new DataTable();

            var con = ConectarBancoSQLite();
            var cmd = con.CreateCommand();
            cmd.CommandText = "SELECT * FROM "+ tabelaBanco;

            da = new SQLiteDataAdapter(cmd.CommandText, con);
            da.Fill(dt);

            if (dt.Rows.Count > 0)
            {
                res = true;
            }
            else
            {
                res = false;
            }

            con.Close();
            return res;
        }

    }
}
