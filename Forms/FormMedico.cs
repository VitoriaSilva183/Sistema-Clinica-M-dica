using System;
using System.Drawing;
using System.Windows.Forms;
using ClinicaMedica.Models;
using ClinicaMedica.Repositories;

namespace ClinicaMedica.Forms
{
    // tela pra cadastrar, editar e excluir medico
    public class FormMedico : Form
    {
        private MedicoRepository _repositorio = new MedicoRepository();
        private int _idSelecionado = 0;

        private TextBox txtNome;
        private TextBox txtCrm;
        private TextBox txtEspecialidade;
        private TextBox txtTelefone;
        private DataGridView grade;

        public FormMedico()
        {
            this.Text = "Medicos";
            this.Size = new Size(820, 540);
            this.StartPosition = FormStartPosition.CenterScreen;

            // campo nome
            Label lblNome = new Label();
            lblNome.Text = "Nome:";
            lblNome.Location = new Point(20, 20);
            lblNome.AutoSize = true;
            this.Controls.Add(lblNome);
            txtNome = new TextBox();
            txtNome.Location = new Point(20, 40);
            txtNome.Size = new Size(250, 25);
            this.Controls.Add(txtNome);

            // campo crm
            Label lblCrm = new Label();
            lblCrm.Text = "CRM:";
            lblCrm.Location = new Point(300, 20);
            lblCrm.AutoSize = true;
            this.Controls.Add(lblCrm);
            txtCrm = new TextBox();
            txtCrm.Location = new Point(300, 40);
            txtCrm.Size = new Size(150, 25);
            this.Controls.Add(txtCrm);

            // campo especialidade
            Label lblEsp = new Label();
            lblEsp.Text = "Especialidade:";
            lblEsp.Location = new Point(480, 20);
            lblEsp.AutoSize = true;
            this.Controls.Add(lblEsp);
            txtEspecialidade = new TextBox();
            txtEspecialidade.Location = new Point(480, 40);
            txtEspecialidade.Size = new Size(200, 25);
            this.Controls.Add(txtEspecialidade);

            // campo telefone
            Label lblTel = new Label();
            lblTel.Text = "Telefone:";
            lblTel.Location = new Point(20, 75);
            lblTel.AutoSize = true;
            this.Controls.Add(lblTel);
            txtTelefone = new TextBox();
            txtTelefone.Location = new Point(20, 95);
            txtTelefone.Size = new Size(250, 25);
            this.Controls.Add(txtTelefone);

            // botao salvar
            Button btnSalvar = new Button();
            btnSalvar.Text = "Salvar";
            btnSalvar.Location = new Point(20, 140);
            btnSalvar.Size = new Size(120, 35);
            btnSalvar.Click += Salvar;
            this.Controls.Add(btnSalvar);

            // botao limpar
            Button btnLimpar = new Button();
            btnLimpar.Text = "Limpar";
            btnLimpar.Location = new Point(150, 140);
            btnLimpar.Size = new Size(120, 35);
            btnLimpar.Click += (sender, e) => Limpar();
            this.Controls.Add(btnLimpar);

            // botao excluir
            Button btnExcluir = new Button();
            btnExcluir.Text = "Excluir";
            btnExcluir.Location = new Point(280, 140);
            btnExcluir.Size = new Size(120, 35);
            btnExcluir.Click += Excluir;
            this.Controls.Add(btnExcluir);

            // tabela dos medicos
            grade = new DataGridView();
            grade.Location = new Point(20, 200);
            grade.Size = new Size(760, 280);
            grade.ReadOnly = true;
            grade.AllowUserToAddRows = false;
            grade.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            grade.CellClick += Grade_CellClick;
            this.Controls.Add(grade);

            CarregarGrade();
        }

        // joga os medicos do banco pra tabela
        private void CarregarGrade()
        {
            grade.DataSource = null;
            grade.DataSource = _repositorio.ListarTodos();
        }

        // ao clicar numa linha preenche os campos
        private void Grade_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            Medico m = grade.Rows[e.RowIndex].DataBoundItem as Medico;
            if (m == null) return;

            _idSelecionado = m.Id;
            txtNome.Text = m.Nome;
            txtCrm.Text = m.CRM;
            txtEspecialidade.Text = m.Especialidade;
            txtTelefone.Text = m.Telefone;
        }

        // salva o medico
        private void Salvar(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNome.Text) || string.IsNullOrWhiteSpace(txtCrm.Text))
            {
                MessageBox.Show("Preenche pelo menos o nome e o CRM.");
                return;
            }

            Medico m = new Medico();
            m.Id = _idSelecionado;
            m.Nome = txtNome.Text;
            m.CRM = txtCrm.Text;
            m.Especialidade = txtEspecialidade.Text;
            m.Telefone = txtTelefone.Text;

            try
            {
                if (_idSelecionado == 0)
                    _repositorio.Inserir(m);
                else
                    _repositorio.Atualizar(m);

                MessageBox.Show("Salvo com sucesso!");
                Limpar();
                CarregarGrade();
            }
            catch (Exception erro)
            {
                MessageBox.Show("Deu erro ao salvar (o CRM nao pode repetir): " + erro.Message);
            }
        }

        // exclui o medico
        private void Excluir(object sender, EventArgs e)
        {
            if (_idSelecionado == 0)
            {
                MessageBox.Show("Clica num medico da tabela primeiro.");
                return;
            }

            var resposta = MessageBox.Show("Tem certeza que quer excluir?", "Confirmar", MessageBoxButtons.YesNo);
            if (resposta == DialogResult.Yes)
            {
                try
                {
                    _repositorio.Remover(_idSelecionado);
                    Limpar();
                    CarregarGrade();
                }
                catch (Exception erro)
                {
                    MessageBox.Show("Nao deu pra excluir (talvez tenha consulta com esse medico): " + erro.Message);
                }
            }
        }

        // limpa os campos
        private void Limpar()
        {
            _idSelecionado = 0;
            txtNome.Text = "";
            txtCrm.Text = "";
            txtEspecialidade.Text = "";
            txtTelefone.Text = "";
        }
    }
}
