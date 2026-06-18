using System;
using System.Drawing;
using System.Windows.Forms;
using ClinicaMedica.Models;
using ClinicaMedica.Repositories;

namespace ClinicaMedica.Forms
{
    // tela pra cadastrar, editar e excluir consultorio
    public class FormConsultorio : Form
    {
        private ConsultorioRepository _repositorio = new ConsultorioRepository();
        private int _idSelecionado = 0;

        private TextBox txtNome;
        private TextBox txtEndereco;
        private TextBox txtNumero;
        private TextBox txtComplemento;
        private DataGridView grade;

        public FormConsultorio()
        {
            this.Text = "Consultorios";
            this.Size = new Size(820, 540);
            this.StartPosition = FormStartPosition.CenterScreen;

            // campo nome
            Label lblNome = new Label();
            lblNome.Text = "Nome (ex: Sala 1):";
            lblNome.Location = new Point(20, 20);
            lblNome.AutoSize = true;
            this.Controls.Add(lblNome);
            txtNome = new TextBox();
            txtNome.Location = new Point(20, 40);
            txtNome.Size = new Size(200, 25);
            this.Controls.Add(txtNome);

            // campo endereco
            Label lblEnd = new Label();
            lblEnd.Text = "Endereco:";
            lblEnd.Location = new Point(250, 20);
            lblEnd.AutoSize = true;
            this.Controls.Add(lblEnd);
            txtEndereco = new TextBox();
            txtEndereco.Location = new Point(250, 40);
            txtEndereco.Size = new Size(250, 25);
            this.Controls.Add(txtEndereco);

            // campo numero
            Label lblNum = new Label();
            lblNum.Text = "Numero:";
            lblNum.Location = new Point(520, 20);
            lblNum.AutoSize = true;
            this.Controls.Add(lblNum);
            txtNumero = new TextBox();
            txtNumero.Location = new Point(520, 40);
            txtNumero.Size = new Size(100, 25);
            this.Controls.Add(txtNumero);

            // campo complemento
            Label lblComp = new Label();
            lblComp.Text = "Complemento:";
            lblComp.Location = new Point(20, 75);
            lblComp.AutoSize = true;
            this.Controls.Add(lblComp);
            txtComplemento = new TextBox();
            txtComplemento.Location = new Point(20, 95);
            txtComplemento.Size = new Size(250, 25);
            this.Controls.Add(txtComplemento);

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

            // tabela dos consultorios
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

        // joga os consultorios do banco pra tabela
        private void CarregarGrade()
        {
            grade.DataSource = null;
            grade.DataSource = _repositorio.ListarTodos();
        }

        // ao clicar numa linha preenche os campos
        private void Grade_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            Consultorio c = grade.Rows[e.RowIndex].DataBoundItem as Consultorio;
            if (c == null) return;

            _idSelecionado = c.Id;
            txtNome.Text = c.Nome;
            txtEndereco.Text = c.Endereco;
            txtNumero.Text = c.Numero;
            txtComplemento.Text = c.Complemento;
        }

        // salva o consultorio
        private void Salvar(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNome.Text))
            {
                MessageBox.Show("Preenche o nome do consultorio.");
                return;
            }

            Consultorio c = new Consultorio();
            c.Id = _idSelecionado;
            c.Nome = txtNome.Text;
            c.Endereco = txtEndereco.Text;
            c.Numero = txtNumero.Text;
            c.Complemento = txtComplemento.Text;

            try
            {
                if (_idSelecionado == 0)
                    _repositorio.Inserir(c);
                else
                    _repositorio.Atualizar(c);

                MessageBox.Show("Salvo com sucesso!");
                Limpar();
                CarregarGrade();
            }
            catch (Exception erro)
            {
                MessageBox.Show("Deu erro ao salvar: " + erro.Message);
            }
        }

        // exclui o consultorio
        private void Excluir(object sender, EventArgs e)
        {
            if (_idSelecionado == 0)
            {
                MessageBox.Show("Clica num consultorio da tabela primeiro.");
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
                    MessageBox.Show("Nao deu pra excluir (talvez tenha consulta nesse consultorio): " + erro.Message);
                }
            }
        }

        // limpa os campos
        private void Limpar()
        {
            _idSelecionado = 0;
            txtNome.Text = "";
            txtEndereco.Text = "";
            txtNumero.Text = "";
            txtComplemento.Text = "";
        }
    }
}
