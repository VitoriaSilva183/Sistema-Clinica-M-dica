using System;
using System.Windows.Forms;
using ClinicaMedica.Forms;

namespace ClinicaMedica
{
    // aqui que o programa comeca
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            // liga as config padrao do windows forms
            ApplicationConfiguration.Initialize();
            // abre a tela do menu principal
            Application.Run(new FormPrincipal());
        }
    }
}
