using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SimpleStatusIcons {
    public class IconDisplayContext : ApplicationContext {
        //Component declarations
        private ContextMenuStrip TrayIconContextMenu;
        private ToolStripMenuItem CloseMenuItem;

        private static object SyncRootTrayIcon = new object();
        private static NotifyIcon TrayIcon {
            get {
                return _TrayIcon;
            }
            set {
                if (value == _TrayIcon) {
                    return;
                }
                lock (SyncRootTrayIcon) {
                    if (value != _TrayIcon) {
                        _TrayIcon = value;
                    }
                }
            }
        }
        private static NotifyIcon _TrayIcon = new NotifyIcon();

        private static object SyncRootTask = new object();
        private static Task RenderTask {
            get {
                return _RenderTask;
            }
            set {
                if (value == _RenderTask) {
                    return;
                }
                lock (SyncRootTask) {
                    if (value != _RenderTask) {
                        _RenderTask = value;
                    }
                }
            }
        }
        private static Task _RenderTask = null;


        public IconDisplayContext() {
            Application.ApplicationExit += new EventHandler(this.OnApplicationExit);
        }

        public void SetIcon(Models.IconConfig iconConfig) {
            TrayIcon?.Dispose();

            RenderTask = Task.Run(() => {
                TrayIcon = new NotifyIcon();
                TrayIcon.Text = iconConfig.HoverText;
                TrayIcon.Icon = new Icon(iconConfig.Path);
                TrayIcon.Visible = true;

                Application.EnableVisualStyles();
                Application.Run(this);
            });
        }

        private void OnApplicationExit(object sender, EventArgs e) {
            //Cleanup so that the icon will be removed when the application is closed
            TrayIcon.Visible = false;
        }
    }
}