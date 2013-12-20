using System;
using System.IO;
using System.Diagnostics;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SQLite;

namespace SkypeHistoryEnc
{
    public partial class frmLogin : Form
    {
        public string Folder
        {
            get
            {
                return Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData).TrimEnd('\\') + "\\Skype\\";
            }
        }

        public string AppDB
        {
            get
            {
                return System.AppDomain.CurrentDomain.BaseDirectory.TrimEnd('\\') + "\\db.sl";
            }
        }

        public frmLogin()
        {
            InitializeComponent();
        }

        private void login_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;

            SkypeDB.SkypeDBfile = Folder + username.Text.Trim() + "\\main.db";

            if (!File.Exists(SkypeDB.SkypeDBfile))
            {
                MessageBox.Show("Skype profile doesn't exist");
                return;
            }

            if (!File.Exists(AppDB))
            {
                DBs.App.ExecuteQuery("create table chats (cid integer not null primary key autoincrement, name text not null, skid text not null, stamp text not null)");
                DBs.App.ExecuteQuery("create table msgs (mid integer not null primary key autoincrement, skid text not null, msg text not null, stamp text not null, author text not null)");
            }

            try
            {
                var acc = DBs.Skype.GetDataSource("select * from accounts");
                if (acc == null)
                {
                    MessageBox.Show("No Skype accounts");
                    return;
                }
            }
            catch (Exception ex)
            {
                KillProcess();
            }

            Crypt.Key = password.Text;
            var test = DBs.App.GetDataSource("select stamp from chats order by cid desc limit 1");
            if (test.Rows.Count > 0)
            {
                try
                {
                    var stamp = Crypt.DecDate((string)test.Rows[0]["stamp"]);
                }
                catch
                {
                    MessageBox.Show("Password is incorrect");
                    return;
                }
            }

            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }

        private void KillProcess()
        {
            try
            {
                int tries = 0;
                while (Process.GetProcessesByName("Skype").Length > 0 && tries < 100)
                {
                    tries++;
                    foreach (Process proc in Process.GetProcessesByName("Skype"))
                        try
                        {
                            proc.Kill();
                            System.Threading.Thread.Sleep(500);
                        }
                        catch { }
                }
                if (tries == 100)
                    MessageBox.Show("Couldn't kill Skype.exe");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void password_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                login_Click(null, null);
        }

    }
}
