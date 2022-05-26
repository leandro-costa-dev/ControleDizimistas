using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ControleDizimistas
{
    public partial class frm_parametros : Form
    {
        public frm_parametros()
        {
            InitializeComponent();
        }

        private void frm_configuracoes_Load(object sender, EventArgs e)
        {
            string sql = "SELECT * FROM PARAMETROS";

            DataTable tabela = new DataTable();

            class_banco banco = new class_banco();
            tabela = banco.ConsultaSQL(sql);

            txt_nomeEntidade.Text = tabela.Rows[0].Field<string>("NOME_ENTIDADE");
            txt_cnpjEntidade.Text = tabela.Rows[0].Field<string>("CNPJ");
            txt_nomeBanco.Text = tabela.Rows[0].Field<string>("NOME_BANCO");
            txt_caminhoBanco.Text = tabela.Rows[0].Field<string>("CAMINHO_BANCO");
            txt_usuarioBanco.Text = tabela.Rows[0].Field<string>("USUARIO_BANCO");
            txt_senhaBanco.Text = tabela.Rows[0].Field<string>("SENHA_BANCO");

        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {

            if (string.IsNullOrWhiteSpace(txt_nomeEntidade.Text) ||
                string.IsNullOrWhiteSpace(txt_cnpjEntidade.Text) ||
                string.IsNullOrWhiteSpace(txt_nomeBanco.Text) ||
                string.IsNullOrWhiteSpace(txt_caminhoBanco.Text) ||
                string.IsNullOrWhiteSpace(txt_usuarioBanco.Text) ||
                string.IsNullOrWhiteSpace(txt_senhaBanco.Text))
            {
                MessageBox.Show("Todos os campos devem ser preenchidos! ", "Atenção!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            
            if (MessageBox.Show("Deseja gravar as configurações?", "Parâmetros", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                class_banco banco = new class_banco();
                banco.InicializaBancoSQLite();
                banco.CriarBancoSQLite();

                class_parametros param = new class_parametros();
                param.NOME_ENTIDADE = txt_nomeEntidade.Text;
                param.CNPJ = txt_cnpjEntidade.Text;
                param.NOME_BANCO = txt_nomeBanco.Text;
                param.CAMINHO_BANCO = txt_caminhoBanco.Text;
                param.USUARIO_BANCO = txt_usuarioBanco.Text;
                param.SENHA_BANCO = txt_senhaBanco.Text;

                banco.GravarConfiguracoes(param);
            }
            else
            {
                return;
            }
                                
        }
    }
}
