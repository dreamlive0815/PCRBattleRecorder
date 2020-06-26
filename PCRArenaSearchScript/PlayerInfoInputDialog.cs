using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PCRBattleRecorder.Script;

namespace PCRArenaSearchScript
{
    public partial class PlayerInfoInputDialog : Form
    {
        public PlayerInfoInputDialog()
        {
            InitializeComponent();
        }

        public ArenaSearchOp NameRankOp { get; set; } = ArenaSearchOp.And;

        public string PlayerName { get { return txtName.Text; } }

        public string PlayerRank { get { return txtRank.Text; } }

        void RefreshOpRadios()
        {
            rdoAnd.Checked = NameRankOp == ArenaSearchOp.And;
            rdoOr.Checked = NameRankOp == ArenaSearchOp.Or;
        }

        private void PlayerInfoInputDialog_Load(object sender, EventArgs e)
        {
            RefreshOpRadios();
        }

        private void rdoAnd_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoAnd.Checked)
            {
                NameRankOp = ArenaSearchOp.And;
                RefreshOpRadios();
            }
        }

        private void rdoOr_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoOr.Checked)
            {
                NameRankOp = ArenaSearchOp.Or;
                RefreshOpRadios();
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
    }
}
