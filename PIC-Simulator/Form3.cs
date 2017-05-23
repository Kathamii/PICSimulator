using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PIC_Simulator
{
    public partial class Help : Form
    {
        //öffnen der pdf
        public Help()
        {
            InitializeComponent();
            axAcroPDF1.src = "C:/Users/z003jy6p.AD001/Desktop/Signale_und_Systeme.pdf";
        }


    }
}
