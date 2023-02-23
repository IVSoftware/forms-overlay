using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace forms_overlay
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            _overlayForm = new OverlayForm(this);
            _overlayForm.Show(); // Avoid setting owner with 'this'.
        }

        private readonly OverlayForm _overlayForm;
    }
}
