using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using ClinicaMedica.Models;
using ClinicaMedica.Repositories;

namespace ClinicaMedica.Forms
{
    // tela principal do sistema: marcar uma consulta
    // o usuario escolhe especialidade, medico, dia, horario, consultorio e paciente
    public class FormAgendar : Form
    {
        private MedicoRepository _medicoRepo = new MedicoRepository();
        private PacienteRepository _pacienteRepo = new PacienteRepository();
        private ConsultorioRepository _consultorioRepo = new ConsultorioRepository();
        private ConsultaRepository _consultaRepo = new ConsultaRepository();

        private ComboBox cboEspecialidade;
        private ComboBox cboMedico;
        private ComboBox cboPaciente;
        private ComboBox cboConsultorio;
        private DateTimePicker dtpData;
        private ComboBox cboHora;

        public FormAgendar()
        {
            this.Text = "Agendar Consulta";
            this.Size = new Size(480, 480);
            this.StartPosition = FormStartPosition.CenterScreen;

            // especialidade
            Label lblEsp = new Label();
            lblEsp.Text = "Especialidade:";
            lblEsp.Location = new Point(30, 30);
            lblEsp.AutoSize = true;
            this.Controls.Add(lblEsp);
            cboEspecialidade = new ComboBox();
            cboEspecialidade.Location = new Point(180, 27);
            cboEspecialidade.Size = new Size(250, 25);
            cboEspecialidade.DropDownStyle = ComboBoxStyle.DropDownList;
            // quando troca a especialidade recarrega os medicos
            cboEspecialidade.SelectedIndexChanged += (sender, e) => CarregarMedicos();
            this.Controls.Add(cboEspecialidade);

            // medico
            Label lblMed = new Label();
            lblMed.Text = "Medico:";
            lblMed.Location = new Point(30, 75);
            lblMed.AutoSize = true;
            this.Controls.Add(lblMed);
            cboMedico = new ComboBox();
            cboMedico.Location = new Point(180, 72);
            cboMedico.Size = new Size(250, 25);
            cboMedico.DropDownStyle = ComboBoxStyle.DropDownList;
            this.Controls.Add(cboMedico);

            // paciente
            Label lblPac = new Label();
            lblPac.Text = "Paciente:";
            lblPac.Location = new Point(30, 120);
            lblPac.AutoSize = true;
            this.Controls.Add(lblPac);
            cboPaciente = new ComboBox();
            cboPaciente.Location = new Point(180, 117);
            cboPaciente.Size = new Size(250, 25);
            cboPaciente.DropDownStyle = ComboBoxStyle.DropDownList;
            this.Controls.Add(cboPaciente);

            // consultorio
            Label lblCon = new Label();
            lblCon.Text = "Consultorio:";
            lblCon.Location = new Point(30, 165);
            lblCon.AutoSize = true;
            this.Controls.Add(lblCon);
            cboConsultorio = new ComboBox();
            cboConsultorio.Location = new Point(180, 162);
            cboConsultorio.Size = new Size(250, 25);
            cboConsultorio.DropDownStyle = ComboBoxStyle.DropDownList;
            this.Controls.Add(cboConsultorio);

            // dia
            Label lblData = new Label();
            lblData.Text = "Dia:";
            lblData.Location = new Point(30, 210);
            lblData.AutoSize = true;
            this.Controls.Add(lblData);
            dtpData = new DateTimePicker();
            dtpData.Format = DateTimePickerFormat.Short;
            dtpData.Location = new Point(180, 207);
            dtpData.Size = new Size(150, 25);
            this.Controls.Add(dtpData);

            // horario
            Label lblHora = new Label();
            lblHora.Text = "Horario:";
            lblHora.Location = new Point(30, 255);
            lblHora.AutoSize = true;
            this.Controls.Add(lblHora);
            cboHora = new ComboBox();
            cboHora.Location = new Point(180, 252);
            cboHora.Size = new Size(150, 25);
            cboHora.DropDownStyle = ComboBoxStyle.DropDownList;
            this.Controls.Add(cboHora);

            // botao de agendar
            Button btnAgendar = new Button();
            btnAgendar.Text = "Agendar";
            btnAgendar.Location = new Point(180, 310);
            btnAgendar.Size = new Size(150, 45);
            btnAgendar.Click += Agendar;
            this.Controls.Add(btnAgendar);

            // carrega tudo que vai aparecer nos combos
            CarregarCombos();
        }

        // preenche os combos com os dados do banco
        private void CarregarCombos()
        {
            // especialidades
            cboEspecialidade.DataSource = _medicoRepo.ListarEspecialidades();

            // pacientes
            cboPaciente.DataSource = _pacienteRepo.ListarTodos();
            cboPaciente.DisplayMember = "Nome";
            cboPaciente.ValueMember = "Id";

            // consultorios
            cboConsultorio.DataSource = _consultorioRepo.ListarTodos();
            cboConsultorio.DisplayMember = "Nome";
            cboConsultorio.ValueMember = "Id";

            // horarios fixos de 30 em 30 min, das 8h as 17h30
            for (int hora = 8; hora <= 17; hora++)
            {
                cboHora.Items.Add(hora.ToString("00") + ":00");
                cboHora.Items.Add(hora.ToString("00") + ":30");
            }
            if (cboHora.Items.Count > 0)
                cboHora.SelectedIndex = 0;

            // ja carrega os medicos da primeira especialidade
            CarregarMedicos();
        }

        // recarrega os medicos de acordo com a especialidade escolhida
        private void CarregarMedicos()
        {
            // se nao tiver especialidade selecionada nao faz nada
            if (cboEspecialidade.SelectedItem == null)
            {
                cboMedico.DataSource = null;
                return;
            }

            string especialidade = cboEspecialidade.SelectedItem.ToString();
            cboMedico.DataSource = _medicoRepo.ListarPorEspecialidade(especialidade);
            cboMedico.DisplayMember = "Nome";
            cboMedico.ValueMember = "Id";
        }

        // faz o agendamento
        private void Agendar(object sender, EventArgs e)
        {
            // ve se tem medico, paciente e consultorio escolhidos
            if (cboMedico.SelectedValue == null)
            {
                MessageBox.Show("Escolhe um medico.");
                return;
            }
            if (cboPaciente.SelectedValue == null)
            {
                MessageBox.Show("Escolhe um paciente.");
                return;
            }
            if (cboConsultorio.SelectedValue == null)
            {
                MessageBox.Show("Escolhe um consultorio.");
                return;
            }
            if (cboHora.SelectedItem == null)
            {
                MessageBox.Show("Escolhe um horario.");
                return;
            }

            int idMedico = Convert.ToInt32(cboMedico.SelectedValue);
            int idPaciente = Convert.ToInt32(cboPaciente.SelectedValue);
            int idConsultorio = Convert.ToInt32(cboConsultorio.SelectedValue);
            DateTime dia = dtpData.Value.Date;
            // transforma o texto "08:00" em horario de verdade
            TimeOnly hora = TimeOnly.ParseExact(cboHora.SelectedItem.ToString(), "HH:mm", CultureInfo.InvariantCulture);

            // antes de marcar ve se o medico ja nao ta ocupado nesse dia e horario
            if (_consultaRepo.ExisteConflito(idMedico, dia, hora))
            {
                MessageBox.Show("Esse medico ja tem consulta nesse dia e horario. Escolhe outro horario.");
                return;
            }

            // monta a consulta e salva
            Consulta consulta = new Consulta();
            consulta.MedicoId = idMedico;
            consulta.PacienteId = idPaciente;
            consulta.ConsultorioId = idConsultorio;
            consulta.DataConsulta = dia;
            consulta.HoraConsulta = hora;
            consulta.Status = StatusConsulta.Agendada;

            try
            {
                _consultaRepo.Inserir(consulta);
                MessageBox.Show("Consulta agendada com sucesso!");
            }
            catch (Exception erro)
            {
                MessageBox.Show("Deu erro ao agendar: " + erro.Message);
            }
        }
    }
}
