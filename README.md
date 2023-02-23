As I understand it, the app's initial state might consist of a visible `MainForm` where the overlay button is:

 > [...] minimized(by controlling it's size) to the screens top.

[![overlay-minimized][1]][1]

***
Clicking [Overlay] causes app to enter a state where the overlay is expanded and the main form "might" be:

> [...] minimized to taskbar or system tray.

 [![show-overlay-minimize-main][2]][2]

***
I believe the intention of your question about **this.Deactivated** is that any loss of focus results in a revert to the original state and the overlay should minimize if:
- [Overlay] button is clicked a second time.
- Mouse click occurs outside the client rectangle of overlay form.

My first suggestion is to make the `OverlayForm` autonomous by instantiating it with a reference to the main form. At the same time, I would avoid using the `this` pointer in the `Show()` method because if main form is `Owner` of overlay form then when main form minimizes it takes overlay form down with it! So avoid that. 

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

***
**OverlayForm**

In this scheme of things the messaging is simplified because `OverlayForm` can act upon `MainForm` directly. All the `Deactivate` event has to do is uncheck the `Overlay` button.

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

            // Init and give focus back to main form.
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


  [1]: https://i.stack.imgur.com/PqcMU.png
  [2]: https://i.stack.imgur.com/2Sqqh.png