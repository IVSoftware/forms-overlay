using System;
using System.Drawing;
using System.Windows.Forms;

namespace forms_overlay
{
    public partial class OverlayForm : Form
    {
        public OverlayForm(Form mainForm)
        {
            InitializeComponent();
            FormBorderStyle= FormBorderStyle.None;
            BackColor= Color.Azure;
            ShowInTaskbar= false;
            _checkBoxShowOverlay.CheckedChanged += (sender, e) =>
            {
                IsOverlayVisible = _checkBoxShowOverlay.Checked;
            };
            HandleCreated += (sender, e) =>
            {
                BeginInvoke(new Action(() =>onOverlayVisibleChanged())); // Init
            };
            _mainForm = mainForm;
            _saveWinState = mainForm.WindowState;
        }

        private readonly Form _mainForm;

        public bool IsOverlayVisible
        {
            get => _isOverlayVisible;
            private set
            {
                if (!Equals(_isOverlayVisible, value))
                {
                    _isOverlayVisible = value;
                    onOverlayVisibleChanged();
                }
            }
        }
        bool _isOverlayVisible = false;

        private void onOverlayVisibleChanged()
        {
            if (_isOverlayVisible)
            {
                Size = _mainForm.Size;
                Location = _mainForm.Location;
                _saveWinState= _mainForm.WindowState;
                _mainForm.WindowState = FormWindowState.Minimized;
            }
            else
            {
                Size = new Size(
                    _checkBoxShowOverlay.Width + 10,
                    _checkBoxShowOverlay.Height + 10);
                Location = new Point(
                    Screen.FromControl(this).Bounds.X,
                    Screen.FromControl(this).Bounds.Y);
                _mainForm.WindowState = _saveWinState;
            }
        }
        FormWindowState _saveWinState;

        protected override void OnDeactivate(EventArgs e)
        {
            base.OnDeactivate(e);
            _checkBoxShowOverlay.Checked = false;
            BeginInvoke(new Action(() => _mainForm?.Activate()));
        }
    }
}
