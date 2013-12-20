using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace SkypeHistoryEnc
{
    
    public partial class frmMain : Form
    {
        bool loaded = false;
        public List<unenc> Unencd = new List<unenc>();
        public List<chat> Chats = new List<chat>();
        public string prevLoadedSkid = null;
        public List<string> msglines = new List<string>();

        public frmMain()
        {
            InitializeComponent();
        }

        private void frmMain_Activated(object sender, EventArgs e)
        {
            if (!loaded)
            {
                loaded = true;
                frmLogin login = new frmLogin();
                if (login.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    LoadUnenc();
                    LoadEnc();
                    login.Dispose();
                }
                else
                    Application.Exit();
            }
        }

        private void LoadUnenc()
        {
            string selconv = lstUnenc.SelectedRows.Count > 0 ? ((unenc)lstUnenc.SelectedRows[0].DataBoundItem).convo_id : null;

            string filter = txtglobal.Text.Trim() == "" ? "m.timestamp > @val" : "m.body_xml like @val";

            object val = txtglobal.Text.Trim() == "" ? (object)Fn.DateTimeToUnixTimestamp(DateTime.Now.AddMonths(-6)) : "%" + txtglobal.Text.Trim() + "%";

            var table = DBs.Skype.GetDataSource(@"select MIN(m.timestamp) as begin, MAX(m.timestamp) as end, m.convo_id, c.displayname from messages m
                                                    inner join conversations c on m.convo_id = c.id
                                                    where " + filter + @" and (m.chatmsg_type is null or m.chatmsg_type = 3)
                                                    group by m.convo_id, c.displayname 
                                                    order by MAX(m.timestamp) desc", "@val", val);

            Unencd = JsonConvert.DeserializeObject<List<unenc>>(JsonConvert.SerializeObject(table));
            lstUnenc.DataSource = Unencd;

            if (selconv != null && Unencd.Where(i => i.convo_id == selconv).Count() > 0)
                foreach (DataGridViewRow row in lstUnenc.Rows)
                    if (((unenc)row.DataBoundItem).convo_id == selconv)
                        row.Selected = true;
        }

        private void LoadEnc()
        {
            string selconv = lstEnc.SelectedRows.Count > 0 ? ((chat)lstEnc.SelectedRows[0].DataBoundItem).skid : null;

            var table = DBs.App.GetDataSource(@"select * from chats order by cid desc");

            Chats = JsonConvert.DeserializeObject<List<chat>>(JsonConvert.SerializeObject(table));
            Chats = Chats.OrderByDescending(c => c.Last).ToList();

            lstEnc.DataSource = Chats;

            if (selconv != null && Chats.Where(i => i.skid == selconv).Count() > 0)
                foreach (DataGridViewRow row in lstEnc.Rows)
                    if (((chat)row.DataBoundItem).skid == selconv)
                        row.Selected = true;
            if (selconv != null && lstEnc.SelectedRows.Count > 0)
                LoadMsgsEnc(selconv);
        }

        private void LoadMsgsEnc(string skid)
        {
            if (prevLoadedSkid == skid)
                return;

            var table = DBs.App.GetDataSource(@"select * from msgs where skid = @skid order by mid desc", "@skid", skid);

            txtmsgs.Text = "";
            msglines = new List<string>();

            foreach (DataRow msg in table.Rows)
                msglines.Add(Crypt.DecDate((string)msg["stamp"]).ToString("yyyy-MM-dd HH:mm") + " "
                    + Crypt.DecString((string)msg["author"]) + ": "
                    + System.Web.HttpUtility.HtmlDecode(Crypt.DecString((string)msg["msg"])));

            ckbfiltermsg_CheckedChanged(null, null);

            prevLoadedSkid = skid;
        }

        private void LoadMsgsUnenc(string skid)
        {
            if (prevLoadedSkid == skid)
                return;

            var table = DBs.Skype.GetDataSource(@"select timestamp, body_xml, from_dispname from messages 
                                                where convo_id = @skid and (chatmsg_type is null or chatmsg_type = 3) 
                                                order by timestamp desc", "@skid", skid);

            txtmsgs.Text = "";
            msglines = new List<string>();

            foreach (DataRow msg in table.Rows)
                msglines.Add(Fn.UnixTimeStampToDateTime(Convert.ToInt32(msg["timestamp"])).ToString("yyyy-MM-dd HH:mm") + " "
                    + msg["from_dispname"] + ": "
                    + System.Web.HttpUtility.HtmlDecode("" + msg["body_xml"]));

            ckbfiltermsg_CheckedChanged(null, null);

            prevLoadedSkid = skid;
        }

        private void lstEnc_SelectionChanged(object sender, EventArgs e)
        {
            if (lstEnc.SelectedRows.Count > 0)
            {
                string skid = Chats[lstEnc.SelectedRows[0].Index].skid;
                LoadMsgsEnc(skid);
            }
        }

        private void lstUnenc_SelectionChanged(object sender, EventArgs e)
        {
            if (lstUnenc.SelectedRows.Count > 0)
            {
                string skid = Unencd[lstUnenc.SelectedRows[0].Index].convo_id;
                LoadMsgsUnenc(skid);
            }
        }

        private void ctxUnenc_Opening(object sender, CancelEventArgs e)
        {
            e.Cancel = lstUnenc.SelectedRows.Count <= 0;
        }

        private void ctxEnc_Opening(object sender, CancelEventArgs e)
        {
            e.Cancel = lstEnc.SelectedRows.Count <= 0;
        }

        private void encryptToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lstUnenc.SelectedRows.Count <= 0)
                return;

            if (ckbdelete.Checked 
                && MessageBox.Show("Are you sure that you want to delete all messages in this chat after encrypting them?",
                    "Encrypt And Delete Chat?", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.No)
                return;


            var orig = Unencd[lstUnenc.SelectedRows[0].Index];
            string skid = Crypt.Enc(orig.convo_id);

            var exist = DBs.App.GetDataRow("select * from chats where skid = @skid", "@skid", skid);
            if (exist != null)
                DBs.App.ExecuteQuery("update chats set stamp = @stamp where skid = @skid", new Hashtable()
                    {
                        { "@stamp", Crypt.Enc(orig.Last) },
                        { "@skid", skid }
                    });
            else
                DBs.App.InsertOrUpdateRow2("chats", "cid", Int32.MinValue, new Hashtable()
                    {
                        { "name", Crypt.Enc(orig.displayname) },
                        { "skid", skid },
                        { "stamp", Crypt.Enc(orig.Last) },
                    });

            DateTime firsttime = DateTime.Now.AddYears(-5);
            var lastenc = DBs.App.GetFieldFromTable("select stamp from msgs where skid = @skid order by mid desc limit 1", "skid", skid);
            if (lastenc != DBNull.Value && lastenc != null && !String.IsNullOrEmpty(lastenc as string))
                firsttime = Crypt.DecDate(lastenc as string);

            var msgs = DBs.Skype.GetDataSource(@"select id, timestamp, body_xml, from_dispname from messages where convo_id = @convo
                                                 and timestamp > " + Fn.DateTimeToUnixTimestamp(firsttime)
                                            + @" and (chatmsg_type is null or chatmsg_type = 3) 
                                                 order by timestamp asc", "@convo", orig.convo_id);

            ctxUnenc.Enabled = false;
            bool dodelete = ckbdelete.Checked;

            BackgroundWorker work = new BackgroundWorker();
            work.WorkerReportsProgress = true;
            work.DoWork += (bw, be) =>
            {
                for (int i = 0; i < msgs.Rows.Count; i++)
                {
                    DataRow msg = msgs.Rows[i];
                    DBs.App.InsertOrUpdateRow2("msgs", "mid", Int32.MinValue, new Hashtable()
                    {
                        { "msg", Crypt.Enc(msg["body_xml"].ToString().Trim()) },
                        { "skid", skid },
                        { "stamp", Crypt.Enc(Fn.UnixTimeStampToDateTime(Convert.ToInt32(msg["timestamp"]))) },
                        { "author", Crypt.Enc(msg["from_dispname"].ToString().Trim()) }
                    });

                    if (dodelete)
                        DBs.Skype.DeleteFromTable("messages", new Hashtable() { { "id", msg["id"] } });

                    ((BackgroundWorker)bw).ReportProgress(100 * i / msgs.Rows.Count);
                }
            };
            work.ProgressChanged += (bw, be) => { this.Text = "Encrypting " + be.ProgressPercentage + " %"; };
            work.RunWorkerCompleted += (bw, be) =>
            {
                this.Text = "HistEncrypter";
                LoadEnc();
                ctxUnenc.Enabled = true;
            };
            work.RunWorkerAsync();
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lstEnc.SelectedRows.Count > 0)
            {
                if (MessageBox.Show("Are you sure that you want to delete all messages in this chat?", 
                    "Delete Chat?", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                {
                    string skid = Chats[lstEnc.SelectedRows[0].Index].skid;
                    DBs.App.ExecuteQuery(@"delete from msgs where skid = @skid", "@skid", skid);
                }
                LoadEnc();
            }
        }

        private void btnfindnext_Click(object sender, EventArgs e)
        {
            Find(RichTextBoxFinds.None);
        }

        private void btnfindprev_Click(object sender, EventArgs e)
        {
            Find(RichTextBoxFinds.Reverse);
        }

        private void Find(RichTextBoxFinds direction)
        {
            txtmsgs.Focus();

            int pos;
            int start = txtmsgs.SelectionStart;
            if (direction == RichTextBoxFinds.Reverse)
            {
                if (start == 0)
                    start = txtmsgs.Text.Length;

                start = start - 1;

                int end = 0;

                pos = txtmsgs.Find(txtfilter.Text, end, start, direction);
            }
            else
            {
                if (start > txtmsgs.Text.Length - 1)
                    start = 0;

                start = start + 1;

                int end = -1;

                pos = txtmsgs.Find(txtfilter.Text, start, end, direction);
            }

            if (pos > 0)
            {
                txtmsgs.SelectionStart = pos;
                txtmsgs.SelectionLength = txtfilter.Text.Length;
            }

        }

        private void btnglobal_Click(object sender, EventArgs e)
        {
            LoadUnenc();
            LoadEnc();
        }

        private void ckbfiltermsg_CheckedChanged(object sender, EventArgs e)
        {
            if (ckbfiltermsg.Checked)
                txtmsgs.Text = String.Join(Environment.NewLine, msglines.Where(
                    l => l.ToLower().Contains(txtfilter.Text.Trim().ToLower())
                    ).ToArray());
            else
                txtmsgs.Text = String.Join(Environment.NewLine, msglines.ToArray());
        }

        private void txtglobal_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                btnglobal_Click(null, null);
        }

        private void txtfilter_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                btnfindnext_Click(null, null);
        }

    }
}
