using System;
using System.Drawing;
using System.Windows.Forms;

namespace ClinicaMedica.Forms
{
    // tela do menu, só tem botoes pra abrir as outras telas
    public class FormPrincipal : Form
    {
        public FormPrincipal()
        {
            // config da janela
            this.Text = "Clinica Medica";
            this.Size = new Size(420, 420);
            this.StartPosition = FormStartPosition.CenterScreen;

            // titulo la em cima
            Label titulo = new Label();
            titulo.Text = "Clinica Medica";
            titulo.Font = new Font("Arial", 18, FontStyle.Bold);
            titulo.AutoSize = true;
            titulo.Location = new Point(110, 20);
            this.Controls.Add(titulo);

            // botao de pacientes
            Button btnPacientes = new Button();
            btnPacientes.Text = "Pacientes";
            btnPacientes.Size = new Size(250, 40);
            btnPacientes.Location = new Point(80, 80);
            btnPacientes.Click += (sender, e) => new FormPaciente().ShowDialog();
            this.Controls.Add(btnPacientes);

            // botao de medicos
            Button btnMedicos = new Button();
            btnMedicos.Text = "Medicos";
            btnMedicos.Size = new Size(250, 40);
            btnMedicos.Location = new Point(80, 130);
            btnMedicos.Click += (sender, e) => new FormMedico().ShowDialog();
            this.Controls.Add(btnMedicos);

            // botao de consultorios
            Button btnConsultorios = new Button();
            btnConsultorios.Text = "Consultorios";
            btnConsultorios.Size = new Size(250, 40);
            btnConsultorios.Location = new Point(80, 180);
            btnConsultorios.Click += (sender, e) => new FormConsultorio().ShowDialog();
            this.Controls.Add(btnConsultorios);

            // botao pra agendar consulta 
            Button btnAgendar = new Button();
            btnAgendar.Text = "Agendar Consulta";
            btnAgendar.Size = new Size(250, 40);
            btnAgendar.Location = new Point(80, 230);
            btnAgendar.Click += (sender, e) => new FormAgendar().ShowDialog();
            this.Controls.Add(btnAgendar);

            // botao pra ver as consultas marcadas
            Button btnConsultas = new Button();
            btnConsultas.Text = "Ver Consultas";
            btnConsultas.Size = new Size(250, 40);
            btnConsultas.Location = new Point(80, 280);
            btnConsultas.Click += (sender, e) => new FormConsultas().ShowDialog();
            this.Controls.Add(btnConsultas);
        }
    }
}
