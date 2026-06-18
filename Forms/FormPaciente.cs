using System;
using System.Drawing;
using System.Windows.Forms;
using ClinicaMedica.Models;
using ClinicaMedica.Repositories;

namespace ClinicaMedica.Forms
{
    // tela pra cadastrar, editar e excluir paciente
    public class FormPaciente : Form
    {
        private PacienteRepository _repositorio = new PacienteRepository();
        // guarda o id do paciente que esta selecionado. 0 quer dizer cadastro novo
        private int _idSelecionado = 0;

        private TextBox txtNome;
        private TextBox txtCpf;
        private DateTimePicker dtpNascimento;
        private TextBox txtEndereco;
        private TextBox txtTelefone;
        private DataGridView grade;

        public FormPaciente()
        {
            // config da janela
            this.Text = "Pacientes";
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

            // campo cpf
            Label lblCpf = new Label();
            lblCpf.Text = "CPF (so numeros):";
            lblCpf.Location = new Point(300, 20);
            lblCpf.AutoSize = true;
            this.Controls.Add(lblCpf);
            txtCpf = new TextBox();
            txtCpf.Location = new Point(300, 40);
            txtCpf.Size = new Size(150, 25);
            this.Controls.Add(txtCpf);

            // campo data de nascimento
            Label lblNasc = new Label();
            lblNasc.Text = "Nascimento:";
            lblNasc.Location = new Point(480, 20);
            lblNasc.AutoSize = true;
            this.Controls.Add(lblNasc);
            dtpNascimento = new DateTimePicker();
            dtpNascimento.Format = DateTimePickerFormat.Short;
            dtpNascimento.Location = new Point(480, 40);
            dtpNascimento.Size = new Size(150, 25);
            this.Controls.Add(dtpNascimento);

            // campo endereco
            Label lblEnd = new Label();
            lblEnd.Text = "Endereco:";
            lblEnd.Location = new Point(20, 75);
            lblEnd.AutoSize = true;
            this.Controls.Add(lblEnd);
            txtEndereco = new TextBox();
            txtEndereco.Location = new Point(20, 95);
            txtEndereco.Size = new Size(250, 25);
            this.Controls.Add(txtEndereco);

            // campo telefone
            Label lblTel = new Label();
            lblTel.Text = "Telefone:";
            lblTel.Location = new Point(300, 75);
            lblTel.AutoSize = true;
            this.Controls.Add(lblTel);
            txtTelefone = new TextBox();
            txtTelefone.Location = new Point(300, 95);
            txtTelefone.Size = new Size(150, 25);
            this.Controls.Add(txtTelefone);

            // botao salvar
            Button btnSalvar = new Button();
            btnSalvar.Text = "Salvar";
            btnSalvar.Location = new Point(20, 140);
            btnSalvar.Size = new Size(120, 35);
            btnSalvar.Click += Salvar;
            this.Controls.Add(btnSalvar);

            // botao limpar (pra cadastrar um novo)
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

            // tabela que mostra os pacientes
            grade = new DataGridView();
            grade.Location = new Point(20, 200);
            grade.Size = new Size(760, 280);
            grade.ReadOnly = true;
            grade.AllowUserToAddRows = false;
            grade.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            grade.CellClick += Grade_CellClick;
            this.Controls.Add(grade);

            // ja carrega a tabela quando abre a tela
            CarregarGrade();
        }

        // joga os pacientes do banco pra dentro da tabela
        private void CarregarGrade()
        {
            grade.DataSource = null;
            grade.DataSource = _repositorio.ListarTodos();
        }

        // quando clica numa linha, joga os dados nos campos pra poder editar
        private void Grade_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            Paciente p = grade.Rows[e.RowIndex].DataBoundItem as Paciente;
            if (p == null) return;

            _idSelecionado = p.Id;
            txtNome.Text = p.Nome;
            txtCpf.Text = p.CPF;
            // se a data tiver zerada usa hoje pra nao dar erro no calendario
            dtpNascimento.Value = (p.DataNascimento == DateTime.MinValue) ? DateTime.Now : p.DataNascimento;
            txtEndereco.Text = p.Endereco;
            txtTelefone.Text = p.Telefone;
        }

        // salva (se for novo insere, se ja existe atualiza)
        private void Salvar(object sender, EventArgs e)
        {
            // nome e obrigatorio
            if (string.IsNullOrWhiteSpace(txtNome.Text))
            {
                MessageBox.Show("Preenche o nome do paciente.");
                return;
            }

            Paciente p = new Paciente();
            p.Id = _idSelecionado;
            p.Nome = txtNome.Text;
            p.CPF = txtCpf.Text;
            p.DataNascimento = dtpNascimento.Value;
            p.Endereco = txtEndereco.Text;
            p.Telefone = txtTelefone.Text;

            try
            {
                if (_idSelecionado == 0)
                    _repositorio.Inserir(p); // cadastro novo
                else
                    _repositorio.Atualizar(p); // editando um que ja existe

                MessageBox.Show("Salvo com sucesso!");
                Limpar();
                CarregarGrade();
            }
            catch (Exception erro)
            {
                MessageBox.Show("Deu erro ao salvar: " + erro.Message);
            }
        }

        // apaga o paciente selecionado
        private void Excluir(object sender, EventArgs e)
        {
            if (_idSelecionado == 0)
            {
                MessageBox.Show("Clica num paciente da tabela primeiro.");
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
                    MessageBox.Show("Nao deu pra excluir (talvez tenha consulta ligada a esse paciente): " + erro.Message);
                }
            }
        }

        // limpa os campos e volta pro modo de cadastro novo
        private void Limpar()
        {
            _idSelecionado = 0;
            txtNome.Text = "";
            txtCpf.Text = "";
            dtpNascimento.Value = DateTime.Now;
            txtEndereco.Text = "";
            txtTelefone.Text = "";
        }
    }
}
