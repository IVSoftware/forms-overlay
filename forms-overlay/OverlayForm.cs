using System;
using System.Drawing;
using System.Windows.Forms;

namespace forms_overlay
{
    public partial class OverlayForm : Form
    {
        public OverlayForm(Form mainForm)
        {
            _mainForm = mainForm;
            _saveWinState = mainForm.WindowState;

            InitializeComponent();
            FormBorderStyle= FormBorderStyle.None;
            BackColor= Color.Azure;
            ShowInTaskbar= false;
            _checkBoxShowOverlay.Appearance = Appearance.Button;

            _checkBoxShowOverlay.CheckedChanged += onCheckedChangedShowOverlay;
            Deactivate += (sender, e) => _checkBoxShowOverlay.Checked = false;

            // Init
            _mainForm.HandleCreated += (sender, e) =>
                BeginInvoke(new Action(() => onCheckedChangedShowOverlay(_checkBoxShowOverlay, EventArgs.Empty)));
        }
        private readonly Form _mainForm;
        private void onCheckedChangedShowOverlay(object sender, EventArgs e)
        {
            if (_checkBoxShowOverlay.Checked)
            {
                Size = _mainForm.Size;
                Location = _mainForm.Location;
                _saveWinState = _mainForm.WindowState;

                BeginInvoke(new Action(() =>
                {
                    _mainForm.WindowState = FormWindowState.Minimized;
                }));
            }
            else
            {
                Size = new Size(
                    _checkBoxShowOverlay.Width + 10,
                    _checkBoxShowOverlay.Height + 10);
                Screen screen = Screen.FromControl(this);
                Location = new Point(
                    screen.Bounds.X,
                    screen.Bounds.Y);

                BeginInvoke(new Action(() =>
                {
                    _mainForm.WindowState = _saveWinState;
                    _mainForm?.Activate();
                }));
            }
        }
        FormWindowState _saveWinState;
    }
}
