using System;
using System.Drawing;
using System.Windows.Forms;
using ClinicaMedica.Models;
using ClinicaMedica.Repositories;

namespace ClinicaMedica.Forms
{
    // tela que mostra as consultas marcadas e deixa cancelar ou excluir
    public class FormConsultas : Form
    {
        private ConsultaRepository _repositorio = new ConsultaRepository();
        private int _idSelecionado = 0;

        private DataGridView grade;

        public FormConsultas()
        {
            this.Text = "Consultas";
            this.Size = new Size(900, 540);
            this.StartPosition = FormStartPosition.CenterScreen;

            // botao atualizar lista
            Button btnAtualizar = new Button();
            btnAtualizar.Text = "Atualizar lista";
            btnAtualizar.Location = new Point(20, 20);
            btnAtualizar.Size = new Size(140, 35);
            btnAtualizar.Click += (sender, e) => CarregarGrade();
            this.Controls.Add(btnAtualizar);

            // botao cancelar consulta (muda o status, nao apaga)
            Button btnCancelar = new Button();
            btnCancelar.Text = "Cancelar consulta";
            btnCancelar.Location = new Point(170, 20);
            btnCancelar.Size = new Size(150, 35);
            btnCancelar.Click += Cancelar;
            this.Controls.Add(btnCancelar);

            // botao excluir de vez
            Button btnExcluir = new Button();
            btnExcluir.Text = "Excluir";
            btnExcluir.Location = new Point(330, 20);
            btnExcluir.Size = new Size(120, 35);
            btnExcluir.Click += Excluir;
            this.Controls.Add(btnExcluir);

            // tabela das consultas
            grade = new DataGridView();
            grade.Location = new Point(20, 70);
            grade.Size = new Size(850, 410);
            grade.ReadOnly = true;
            grade.AllowUserToAddRows = false;
            grade.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            grade.CellClick += Grade_CellClick;
            this.Controls.Add(grade);

            CarregarGrade();
        }

        // joga as consultas (ja com os nomes) na tabela
        private void CarregarGrade()
        {
            _idSelecionado = 0;
            grade.DataSource = null;
            grade.DataSource = _repositorio.ListarParaGrade();
        }

        // ao clicar pega o id da coluna "Id"
        private void Grade_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            _idSelecionado = Convert.ToInt32(grade.Rows[e.RowIndex].Cells["Id"].Value);
        }

        // cancela a consulta selecionada (muda status pra Cancelada)
        private void Cancelar(object sender, EventArgs e)
        {
            if (_idSelecionado == 0)
            {
                MessageBox.Show("Clica numa consulta da tabela primeiro.");
                return;
            }

            try
            {
                _repositorio.AtualizarStatus(_idSelecionado, StatusConsulta.Cancelada);
                MessageBox.Show("Consulta cancelada.");
                CarregarGrade();
            }
            catch (Exception erro)
            {
                MessageBox.Show("Deu erro: " + erro.Message);
            }
        }

        // apaga a consulta de vez
        private void Excluir(object sender, EventArgs e)
        {
            if (_idSelecionado == 0)
            {
                MessageBox.Show("Clica numa consulta da tabela primeiro.");
                return;
            }

            var resposta = MessageBox.Show("Tem certeza que quer excluir?", "Confirmar", MessageBoxButtons.YesNo);
            if (resposta == DialogResult.Yes)
            {
                try
                {
                    _repositorio.Remover(_idSelecionado);
                    CarregarGrade();
                }
                catch (Exception erro)
                {
                    MessageBox.Show("Deu erro ao excluir: " + erro.Message);
                }
            }
        }
    }
}
